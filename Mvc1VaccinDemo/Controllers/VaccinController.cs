using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Connections.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Mvc1VaccinDemo.Data;
using Mvc1VaccinDemo.Services;
using Mvc1VaccinDemo.Services.Krisinformation;
using Mvc1VaccinDemo.ViewModels;
using SharedThings.Data;

namespace Mvc1VaccinDemo.Controllers
{
    public class VaccinController : BaseController
    {
        private readonly IOrderedVaccinService _orderedVaccinService;

        public VaccinController(ApplicationDbContext dbContext, IOrderedVaccinService orderedVaccinService, IKrisInfoService krisInfoService)
            : base(dbContext, krisInfoService)
        {
            _orderedVaccinService = orderedVaccinService;
        }


        public IActionResult SearchResult(string q)
        {
            var viewModel = new VaccinSearchResultViewModel();

            viewModel.Vacciner = _dbContext.Vacciner.Include(r => r.Supplier)
                .Where(r => q == null || r.Namn.Contains(q) || r.Supplier.Name.Contains(q))
                .Select(dbVacc => new VaccinViewModel
                {
                    Id = dbVacc.Id,
                    Supplier = dbVacc.Supplier.Name,
                    Name = dbVacc.Namn
                }).ToList();


            return View(viewModel);

        }


        // Kunna nås av "Admin"
        // men INTE av "Nurse" 
        //
        //[Authorize(Roles="Admin")]
        //   /Vaccin?q=qwer
        public IActionResult Index(string q)
        {
            var viewModel = new VaccinIndexViewModel();

            viewModel.Vacciner = _dbContext.Vacciner.Include(r=>r.Supplier)
                .Where(r=> q == null || r.Namn.Contains(q) || r.Supplier.Name.Contains(q) )
                .Select(dbVacc => new VaccinViewModel
            {
                Id = dbVacc.Id,
                Supplier = dbVacc.Supplier.Name,
                Name = dbVacc.Namn
            }).ToList();


            return View(viewModel);
        }

        public IActionResult _SelectVaccine(int SelectedId)
        {
            var viewModel = new SelectVaccineViewModel();
            var selectedVaccin = _dbContext.Vacciner.First(r => r.Id == SelectedId);
            viewModel.SelectedVaccinName = selectedVaccin.Namn;
            var result = _orderedVaccinService.GetTotal(selectedVaccin.Namn);
            viewModel.FirstDeliveryDate = result.FirstDeliveryDate;
            viewModel.NrInFirstDelivery = result.NrInFirstDelivery;
            viewModel.NrOrdered = result.NrOrdered;

            return View(viewModel);
        }

        //[Authorize(Roles = "Admin")]
        public IActionResult New()
        {
            var viewModel = new VaccinNewViewModel();
            viewModel.Types = GetTypeSelectListItems();
            viewModel.AllSuppliers = GetSuppliersListItems();

            return View(viewModel);
        }



        [HttpGet]
        public IActionResult ValidateNoDuplicateName(string Namn)
        {
            if (_dbContext.Vacciner.Any(r => r.Namn == Namn))
            {
                return  Json("Detta vaccin verkar redan vara reggat");
            }

            return Json(true);
        }


        //[Authorize(Roles = "Admin")]
        [HttpPost]

        public IActionResult New(VaccinNewViewModel viewModel)
        {
            if (ModelState.IsValid)
            {

                var dbVaccin = new Vaccin();
                _dbContext.Vacciner.Add(dbVaccin);
                dbVaccin.Supplier = _dbContext.Suppliers.First(r => r.Id == viewModel.SelectedSupplierId);
                dbVaccin.Type = (Vaccin.VaccinType)viewModel.Type;
                dbVaccin.Namn = viewModel.Namn;
                dbVaccin.AntalDoser = viewModel.AntalDoser;
                dbVaccin.EuOkStatus = viewModel.EuOkStatus;
                dbVaccin.Comment = viewModel.Comment;
                _dbContext.SaveChanges();
                return RedirectToAction("Index");
            }
            viewModel.AllSuppliers = GetSuppliersListItems();
            viewModel.Types = GetTypeSelectListItems();
            return View(viewModel);
        }

        //[Authorize(Roles = "Admin")]
        public IActionResult Edit(int Id)
        {
            var viewModel = new VaccinEditViewModel();

            var dbVaccin = _dbContext.Vacciner.Include(p=>p.Supplier).First(r => r.Id == Id);

            viewModel.Id = dbVaccin.Id;
            viewModel.SelectedSupplierId = dbVaccin.Supplier.Id;
            viewModel.AllSuppliers = GetSuppliersListItems();
            viewModel.EuOkStatus = dbVaccin.EuOkStatus;
            viewModel.Namn = dbVaccin.Namn;
            viewModel.AntalDoser = dbVaccin.AntalDoser;
            viewModel.Comment = dbVaccin.Comment; 

            viewModel.Type = (int)dbVaccin.Type;
            viewModel.Types = GetTypeSelectListItems();

            viewModel.IsActive = true;

            return View(viewModel);
        }

        private List<SelectListItem> GetSuppliersListItems()
        {
            var list = new List<SelectListItem>();
            list.Add(new SelectListItem {  Value="0", Text = "..please select..."  });

            list.AddRange(_dbContext.Suppliers.Select(r => new SelectListItem
            {
                Text = r.Name,
                Value = r.Id.ToString()
            }));
            return list;
        }


        //[Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Edit(int Id, VaccinEditViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var dbVaccin = _dbContext.Vacciner.Include(p => p.Supplier).First(r => r.Id == Id);
                dbVaccin.Supplier = _dbContext.Suppliers.First(r => r.Id == viewModel.SelectedSupplierId);
                dbVaccin.Type = (Vaccin.VaccinType)viewModel.Type;
                dbVaccin.Namn = viewModel.Namn;
                dbVaccin.AntalDoser = viewModel.AntalDoser;
                dbVaccin.EuOkStatus = viewModel.EuOkStatus;
                dbVaccin.Comment = viewModel.Comment;
                _dbContext.SaveChanges();
                return RedirectToAction("Index");
            }

            viewModel.AllSuppliers = GetSuppliersListItems();
            viewModel.Types = GetTypeSelectListItems();
            return View(viewModel);
        }
        /*
            if (ModelState.IsValid)
            {
                //ALLT ÄR OK!!!
                //Spara i databas...
                //kanske redirect to index
            }
            return View(viewModel);

         *
         */

            ////namn=varde&namn2=varde&namn3=varde
            //[HttpPost]
            //public IActionResult Edit(int Id, VaccinEditViewModel viewModel)
            //{
            //    if (ModelState.IsValid)
            //    {
            //        var dbVaccin = _dbContext.Vacciner.Include(p => p.Supplier)
            //            .First(r => r.Id == Id);
            //        dbVaccin.Namn = viewModel.Namn;
            //        dbVaccin.Type = (Vaccin.VaccinType) viewModel.Type;
            //        _dbContext.SaveChanges();
            //        return RedirectToAction("Index");
            //    }
            //    return View(viewModel);
            //}



            List<SelectListItem> GetTypeSelectListItems()
        {
            var list = new List<SelectListItem>();
            list.Add(new SelectListItem("Okänd", "0"));
            list.Add(new SelectListItem("mRNA", "1"));
            list.Add(new SelectListItem("Vector", "2"));
            return list;
        }



    }
}