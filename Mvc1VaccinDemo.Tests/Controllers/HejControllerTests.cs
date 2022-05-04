using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using AutoFixture;
using Microsoft.AspNetCore.Identity.UI.V4.Pages.Internal.Account;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Mvc1VaccinDemo.Controllers;
using Mvc1VaccinDemo.Services.Krisinformation;
using Mvc1VaccinDemo.ViewModels;
using SharedThings.Data;

namespace Mvc1VaccinDemo.Tests
{
    public class BaseTest
    {
        protected AutoFixture.Fixture fixture = new AutoFixture.Fixture();
        private ApplicationDbContext ctx;

    }

    [TestClass]
    public class HejControllerTests : BaseTest
    {
        private HejController sut;
        private Mock<IKrisInfoService> krisInfoServiceMock;
        private Mock<IAccountRepository> accountRepositoryMock;
        private KrisInfo info;
        private ApplicationDbContext ctx;


        public HejControllerTests()
        {


            info = fixture.Create<KrisInfo>();

            //info = new KrisInfo
            //{
            //    Displaymode = 0,
            //    Emergency = false,
            //    LinkUrl = "1",
            //    Id = "32132321",
            //    ImageUrl = "3213",
            //    Text = "23321321123312",
            //    Title = "231"
            //};
            krisInfoServiceMock = new Mock<IKrisInfoService>();
            accountRepositoryMock = new Mock<IAccountRepository>();
            krisInfoServiceMock.Setup(r => r.GetKrisInformation(It.IsAny<string>())).Returns(info);

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .EnableSensitiveDataLogging()
                .Options;
            ctx = new ApplicationDbContext(options);

            sut = new HejController(krisInfoServiceMock.Object, accountRepositoryMock.Object, ctx);
        }



        [TestMethod]
        public void WhenNew23312234()
        {
            var viewModel = fixture.Create<NewTransactionViewModel>();
            viewModel.Operation = "Withdrawal in Cash";
            viewModel.Amount = 20;

            var p = fixture.Create<Person>();
            viewModel.CustomerId = p.Id;
            ctx.Personer.Add(p);
            ctx.SaveChanges();

            sut.New2(viewModel);

            Assert.IsTrue( ctx.Myndigheter.Any(r=>r.Name == viewModel.Operation));

        }

        [TestMethod]
        public void WhenNew23312233122314()
        {
            var viewModel = fixture.Create<NewTransactionViewModel>();
            viewModel.Operation = "Withdrawal in Cash";
            viewModel.Amount = 20;

            var p = fixture.Create<Person>();
            viewModel.CustomerId = p.Id;
            ctx.Personer.Add(p);
            ctx.SaveChanges();

            sut.New2(viewModel);


            var customer = ctx.Personer.First(r => r.Id == viewModel.CustomerId);
            Assert.AreEqual(viewModel.Operation, customer.Name);

        }


        [TestMethod]
        public void aaa342324()
        {
            var viewModel = fixture.Create<NewTransactionViewModel>();
            viewModel.Operation = "Credit in Cash";
            viewModel.Amount = 20;

            var p = fixture.Create<Person>();
            viewModel.CustomerId = p.Id;
            ctx.Personer.Add(p);
            ctx.SaveChanges();

            sut.New2(viewModel);


            var customer = ctx.Personer.First(r => r.Id == viewModel.CustomerId);
            Assert.AreEqual(viewModel.Operation, customer.EmailAddress);

        }





        [TestMethod]
        public void WhenNewTransactionIsCalledAndBalanceIsSufficientANewTransactionShouldBeCreated()
        {
            var viewModel = fixture.Create<NewTransactionViewModel>();
            viewModel.Operation = "Withdrawal in Cash";
            viewModel.Amount = 20;

            accountRepositoryMock.Setup(e => e.CheckIfSufficientBalance(viewModel.AccountId, viewModel.Amount))
                .Returns(true);
            sut.NewTransaction(viewModel);

            accountRepositoryMock.Verify(e=>e.NewTransaction(viewModel.AccountId,viewModel.Amount, viewModel.Operation)
                , Times.Once);
        }


        [TestMethod]
        public void WhenNewTransactionIsCalledAndBalanceIsSufficientAndOperationIsWithdrawalThenWithdrawIsCalled()
        {
            var viewModel = fixture.Create<NewTransactionViewModel>();
            viewModel.Operation = "Withdrawal in Cash";
            viewModel.Amount = 20;

            accountRepositoryMock.Setup(e => e.CheckIfSufficientBalance(viewModel.AccountId, viewModel.Amount))
                .Returns(true);
            sut.NewTransaction(viewModel);

            accountRepositoryMock.Verify(e => e.Withdrawal(viewModel.AccountId,viewModel.Amount), Times.Once);
        }

        [TestMethod]
        public void WhenNewTransactionIsCalledANdOperationIsDepositThenMoneyIsDeposited()
        {
            var viewModel = fixture.Create<NewTransactionViewModel>();
            viewModel.Operation = "Credit in cash";
            viewModel.Amount = 20;

            accountRepositoryMock.Setup(e => e.CheckIfSufficientBalance(viewModel.AccountId, viewModel.Amount))
                .Returns(true);
            sut.NewTransaction(viewModel);

            accountRepositoryMock.Verify(e => e.Deposit(viewModel.AccountId, viewModel.Amount), Times.Once);
        }




        [TestMethod]
        public void WhenShowIsCalledKrisInfoServicedIsCalledWithCorrectId()
        {
            string id = "123";

            sut.Show(id);
            //Verifiera att _service.GHetKLrisInfo 123
            krisInfoServiceMock.Verify(r=>r.GetKrisInformation(id),Times.Once);
        }


        [TestMethod]
        public void When12345ExceptionShouldBeThrowm()
        {
            string id = "12345";

            Assert.ThrowsException<InvalidDataException>(() => sut.Show(id));
        }



        [TestMethod]
        public void WhenShowIsCalledViewModelIsCorrectlyPopulated()
        {
            string id = "123";

            var result = sut.Show(id);

            var viewResult = result as ViewResult;
            var model = viewResult.ViewData.Model as ArticleViewModel;

            Assert.AreEqual(info.Text,model.Text);
            Assert.AreEqual(info.LinkUrl, model.LinkUrl);
            Assert.AreEqual(info.ImageUrl, model.ImageUrl);
            Assert.AreEqual(info.Displaymode, model.Displaymode);
            Assert.AreEqual(info.Emergency, model.Emergency);
            Assert.AreEqual(info.Title, model.Title);


            //Verifiera att ViewModel.Text = info.Text
        }



    }
}
