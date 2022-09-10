// Find your Service Plan ID and API Token at dashboard.sinch.com/sms/api/rest
// Find your Sinch numbers at dashboard.sinch.com/numbers/your-numbers/numbers
using System.Text;
using Contacts.Application.Common.Interfaces;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Contacts.Infrastructure.Services;

public class SmsService : ISmsService
{
    private readonly IConfiguration _configuration;

    public SmsService
    (
        IConfiguration configuration
    )
    {
        _configuration = configuration;
    }

    private class SMS
    {
        public string from { get; set; }
        public string[] to { get; set; }
        public string body { get; set; }

        public SMS(string fromVar, string[] toVar, string body)
        {
            this.from = fromVar;
            this.to = toVar;
            this.body = body;
        }
    }

    private class SmsSuccesResponse
    {
        public string id { get; set; }
        public string[] to { get; set; }
        public string from { get; set; }
        public bool canceled { get; set; }
        public string body { get; set; }
        public string type { get; set; }
        public DateTime created_at { get; set; }
        public DateTime modified_at { get; set; }
        public string delivery_report { get; set; }
        public DateTime expire_at { get; set; }
        public bool flash_message { get; set; }
    }

    public async Task SendAsync(string[] to, string body, string from = null)
    {
        using (var client = new HttpClient())
        {
            var sms = new SMS(from ?? _configuration["SinchSms:From"], to, body);

            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + _configuration["SinchSms:ApiToken"]);
            string json = JsonConvert.SerializeObject(sms);
            var postData = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("https://us.sms.api.sinch.com/xms/v1/" + _configuration["SinchSms:ServicePlanId"] + "/batches", postData);

            if (!response.IsSuccessStatusCode)
            {
                var responseMessage = await response.Content.ReadAsStringAsync();
                throw new Exception(responseMessage);
            }

            //otherwise, success
        }
    }

}