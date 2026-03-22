using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using LoanApp.Models;
using LoanApp.Repository.IRepository;

namespace LoanApp.Areas.Client.Controllers;

[Area("Client")]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public IActionResult Index()
    {
        if (User.Identity != null && User.Identity.IsAuthenticated)
        {
            return Redirect("/PostLogin");
        }
        ViewBag.LoanTypes = _unitOfWork.LoanType.GetAll(lt => lt.IsActive).ToList();
        ViewBag.SiteSettings = _unitOfWork.SiteSettings.Get(s => s.Id == 1);
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
