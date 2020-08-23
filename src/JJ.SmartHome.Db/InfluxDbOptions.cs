namespace JJ.SmartHome.Db
{
    public class InfluxDbOptions
    {
        public string Uri { get; set; }
        public string Organization { get; set; }
        public string Bucket { get; set; }
        public string Token { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        public string RetentionPolicy { get; set; } = "7d";
        public bool UseV1 { get; set; } = true;
    }
}