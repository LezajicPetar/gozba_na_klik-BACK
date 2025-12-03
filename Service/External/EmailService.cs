namespace gozba_na_klik.Service.External
{
    public class EmailService : IEmailService
    {
        public Task SendActivationEmailAsync(string to, string link)
        {
            Console.WriteLine($"[EMAIL] Activation link for {to}: {link}");
            return Task.CompletedTask;
        }

        public Task SendResetEmailAsync(string to, string link)
        {
            {
                Console.WriteLine($"[EMAIL] Reset link for {to}: {link}");
                return Task.CompletedTask;
            }
        }
    }
}
