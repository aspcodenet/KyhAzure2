using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Mvc1VaccinDemo.Controllers;
using Mvc1VaccinDemo.Services.Krisinformation;
using Mvc1VaccinDemo.ViewModels;
using SharedThings.Data;
using AutoFixture;


namespace Mvc1VaccinDemo.Tests
{
    [TestClass]
    public class FaserControllerTests : BaseTest
    {
        private FaserController sut;
        private Mock<IKrisInfoService> krisInfoServiceMock;
        private ApplicationDbContext ctx;
        public FaserControllerTests()
        {
            krisInfoServiceMock = new Mock<IKrisInfoService>();

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .EnableSensitiveDataLogging()
                .Options;
            ctx = new ApplicationDbContext(options);

            sut = new FaserController( ctx,  krisInfoServiceMock.Object);
        }

        [TestMethod]
        public void NewShouldFillSelectListItems()
        {
            ctx.Myndigheter.Add(fixture.Create<Myndighet>());
            ctx.Myndigheter.Add(fixture.Create<Myndighet>());
            ctx.Myndigheter.Add(fixture.Create<Myndighet>());
            ctx.SaveChanges();
            var antalIDatabasen = ctx.Myndigheter.Count();

            var result = sut.New();


            var viewResult = result as ViewResult;
            var model = viewResult.ViewData.Model as VaccineringsFasNewViewModel;

            Assert.AreEqual(antalIDatabasen + 1, model.AllaMyndigheter.Count);

            //viewModel.AllaMyndigheter

        }

    }
}