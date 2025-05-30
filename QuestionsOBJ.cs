﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms; // Add this for MessageBox

namespace LMS
{
    public class QuestionsOBJ
    {
        public class Option
        {
            public string Question { get; set; }
            public string Answer { get; set; } // Format: "Option A|Option B|Option C|Option D"
            public List<string> Options { get; set; }
            public string Source { get; set; }
            public string Explanation { get; set; }
            public string Difficulty { get; set; }
            public string Domain { get; set; }

            // Helper method to get the correct answer text
            public string GetCorrectAnswerText()
            {
                try
                {
                    // Extract the option letter (A, B, C, D) from the Answer property
                    string optionLetter = Answer.Replace("Option ", "").Trim();

                    // Find the matching option text
                    string correctOption = Options.FirstOrDefault(opt => opt.StartsWith($"Option {optionLetter}:"));

                    if (correctOption != null)
                    {
                        // Return just the text part after "Option X: "
                        int colonIndex = correctOption.IndexOf(": ");
                        if (colonIndex >= 0)
                        {
                            return correctOption.Substring(colonIndex + 2).Trim();
                        }
                    }
                    return Answer;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error in GetCorrectAnswerText: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return Answer; // Fallback to the original Answer
                }
            }
        }

        public class YesNO
        {
            public string Question { get; set; }
            public string Answer { get; set; } // "Yes" or "No"
            public string Source { get; set; }
            public string Explanation { get; set; }
            public string Difficulty { get; set; }
            public string Domain { get; set; }
        }

        public class Result
        {
            public List<Option> Options { get; set; }
            public List<YesNO> YesNos { get; set; }
        }

        public static Result FromJson(string json)
        {
            Result result = new Result();

            try
            {
                // Try parsing as options questions
                var optionsList = JsonConvert.DeserializeObject<List<Option>>(json);
                if (optionsList != null && optionsList.Count > 0 && optionsList[0].Options != null)
                {
                    result.Options = optionsList;
                    return result;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error parsing Options JSON: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            try
            {
                // Try parsing as Yes/No questions
                var yesNoList = JsonConvert.DeserializeObject<List<YesNO>>(json);
                if (yesNoList != null && yesNoList.Count > 0)
                {
                    result.YesNos = yesNoList;
                    return result;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error parsing Yes/No JSON: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return result; // Return an empty result if both parsing attempts fail
        }

        public class QuestionDetailsOption
        {
            public string QuestionText { get; set; }
            public string CorrectAnswer { get; set; }
            public List<string> Options { get; set; }
            public string Explanation { get; set; }
            public string Difficulty { get; set; }
            public string Domain { get; set; }
            public string Source { get; set; }
        }
    }
}