using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LMS
{
    public partial class QuestionsYesNO : UserControl
    {
        private HtmlYesNoQuestion htmlQuestion;
        public decimal QAcount;
        public QuestionsYesNO(string question, string Answer, string Explanation, string Difficulty, string Source, int QACount)
        {
            InitializeComponent();
            this.Height = 700;
            this.Width = 700;
            QAcount = QACount;
            try
            {
                InitializeComponents(question, Answer, Explanation, Difficulty, Source);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while initializing the question: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void InitializeComponents(string question, string Answer, string Explanation, string Difficulty, string Source)
        {
            try
            {
                // Initialize HTML question component
                htmlQuestion = new HtmlYesNoQuestion
                {
                    QuestionText = question,
                    CorrectAnswer = Answer,
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
