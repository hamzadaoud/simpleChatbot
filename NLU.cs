using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace HamzaAI
{
    public class NLU
    {
        private Dictionary<string, List<string>> intentExamples;

        public NLU()
        {
            intentExamples = new Dictionary<string, List<string>>();
        }

        public void LoadTrainingData(string filePath)
{
    // Read training data from file
    string jsonData = File.ReadAllText(filePath);
    var data = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, List<string>>>>(jsonData);

    // Check if data is null
    if (data != null)
    {
        // Check if the "intents" key exists in the data dictionary
        if (data.ContainsKey("intents"))
        {
            // Assign the value associated with the "intents" key to intentExamples
            intentExamples = data["intents"];
        }
        else
        {
            // Handle the case where "intents" key is missing in the data
            throw new Exception("The 'intents' key is missing in the training data.");
        }
    }
    else
    {
        // Handle the case where data is null
        throw new Exception("Failed to deserialize training data.");
    }
}


        public string ClassifyIntent(string userInput)
{
    string detectedIntent = "unknown";
    int maxScore = 0;

    foreach (var intent in intentExamples.Keys)
    {
        int score = CalculateMatchingScore(userInput, intentExamples[intent]);
        if (score > maxScore)
        {
            detectedIntent = intent;
            maxScore = score;
        }
    }

    return detectedIntent;
}

private int CalculateMatchingScore(string userInput, List<string> examples)
{
    int score = 0;
    foreach (string example in examples)
    {
        if (userInput.ToLower() == example.ToLower())
        {
            score = examples.Count;
            break; // Exit loop early if exact match found
        }
    }
    return score;
}



        public string? ExtractEntity(string userInput)
{
    foreach (var examples in intentExamples.Values)
    {
        foreach (var example in examples)
        {
            if (example.Contains('[') && example.Contains(']'))
            {
                int startIndex = example.IndexOf("[") + 1;
                int endIndex = example.IndexOf("]");
                string entity = example.Substring(startIndex, endIndex - startIndex);
                if (userInput.ToLower().Contains(entity.ToLower()))
                {
                    return entity;
                }
            }
        }
    }
    return null; 
}
    }
}

    