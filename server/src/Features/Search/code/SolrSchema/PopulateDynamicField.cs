using System;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using Sitecore.ContentSearch.SolrProvider;
using Sitecore.ContentSearch.SolrProvider.Pipelines.PopulateSolrSchema;
using Sitecore.Diagnostics;

namespace DevsWeb.Features.Search.SolrSchema
{
    public class PopulateDynamicField : PopulateManagedSchemaProcessor
    {
        public PopulateDynamicField(string fieldName, string fieldType, string multiValued)
        {
            FieldName = fieldName;
            FieldType = fieldType;
            MultiValued = !string.IsNullOrEmpty(multiValued) && multiValued.ToLower() == "true";
        }

        public string FieldName { get; private set; }

        public string FieldType { get; private set; }

        public bool MultiValued { get; private set; }

        private String SolrSearchConnectionString { get; set; }

        public override void Process(PopulateManagedSchemaArgs args)
        {
            SolrSearchConnectionString = ConfigurationManager.ConnectionStrings["solr.search"].ConnectionString?.Split(';').FirstOrDefault();

            var solrIndex = args.Index as SolrSearchIndex;

            if (solrIndex == null)
            {
                Log.Info("Not a solr index", this);
                return;
            }

            var body = JsonConvert.SerializeObject((new DynamicFieldSchema() { name = FieldName, type = FieldType, stored = true, indexed = true, multiValued = MultiValued }));

            body = "{" + $"\"add-dynamic-field\":{body}" + "}";

            foreach (var coreName in solrIndex.GetCoreNames())
            {
                var addFieldEndPoint = $"{SolrSearchConnectionString}/{coreName}/schema";

                using (var client = new HttpClient())
                {
                    try
                    {
                        var response = client.PostAsync(addFieldEndPoint, new StringContent(body, Encoding.UTF8, "application/json")).Result;
                        Log.Info($"Add dyanmic field:{FieldName}, Field type:{FieldType} to index: {solrIndex.Name} - {coreName}: {response.Content.ReadAsStringAsync().Result}", this);
                    }
                    catch (Exception e)
                    {
                        Log.Error($"Unable to populate the schema(Field:{FieldName}) to {solrIndex.Name}({coreName}). {e.Message}", e, this);
                    }
                }
            }
        }

        private class DynamicFieldSchema
        {
            public string name { get; set; }

            public string type { get; set; }

            public bool stored { get; set; }

            public bool indexed { get; set; }

            public bool multiValued { get; set; }
        }
    }
}