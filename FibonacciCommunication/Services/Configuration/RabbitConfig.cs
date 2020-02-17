namespace Services.Configuration
{
    public class RabbitConfig
    {
        public string Host { get; set; }
        public ushort Port { get; set; }
        public string VirtualHost { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string QueueName { get; set; }

        public string CreateConnectionString()
        {
            return $"host={Host};virtualHost={VirtualHost};username={Username};password={Password}";
        }
    }
}
