using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;
using Microsoft.Extensions.Configuration;
using SportsFacilityCustomerService.QnA;
using System;
using System.Threading.Tasks;

namespace SportsFacilityCustomerService.Speech
{
    public class SpeechToText
    {
        private static SpeechConfig speechConfig;

        public static async Task<string> SpeechAsync()
        {
            string command = "";

            try
            {
                // Ladda konfiguration
                var builder = new ConfigurationBuilder().AddJsonFile("appsettings.json");
                var configuration = builder.Build();
                string cogSvcKey = configuration["Speech:CognitiveServiceKey"];
                string cogSvcRegion = configuration["Speech:CognitiveServiceRegion"];

                // Konfigurera taligenkänning
                speechConfig = SpeechConfig.FromSubscription(cogSvcKey, cogSvcRegion);

                int choice;

                // Loop för att möjliggöra fler inspelningar
                do
                {
                    command = await TranscribeCommandAsync();

                    Console.WriteLine("Press 1 if you want to record a new question.\nPress 0 to exit.");
                    string input = Console.ReadLine();
                    bool success = int.TryParse(input, out choice);

                    if (choice == 0)
                    {
                        Console.WriteLine("Exiting...");
                        break;
                    }
                } while (choice == 1);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }

            return command; // Returnera den transkriberade texten
        }

        private static async Task<string> TranscribeCommandAsync()
        {
            string command = "";

            // Konfigurera taligenkännare och ljudkälla
            using (var audioConfig = AudioConfig.FromDefaultMicrophoneInput())
            using (var speechRecognizer = new SpeechRecognizer(speechConfig, audioConfig))
            {
                Console.WriteLine("Speak now...");

                try
                {
                    var result = await speechRecognizer.RecognizeOnceAsync();

                    if (result.Reason == ResultReason.RecognizedSpeech)
                    {
                        command = result.Text;
                        Console.WriteLine($"Your question: {command}");
                    }
                    else if (result.Reason == ResultReason.NoMatch)
                    {
                        Console.WriteLine("No speech recognized.");
                    }
                    else if (result.Reason == ResultReason.Canceled)
                    {
                        var cancellation = CancellationDetails.FromResult(result);
                        Console.WriteLine($"Speech recognition canceled: {cancellation.Reason}");
                        if (cancellation.Reason == CancellationReason.Error)
                        {
                            Console.WriteLine($"Error details: {cancellation.ErrorDetails}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                }
            }

            return command;
        }
    }
}
