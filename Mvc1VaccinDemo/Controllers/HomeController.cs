using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Mvc1VaccinDemo.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Server.HttpSys;
using Microsoft.Extensions.Caching.Memory;
using Mvc1VaccinDemo.Data;
using Mvc1VaccinDemo.Services.Krisinformation;
using Mvc1VaccinDemo.ViewModels;
using SharedThings.Data;

namespace Mvc1VaccinDemo.Controllers
{
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;
        private readonly SignInManager<IdentityUser> _signInManager;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext dbContext, 
            SignInManager<IdentityUser> signInManager, IKrisInfoService krisInfoService)
            : base(dbContext, krisInfoService)
        {
            _logger = logger;
            _signInManager = signInManager;
        }

        [ResponseCache(Duration = 29, Location = ResponseCacheLocation.Any)]
        public IActionResult Index()
        {
            var viewModel = new HomeIndexViewModel();

            viewModel.AntalGjordaVaccineringar = _dbContext.Vaccineringar.Count();
            viewModel.AntalGodkandaVaccin = _dbContext.Vacciner.Count(r=>r.EuOkStatus != null);
            viewModel.NumberOfPersons = _dbContext.Personer.Count();
            viewModel.NumberOfSuppliers = _dbContext.Suppliers.Count();
            var senast = _dbContext.Vacciner.OrderByDescending(r => r.EuOkStatus).Take(1).First();
            viewModel.SenastGodkand = senast.EuOkStatus.Value;
            viewModel.SenastGodkandVaccin = senast.Namn;
            return View(viewModel);
        }


        public IActionResult Dump()
        {
            return Content(@"insert into creditcard(cc,csv,name) values(
'1212-1222222', '877','Stefan Holmberg'
)");
        }


        public IActionResult Backup()
        {
            return File(@"files\backup.zip", "application/zip");
        }


        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Logout()
        {
            _signInManager.SignOutAsync().Wait();
            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
