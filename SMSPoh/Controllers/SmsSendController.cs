using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SMSPoh.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SmsSendController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public SmsSendController (HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }


    }
}
