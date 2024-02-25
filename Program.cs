using System;
using HamzaAI;


class Program
    {
        static void Main(string[] args)
        {
            NLU nlu = new NLU();

            // Load training data from file
            nlu.LoadTrainingData("data.json");

            Console.WriteLine("Welcome to the Hamza's AI!");
            Console.WriteLine("You can start chatting. Type 'exit' to quit.");

            while (true)
{
    string? userInput = Console.ReadLine();

    if (userInput?.ToLower() == "exit")
    {
        break;
    }

    string intent = nlu.ClassifyIntent(userInput ?? "");
    string entity = nlu.ExtractEntity(userInput ?? "") ?? "";


if (intent == "unknown")
{
    Console.WriteLine("Bot: I'm sorry, I didn't understand that.");
}
else if (intent == "greet" && entity != "")
{
    // Respond with a personalized greeting
    Console.WriteLine($"Bot: Hello {entity}! Nice to meet you.");
}
else if (intent == "ask_age")
{
    Console.WriteLine("Bot: I am not a human, so I don't have an age.");
}
else if (intent == "about_chatbot")
{
    Console.WriteLine("Bot: I am a chatbot created by Hamza Daoud. I was created in February 2024.");
}
else if (intent == "farewell")
{
    Console.WriteLine("Bot: Goodbye! Have a great day!");
}
else if (intent == "thanks")
{
    Console.WriteLine("Bot: You're welcome!");
}
else if (intent == "weather")
{
    Console.WriteLine("Bot: I'm sorry, I don't have access to weather information.");
}
else if (intent == "gaming")
{
    Console.WriteLine("Bot: I'm not capable of playing video games, but I can chat about them!");
}
else if (intent == "music")
{
    Console.WriteLine("Bot: Unfortunately, I can't play music, but I'm happy to discuss it with you!");
}
else if (intent == "food")
{
    Console.WriteLine("Bot: I love all kinds of food, but I don't have taste buds!");
}
else if (intent == "technology")
{
    Console.WriteLine("Bot: Technology is fascinating! What specific tech topic are you interested in?");
}
else if (intent == "health")
{
    Console.WriteLine("Bot: Your health is important! Do you have any health-related questions?");
}
else if (intent == "travel")
{
    Console.WriteLine("Bot: Traveling can be so exciting! Where would you like to go next?");
}
else if (intent == "education")
{
    Console.WriteLine("Bot: Education is the key to success! How can I assist you with your studies?");
}
else if (intent == "entertainment")
{
    Console.WriteLine("Bot: Let's have some fun! What type of entertainment are you in the mood for?");
}
else if (intent == "current_events")
{
    Console.WriteLine("Bot: Staying informed is crucial! What current event would you like to discuss?");
}
else
{
    Console.WriteLine($"Bot: Intent: {intent}");
}

    }}

    }