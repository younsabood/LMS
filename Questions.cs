// Exam.cs
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LMS
{
    public partial class Questions : UserControl
    {
        private HtmlMultipleChoice htmlQuestion;
        private string correctAnswerText;
        public Questions(string question, string correctAnswer, List<string> options, string Explanation, string Difficulty)
        {
            InitializeComponent();
            this.Height = 700;
            this.Width = 700;
            InitializeComponents(question, correctAnswer, options, Explanation, Difficulty);
        }

        private void InitializeComponents(string question, string correctAnswer, List<string> options, string Explanation, string Difficulty)
        {
            // Parse correct answer
            string correctOptionPrefix = correctAnswer.Split('|')[0].Trim();
            string correctOptionLetter = correctOptionPrefix.Replace("Option ", "").Trim();
            string correctOption = options.Find(opt => opt.StartsWith($"Option {correctOptionLetter}:")) ?? "";

            // Extract correct answer text
            int colonIndex = correctOption.IndexOf(": ");
            correctAnswerText = colonIndex >= 0
                ? correctOption.Substring(colonIndex + 2).Trim()
                : correctOption.Trim();

            // Process options for display
            var processedOptions = new List<string>();
            foreach (var option in options)
            {
                int optionColonIndex = option.IndexOf(": ");
                processedOptions.Add(optionColonIndex >= 0
                    ? option.Substring(optionColonIndex + 2).Trim()
                    : option.Trim());
            }

            // Initialize HTML question component
            htmlQuestion = new HtmlMultipleChoice
            {
                QuestionText = question,
                Options = processedOptions.ToArray(),
                CorrectAnswer = correctAnswerText,
                Explanation = Explanation,
                Difficulty = Difficulty
            };
            htmlQuestion.Dock = DockStyle.Fill;
            mainPanel.Controls.Add(htmlQuestion);
        }
    }
}