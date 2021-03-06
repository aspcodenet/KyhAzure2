using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Mvc1VaccinDemo.Data;
using Mvc1VaccinDemo.Services.Krisinformation;
using Mvc1VaccinDemo.ViewModels;
using SharedThings.Data;

namespace Mvc1VaccinDemo.Controllers
{
    public class SupplierController : BaseController
    {

        public SupplierController(ApplicationDbContext dbContext, IKrisInfoService krisInfoService) 
            :base(dbContext, krisInfoService)

        {
        }

        // GET
        public IActionResult Index()
        {
            return View();
        }


        public IActionResult ListaOk()
        {
            var viewModel = new SupplierListaOkViewModel();
            viewModel.Suppliers = _dbContext.Suppliers.Where(r => r.UnderInvestigation == false)
                .Select(s=>new SupplierListItem
                {
                    Country = s.Country,
                    Id = s.Id,
                    Namn = s.Name
                }).ToList();
            return View(viewModel);
        }


        public IActionResult ListaInvest()
        {
            var viewModel = new SupplierListaInvestViewModel();
            viewModel.Suppliers = _dbContext.Suppliers.Where(r => r.UnderInvestigation == true)
                .Select(s => new SupplierListItem
                {
                    Country = s.Country,
                    Id = s.Id,
                    Namn = s.Name
                }).ToList();
            return View(viewModel);
        }


        public IActionResult New()
        {
            var viewModel = new VaccineringsFasNewViewModel();
            viewModel.AllaMyndigheter = GetAllMyndigheter();
            return View(viewModel);
        }

        [HttpPost]
        public IActionResult New(VaccineringsFasNewViewModel viewModel)
        {
            //viewModel.Name
            bool redanFinnsIDatabasen = _dbContext.VaccineringsFaser.Any(r=>r.Name == viewModel.Name);
            
            if (redanFinnsIDatabasen)
                ModelState.AddModelError("Name", "Namnet upptaget");

            if (ModelState.IsValid)
            {
                var fas = new VaccineringsFas();
                _dbContext.VaccineringsFaser.Add(fas);
                fas.Name = viewModel.Name;
                fas.Description = viewModel.Comment;
                fas.AnsvarigMyndighet = _dbContext.Myndigheter.First(r => r.Id == viewModel.SelectedMyndighetsId);
                _dbContext.SaveChanges();
                return RedirectToAction("Index");
            }
            viewModel.AllaMyndigheter = GetAllMyndigheter();
            return View(viewModel);
        }





        public IActionResult Edit(int Id)
        {
            var viewModel = new VaccineringsFasEditViewModel();

            var dbPerson = _dbContext.VaccineringsFaser.Include(p=>p.AnsvarigMyndighet).First(r => r.Id == Id);

            viewModel.Id = dbPerson.Id;
            viewModel.Name = dbPerson.Name;
            viewModel.Comment = dbPerson.Description;
            if(dbPerson.AnsvarigMyndighet != null)
                viewModel.SelectedMyndighetsId = dbPerson.AnsvarigMyndighet.Id;

            viewModel.AllaMyndigheter = GetAllMyndigheter(); 

            return View(viewModel);
        }

        private List<SelectListItem> GetAllMyndigheter()
        {
            var l = _dbContext.Myndigheter.Select(d => new SelectListItem(d.Name, d.Id.ToString())).ToList();
            l.Insert(0,new SelectListItem("*** Välj en","0"));
            return l;
        }

        [HttpPost]
        public IActionResult Edit(int Id, VaccineringsFasEditViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var fas = _dbContext.VaccineringsFaser.First(r => r.Id == Id);
                fas.Name = viewModel.Name;
                fas.Description = viewModel.Comment;
                fas.AnsvarigMyndighet = _dbContext.Myndigheter.First(r => r.Id == viewModel.SelectedMyndighetsId);
                _dbContext.SaveChanges();
                return RedirectToAction("Index");
            }
            viewModel.AllaMyndigheter = GetAllMyndigheter();
            return View(viewModel);
        }

    }
}