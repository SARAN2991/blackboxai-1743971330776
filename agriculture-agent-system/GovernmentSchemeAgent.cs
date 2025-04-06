using Microsoft.SemanticKernel;
using System.Text.Json;

namespace AgricultureAgentSystem
{
    public class GovernmentSchemeAgent
    {
        private readonly Kernel _kernel;
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public GovernmentSchemeAgent(Kernel kernel, HttpClient httpClient, IConfiguration configuration)
        {
            _kernel = kernel;
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<string> GetSchemesAsync(string state, string cropType, double landSize)
        {
            // First try to get from government API
            try
            {
                var apiResponse = await _httpClient.GetAsync($"{_configuration["GovernmentSchemesApi:BaseUrl"]}?state={state}&crop={cropType}");
                if (apiResponse.IsSuccessStatusCode)
                {
                    var schemes = await apiResponse.Content.ReadFromJsonAsync<List<GovernmentScheme>>();
                    return JsonSerializer.Serialize(schemes);
                }
            }
            catch {}

            // Fallback to Semantic Kernel if API fails
            var prompt = $@"List current Indian government schemes for {cropType} farmers in {state} with {landSize} acres land. 
                          Include scheme name, benefits, eligibility and application link.";
            
            var result = await _kernel.InvokePromptAsync<string>(prompt);
            return result;
        }
    }

    public class GovernmentScheme
    {
        public string SchemeName { get; set; }
        public string Description { get; set; }
        public string Eligibility { get; set; }
        public string Benefits { get; set; }
        public string ApplicationLink { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}