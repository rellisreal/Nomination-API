using System.Net.Mail;
using nomination_api.models; 
namespace nomination_api.internal_methods; 
public class MailManager
{
    private readonly MailMessage mail;
    private readonly SmtpClient smtpClient;
    public MailManager()
    {
        mail = new MailMessage();
        mail.From = new MailAddress("rewards@domain.com", "Rewards Email");
        smtpClient = new SmtpClient("localhost", 25);
    }
    public void SendNominationEmail(User nominator, User nominated, Nomination nomination)
    {
        try{
            mail.To.Add(nominated.Email);
            if (nomination.Formal) 
            {
            mail.Subject = $"Nomination Recieved From {nominator.UserName}";
            mail.Body = $"Dear {nominated.UserName}, \n\n You have recieved a nomination from {nominator.UserName}.With the following message: \n\n {nomination.NominationMessage}";
            smtpClient.Send(mail);
            }
            else
            {
                mail.Subject = $"HighFive Recieved From {nominator.UserName}";
                mail.Body = $"Dear {nominated.UserName}, \n\n You have recieved a HighFive from {nominator.UserName}.With the following message: \n\n {nomination.NominationMessage}";
                smtpClient.Send(mail);
            }
        }
        catch(SmtpException smtpEx)
        {
            Console.WriteLine($"SMTP Error: {smtpEx.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}
