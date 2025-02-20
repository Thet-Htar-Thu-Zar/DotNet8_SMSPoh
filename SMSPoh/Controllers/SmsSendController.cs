using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SMSPoh.Models;
using System.Net.Http.Headers;
using System.Text;
using static System.Net.Mime.MediaTypeNames;
using System.Net;
using System.Buffers.Text;
using Microsoft.AspNetCore.Authentication;

namespace SMSPoh.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SmsSendController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public SmsSendController(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        [HttpPost]
        public async Task<IActionResult> SendSms([FromBody] SmsDTOs Sms)
        {
            try
            {
                string apiKey = _configuration.GetSection("ApiKey").Value;
                string apiSecret = _configuration.GetSection("ApiSecret").Value;

                // Concatenate the API Key and API Secret with a colon
                string concatenated = $"{apiKey}:{apiSecret}";

                // Convert the concatenated string to a byte array
                byte[] bytesToEncode = Encoding.UTF8.GetBytes(concatenated);

                // Encode the byte array to a Base64 string
                string base64Encoded = Convert.ToBase64String(bytesToEncode);

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", base64Encoded);
                string jsonStr = JsonConvert.SerializeObject(Sms);
                HttpContent content = new StringContent(jsonStr, Encoding.UTF8, Application.Json);
                HttpResponseMessage response = await _httpClient.PostAsync("https://v3.smspoh.com/api/rest/send", content);
                //HttpResponseMessage response = await _httpClient.PostAsync("/api/send", content);

                string message = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    return Ok(JsonConvert.DeserializeObject(message));
                }
                return BadRequest(message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
