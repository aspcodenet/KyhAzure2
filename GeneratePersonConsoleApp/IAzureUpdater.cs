using System;
using System.Net.Mime;
using Azure;
using Azure.Search.Documents;
using Azure.Search.Documents.Indexes;
using Azure.Search.Documents.Indexes.Models;
using Azure.Search.Documents.Models;
using SharedThings;
using SharedThings.Data;

namespace GeneratePersonConsoleApp
{
    public interface IAzureUpdater
    {
        void Run();
    }





    class AzureUpdater : IAzureUpdater
    {
        private readonly ApplicationDbContext _dbContext;
        string indexName = "personerna1";
        private string searchUrl = "https://stefanpersonsearch.search.windows.net";
        private string key = "EECC1FE33FD0246082BB240CCE37BF04";

        public AzureUpdater(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public void Run()
        {
            //
            CreateIndexIfNotExists();


            var searchClient = new SearchClient(new Uri(searchUrl),
                indexName, new AzureKeyCredential(key));

            var batch = new IndexDocumentsBatch<PersonInAzure>();
            foreach (var person in _dbContext.Personer)
            {
                //Update or add new in Azure
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

            }
            IndexDocumentsResult result = searchClient.IndexDocuments(batch).Value;

        }

        private void CreateIndexIfNotExists()
        {
            var serviceEndpoint = new Uri(searchUrl);
            var credential = new AzureKeyCredential(key);
            var adminClient = new SearchIndexClient(serviceEndpoint, credential);

            var fieldBuilder = new FieldBuilder();
            var searchFields = fieldBuilder.Build(typeof(PersonInAzure));

            var definition = new SearchIndex(indexName, searchFields);

            adminClient.CreateOrUpdateIndex(definition);

        }
    }
}