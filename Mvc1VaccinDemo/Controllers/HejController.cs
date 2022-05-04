using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Mvc1VaccinDemo.Services.Krisinformation;
using Mvc1VaccinDemo.ViewModels;
using Newtonsoft.Json.Converters;
using SharedThings.Data;

namespace Mvc1VaccinDemo.Controllers
{

    public interface IAccountRepository
    {
        bool CheckIfSufficientBalance(int viewModelAccountId, decimal viewModelAmount);
        void Withdrawal(int viewModelAccountId, decimal viewModelAmount);
        void Deposit(int viewModelAccountId, decimal viewModelAmount);
        void Transfer(int viewModelAccountId, decimal viewModelAmount);
        void NewTransaction(int viewModelAccountId, decimal viewModelAmount, string viewModelOperation);
    }

    public class NewTransactionViewModel
    {
        public int AccountId { get; set; }
        public decimal Amount { get; set; }
        public string Operation { get; set; }
        public int CustomerId { get; set; }
    }
    public class HejController : Controller
    {
        private readonly IKrisInfoService _service;
        private readonly IAccountRepository _accountRepository;
        private readonly ApplicationDbContext _applicationDbContext;

        public HejController(IKrisInfoService service, IAccountRepository accountRepository, ApplicationDbContext applicationDbContext)
        {
            _service = service;
            _accountRepository = accountRepository;
            _applicationDbContext = applicationDbContext;
        }

        //1. Denna = Vi gör tester på en klass med lite fler DI dependencies
        //2. fixture om vi behöver speciella  värden och inte bara random?
        //3. Azure Search kl 14:00


        public IActionResult NewTransaction(NewTransactionViewModel viewModel)
        {

            if (!_accountRepository.CheckIfSufficientBalance(viewModel.AccountId, viewModel.Amount))
                ModelState.AddModelError("Amount", "Insufficient funds. Amount can not be greater than balance");

            if (ModelState.IsValid)
            {
                switch (viewModel.Operation)
                {
                    case "Withdrawal in Cash":
                        _accountRepository.Withdrawal(viewModel.AccountId, viewModel.Amount);
                        break;
                    case "Credit in Cash":
                        _accountRepository.Deposit(viewModel.AccountId, viewModel.Amount);
                        break;
                    default:
                        _accountRepository.Transfer(viewModel.AccountId, viewModel.Amount);
                        break;
                }
                _accountRepository.NewTransaction(viewModel.AccountId, viewModel.Amount, viewModel.Operation);

                return RedirectToAction("CustomerDetails", "Customer", new { Id = viewModel.CustomerId });
            }
            //viewModel.AvalibleOperations = _accountRepository.GetOperations();
            return View(viewModel);
        }




        public IActionResult New2(NewTransactionViewModel viewModel)
        {
            var customer = _applicationDbContext.Personer.FirstOrDefault(r => r.Id == viewModel.CustomerId);
            if (customer == null)
            {
                //ModelState.Err
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                switch (viewModel.Operation)
                {
                    case "Withdrawal in Cash":
                        customer.Name = viewModel.Operation;
                        break;
                    case "Credit in Cash":
                        customer.EmailAddress = viewModel.Operation;
                        break;
                    default:
                        break;
                }

                _applicationDbContext.Myndigheter.Add(new Myndighet
                    {Name = viewModel.Operation});
                _applicationDbContext.SaveChanges();

                return RedirectToAction("CustomerDetails", "Customer", new { Id = viewModel.CustomerId });
            }
            //viewModel.AvalibleOperations = _accountRepository.GetOperations();
            return View(viewModel);
        }





        public class Highscore
        {
            public string Namn { get; set; }
            public int Points { get; set; }
        }
        public IActionResult GetHighscore()
        {
            var l = new List<Highscore>();
            l.Add(new Highscore{Namn="Stefan", Points = 123});
            l.Add(new Highscore { Namn = "Oliver", Points = 124 });
            l.Add(new Highscore { Namn = "Josefine", Points = 133 });
            return Json(l);
        }


        public IActionResult Show(string id)
        {
            if (id == "12345")
            {
                throw new InvalidDataException("aswraasdf");
            }
            var viewModel = new ArticleViewModel();
            var db = _service.GetKrisInformation(id); //"123111"

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