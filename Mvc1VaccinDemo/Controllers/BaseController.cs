using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Mvc1VaccinDemo.Data;
using Mvc1VaccinDemo.Services.Krisinformation;
using Mvc1VaccinDemo.ViewModels;
using SharedThings.Data;

namespace Mvc1VaccinDemo.Controllers
{
    public class BaseController : Controller
    {
        protected readonly ApplicationDbContext _dbContext;
        protected readonly IKrisInfoService _krisInfoServices;

        public BaseController(ApplicationDbContext dbContext, IKrisInfoService krisInfoServices)
        {
            _dbContext = dbContext;
            _krisInfoServices = krisInfoServices;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            //OM kl 
            ViewData["SenasteArtiklar"] = _krisInfoServices.GetAllKrisInformation().Take(5).ToList();

            ViewData["AllaFaser"] = _dbContext.VaccineringsFaser
                .Select(dbVacc => new FaserIndexViewModel.FasViewModel
                {
                    Id = dbVacc.Id,
                    Name = dbVacc.Name
                }).ToList();

            base.OnActionExecuting(context);
        }

    }
}