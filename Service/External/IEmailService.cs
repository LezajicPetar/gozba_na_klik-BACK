namespace gozba_na_klik.Service.External
{
    public interface IEmailService
    {
        Task SendActivationEmailAsync(string to, string link);
        Task SendResetEmailAsync(string to, string link);
    }
}
