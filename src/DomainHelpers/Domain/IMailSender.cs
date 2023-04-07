namespace DomainHelpers.Domain; 
public interface IMailSender {
    Task SendAsync(string from, string to, string subject, string body);
}