namespace DataAbstraction.Models
{
    public class ElasticSearchConnection
    {
        public string Uri { get; set; } = "http://localhost:9200/";
        public string DefaultIndex { get; set; } = "default";
    }
}
