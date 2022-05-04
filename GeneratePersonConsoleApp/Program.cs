using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SharedThings;
using SharedThings.Data;

namespace GeneratePersonConsoleApp
{
    class Program
    {
        private static ServiceProvider _serviceProvider;
        private static void RegisterServices()
        {
            var services = new ServiceCollection();

            services.AddTransient<IPersonGeneratorService, PersonGeneratorService>();
            services.AddTransient<IBatchPerson, BatchPerson>();
            services.AddTransient<IAzureUpdater, AzureUpdater>();
            services.AddTransient<IAzureSearcher, AzureSearcher>();
            
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    "Server=localhost;Database=Mvc1VaccinDemo;Trusted_Connection=True;MultipleActiveResultSets=true")
                );

            _serviceProvider = services.BuildServiceProvider(true);
        }

        static void Main(string[] args)
        {
            RegisterServices();
            var scope = _serviceProvider.CreateScope();
            //scope.ServiceProvider.GetRequiredService<IBatchPerson>().Run();
            //scope.ServiceProvider.GetRequiredService<IAzureUpdater>().Run();

            scope.ServiceProvider.GetRequiredService<IAzureSearcher>().Run();



            /*
             *
                         var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
                        optionsBuilder.UseSqlServer("Server=localhost;Database=Mvc1VaccinDemo;Trusted_Connection=True;MultipleActiveResultSets=true");
                        var dbContext = new ApplicationDbContext(optionsBuilder.Options);

             *
             */

        }
    }
}
