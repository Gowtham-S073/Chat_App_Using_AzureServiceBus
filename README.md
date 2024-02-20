# Projects Overview

## **Project 1: Simple Brokered Messaging**

### **Description:**

This project demonstrates the use of simple messaging on the Azure Service Bus. It involves creating a Service Bus namespace and a queue in the Azure portal, and then using Visual Studio with console applications to send and receive messages.

#### **Steps:**

1. Create a Service Bus namespace and a queue in the Azure portal.
2. Add the Azure.Messaging.ServiceBus NuGet package to the SenderConsole application.
3. Implement code in the SenderConsole application to send messages to the Service Bus queue.
4. Add the Azure.Messaging.ServiceBus NuGet package to the ReceiverConsole application.
5. Implement code in the ReceiverConsole application to receive messages from the Service Bus queue.

#### **Required Packages:**
- <em>Azure.Messaging.ServiceBus</em><br><br>

## **Project 2: Simple Chat Application**

### **Description:**

This project presents a simple chat application using publish/subscribe messaging with topics and subscriptions on the Azure Service Bus.

#### **Steps:**

Create a topic and subscriptions programmatically using code.
Create a Service Bus client and sender to send messages to the topic.
Create a message processor to receive and process messages from the subscription.
Implement chat functionality in a while loop to send and receive messages.
Handle message processing and errors.

#### **Required Packages:**
- <em>Azure.Messaging.ServiceBus</em><br><br>

## **OUTPUT SCREEN**

![image](https://github.com/Gowtham-S073/Chat_App_Using_AzureServiceBus/assets/127298215/197fec9c-2717-4dca-b13e-c798503c9391)
