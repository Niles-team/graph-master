namespace graph_master.models.helpers
{
    public class AppSettings
    {
        public string Secret { get; set; }

        public string SMTPHost { get; set; }
        public string TeamName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool NeedSSL { get; set; }
        public int SSLPort { get; set; }
    }
}