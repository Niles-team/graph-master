using System;

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
        public bool Confirmed { get; set; }
    }

    public class UserAuthenticated : User
    {
        public string Token { get; set; }
    }

    public class UserSignIn
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool? RememberMe { get; set; }
    }

    public class UserConfirm
    {
        public Guid Code { get; set; }
    }
}