using System;
using System.Net.Mail;
using System.Text;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Microsoft.Graph;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace fn_entra_id_utility
{
    public class fn_cron_secret_expiry
    {

        [FunctionName("fn_cron_secret_expiry")]
        public async Task Run([TimerTrigger("0 0 10 * * *")] TimerInfo myTimer, ILogger log)
        {
            var result = await NotifyClientSecretExpiry(log);

        }
        public async Task<bool> NotifyClientSecretExpiry(ILogger logger)
        {
            var appList = new List<Models.Application>();
            try
            {
                var scopes = new[] { "https://graph.microsoft.com/.default" };
                var tenantId = Environment.GetEnvironmentVariable("TenantId");
                var clientId = Environment.GetEnvironmentVariable("ClientId");
                var clientSecret = await GetSecretFromKeyVault(logger, "keyName");
                var clientSecretCredential = new ClientSecretCredential(
                                tenantId, clientId, clientSecret);
                var _graphServiceClient = new GraphServiceClient(clientSecretCredential, scopes);


                DateTime currentDate = DateTime.Now;
                TimeSpan duration = new TimeSpan(7, 0, 0, 0); // 7 days
                DateTime resultDate = currentDate.Add(duration);
                var result = await _graphServiceClient.Applications.GetAsync();


                foreach (var app in result.Value)
                {
                    var isToBeExpired = app.PasswordCredentials.Where(e => e.EndDateTime <= resultDate).Count() > 0 ? true : false;

                    if (app.PasswordCredentials.Count() > 0)
                    {
                        //logger.LogInformation($"Expiry date {app.PasswordCredentials[0].EndDateTime}, status: {isToBeExpired}");
                        if (isToBeExpired)
                        {
                            appList.Add(new Models.Application
                            {
                                Id = app.Id,
                                ApplicationId = app.AppId,
                                ApplicationName = app.DisplayName,
                                SecretToBeExpired = isToBeExpired
                            });
                        }

                    }
                }

            }
            catch (Exception ex)
            {
                logger.LogInformation($"Error Message {ex.Message}");
                //  Console.WriteLine(ex.Message);
            }
            var response = await SendEmailAsync(Environment.GetEnvironmentVariable("EmailTo"), "Entra ID applications client secrets to-be expired/expired", GenerateHtmlContent(appList), logger);
            return response;
        }
        public static string GenerateHtmlContent(List<Models.Application> appList)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<html><body>");
            sb.Append("<h3>Entra ID applications client secrets to-be expired/expired</h3>");
            sb.Append("<table border='1'>");
            sb.Append("<tr><th>Application Id</th><th>Application Id</th><<th>Application Name</th></tr>");

            foreach (var app in appList)
            {
                sb.AppendFormat("<tr><td>{0}</td><td>{1}</td></tr>", app.Id, app.ApplicationName);
            }

            sb.Append("</table>");
            sb.Append("</body></html>");

            return sb.ToString();
        }

        private async Task<bool> SendEmailAsync(string emailTo, string subject, string emailBody, ILogger logger)
        {

            try
            {
                var apiKey = await GetSecretFromKeyVault(logger, "SendGridApiKey");
                var client = new SendGridClient(apiKey);
                var from = new EmailAddress(Environment.GetEnvironmentVariable("EmailFrom"), "Gowtham CBE");
                var to = new EmailAddress(emailTo, "Gowtham K");
                var plainTextContent = "";
                var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, emailBody);
                var response = await client.SendEmailAsync(msg);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                logger.LogInformation($"Error Message {ex.Message}");

            }
            return false;
        }

        private static async Task<string> GetSecretFromKeyVault(ILogger logger, string keyName)
        {

            // Get the Microsoft Entra ID access token using Managed Identity
            var credential = new ManagedIdentityCredential();
            var client = new SecretClient(new Uri(Environment.GetEnvironmentVariable("KeyVaultUri")), credential);

            try
            {
                KeyVaultSecret secret = await client.GetSecretAsync(keyName);
                return secret.Value;
            }
            catch (AuthenticationFailedException e)
            {
                logger.LogInformation($"Error Message {e.Message}");
                return string.Empty;
            }


        }
    }
}
