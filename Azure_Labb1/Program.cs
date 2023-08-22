using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Azure;
using Azure.AI.Language.QuestionAnswering;
using Azure.Core;
using Azure.AI.TextAnalytics;

namespace Azure_Labb1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // QnA Nycklar och configueration
            string qnaEndpoint = "https://testbottabort666.cognitiveservices.azure.com/";
            string qnaKey = "bfd2331af7ad44208cc0cd81f4befb21";
            string projectName = "LearnFAQ";
            string deploymentName = "production";

            var qnaCredential = new AzureKeyCredential(qnaKey);
            var qnaClient = new QuestionAnsweringClient(new Uri(qnaEndpoint), qnaCredential);
            var qnaProject = new QuestionAnsweringProject(projectName, deploymentName);

            // Azure Text Analytics Nycklar och configueration
            string textAnalyticsEndpoint = "https://jaja666jaja.cognitiveservices.azure.com/";
            string textAnalyticsKey = "2e776c54852c44cab2212461bb58fb63";

            var textAnalyticsCredential = new AzureKeyCredential(textAnalyticsKey);
            var textAnalyticsClient = new TextAnalyticsClient(new Uri(textAnalyticsEndpoint), textAnalyticsCredential);


            // Användarinfo för loop
            string userInput = "";


            // Huvudloop för användarinfo
            while (userInput.ToLower() != "quit")
            {
                Console.WriteLine("Enter your question (or 'quit' to exit):");
                userInput = Console.ReadLine();

                if (userInput.ToLower() != "quit")
                {

                    // Använd QnA Maker för att få svar på användarens fråga
                    Response<AnswersResult> qnaResponse = qnaClient.GetAnswers(userInput, qnaProject);


                    // Skriv ut svaren från QnA
                    foreach (KnowledgeBaseAnswer answer in qnaResponse.Value.Answers)
                    {
                        Console.WriteLine("");
                        Console.WriteLine($"Q: {userInput}");
                        Console.WriteLine($"A: {answer.Answer}");
                        Console.WriteLine();
                    }


                    // Använd Azure Text Analytics för att analysera känslan från användarinfo
                    List<string> documents = new List<string> { userInput };
                    AnalyzeSentimentResultCollection sentimentResults = textAnalyticsClient.AnalyzeSentimentBatch(documents);


                    // Skriv ut resultatet av känslanalysen från Azure Text Analytics
                    foreach (AnalyzeSentimentResult sentimentResult in sentimentResults)
                    {
                        Console.WriteLine($"Sentiment: {sentimentResult.DocumentSentiment.Sentiment}");
                        Console.WriteLine($"Positive score: {sentimentResult.DocumentSentiment.ConfidenceScores.Positive}");
                        Console.WriteLine($"Neutral score: {sentimentResult.DocumentSentiment.ConfidenceScores.Neutral}");
                        Console.WriteLine($"Negative score: {sentimentResult.DocumentSentiment.ConfidenceScores.Negative}");
                        Console.WriteLine();
                    }
                }
            }
        }
    }
}