using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SMSPoh.Models;
using System.Net.Http.Headers;
using System.Text;
using static System.Net.Mime.MediaTypeNames;
using System.Net;

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
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _configuration.GetSection("AuthKey").Value!);
                string jsonStr = JsonConvert.SerializeObject(Sms);
                HttpContent content = new StringContent(jsonStr, Encoding.UTF8, Application.Json);
                HttpResponseMessage response = await _httpClient.PostAsync("/api/rest/send", content);

                string message = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    return Ok(message);
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
