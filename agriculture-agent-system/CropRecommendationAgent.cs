using System;
using System.Threading.Tasks;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Orchestration;

namespace AgricultureAgentSystem
{
    public class CropRecommendationAgent
    {
        private readonly IKernel _kernel;

        public CropRecommendationAgent(IKernel kernel)
        {
            _kernel = kernel;
        }

        public async Task<string> RecommendCropsAsync(string region)
        {
            // Create semantic function for crop recommendation
            string prompt = @$"Based on the agricultural knowledge of {region} in India, 
                            recommend suitable crops to grow considering:
                            - Soil type common in the region
                            - Typical weather patterns
                            - Water availability
                            - Market demand
                            Return the recommendations as a bulleted list.";

            var recommendFunction = _kernel.CreateSemanticFunction(prompt);

            // Execute the function
            var result = await recommendFunction.InvokeAsync(region);
            return result.ToString();
        }
    }
}