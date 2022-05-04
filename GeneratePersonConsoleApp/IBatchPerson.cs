using Microsoft.EntityFrameworkCore;
using SharedThings;
using SharedThings.Data;

namespace GeneratePersonConsoleApp
{
    public interface IBatchPerson
    {
        void Run();
    }

    class BatchPerson : IBatchPerson
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IPersonGeneratorService _personGeneratorService;

        public BatchPerson(ApplicationDbContext dbContext, IPersonGeneratorService personGeneratorService)
        {
            _dbContext = dbContext;
            _personGeneratorService = personGeneratorService;
        }

        public void Run()
        {
            for (int i = 0; i < 20; i++)
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


        }
    }
}