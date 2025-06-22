using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace ReservasiAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly HttpClient _httpClient;

        public PaymentController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }

        [HttpPost("generate-snap")]
        public async Task<IActionResult> GenerateSnapToken([FromBody] SnapRequestModel request)
        {
            var serverKey = "SB-Mid-server-6r7IWmbYzEqf-s5Lg7Xtk9x2";
            var encodedKey = Convert.ToBase64String(Encoding.UTF8.GetBytes(serverKey + ":"));

            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Basic", encodedKey);

            var body = new
            {
                transaction_details = new
                {
                    order_id = "ORDER-" + Guid.NewGuid().ToString("N").Substring(0, 10),
                    gross_amount = request.TotalPrice
                },
                customer_details = new
                {
                    first_name = request.Fullname,
                    email = request.Email,
                    phone = request.PhoneNumber
                }
            };

            var jsonBody = JsonSerializer.Serialize(body);
            var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("https://app.sandbox.midtrans.com/snap/v1/transactions", content);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                return BadRequest(new { message = "Gagal generate Snap Token", error });
            }

            var result = await response.Content.ReadAsStringAsync();
            return Content(result, "application/json");
        }
    }

    public class SnapRequestModel
    {
        public string Fullname { get; set; } = "";
        public string Email { get; set; } = "";
        public string PhoneNumber { get; set; } = "";
        public decimal TotalPrice { get; set; }
    }
}
