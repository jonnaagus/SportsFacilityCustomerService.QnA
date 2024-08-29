using System;
using System.Threading.Tasks;
using SportsFacilityCustomerService.QnA;
using SportsFacilityCustomerService.Speech;
using Microsoft.CognitiveServices.Speech;

namespace SportsFacilityCustomerService
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var questionAnswer = new QuestionAnswer();
            var choice = 0;

            while (true)
            {
                Console.WriteLine("\nWelcome to the sports facility customer support!\n\n" +
                                  "Press 1 to type your question.\n" +
                                  "Press 2 to record your question.\n" +
                                  "Press 0 to exit.");
                Console.WriteLine("-------------------------------------");

                var input = Console.ReadLine();

                if (!int.TryParse(input, out choice))
                {
                    Console.WriteLine("Invalid input, you must enter a number.");
                    continue;
                }

                switch (choice)
                {
                    case 1:
                        Console.WriteLine("Please type your question:");
                        var typedQuestion = Console.ReadLine();
                        await questionAnswer.AskQuestionAsync(typedQuestion);
                        break;
                    case 2:
                        string spokenQuestion = await SpeechToText.SpeechAsync();
                        if (!string.IsNullOrEmpty(spokenQuestion))
                        {
                            await questionAnswer.AskQuestionAsync(spokenQuestion);
                        }
                        break;
                    case 0:
                        return;
                    default:
                        Console.WriteLine("Invalid choice, please try again.");
                        break;
                }
            }
        }

    }
}
