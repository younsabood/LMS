using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Spire.Pdf.Exporting.XPS.Schema;

namespace LMS
{
    public partial class QAShow : Form
    {
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x02000000; // WS_EX_COMPOSITED
                return cp;
            }
        }
        List<QuestionsOBJ.QuestionDetailsOption> questionsShow;
        public QAShow(List<QuestionsOBJ.QuestionDetailsOption> questionsList)
        {
            Questions questions;
            InitializeComponent();
            questionsShow = questionsList;
            foreach (var q in questionsList)
            {
                questions = new Questions(q.QuestionText, q.CorrectAnswer, q.Options, q.Explanation, q.Difficulty, questionsList.Count);
                questions.Dock = DockStyle.Top;
                exam.Controls.Add(questions);
            }
        }

        private void QAShow_Shown(object sender, EventArgs e)
        {
            label2.Text += questionsShow.Count.ToString();
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
        }
    }
}
