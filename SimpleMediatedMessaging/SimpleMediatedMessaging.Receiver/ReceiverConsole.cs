using Azure.Messaging.ServiceBus;

namespace SimpleMediatedMessaging.Receiver
{
    internal class ReceiverConsole
    {
        //To Establish the connection
        static string connectionString = "";
        static string QueueName = "";

        static async Task Main(string[] args)
        {
            //To create the client
            var client = new ServiceBusClient(connectionString);

            //To create the receiver
            var receiver = client.CreateReceiver(QueueName);

            //To receive the message
            Console.WriteLine("Recieve messages...");

            while (true)
            {
                var message = await receiver.ReceiveMessageAsync();

                if (message != null)
                {
                    Console.Write( message.Body.ToString());

                    //To complete the message
                    await receiver.CompleteMessageAsync(message);
                }
                else
                {
                    Console.WriteLine();
                    Console.WriteLine("    No more messages.");
                    Console.WriteLine("All Messages Received.");
                    break;
                }
            }

            //To close the receiver
            await receiver.CloseAsync();
        }
    }
}
