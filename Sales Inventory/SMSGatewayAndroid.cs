using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace Sales_Inventory
{
    internal class SMSGatewayAndroid
    {
        private readonly string phoneIP;
        private readonly int port;
        private readonly string username = "sms";       // your SMS Gateway username
        private readonly string password = "test123456"; // your SMS Gateway password
        private readonly string serverUrl;

        public SMSGatewayAndroid(string ip, int portNumber)
        {
            phoneIP = ip;
            port = portNumber;
            serverUrl = $"http://{phoneIP}:{port}";
        }

        public string SendSMS(string number, string message)
        {
            try
            {
                using (var client = new WebClient())
                {
                    client.Encoding = Encoding.UTF8;
                    client.Headers[HttpRequestHeader.Authorization] =
                        "Basic " + Convert.ToBase64String(Encoding.ASCII.GetBytes($"{username}:{password}"));
                    client.Headers[HttpRequestHeader.ContentType] = "application/json";
                    client.Proxy = null;

                    // Normalize number: +63 format
                    if (!number.StartsWith("+"))
                        number = "+63" + number.TrimStart('0');

                    string url = $"{serverUrl}/message";

                    // Use safe JSON format
                    string json = $@"{{
                        ""textMessage"": {{ ""text"": ""{message.Replace("\"", "\\\"")}"" }},
                        ""phoneNumbers"": [""{number}""]
                    }}";

                    string response = client.UploadString(url, "POST", json);
                    return response;
                }
            }
            catch (WebException ex)
            {
                string errorMsg = "❌ Failed to send SMS: " + ex.Message;
                if (ex.Response != null)
                {
                    using (var reader = new StreamReader(ex.Response.GetResponseStream()))
                        errorMsg += "\nServer response: " + reader.ReadToEnd();
                }
                throw new Exception(errorMsg);
            }
        }
    }
}
