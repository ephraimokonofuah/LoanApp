using LoanApp.Models;
using LoanApp.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LoanApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class SupportController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;

        public SupportController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            var tickets = _unitOfWork.SupportTicket.GetAll(
                includeProperties: "User,AssignedTo,Messages"
            ).OrderByDescending(t => t.UpdatedAt).ToList();

            ViewBag.TotalTickets = tickets.Count;
            ViewBag.OpenTickets = tickets.Count(t => t.Status == TicketStatus.Open);
            ViewBag.InProgressTickets = tickets.Count(t => t.Status == TicketStatus.InProgress);
            ViewBag.AwaitingResponse = tickets.Count(t => t.Status == TicketStatus.AwaitingResponse);
            ViewBag.ResolvedTickets = tickets.Count(t => t.Status == TicketStatus.Resolved);
            ViewBag.ClosedTickets = tickets.Count(t => t.Status == TicketStatus.Closed);

            return View(tickets);
        }

        public IActionResult Details(int id)
        {
            var ticket = _unitOfWork.SupportTicket.Get(
                t => t.Id == id,
                includeProperties: "Messages,Messages.Sender,User,AssignedTo",
                tracked: true
            );

            if (ticket == null)
            {
                return NotFound();
            }

            // Mark as read by admin
            ticket.IsReadByAdmin = true;
            _unitOfWork.SupportTicket.Update(ticket);
            _unitOfWork.Save();

            return View(ticket);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Reply(int ticketId, string message)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                TempData["error"] = "Message cannot be empty.";
                return RedirectToAction(nameof(Details), new { id = ticketId });
            }

            var adminId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (adminId == null) return Challenge();
            var ticket = _unitOfWork.SupportTicket.Get(t => t.Id == ticketId, tracked: true);

            if (ticket == null)
            {
                return NotFound();
            }

            var ticketMessage = new TicketMessage
            {
                SupportTicketId = ticketId,
                SenderId = adminId,
                Message = message.Trim(),
                IsAdminReply = true
            };

            _unitOfWork.TicketMessage.Add(ticketMessage);

            ticket.UpdatedAt = DateTime.UtcNow;
            ticket.IsReadByUser = false;
            ticket.IsReadByAdmin = true;

            // Auto-assign to the replying admin if not yet assigned
            if (string.IsNullOrEmpty(ticket.AssignedToId))
            {
                ticket.AssignedToId = adminId;
            }

            // Move to awaiting response
            if (ticket.Status == TicketStatus.Open || ticket.Status == TicketStatus.InProgress)
            {
                ticket.Status = TicketStatus.AwaitingResponse;
            }

            _unitOfWork.SupportTicket.Update(ticket);
            _unitOfWork.Save();

            TempData["success"] = "Reply sent successfully!";
            return RedirectToAction(nameof(Details), new { id = ticketId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateStatus(int ticketId, TicketStatus status)
        {
            var ticket = _unitOfWork.SupportTicket.Get(t => t.Id == ticketId, tracked: true);

            if (ticket == null)
            {
                return NotFound();
            }

            var adminId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            ticket.Status = status;
            ticket.UpdatedAt = DateTime.UtcNow;
            ticket.IsReadByUser = false;

            if (status == TicketStatus.Resolved || status == TicketStatus.Closed)
            {
                ticket.ClosedAt = DateTime.UtcNow;
            }
            else
            {
                ticket.ClosedAt = null;
            }

            if (string.IsNullOrEmpty(ticket.AssignedToId))
            {
                ticket.AssignedToId = adminId;
            }

            _unitOfWork.SupportTicket.Update(ticket);
            _unitOfWork.Save();

            TempData["success"] = $"Ticket status updated to {status}.";
            return RedirectToAction(nameof(Details), new { id = ticketId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Assign(int ticketId, string adminId)
        {
            var ticket = _unitOfWork.SupportTicket.Get(t => t.Id == ticketId, tracked: true);
            if (ticket == null)
            {
                return NotFound();
            }

            ticket.AssignedToId = adminId;
            ticket.UpdatedAt = DateTime.UtcNow;

            if (ticket.Status == TicketStatus.Open)
            {
                ticket.Status = TicketStatus.InProgress;
            }

            _unitOfWork.SupportTicket.Update(ticket);
            _unitOfWork.Save();

            TempData["success"] = "Ticket assigned successfully.";
            return RedirectToAction(nameof(Details), new { id = ticketId });
        }

        #region API

        [HttpGet]
        public IActionResult GetAll()
        {
            var tickets = _unitOfWork.SupportTicket.GetAll(
                includeProperties: "User,AssignedTo,Messages"
            ).Select(t => new
            {
                t.Id,
                t.TicketNumber,
                t.Subject,
                User = t.User?.FullName ?? "N/A",
                Category = t.Category.ToString(),
                Priority = t.Priority.ToString(),
                Status = t.Status.ToString(),
                Messages = t.Messages?.Count ?? 0,
                LastUpdated = t.UpdatedAt.ToString("dd MMM yyyy HH:mm"),
                AssignedTo = t.AssignedTo?.FullName ?? "Unassigned",
                IsReadByAdmin = t.IsReadByAdmin
            });

            return Json(new { data = tickets });
        }

        #endregion
    }
}
