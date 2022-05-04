using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Mvc1VaccinDemo.Data;
using Mvc1VaccinDemo.Services.Krisinformation;
using Mvc1VaccinDemo.ViewModels;
using SharedThings.Data;

namespace Mvc1VaccinDemo.Controllers
{
    public class ArticleController : BaseController
    {
        public ArticleController(ApplicationDbContext dbContext, IKrisInfoService krisInfoServices) : base(dbContext, krisInfoServices)
        {
        }




        [ResponseCache(Duration = 29, VaryByQueryKeys = new[] { "id" })]
        public IActionResult Show(string id)
        {
            var viewModel = new ArticleViewModel();
            //var db = new KrisInfoService().GetKrisInformation(id);
            var db = _krisInfoServices.GetKrisInformation(id);
            viewModel.Text = db.Text;
            viewModel.Displaymode = db.Displaymode;
            viewModel.Text = db.Text;
            viewModel.Emergency = db.Emergency;
            viewModel.Id = db.Id;
            viewModel.ImageUrl = db.ImageUrl;
            viewModel.LinkUrl = db.LinkUrl;
            viewModel.Title = db.Title;

            return View(viewModel);
        }
    }
}