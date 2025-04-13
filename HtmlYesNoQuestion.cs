using Microsoft.Web.WebView2.WinForms;
using Microsoft.Web.WebView2.Core;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using GenerativeAI.Types;

namespace LMS
{
    public partial class HtmlYesNoQuestion : UserControl
    {
        private WebView2 webView;
        private string userDataFolder;
        private string _correctAnswer;
        private string _Explanation;
        private string _Difficulty;
        public decimal QAencrement;
        private string _Source;

        public HtmlYesNoQuestion()
        {
            try
            {
                userDataFolder = Path.Combine(Path.GetTempPath(), "WebView2", Guid.NewGuid().ToString());
                InitializeWebView();
                this.Height = 600;
                this.Width = 600;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error initializing form: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public string QuestionText { get; set; } = "Question";

        public string CorrectAnswer
        {
            get => _correctAnswer;
            set
            {
                try
                {
                    // Validate the correct answer is either "Yes" or "No"
                    if (value != "Yes" && value != "No")
                        throw new ArgumentException("CorrectAnswer must be either 'Yes' or 'No'");

                    _correctAnswer = value;
                    RenderHtml();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error setting CorrectAnswer: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        public string Explanation
        {
            get => _Explanation;
            set
            {
                try
                {
                    _Explanation = value;
                    RenderHtml();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error setting Explanation: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        public string Source
        {
            get => _Source;
            set
            {
                try
                {
                    _Source = value;
                    RenderHtml();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error setting Source: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        public string Difficulty
        {
            get => _Difficulty;
            set
            {
                try
                {
                    _Difficulty = value;
                    RenderHtml();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error setting Difficulty: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private async void InitializeWebView()
        {
            try
            {
                webView = new WebView2 { Dock = DockStyle.Fill };
                var env = await CoreWebView2Environment.CreateAsync(null, userDataFolder);
                await webView.EnsureCoreWebView2Async(env);
                webView.CoreWebView2.WebMessageReceived += (sender, args) =>
                {
                    if (args.TryGetWebMessageAsString() == "correct")
                    {
                        AI.counter += QAencrement;
                    }
                };
                Controls.Add(webView);
                RenderHtml();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error initializing WebView: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public async Task ResetAsync()
        {
            try
            {
                if (webView?.CoreWebView2 != null)
                {
                    await webView.CoreWebView2.ExecuteScriptAsync(
                        "document.querySelectorAll('input').forEach(i => i.checked = false);");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error resetting WebView: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void RenderHtml()
        {
            try
            {
                if (webView?.CoreWebView2 == null || _correctAnswer == null) return;

                var html = $@"
                <!DOCTYPE html>
                <html lang='en'>
                <head>
                    <meta charset='UTF-8'>
                    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                    <title>Yes/No Question</title>
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
                                    {GenerateYesNoOptions()}
                                </div>
                                <button class='flip-btn' onclick='flipCard()'>Check Answer</button>
                            </div>
                            <div class='card-face card-back'>
                                <div class='correct-answer'>Correct Answer: {CorrectAnswer}</div>
                                <div class='explanation-text'>{Explanation}</div>
                                <hr style=""height:2px;border-width:0;color:gray;background-color:gray"">
                                <div class='explanation-text'>{Source}</div>
                                <div class='difficulty-box'>{Difficulty}</div>
                                <button class='flip-btn back-btn' onclick='flipBack()'>Back to Question</button>
                            </div>
                        </div>
                    </div>
                    <script>
                        const correctAnswer = '{CorrectAnswer}'.trim();

                        function flipCard() {{
                            const selected = document.querySelector('input[name=""answer""]:checked');
                            if (!selected) {{
                                return;
                            }}
                            const options = document.querySelectorAll('.option');

                            // Reset all styles first
                            options.forEach(option => {{
                                option.classList.remove('correct', 'incorrect');
                            }});

                            // Highlight the CORRECT answer
                            options.forEach(option => {{
                                const value = option.querySelector('input').value.trim();
                                if (value === correctAnswer) {{
                                    option.classList.add('correct');
                                }}
                            }});

                            // Highlight the INCORRECT selection (if any)
                            if (selected && selected.value.trim() !== correctAnswer) {{
                                const incorrectOption = selected.closest('.option');
                                incorrectOption.classList.add('incorrect');
                            }}

                            // Disable inputs and notify
                            document.querySelectorAll('input[name=""answer""]').forEach(input => {{
                                input.disabled = true;
                            }});
                            if (selected?.value.trim() === correctAnswer) {{
                                chrome.webview.postMessage('correct');
                            }}

                            // Delay flip to show highlights
                            setTimeout(() => {{
                                document.getElementById('flipCard').classList.add('flipped');
                            }}, 1000); // 2-second delay to see both highlights
                        }}

                        function flipBack() {{
                            document.getElementById('flipCard').classList.remove('flipped');
                        }}
                    </script>
                </body>
                </html>";

                webView.NavigateToString(html);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error rendering HTML: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string GenerateYesNoOptions()
        {
            try
            {
                return $@"
                <label class='option'>
                    <span>Yes</span>
                    <input type='radio' name='answer' value='Yes'>
                    <span class='custom-radio'></span>
                </label>
                <label class='option'>
                    <span>No</span>
                    <input type='radio' name='answer' value='No'>
                    <span class='custom-radio'></span>
                </label>";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error generating Yes/No options: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return string.Empty;
            }
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
                    background: #bb9b55;
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
                    body{font-family:'Segoe UI',sans-serif;background:#fcfaf5;align-items:center;justify-content:center;min-height:100vh;padding:20px;color:#333;line-height:1.5;direction:rtl;text-align:right}
                    .flip-container{perspective:1000px;width:100%;max-width:900px;margin:auto}
                    .card{width:100%;height:auto;min-height:550px;transition:transform .6s cubic-bezier(.175,.885,.32,1.275);transform-style:preserve-3d;position:relative}
                    .card-back,.flip-container.flipped .card{transform:rotateY(-180deg)}
                    .card-face{background:#fff;border-radius:16px;box-shadow:0 8px 25px rgba(0,0,0,0.1);position:absolute;width:100%;height:100%;backface-visibility:hidden;padding:30px;flex-direction:column;justify-content:space-between}
                    .card-front{z-index:2}
                    .question{font-size:1.4rem;font-weight:500;text-align:center;margin-bottom:20px}
                    .back-btn,.flip-btn,.reasoning{font-size:1rem;text-align:center;margin-top:20px}
                    .options{flex-direction:column;gap:15px}
                    .option{justify-content:space-between;align-items:center;padding:15px 20px;border:2px solid transparent;border-radius:10px;background:#fcfaf5;cursor:pointer;transition:border-color .3s,background-color .3s;flex-direction:row-reverse}
                    .option:focus-within,.option:hover{border-color:#dac67f;background:#f9f5eb}
                    input[type=radio]{display:none}
                    .custom-radio{margin-left:10px;width:20px;height:20px;border:2px solid #aaa;border-radius:50%;display:flex;align-items:center;justify-content:center}
                    input[type=radio]:checked+.custom-radio{border-color:#bb9b55}
                    input[type=radio]:checked+.custom-radio::after{content:'';width:10px;height:10px;background:#bb9b55;border-radius:50%}
                    .back-btn,.flip-btn{display:inline-block;padding:12px 24px;font-weight:600;border:none;border-radius:10px;cursor:pointer;transition:background .3s,box-shadow .3s}
                    .flip-btn{background:#bb9b55;color:#fff;margin-left:10px}
                    .flip-btn:focus,.flip-btn:hover{background:#a16f1b;box-shadow:0 4px 10px rgba(0,0,0,0.15)}
                    .back-btn{background:#e0e0e0;color:#333;margin-right:10px}
                    .back-btn:focus,.back-btn:hover{background:#ccc;box-shadow:0 4px 10px rgba(0,0,0,0.1)}
                    @media (max-width:768px){.card{min-height:500px}.question{font-size:1.2rem}.back-btn,.flip-btn{width:100%;padding:12px;margin:5px 0}.option{padding:10px 15px;flex-direction:row-reverse}.custom-radio{margin-left:0;margin-right:10px}}"
                : baseStyles + @"
                    .card-face,.option,.options,body{display:flex}
                    *{box-sizing:border-box;margin:0;padding:0}
                    body{font-family:'Segoe UI',sans-serif;background:#fcfaf5;align-items:center;justify-content:center;min-height:100vh;padding:20px;color:#333;line-height:1.5;direction:ltr}
                    .flip-container{perspective:1000px;width:100%;max-width:900px;margin:auto}
                    .card{width:100%;height:auto;min-height:550px;transition:transform .6s cubic-bezier(.175,.885,.32,1.275);transform-style:preserve-3d;position:relative}
                    .card-back,.flip-container.flipped .card{transform:rotateY(180deg)}
                    .card-face{background:#fff;border-radius:16px;box-shadow:0 8px 25px rgba(0,0,0,0.1);position:absolute;width:100%;height:100%;backface-visibility:hidden;padding:30px;flex-direction:column;justify-content:space-between}
                    .card-front{z-index:2}
                    .question{font-size:1.4rem;font-weight:500;text-align:center;margin-bottom:20px}
                    .back-btn,.flip-btn,.reasoning{font-size:1rem;text-align:center;margin-top:20px}
                    .options{flex-direction:column;gap:15px}
                    .option{justify-content:space-between;align-items:center;padding:15px 20px;border:2px solid transparent;border-radius:10px;background:#fcfaf5;cursor:pointer;transition:border-color .3s,background-color .3s}
                    .option:focus-within,.option:hover{border-color:#dac67f;background:#f9f5eb}
                    input[type=radio]{display:none}
                    .custom-radio{width:20px;height:20px;border:2px solid #aaa;border-radius:50%;display:flex;align-items:center;justify-content:center}
                    input[type=radio]:checked+.custom-radio{border-color:#bb9b55}
                    input[type=radio]:checked+.custom-radio::after{content:'';width:10px;height:10px;background:#bb9b55;border-radius:50%}
                    .back-btn,.flip-btn{display:inline-block;padding:12px 24px;font-weight:600;border:none;border-radius:10px;cursor:pointer;transition:background .3s,box-shadow .3s}
                    .flip-btn{background:#bb9b55;color:#fff}
                    .flip-btn:focus,.flip-btn:hover{background:#a16f1b;box-shadow:0 4px 10px rgba(0,0,0,0.15)}
                    .back-btn{background:#e0e0e0;color:#333}
                    .back-btn:focus,.back-btn:hover{background:#ccc;box-shadow:0 4px 10px rgba(0,0,0,0.1)}
                    @media (max-width:768px){.card{min-height:500px}.question{font-size:1.2rem}.back-btn,.flip-btn{width:100%;padding:12px}.option{padding:10px 15px}}";
        }
    }
}