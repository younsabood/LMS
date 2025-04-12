using Microsoft.Web.WebView2.WinForms;
using Microsoft.Web.WebView2.Core;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LMS
{
    public partial class HtmlMultipleChoice : UserControl
    {
        private WebView2 webView;
        private string[] _options;
        private string _correctAnswer;
        private string _Explanation;
        private string _Difficulty;

        public HtmlMultipleChoice()
        {
            InitializeWebView();
            this.Height = 600;
            this.Width = 600;
        }

        public string QuestionText { get; set; } = "Question";

        public string CorrectAnswer
        {
            get => _correctAnswer;
            set { _correctAnswer = value; RenderHtml(); }
        }

        public string Explanation
        {
            get => _Explanation;
            set { _Explanation = value; RenderHtml(); }
        }

        public string Difficulty
        {
            get => _Difficulty;
            set { _Difficulty = value; RenderHtml(); }
        }

        private async void InitializeWebView()
        {
            webView = new WebView2 { Dock = DockStyle.Fill };
            Controls.Add(webView);
            await webView.EnsureCoreWebView2Async(null);

            // Add message handler
            webView.CoreWebView2.WebMessageReceived += (sender, args) =>
            {
                if (args.TryGetWebMessageAsString() == "correct")
                {
                    Exam.counter++;
                }
            };

            RenderHtml();
        }

        public string[] Options
        {
            get => _options;
            set
            {
                if (value == null || value.Length != 4)
                    throw new ArgumentException("Exactly four options required");
                _options = value;
                RenderHtml();
            }
        }

        public async Task ResetAsync()
        {
            if (webView?.CoreWebView2 != null)
            {
                await webView.CoreWebView2.ExecuteScriptAsync(
                    "document.querySelectorAll('input').forEach(i => i.checked = false);");
            }
        }

        private void RenderHtml()
        {
            if (webView?.CoreWebView2 == null || _options == null || _correctAnswer == null) return;

            var html = $@"
                <!DOCTYPE html>
                <html lang='en'>
                <head>
                    <meta charset='UTF-8'>
                    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                    <title>Flashcard MCQ</title>
                    <style>
                        {GetNewStyles()}
                    </style>
                </head>
                <body>
                    <div class='flip-container' id='flipCard'>
                        <div class='card'>
                            <div class='card-face card-front'>
                                <div class='question'>{QuestionText}</div>
                                <div class='options'>
                                    {GenerateOptionHtml(0)}
                                    {GenerateOptionHtml(1)}
                                    {GenerateOptionHtml(2)}
                                    {GenerateOptionHtml(3)}
                                </div>
                                <button class='flip-btn' onclick='flipCard()'>Check Answer</button>
                            </div>
                            <div class='card-face card-back'>
                                <div class='correct-answer'>{CorrectAnswer}</div>
                                <div class='explanation-text'>{Explanation}</div>
                                <div class='difficulty-box'>{Difficulty}</div>
                                <button class='flip-btn back-btn' onclick='flipBack()'>Back to Question</button>
                            </div>
                        </div>
                    </div>
                    <script>
                        const correctAnswer = '{CorrectAnswer}'.trim().toLowerCase();

                        function flipCard() {{
                            const selected = document.querySelector('input[name=""answer""]:checked');
                            const options = document.querySelectorAll('.option');

                            // Reset all styles first
                            options.forEach(option => {{
                                option.classList.remove('correct', 'incorrect');
                            }});

                            // Highlight the CORRECT answer
                            options.forEach(option => {{
                                const value = option.querySelector('input').value.trim().toLowerCase();
                                if (value === correctAnswer) {{
                                    option.classList.add('correct');
                                }}
                            }});

                            // Highlight the INCORRECT selection (if any)
                            if (selected && selected.value.trim().toLowerCase() !== correctAnswer) {{
                                const incorrectOption = selected.closest('.option');
                                incorrectOption.classList.add('incorrect');
                            }}

                            // Disable inputs and notify
                            document.querySelectorAll('input[name=""answer""]').forEach(input => {{
                                input.disabled = true;
                            }});
                            if (selected?.value.trim().toLowerCase() === correctAnswer) {{
                                chrome.webview.postMessage('correct');
                            }}

                            // Delay flip to show highlights
                            setTimeout(() => {{
                                document.getElementById('flipCard').classList.add('flipped');
                            }}, 2000); // 2-second delay to see both highlights
                        }}

                        function flipBack() {{
                            document.getElementById('flipCard').classList.remove('flipped');
                        }}
                    </script>
                </body>
                </html>";

            webView.NavigateToString(html);
        }

        private string GenerateOptionHtml(int index)
        {
            return $@"
            <label class='option'>
                <span>{_options[index]}</span>
                <input type='radio' name='answer' value='{_options[index]}'>
                <span class='custom-radio'></span>
            </label>";
        }

        private string GetNewStyles()
        {
            var baseStyles = @"
                .correct { 
                    background: #e6ffe6 !important; 
                    border-color: #4CAF50 !important; 
                }
                .incorrect { 
                    background: #ffe6e6 !important; 
                    border-color: #f44336 !important; 
                }
                .unselected { 
                    background: #f0f0f0 !important; 
                    opacity: 0.8;
                }
                .correct-answer {
                    color: #4CAF50;
                    font-size: 1.2rem;
                    font-weight: 600;
                    margin-bottom: 15px;
                    text-align: center;
                }
                .explanation-text {
                    font-size: 1rem;
                    line-height: 1.6;
                    margin: 20px 0;
                    color: #444;
                }
                .difficulty-box {
                    position: absolute;
                    bottom: 20px;
                    left: 20px;
                    background: #6a5dfc;
                    color: white;
                    padding: 6px 12px;
                    border-radius: 4px;
                    font-size: 0.9rem;
                    font-weight: 500;
                }";

            return AI.LanguageAI == "Arabic"
                ? baseStyles + @"
                    .card-face,.option,.options,body{display:flex}
                    *{box-sizing:border-box;margin:0;padding:0}
                    body{font-family:'Segoe UI',sans-serif;background:#f0fafb;align-items:center;justify-content:center;min-height:100vh;padding:20px;color:#333;line-height:1.5;direction:rtl;text-align:right}
                    .flip-container{perspective:1000px;width:100%;max-width:900px;margin:auto}
                    .card{width:100%;height:auto;min-height:550px;transition:transform .6s cubic-bezier(.175,.885,.32,1.275);transform-style:preserve-3d;position:relative}
                    .card-back,.flip-container.flipped .card{transform:rotateY(-180deg)}
                    .card-face{background:#fff;border-radius:16px;box-shadow:0 8px 25px rgba(0,0,0,0.1);position:absolute;width:100%;height:100%;backface-visibility:hidden;padding:30px;flex-direction:column;justify-content:space-between}
                    .card-front{z-index:2}
                    .question{font-size:1.4rem;font-weight:500;text-align:center;margin-bottom:20px}
                    .back-btn,.flip-btn,.reasoning{font-size:1rem;text-align:center;margin-top:20px}
                    .options{flex-direction:column;gap:15px}
                    .option{justify-content:space-between;align-items:center;padding:15px 20px;border:2px solid transparent;border-radius:10px;background:#eef3ff;cursor:pointer;transition:border-color .3s,background-color .3s;flex-direction:row-reverse}
                    .option:focus-within,.option:hover{border-color:#6a5dfc;background:#e8ecff}
                    input[type=radio]{display:none}
                    .custom-radio{margin-left:10px;width:20px;height:20px;border:2px solid #aaa;border-radius:50%;display:flex;align-items:center;justify-content:center}
                    input[type=radio]:checked+.custom-radio{border-color:#6a5dfc}
                    input[type=radio]:checked+.custom-radio::after{content:'';width:10px;height:10px;background:#6a5dfc;border-radius:50%}
                    .back-btn,.flip-btn{display:inline-block;padding:12px 24px;font-weight:600;border:none;border-radius:10px;cursor:pointer;transition:background .3s,box-shadow .3s}
                    .flip-btn{background:#6a5dfc;color:#fff;margin-left:10px}
                    .flip-btn:focus,.flip-btn:hover{background:#5946e2;box-shadow:0 4px 10px rgba(0,0,0,0.15)}
                    .back-btn{background:#e0e0e0;color:#333;margin-right:10px}
                    .back-btn:focus,.back-btn:hover{background:#ccc;box-shadow:0 4px 10px rgba(0,0,0,0.1)}
                    @media (max-width:768px){.card{min-height:500px}.question{font-size:1.2rem}.back-btn,.flip-btn{width:100%;padding:12px;margin:5px 0}.option{padding:10px 15px;flex-direction:row-reverse}.custom-radio{margin-left:0;margin-right:10px}}"
                : baseStyles + @"
                    .card-face,.option,.options,body{display:flex}
                    *{box-sizing:border-box;margin:0;padding:0}
                    body{font-family:'Segoe UI',sans-serif;background:#f0fafb;align-items:center;justify-content:center;min-height:100vh;padding:20px;color:#333;line-height:1.5;direction:ltr}
                    .flip-container{perspective:1000px;width:100%;max-width:600px;margin:auto}
                    .card{width:100%;height:auto;min-height:550px;transition:transform .6s cubic-bezier(.175,.885,.32,1.275);transform-style:preserve-3d;position:relative}
                    .card-back,.flip-container.flipped .card{transform:rotateY(180deg)}
                    .card-face{background:#fff;border-radius:16px;box-shadow:0 8px 25px rgba(0,0,0,0.1);position:absolute;width:100%;height:100%;backface-visibility:hidden;padding:30px;flex-direction:column;justify-content:space-between}
                    .card-front{z-index:2}
                    .question{font-size:1.4rem;font-weight:500;text-align:center;margin-bottom:20px}
                    .back-btn,.flip-btn,.reasoning{font-size:1rem;text-align:center;margin-top:20px}
                    .options{flex-direction:column;gap:15px}
                    .option{justify-content:space-between;align-items:center;padding:15px 20px;border:2px solid transparent;border-radius:10px;background:#eef3ff;cursor:pointer;transition:border-color .3s,background-color .3s}
                    .option:focus-within,.option:hover{border-color:#6a5dfc;background:#e8ecff}
                    input[type=radio]{display:none}
                    .custom-radio{width:20px;height:20px;border:2px solid #aaa;border-radius:50%;display:flex;align-items:center;justify-content:center}
                    input[type=radio]:checked+.custom-radio{border-color:#6a5dfc}
                    input[type=radio]:checked+.custom-radio::after{content:'';width:10px;height:10px;background:#6a5dfc;border-radius:50%}
                    .back-btn,.flip-btn{display:inline-block;padding:12px 24px;font-weight:600;border:none;border-radius:10px;cursor:pointer;transition:background .3s,box-shadow .3s}
                    .flip-btn{background:#6a5dfc;color:#fff}
                    .flip-btn:focus,.flip-btn:hover{background:#5946e2;box-shadow:0 4px 10px rgba(0,0,0,0.15)}
                    .back-btn{background:#e0e0e0;color:#333}
                    .back-btn:focus,.back-btn:hover{background:#ccc;box-shadow:0 4px 10px rgba(0,0,0,0.1)}
                    @media (max-width:768px){.card{min-height:500px}.question{font-size:1.2rem}.back-btn,.flip-btn{width:100%;padding:12px}.option{padding:10px 15px}}";
        }
    }
}