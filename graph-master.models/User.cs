namespace graph_master.models
{
    public class User
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public int? TeamId { get; set; }
    }
    public class UserAuthenticate : User
    {
        public string Token { get; set; }
    }
}