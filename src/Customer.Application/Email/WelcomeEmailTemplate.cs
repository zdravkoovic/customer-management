namespace Customer.Application.Email;

public static class EmailTemplates
{
    public static EmailTemplate WelcomeEmail => new(Subject, Body);
    private static string Body => @"
        <p> Welcome to our app! 👋 <br /> We’re really glad to have you on board. </p> <p> Your account has been successfully created, and you can now start exploring all the features we’ve built to make your experience simple, fast, and enjoyable. </p> <p> If you have any questions, need help, or just want to give us feedback, feel free to reach out — our team is always happy to help. </p> <p> Thanks for joining us, and welcome once again! </p> <div class='footer'> <p>Best regards,<br />The Team</p> </div>
    ";
    private static string Subject => "Welcome to Our App, {{CustomerName}} 🎉";

}

public class EmailTemplate
{
    public string Subject { get; }
    public string Body { get; }

    public EmailTemplate(string subject, string body)
    {
        Subject = subject;
        Body = body;
    }
}