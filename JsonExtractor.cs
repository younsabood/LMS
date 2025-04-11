using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

/// <summary>
/// Provides methods for extracting and validating JSON data from text input.
/// </summary>
public static class JsonExtractor
{
    private static readonly Regex CodeBlockRegex = new Regex(
        @"^\s*```(?:json)?\s*|```\s*$",
        RegexOptions.IgnoreCase | RegexOptions.Multiline);

    private static readonly char[] JsonSeparators = { '{', '}' };
    public static string ExtractAndFormatJson(string rawText, List<string> requiredFields = null)
    {
        if (string.IsNullOrWhiteSpace(rawText))
            return "[]";

        requiredFields = new List<string> { "question", "answer", "source", "explanation", "difficulty", "domain" };

        string cleanedText = CleanCodeBlockMarkers(rawText);
        List<JObject> validBlocks = ExtractValidJsonBlocks(cleanedText, requiredFields);

        return validBlocks.Count > 0
            ? JsonConvert.SerializeObject(validBlocks, Formatting.Indented)
            : "[]";
    }

    private static string CleanCodeBlockMarkers(string text)
    {
        return CodeBlockRegex.Replace(text, "")
                             .Trim()
                             .Replace("\0", ""); // Remove null characters separately for clarity
    }

    private static List<JObject> ExtractValidJsonBlocks(string jsonText, List<string> requiredFields)
    {
        var validBlocks = new List<JObject>();

        try
        {
            // First attempt to parse as JSON array
            if (TryParseAsArray(jsonText, requiredFields, validBlocks))
                return validBlocks;
        }
        catch (JsonReaderException)
        {
            // Fall through to object splitting
        }

        // If array parsing failed, try parsing individual objects
        foreach (var objStr in SplitJsonObjects(jsonText))
        {
            if (TryParseSingleObject(objStr, requiredFields, validBlocks))
                continue;
        }

        return validBlocks;
    }

    private static bool TryParseAsArray(string jsonText, List<string> requiredFields, List<JObject> validBlocks)
    {
        var jArray = JArray.Parse(jsonText);
        foreach (var token in jArray)
        {
            if (token is JObject obj && IsValidObject(obj, requiredFields))
                validBlocks.Add(obj);
        }
        return true;
    }

    private static bool TryParseSingleObject(string objStr, List<string> requiredFields, List<JObject> validBlocks)
    {
        try
        {
            var obj = JObject.Parse(objStr);
            if (IsValidObject(obj, requiredFields))
                validBlocks.Add(obj);
            return true;
        }
        catch (JsonReaderException)
        {
            return false;
        }
    }

    private static bool IsValidObject(JObject obj, List<string> requiredFields)
    {
        foreach (var field in requiredFields)
        {
            if (!obj.TryGetValue(field, out var token) ||
                string.IsNullOrWhiteSpace(token?.ToString()))
            {
                return false;
            }
        }
        return true;
    }

    private static IEnumerable<string> SplitJsonObjects(string jsonText)
    {
        var objects = new List<string>();
        int balance = 0;
        int start = -1;

        for (int i = 0; i < jsonText.Length; i++)
        {
            if (jsonText[i] == '{')
            {
                if (balance == 0)
                    start = i;
                balance++;
            }
            else if (jsonText[i] == '}')
            {
                balance--;
                if (balance == 0 && start != -1)
                {
                    objects.Add(jsonText.Substring(start, i - start + 1));
                    start = -1;
                }
            }
        }

        return objects;
    }
}