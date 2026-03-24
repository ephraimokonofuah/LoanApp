using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using LoanApp.Models;
using LoanApp.Repository.IRepository;
using LoanApp.Utility;
using System.Security.Claims;

namespace LoanApp.Areas.Client.Controllers
{
    [Area("Client")]
    [Authorize(Roles = "User")]
    public class SupportController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;
        private readonly IConfiguration _config;

        public SupportController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager, IEmailSender emailSender, IConfiguration config)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _emailSender = emailSender;
            _config = config;
        }

        public IActionResult Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var tickets = _unitOfWork.SupportTicket.GetAll(
                t => t.UserId == userId,
                includeProperties: "Messages"
            ).OrderByDescending(t => t.UpdatedAt).ToList();

            return View(tickets);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string subject, string message, TicketCategory category, TicketPriority priority)
        {
            if (string.IsNullOrWhiteSpace(subject) || string.IsNullOrWhiteSpace(message))
            {
                TempData["error"] = "Subject and message are required.";
                return View();
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var ticket = new SupportTicket
            {
                UserId = userId,
                Subject = subject.Trim(),
                TicketNumber = GenerateTicketNumber(),
                Category = category,
                Priority = priority,
                Status = TicketStatus.Open,
                IsReadByAdmin = false,
                IsReadByUser = true
            };

            _unitOfWork.SupportTicket.Add(ticket);
            _unitOfWork.Save();

            // Add the initial message
            var ticketMessage = new TicketMessage
            {
                SupportTicketId = ticket.Id,
                SenderId = userId,
                Message = message.Trim(),
                IsAdminReply = false
            };

            _unitOfWork.TicketMessage.Add(ticketMessage);
            _unitOfWork.Save();

            // Notify all admins of new support ticket
            var admins = await _userManager.GetUsersInRoleAsync("Admin");
            var globalAdmins = await _userManager.GetUsersInRoleAsync("GlobalAdmin");
            var emailBody = EmailTemplates.SupportTicketCreated(ticket.TicketNumber, subject.Trim(), category.ToString());
            foreach (var admin in admins.Concat(globalAdmins).Where(a => !string.IsNullOrEmpty(a.Email)).DistinctBy(a => a.Email))
            {
                await _emailSender.SendEmailAsync(admin.Email!, $"New Support Ticket #{ticket.TicketNumber}", emailBody);
            }

            TempData["success"] = "Support ticket created successfully! Ticket #" + ticket.TicketNumber;
            return RedirectToAction(nameof(Details), new { id = ticket.Id });
        }

        public IActionResult Details(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var ticket = _unitOfWork.SupportTicket.Get(
                t => t.Id == id && t.UserId == userId,
                includeProperties: "Messages,Messages.Sender,User,AssignedTo",
                tracked: true
            );

            if (ticket == null)
            {
                return NotFound();
            }

            // Mark as read by user
            ticket.IsReadByUser = true;
            _unitOfWork.SupportTicket.Update(ticket);
            _unitOfWork.Save();

            return View(ticket);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reply(int ticketId, string message)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                TempData["error"] = "Message cannot be empty.";
                return RedirectToAction(nameof(Details), new { id = ticketId });
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var ticket = _unitOfWork.SupportTicket.Get(
                t => t.Id == ticketId && t.UserId == userId,
                tracked: true
            );

            if (ticket == null)
            {
                return NotFound();
            }

            // Don't allow replies on closed tickets
            if (ticket.Status == TicketStatus.Closed)
            {
                TempData["error"] = "This ticket is closed and cannot receive new messages.";
                return RedirectToAction(nameof(Details), new { id = ticketId });
            }

            var ticketMessage = new TicketMessage
            {
                SupportTicketId = ticketId,
                SenderId = userId,
                Message = message.Trim(),
                IsAdminReply = false
            };

            _unitOfWork.TicketMessage.Add(ticketMessage);

            // If was awaiting user response, move to In Progress
            if (ticket.Status == TicketStatus.AwaitingResponse)
            {
                ticket.Status = TicketStatus.InProgress;
            }

            ticket.UpdatedAt = DateTime.UtcNow;
            ticket.IsReadByAdmin = false;
            ticket.IsReadByUser = true;
            _unitOfWork.SupportTicket.Update(ticket);
            _unitOfWork.Save();

            // Notify all admins of new reply
            var replyUser = await _userManager.FindByIdAsync(userId);
            var admins = await _userManager.GetUsersInRoleAsync("Admin");
            var globalAdmins = await _userManager.GetUsersInRoleAsync("GlobalAdmin");
            var emailBody = EmailTemplates.SupportTicketReply(replyUser?.FullName ?? "User", ticket.TicketNumber, ticket.Subject, false);
            foreach (var admin in admins.Concat(globalAdmins).Where(a => !string.IsNullOrEmpty(a.Email)).DistinctBy(a => a.Email))
            {
                await _emailSender.SendEmailAsync(admin.Email!, $"New Reply on Ticket #{ticket.TicketNumber}", emailBody);
            }

            TempData["success"] = "Reply sent successfully!";
            return RedirectToAction(nameof(Details), new { id = ticketId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Close(int ticketId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var ticket = _unitOfWork.SupportTicket.Get(
                t => t.Id == ticketId && t.UserId == userId,
                tracked: true
            );

            if (ticket == null)
            {
                return NotFound();
            }

            ticket.Status = TicketStatus.Closed;
            ticket.ClosedAt = DateTime.UtcNow;
            ticket.UpdatedAt = DateTime.UtcNow;
            ticket.IsReadByAdmin = false;
            _unitOfWork.SupportTicket.Update(ticket);
            _unitOfWork.Save();

            TempData["success"] = "Ticket closed successfully.";
            return RedirectToAction(nameof(Details), new { id = ticketId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Reopen(int ticketId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var ticket = _unitOfWork.SupportTicket.Get(
                t => t.Id == ticketId && t.UserId == userId,
                tracked: true
            );

            if (ticket == null)
            {
                return NotFound();
            }

            if (ticket.Status != TicketStatus.Closed && ticket.Status != TicketStatus.Resolved)
            {
                TempData["error"] = "Only closed or resolved tickets can be reopened.";
                return RedirectToAction(nameof(Details), new { id = ticketId });
            }

            ticket.Status = TicketStatus.Open;
            ticket.ClosedAt = null;
            ticket.UpdatedAt = DateTime.UtcNow;
            ticket.IsReadByAdmin = false;
            _unitOfWork.SupportTicket.Update(ticket);
            _unitOfWork.Save();

            TempData["success"] = "Ticket reopened successfully.";
            return RedirectToAction(nameof(Details), new { id = ticketId });
        }

        private string GenerateTicketNumber()
        {
            var random = new Random();
            string ticketNumber;
            do
            {
                ticketNumber = "TKT-" + random.Next(100000, 999999).ToString();
            } while (_unitOfWork.SupportTicket.Get(t => t.TicketNumber == ticketNumber) != null);

            return ticketNumber;
        }
    }
}
