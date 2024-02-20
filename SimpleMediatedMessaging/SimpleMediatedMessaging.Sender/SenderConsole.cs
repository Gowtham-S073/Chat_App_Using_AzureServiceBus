using Azure.Messaging.ServiceBus;
using System.Text;

namespace SimpleMediatedMessaging.Sender
{
    public class SenderConsole
    {
        static string connectionString = "";
        static string QueueName = "";
        static string Sentence = "I learnt Service Bus";


        static async Task Main(string[] args)
        {
            var client = new ServiceBusClient(connectionString);

            var sender = client.CreateSender(QueueName);

            Console.WriteLine("Sending, Messages...");
            foreach (var character in Sentence)
            {
                var message = new ServiceBusMessage(Encoding.UTF8.GetBytes(character.ToString()));
                await sender.SendMessageAsync(message);
                Console.WriteLine($"    Sent: { character }");
            }

            await sender.CloseAsync();

            Console.WriteLine("Sent, Messages.");
            Console.WriteLine("!");
        }
    }
}
