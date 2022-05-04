using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using AutoMapper;
using Azure;
using Azure.Search.Documents;
using Azure.Search.Documents.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Mvc1VaccinDemo.Data;
using Mvc1VaccinDemo.Services.Krisinformation;
using Mvc1VaccinDemo.ViewModels;
using SharedThings;
using SharedThings.Data;

namespace Mvc1VaccinDemo.Controllers
{
    public class PersonController : BaseController
    {
        private readonly IPersonGeneratorService _personGeneratorService;
        private readonly IMapper _mapper;

        public PersonController(ApplicationDbContext dbContext, IKrisInfoService krisInfoService,
            IPersonGeneratorService personGeneratorService, IMapper mapper)
            : base(dbContext, krisInfoService)
        {
            _personGeneratorService = personGeneratorService;
            _mapper = mapper;
        }


        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult BulkAdd()
        {
            var viewModel = new PersonBulkAddViewModel();
            viewModel.Antal = 0;
            return View(viewModel);
        }


        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult BulkAdd(PersonBulkAddViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                for (int i = 0; i < viewModel.Antal; i++)
                {
                    var generatedPerson = _personGeneratorService.GenerateFakePerson();
                    _dbContext.Personer.Add(new Person
                    {
                        City = generatedPerson.City,
                        EmailAddress = generatedPerson.EmailAddress,
                        Name = generatedPerson.Name,
                        PersonalNumber = generatedPerson.PersonalNumber,
                        PostalCode = generatedPerson.PostalCode,
                        StreetAddress = generatedPerson.StreetAddress
                    });
                }

                _dbContext.SaveChanges();

                return RedirectToAction("Index");
            }
            return View(viewModel);
        }



        public class LocationFilter
        {
            public string Name { get; set; }
            public int pageSize { get; set; }
            public int pageIndex { get; set; }
            public string sortOrder { get; set; }
            public string sortField { get; set; }
        }
        private LocationFilter GetFilter()
        {
            var filter = Request.Query;

            return new LocationFilter
            {
                Name = filter["Name"],
                pageSize = Convert.ToInt32(filter["pageSize"]),
                pageIndex = Convert.ToInt32(filter["pageIndex"]),
                sortOrder = filter.ContainsKey("sortOrder") ? filter["sortOrder"].ToString() : "",
                sortField = filter.ContainsKey("sortField") ? filter["sortField"].ToString() : ""
            };
        }

        public class Result<T>
        {
            public T[] data { get; set; }
            public int itemsCount { get; set; }
        }




        public IActionResult PersonDataSource()
        {
            var filter = GetFilter();

            var result = _dbContext.Personer.Where(x =>
                (string.IsNullOrEmpty(filter.Name) || x.Name.Contains(filter.Name))
            );

            //Sorting
            if (filter.sortField == "name")
            {
                if(filter.sortOrder == "asc")
                    result = result.OrderBy(p => p.Name);
                else
                    result = result.OrderByDescending(p => p.Name);
            }


            var result2 = _mapper.Map<IEnumerable<PersonEditViewModel>>(result);

            var data2 = result2.ToArray();
            return Ok(new Result<PersonEditViewModel>
            {
                data = data2.Skip(filter.pageSize * (filter.pageIndex - 1)).Take(filter.pageSize).ToArray(),
                itemsCount = data2.Length
            });


        }




        // GET
        [Authorize(Roles = "Admin, Nurse")]
        public IActionResult Index(string q, string sortField, string sortOrder, int page = 1)
        {
            var viewModel = new PersonIndexViewModel();

            var query = _dbContext.Personer
                .Where(r => q == null || r.Name.Contains(q) || r.PersonalNumber.Contains(q));


            //ANTAL POSTER SOM MATCHAR FILTRET
            int totalRowCount = query.Count();

            if (string.IsNullOrEmpty(sortField))
                sortField = "Namn";
            if (string.IsNullOrEmpty(sortOrder))
                sortOrder = "asc";

            if (sortField == "Namn")
            {
                if (sortOrder == "asc")
                    query = query.OrderBy(y => y.Name);
                else
                    query = query.OrderByDescending(y => y.Name);
            }

            if (sortField == "Email")
            {
                if (sortOrder == "asc")
                    query = query.OrderBy(y => y.EmailAddress);
                else
                    query = query.OrderByDescending(y => y.EmailAddress);
            }

            if (sortField == "Personnummer")
            {
                if (sortOrder == "asc")
                    query = query.OrderBy(y => y.PersonalNumber);
                else
                    query = query.OrderByDescending(y => y.PersonalNumber);

            }

            int pageSize = 10;

            var pageCount = (double)totalRowCount / pageSize;
            viewModel.TotalPages = (int)Math.Ceiling(pageCount);




            //Skip - hoppa över så många
            //Take - sen ta så många

            int howManyRecordsToSkip = (page - 1) * pageSize;  // Sida 1 ->  0

            query = query.Skip(howManyRecordsToSkip).Take(pageSize);



            viewModel.Personer = query
                .Select(person => new PersonViewModel
                {
                    Id = person.Id,
                    Name = person.Name,
                    EmailAddress = person.EmailAddress,
                    PersonalNumber = person.PersonalNumber
                }).ToList();

            //viewModel.Personer = query
            //    .Select(person => new PersonViewModel
            //    {
            //        Id = person.Id,
            //        Name = person.Name,
            //        EmailAddress = person.EmailAddress,
            //        PersonalNumber = person.PersonalNumber
            //    }).ToList();

            viewModel.q = q;
            viewModel.SortOrder = sortOrder;
            viewModel.SortField = sortField;
            viewModel.Page = page;
            viewModel.OppositeSortOrder = sortOrder == "asc" ? "desc" : "asc";
            return View(viewModel);
        }


        [Authorize(Roles = "Admin, Nurse")]
        public IActionResult _SelectPerson(int id)
        {
            var viewModel = new SelectPersonViewModel();
            var p = _dbContext.Personer.Include(p=>p.VaccineringsFas).First(r => r.Id == id);

            viewModel.Name = p.Name;
            viewModel.PersonalNumber = p.PersonalNumber;
            viewModel.NextVaccinDate = p.PreliminaryNextVaccinDate.HasValue
                ? p.PreliminaryNextVaccinDate.Value.ToString("yyyy-MM-dd")
                : "";
            viewModel.VaccineringsFas = p.VaccineringsFas?.Name;
            return View(viewModel);
        }



        [Authorize(Roles = "Admin")]
        public IActionResult Edit(int Id)
        {
            var viewModel = new PersonEditViewModel();

            var dbPerson = _dbContext.Personer.Include(r=>r.VaccineringsFas).First(r => r.Id == Id);

            viewModel = _mapper.Map<PersonEditViewModel>(dbPerson);




            //var listOfViewwModels = _mapper.Map<IEnumerable<Person>>(_dbContext.Personer).ToList();

            //viewModel.Id = dbPerson.Id;
            //viewModel.Name = dbPerson.Name;
            //viewModel.Email = dbPerson.EmailAddress;
            //viewModel.PersonalNumber = dbPerson.PersonalNumber;
            //viewModel.City = dbPerson.City;
            //viewModel.PostalCode = dbPerson.PostalCode;
            //viewModel.PreliminaryNextVaccinDate = dbPerson.PreliminaryNextVaccinDate;
            //viewModel.StreetAddress = dbPerson.StreetAddress;
            //viewModel.Description = dbPerson.Description;


            if (dbPerson.VaccineringsFas != null)
                viewModel.SelectedVaccineringsFasId = dbPerson.VaccineringsFas.Id;
            viewModel.AllaVaccineringsFaser = GetAllVaccineringsFaserAsListItems(); 

            return View(viewModel);
        }



        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(int Id, PersonEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                var dbPerson = _dbContext.Personer.Include(r => r.VaccineringsFas).First(r => r.Id == Id);

                dbPerson = _mapper.Map<Person>(model);

                //dbPerson.Name = model.Name;
                //dbPerson.EmailAddress = model.Email;
                //dbPerson.PersonalNumber = model.PersonalNumber;
                //dbPerson.City = model.City;
                //dbPerson.PostalCode = model.PostalCode;
                //dbPerson.PreliminaryNextVaccinDate = model.PreliminaryNextVaccinDate;
                //dbPerson.StreetAddress = model.StreetAddress;
                //dbPerson.Description = model.Description;

                UpdateSearchIndex(dbPerson);

                _dbContext.SaveChanges();
                return RedirectToAction("Index");

            }
            model.AllaVaccineringsFaser = GetAllVaccineringsFaserAsListItems();
            return View(model);

        }

        private void UpdateSearchIndex(Person person)
        {
            var searchClient = new SearchClient(new Uri("https://stefanpersonsearch.search.windows.net"),
                "personerna", new AzureKeyCredential("F4B85AB85B8662E8B66EACDF1B98E581"));

            var batch = new IndexDocumentsBatch<PersonInAzure>();
            var personInAzure = new PersonInAzure
            {
                City = person.City,
                Description = person.Description,
                Id = person.Id.ToString(),
                Namn = person.Name,
                StreetAddress = person.StreetAddress
            };
            batch.Actions.Add(new IndexDocumentsAction<PersonInAzure>(IndexActionType.MergeOrUpload,
                personInAzure));

            IndexDocumentsResult result = searchClient.IndexDocuments(batch);
        }


        private List<SelectListItem> GetAllVaccineringsFaserAsListItems()
        {
            var list = new List<SelectListItem>();
            list.AddRange(_dbContext.VaccineringsFaser.Select(r=>new SelectListItem
            {
                Value = r.Id.ToString(),
                Text = r.Name
            }));
            return list;

        }
    }
}