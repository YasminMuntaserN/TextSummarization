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
                string apiKey = "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";  // Use your Hugging Face API key
                string modelUrl = "https://api-inference.huggingface.co/models/facebook/bart-large-cnn"; // Replace with desired model URL

                // Create HTTP client
                using (HttpClient client = new HttpClient())
                {
                    // Add authorization header with API key
                    client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

                    // Create the payload with additional parameters for length control
                    var jsonBody = new
                    {
                        inputs = inputText,
                        parameters = new
                        {
                            max_length = maxLenght,  // Specify the maximum length for the summary 
                            min_length = minLenght,   // Specify the minimum length for the summary
                            do_sample = false  // Ensures deterministic output (no randomness)
                        }
                    };

                    // Send POST request
                    HttpResponseMessage response = await client.PostAsync(
                        modelUrl,
                        new StringContent(JsonConvert.SerializeObject(jsonBody), Encoding.UTF8, "application/json")
                    );

                    // Read the response
                    string responseContent = await response.Content.ReadAsStringAsync();

                    return Ok(responseContent);  // Return the response from the API
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
