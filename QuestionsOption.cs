// Exam.cs
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;
using GenerativeAI.Types;

namespace LMS
{
    public partial class QuestionsOption : UserControl
    {
        private HtmlMultipleChoice htmlQuestion;
        public decimal QAcount;

        public QuestionsOption(string question, string correctAnswer, List<string> options, string Explanation, string Difficulty, int QACount, string Source)
        {
            InitializeComponent();
            this.Height = 600;
            this.Width = 700;
            QAcount = QACount;

            try
            {
                InitializeComponents(question, correctAnswer, options, Explanation, Difficulty, Source);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while initializing the question: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void InitializeComponents(string question, string correctAnswer, List<string> options, string Explanation, string Difficulty, string Source)
        {
            try
            {
                // Parse correct answer
                string correctOptionPrefix = correctAnswer.Split('|')[0].Trim();
                string correctOptionLetter = correctOptionPrefix.Replace("Option ", "").Trim();
                string correctOption = options.Find(opt => opt.StartsWith($"Option {correctOptionLetter}:")) ?? "";

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
                    CorrectAnswer = correctAnswer,
                    Explanation = Explanation,
                    Difficulty = Difficulty,
                    Source = Source
                };
                htmlQuestion.QAencrement = ((100) / (QAcount));

                htmlQuestion.Dock = DockStyle.Fill;
                mainPanel.Controls.Add(htmlQuestion);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while setting up the question components: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}