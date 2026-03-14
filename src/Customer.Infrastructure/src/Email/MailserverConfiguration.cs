namespace Customer.Infrastructure.src.Email;

public class MailserverConfiguration()
{
    public string Hostname { get; set; } = "localhost";
    public int Port { get; set; } = 1025;
}