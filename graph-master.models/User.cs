namespace graph_master.models
{
    public class User
    {
        public int id { get; set; }
        public string userName { get; set; }
        public string password { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string email { get; set; }
        public int? teamId { get; set; }
    }
    public class UserAuthenticate : User
    {
        public string token { get; set; }
    }
}