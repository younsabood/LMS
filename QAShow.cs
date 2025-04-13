using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GenerativeAI.Types;
using Spire.Pdf.Exporting.XPS.Schema;

namespace LMS
{
    public partial class QAShow : Form
    {
        List<QuestionsOBJ.QuestionDetailsOption> questionsoptionShow;
        DateTime dateTime = DateTime.MinValue;
        List<QuestionsOBJ.YesNO> questionsYesNOShow;
        public QAShow(List<QuestionsOBJ.QuestionDetailsOption> questionsListoption = null, List<QuestionsOBJ.YesNO> questionsListYesNO = null)
        {
            InitializeComponent();
            if (questionsListoption != null)
            {
                QuestionsOption questions;
                questionsoptionShow = questionsListoption;
                foreach (var q in questionsListoption)
                {
                    questions = new QuestionsOption(q.QuestionText, q.CorrectAnswer, q.Options, q.Explanation, q.Difficulty, questionsListoption.Count, q.Source);
                    questions.Dock = DockStyle.Top;
                    exam.Controls.Add(questions);
                }
                dateTime = DateTime.Now.AddMinutes(questionsoptionShow.Count * 2.5);
            }
            else if(questionsListYesNO != null)
            {
                QuestionsYesNO questions;
                questionsYesNOShow = questionsListYesNO;
                foreach (var q in questionsListYesNO)
                {
                    questions = new QuestionsYesNO(q.Question, q.Answer, q.Explanation, q.Difficulty, q.Source, questionsListYesNO.Count);
                    questions.Dock = DockStyle.Top;
                    exam.Controls.Add(questions);
                }
                dateTime = DateTime.Now.AddMinutes(questionsYesNOShow.Count * 1.5);
            }
            else
            {
                return;
            }
        }

        private void QAShow_Shown(object sender, EventArgs e)
        {
            if (questionsoptionShow != null)
            {
                label2.Text += questionsoptionShow.Count.ToString();
            }
            else
            {
                label2.Text += questionsYesNOShow.Count.ToString();
            }
            exam.AutoScrollPosition = new Point(0, 0);
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            int Degre = (int)AI.counter;
            label1.Text = "Your Degre : " + Degre.ToString();
            if (AI.counter == 100)
            {
                label1.Text = "Your Degre : " + AI.counter;
            }

            TimeSpan remainingTime = dateTime - DateTime.Now;
            this.Text = "New Exam Time Left : " + remainingTime.ToString(@"hh\:mm\:ss");

            if (dateTime <= DateTime.Now)
            {
                timer1.Stop();
                this.Text = "New Exam Time Left : 00:00:00";
                MessageBox.Show("Time's up! Your Degre : " + AI.counter);
                this.Close();
                return;
            }
        }
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x02000000; // WS_EX_COMPOSITED
                return cp;
            }
        }
    }
}
