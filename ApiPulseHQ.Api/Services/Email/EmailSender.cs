namespace ApiPulseHQ.Api.Services.Email
{
    public class EmailSender : IEmailSender
    {
        public Task SendAsync(string to, string subject, string body)
        {
            // TODO: integrate real email provider later
            Console.WriteLine($"Sending email to {to}: {subject}");
            return Task.CompletedTask;
        }
    }
}
