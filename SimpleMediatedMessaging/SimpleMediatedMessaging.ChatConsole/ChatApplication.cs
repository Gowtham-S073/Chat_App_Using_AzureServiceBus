using Azure.Messaging.ServiceBus;
using Azure.Messaging.ServiceBus.Administration;

namespace SimpleMediatedMessaging.ChatConsole
{
    internal class ChatApplication
    {
        static string connectionString = "";
        static string TopicName = "";


        static async Task Main(string[] args)
        {
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("  ____ _   _    _  _____      _    ____  ____  \r\n / ___| | | |  / \\|_   _|    / \\  |  _ \\|  _ \\ \r\n| |   | |_| | / _ \\ | |     / _ \\ | |_) | |_) |\r\n| |___|  _  |/ ___ \\| |    / ___ \\|  __/|  __/ \r\n \\____|_| |_/_/   \\_\\_|   /_/   \\_\\_|   |_|    ");
            Console.WriteLine("");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Welcome to the Chat Room");
            Console.WriteLine("");
            Console.WriteLine("Enter your name :");
            var userName = Console.ReadLine();

            //Create the administration client for managing the artifacts
            var serviceBusAdministrationClient = new ServiceBusAdministrationClient(connectionString);
            
            //To create the topic if does not exist
            if (!await serviceBusAdministrationClient.TopicExistsAsync(TopicName))
            {
                await serviceBusAdministrationClient.CreateTopicAsync(TopicName);
            }

            //To create a temporary subscription for the user if does not exist
            if (!await serviceBusAdministrationClient.SubscriptionExistsAsync(TopicName, userName))
            {
                var subscriptionOptions = new CreateSubscriptionOptions(TopicName, userName)
                {
                    AutoDeleteOnIdle = TimeSpan.FromMinutes(5)
                };
                await serviceBusAdministrationClient.CreateSubscriptionAsync(subscriptionOptions);
            }

            //To create the Service Bus client
            var ServiceBusClient = new ServiceBusClient(connectionString);

            //To create the Service Bus sender
            var ServiceBusSender = ServiceBusClient.CreateSender(TopicName);

            //To create a message processor
            var processor = ServiceBusClient.CreateProcessor(TopicName, userName);

            //Add a handler to process the message
            processor.ProcessMessageAsync += MessageHandler;

            //Add a handler to process any errors
            processor.ProcessErrorAsync += ErrorHandler;

            //Start the message processor
            await processor.StartProcessingAsync();

            //To send the message
            var Startmessage = new ServiceBusMessage($"{userName} has entered the Chat Room.");
            await ServiceBusSender.SendMessageAsync(Startmessage);

            while(true)
            {
                var text = Console.ReadLine();
                if (text?.ToLower() == "exit")
                {
                    break;
                }
                else if (text.ToLower() == "clear")
                {
                    Console.Clear();

                    Console.WriteLine("  ____ _   _    _  _____      _    ____  ____  \r\n / ___| | | |  / \\|_   _|    / \\  |  _ \\|  _ \\ \r\n| |   | |_| | / _ \\ | |     / _ \\ | |_) | |_) |\r\n| |___|  _  |/ ___ \\| |    / ___ \\|  __/|  __/ \r\n \\____|_| |_/_/   \\_\\_|   /_/   \\_\\_|   |_|    ");
                    Console.WriteLine("");
                    Console.WriteLine($"Welcome again to the Chat Room {userName}");
                    Console.WriteLine("");
                }
                else if (text.ToLower() == "help")
                {
                    Console.WriteLine("Type 'exit' to leave the chat room.");
                    Console.WriteLine("Type 'clear' to clear the chat window.");
                }
                else if(text.ToLower() == "help")
                {
                    Console.WriteLine("Type 'exit' to leave");
                }
                else if(text == null)
                {
                     Console.WriteLine("Type 'help' to get informations or, type 'exit' to leave");
                }

                //Send a chat message
                var message = new ServiceBusMessage($"{userName}> {text}");
                await ServiceBusSender.SendMessageAsync(message);
            }

            //Send a goodbye message
            var Endmessage = new ServiceBusMessage($"{userName} has left the Chat Room.");
            await ServiceBusSender.SendMessageAsync(Endmessage);

            //Stop the message processor
            await processor.StopProcessingAsync();

            //Close the processor and sender
            await processor.CloseAsync();
            await ServiceBusSender.CloseAsync();
        }

        static async Task MessageHandler(ProcessMessageEventArgs args)
        {
            //Retrieve and print the message body
            var body = args.Message.Body.ToString();
            Console.WriteLine($"{ body }");

            //Complete the message
            await args.CompleteMessageAsync(args.Message);
        }

        static Task ErrorHandler(ProcessErrorEventArgs args)
        {
            //Log the error
            Console.WriteLine(args.Exception.ToString());
            return Task.CompletedTask;
        }
    }
}
