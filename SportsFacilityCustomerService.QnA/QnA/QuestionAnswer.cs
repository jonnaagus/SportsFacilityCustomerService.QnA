using Azure;
using Azure.AI.Language.QuestionAnswering;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace SportsFacilityCustomerService.QnA
{
    public class QuestionAnswer
    {
        private readonly IConfiguration _configuration;

        public QuestionAnswer()
        {
            var builder = new ConfigurationBuilder().AddJsonFile("appsettings.json");
            _configuration = builder.Build();
        }

        public async Task AskQuestionAsync(string question)
        {
            try
            {
                // Read configuration settings
                var endpoint = new Uri(_configuration["Azure:QuestionAnswering:Endpoint"]);
                var apiKey = _configuration["Azure:QuestionAnswering:ApiKey"];
                var projectName = _configuration["Azure:QuestionAnswering:ProjectName"];
                var deploymentName = _configuration["Azure:QuestionAnswering:DeploymentName"];

                var credential = new AzureKeyCredential(apiKey);
                var client = new QuestionAnsweringClient(endpoint, credential);

                // Create project object
                var project = new QuestionAnsweringProject(projectName, deploymentName);

                // Get answers
                var response = await client.GetAnswersAsync(question, project);

                // Display answers
                Console.WriteLine("Answers:");
                foreach (var answer in response.Value.Answers)
                {
                    Console.WriteLine($"- {answer.Answer}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}
