using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace TextSummarizationAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SummarizationController : ControllerBase
    {
        // POST api/summarization
        [HttpPost]
        public async Task<IActionResult> Summarize([FromBody] string inputText , int minLenght ,int maxLenght)
        {
            try
            {
                string apiKey = "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"; 
                string modelUrl = "https://api-inference.huggingface.co/models/facebook/bart-large-cnn"; 

                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

                    var jsonBody = new
                    {
                        inputs = inputText,
                        parameters = new
                        {
                            max_length = maxLenght,
                            min_length = minLenght,
                            do_sample = false  
                        }
                    };

                    HttpResponseMessage response = await client.PostAsync(
                        modelUrl,
                        new StringContent(JsonConvert.SerializeObject(jsonBody), Encoding.UTF8, "application/json")
                    );

                    string responseContent = await response.Content.ReadAsStringAsync();

                    return Ok(responseContent);                  }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
