using System.Net.Mail;

namespace MovieSite.Data
{
	public class MailProcessing
	{
		private readonly SmtpClient SmtpServer;
		public MailProcessing()
		{
			this.SmtpServer = new SmtpClient("smtp.office365.com");
			{
				SmtpServer.Port = 587;

				SmtpServer.Credentials = new System.Net.NetworkCredential("MovieSiteBG@outlook.com", "159753134679g");
				SmtpServer.EnableSsl = true;
			};
		}
		/// <summary>
		///         This method sends and question email to the sites email
		/// </summary>
		public void SendEmailToSite(string from, string personName, string Subject, string Body)
		{
			try
			{
				MailMessage mail = new MailMessage();
				mail.From = new MailAddress("MovieSiteBG@outlook.com");
				mail.To.Add("MovieSiteBG@outlook.com");
				mail.Subject = Subject;
				mail.Body = $"{personName} has sent a new email adress with body: " + Body + " Email: " + from;
				SmtpServer.Send(mail);
			}
			catch
			{
				return;
			}

		}
	}
}
