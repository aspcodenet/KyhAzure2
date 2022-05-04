using System;
using Azure;
using Azure.Search.Documents;
using SharedThings;

namespace GeneratePersonConsoleApp
{
    public interface IAzureSearcher
    {
        void Run();

    }

    class AzureSearcher : IAzureSearcher
    {
        string indexName = "personerna";
        private string searchUrl = "https://stefanpersonsearch.search.windows.net";
        private string key = "F4B85AB85B8662E8B66EACDF1B98E581";

        public void Run()     
        {


            var searchClient = new SearchClient(new Uri(searchUrl),
                indexName, new AzureKeyCredential(key));

            while (true)
            {
                Console.Write("Ange sökord:");
                string sok = Console.ReadLine();

                var searchOptions = new SearchOptions
                {
                    OrderBy = { "City desc" },
                    Skip = 0,
                    Size = 10,
                    IncludeTotalCount = true
                };


                var searchResult = searchClient.Search<PersonInAzure>(sok, searchOptions);

                foreach (var result in searchResult.Value.GetResults())
                {
                    Console.WriteLine(result.Document.Id);
                }


            }
        }
    }
}