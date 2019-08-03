using System.Collections.Generic;

namespace graph_master.models
{
    public class Graph
    {
        public int id { get; set; }
        public int? userId { get; set; }
        public int? teamId { get; set; }
        public string name { get; set; }
        public List<Node> nodes { get; set; }
        public List<Link> links { get; set; }
    }

    public class Node
    {
        public int id { get; set; }
        public int graphId { get; set; }
        public string data { get; set; }
    }
    public class Link
    {
        public int id { get; set; }
        public int graphId { get; set; }
        public string date { get; set; }
    }
}