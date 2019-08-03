using System.Collections.Generic;

namespace graph_master.models
{
    public class Graph
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public int? TeamId { get; set; }
        public string Name { get; set; }
        public List<Node> Nodes { get; set; }
        public List<Link> Links { get; set; }
    }

    public class Node
    {
        public int Id { get; set; }
        public int GraphId { get; set; }
        public string Data { get; set; }
    }
    public class Link
    {
        public int Id { get; set; }
        public int GraphId { get; set; }
        public string Data { get; set; }
    }
}