using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Threading.Tasks;
using System.Windows.Forms;
using GenerativeAI.Types;

namespace LMS
{
    public partial class LMSHome : Form
    {
        private readonly SqlHelper _sqlHelper;
        private const string PDF_FILTER = "Supported Files (*.pdf;*.docx;*.pptx)|*.pdf;*.docx;*.pptx";
        private static bool isFormOpen = false;

        protected override CreateParams CreateParams
        {
            get
            {
                try
                {
                    CreateParams cp = base.CreateParams;
                    cp.ExStyle |= 0x02000000; // WS_EX_COMPOSITED
                    return cp;
                }
                catch (Exception ex)
                {
                    ShowErrorMessage("Error setting CreateParams", ex);
                    throw; // Re-throw to avoid breaking the application flow.
                }
            }
        }

        public LMSHome()
        {
            try
            {
                InitializeComponent();
                _sqlHelper = new SqlHelper(Properties.Settings.Default.ConnectionString);
                InitializeUI();
                this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
                this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
                this.SetStyle(ControlStyles.UserPaint, true);
            }
            catch (Exception ex)
            {
                ShowErrorMessage("Error initializing application", ex);
            }
        }

        private void InitializeUI()
        {
            try
            {
                type.SelectedItem = "Regular Multiple Choice";
                contentDomain.SelectedItem = "General Academia";
                language.SelectedItem = "Arabic";
            }
            catch (Exception ex)
            {
                ShowErrorMessage("Error initializing UI components", ex);
            }
        }

        private async void Home_Load(object sender, EventArgs e)
        {
            try
            {
                await LoadUserDataAsync();
            }
            catch (Exception ex)
            {
                ShowErrorMessage("Error loading user data", ex);
            }
        }

        private async Task LoadUserDataAsync()
        {
            try
            {
                var user = await UserInfo.GetUserAsync();
                AccountPic.Load(user.PictureUrl);
                Gmail.Text = user.Email;
                UserName.Text = user.Name;
                starting.Text += AI.DateTime.ToString("d MMM yyyy");
                expiration.Text += AI.expirationDate.ToString("d MMM yyyy");
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to load user data", ex);
            }
        }

        private void Reset_Click(object sender, EventArgs e)
        {
            try
            {
                ResetForm();
            }
            catch (Exception ex)
            {
                ShowErrorMessage("Error resetting form", ex);
            }
        }

        private void ResetForm()
        {
            try
            {
                type.SelectedItem = "Regular Multiple Choice";
                contentDomain.SelectedItem = "General Academia";
                language.SelectedItem = "Arabic";
                qanum.Value = qanum.Minimum;
                deffnum.Value = deffnum.Minimum;
                Path_1.Text = string.Empty;
                Path_2.Text = string.Empty;
                supdate.Text = string.Empty;
                eupdate.Text = string.Empty;
                PicPDF1.Image = null;
                PicPDF2.Image = null;
            }
            catch (Exception ex)
            {
                ShowErrorMessage("Error resetting form", ex);
            }
        }

        private async void Start_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ValidateInputs()) return;
                Start.Text = "Waiting For Response...";
                Start.Enabled = false;
                Reset.Enabled = false;
                var sourceFile = await UploadFileAsync(Path_1.Text, supdate);
                supdate.Text = $"{sourceFile.DisplayName} {sourceFile.Name}";
                var exampleFile = await GetExampleFileAsync();
                if (exampleFile != null)
                {
                    eupdate.Text = $"{exampleFile.DisplayName} {exampleFile.Name}";
                }
                var response = await GenerateContent(sourceFile, exampleFile);
                ProcessResponse(response);
                ResetForm();
            }
            catch (Exception ex)
            {
                ShowErrorMessage("Error generating content", ex);
            }
        }

        private bool ValidateInputs()
        {
            try
            {
                if (new[] { type, contentDomain, language }.Any(c => c.SelectedItem == null))
                {
                    ShowErrorMessage("Please fill all required fields");
                    return false;
                }
                if (qanum.Value < 10 || deffnum.Value < 1)
                {
                    ShowErrorMessage("Minimum requirements: 10 questions and difficulty level 1+");
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                ShowErrorMessage("Error validating inputs", ex);
                return false;
            }
        }

        private async Task<RemoteFile> GetExampleFileAsync()
        {
            try
            {
                if (string.IsNullOrEmpty(Path_2.Text) || !File.Exists(Path_2.Text))
                    return null;
                return await UploadFileAsync(Path_2.Text, eupdate);
            }
            catch (Exception ex)
            {
                ShowErrorMessage("Error retrieving example file", ex);
                return null;
            }
        }

        private async Task<RemoteFile> UploadFileAsync(string path, Label statusLabel)
        {
            try
            {
                if (string.IsNullOrEmpty(path) || !File.Exists(path))
                    throw new FileNotFoundException("Invalid file path", path);

                return await AI.GeminiModel.Files.UploadFileAsync(
                    path,
                    progress => statusLabel.Invoke(new Action(() =>
                        statusLabel.Text = $"Upload Progress: {progress:F2}%")));
            }
            catch (Exception ex)
            {
                ShowErrorMessage("Error uploading file", ex);
                throw;
            }
        }

        private async Task<string> GenerateContent(RemoteFile source, RemoteFile example)
        {
            try
            {
                return example == null
                    ? await AI.GenerateContentAsync(source, GetParameters())
                    : await AI.GenerateContentAsync(source, example, GetParameters());
            }
            catch (Exception ex)
            {
                ShowErrorMessage("Error generating content from files", ex);
                return null;
            }
        }

        private GenerationParameters GetParameters()
        {
            try
            {
                return new GenerationParameters
                {
                    Language = GetSelectedLanguage(),
                    Type = GetSelectedType(),
                    Difficulty = GetDifficulty(),
                    QuestionCount = GetQuestionCount(),
                    ContentDomain = GetContentDomain()
                };
            }
            catch (Exception ex)
            {
                ShowErrorMessage("Error retrieving generation parameters", ex);
                return null;
            }
        }

        private void ProcessResponse(string response)
        {
            try
            {
                if (string.IsNullOrEmpty(response))
                {
                    ShowErrorMessage("Empty response received from AI service");
                    return;
                }

                var formattedJson = JsonExtractor.ExtractAndFormatJson(response);
                var result = QuestionsOBJ.FromJson(formattedJson);

                if (result.Options != null)
                {
                    AI.counter = 0;
                    var questionDetailsList = new List<QuestionsOBJ.QuestionDetailsOption>();

                    foreach (var question in result.Options)
                    {
                        questionDetailsList.Add(new QuestionsOBJ.QuestionDetailsOption
                        {
                            QuestionText = question.Question,
                            CorrectAnswer = question.GetCorrectAnswerText(),
                            Options = question.Options,
                            Explanation = question.Explanation,
                            Difficulty = question.Difficulty,
                            Domain = question.Domain,
                            Source = question.Source
                        });
                    }
                    if (isFormOpen)
                    {
                        MessageBox.Show("A Exam is already open. Please close it before opening a new one.");
                        return;
                    }
                    isFormOpen = true;
                    Start.Text = "Exam is already open";
                    QAShow qAShow = new QAShow(questionDetailsList, null);
                    qAShow.FormClosed += (sender, e) =>
                    {
                        isFormOpen = false;
                        Start.Text = "Start Uploade";
                        Start.Enabled = true;
                        Reset.Enabled = true;
                    };

                    qAShow.Show();
                }
                else if (result.YesNos != null)
                {
                    AI.counter = 0;
                    var questionDetailsList = new List<QuestionsOBJ.YesNO>();

                    foreach (var question in result.YesNos)
                    {
                        questionDetailsList.Add(new QuestionsOBJ.YesNO
                        {
                            Question = question.Question,
                            Answer = question.Answer,
                            Source = question.Source,
                            Explanation = question.Explanation,
                            Difficulty = question.Difficulty,
                            Domain = question.Domain
                        });
                    }
                    if (isFormOpen)
                    {
                        MessageBox.Show("A Exam is already open. Please close it before opening a new one.");
                        return;
                    }
                    isFormOpen = true;
                    Start.Text = "Exam is already open";
                    QAShow qAShow = new QAShow(null, questionDetailsList);
                    qAShow.FormClosed += (sender, e) =>
                    {
                        isFormOpen = false;
                        Start.Text = "Start Uploade";
                        Start.Enabled = true;
                        Reset.Enabled = true;
                    };

                    qAShow.Show();
                }


            }
            catch (Exception ex)
            {
                ShowErrorMessage("Error processing response", ex);
            }
        }

        private string GetSelectedLanguage() => language.SelectedItem.ToString();
        private string GetSelectedType() => type.SelectedItem.ToString();
        private string GetContentDomain() => contentDomain.SelectedItem.ToString();
        private int GetDifficulty() => (int)deffnum.Value;
        private int GetQuestionCount() => (int)qanum.Value;

        private void SharedDragDropLogic(DragEventArgs e)
        {
            try
            {
                var files = (string[])e.Data.GetData(DataFormats.FileDrop);
                if (IsValidDrop(files))
                {
                    string filePath = files[0];
                    string convertedPath = ConvertToPdfIfNeeded(filePath);
                    if (!string.IsNullOrEmpty(convertedPath))
                        UpdatePathLabels(convertedPath);
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage("Error handling drag-and-drop operation", ex);
            }
        }

        private bool IsValidDrop(string[] files)
        {
            try
            {
                return files.Length == 1 &&
                       (Path.GetExtension(files[0]).Equals(".pdf", StringComparison.OrdinalIgnoreCase) ||
                        Path.GetExtension(files[0]).Equals(".docx", StringComparison.OrdinalIgnoreCase) ||
                        Path.GetExtension(files[0]).Equals(".pptx", StringComparison.OrdinalIgnoreCase)) &&
                       !BothPathsFilled();
            }
            catch (Exception ex)
            {
                ShowErrorMessage("Error validating dropped files", ex);
                return false;
            }
        }

        private bool BothPathsFilled() =>
            !string.IsNullOrEmpty(Path_1.Text) && !string.IsNullOrEmpty(Path_2.Text);

        private void SharedDragEnterLogic(DragEventArgs e)
        {
            try
            {
                if (e.Data.GetDataPresent(DataFormats.FileDrop))
                {
                    var files = (string[])e.Data.GetData(DataFormats.FileDrop);
                    e.Effect = IsValidDrop(files) ? DragDropEffects.Copy : DragDropEffects.None;
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage("Error handling drag-enter operation", ex);
            }
        }

        private void UpdatePathLabels(string filePath)
        {
            try
            {
                if (BothPathsFilled())
                {
                    ShowErrorMessage("Maximum 2 files allowed");
                    return;
                }

                if (string.IsNullOrEmpty(Path_1.Text))
                {
                    Path_1.Text = filePath;
                    PicPDF1.Image = PDF.FirstPage(filePath);
                }
                else
                {
                    Path_2.Text = filePath;
                    PicPDF2.Image = PDF.FirstPage(filePath);
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage("Failed to load preview", ex);
            }
        }

        private void SharedDoubleClickLogic()
        {
            try
            {
                using (var openFileDialog = new OpenFileDialog())
                {
                    openFileDialog.Title = "Select a File";
                    openFileDialog.Filter = PDF_FILTER;
                    openFileDialog.Multiselect = false;

                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        string convertedPath = ConvertToPdfIfNeeded(openFileDialog.FileName);
                        if (!string.IsNullOrEmpty(convertedPath))
                            UpdatePathLabels(convertedPath);
                    }
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage("Error selecting file", ex);
            }
        }

        private string ConvertToPdfIfNeeded(string inputPath)
        {
            try
            {
                string extension = Path.GetExtension(inputPath).ToLower();
                if (extension == ".pdf")
                    return inputPath;

                switch (extension)
                {
                    case ".docx":
                        return PDF.ConvertWordToPdf(inputPath);
                    case ".pptx":
                        return PDF.ConvertPowerPointToPdf(inputPath);
                    default:
                        ShowErrorMessage("Unsupported file format");
                        return null;
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage("Conversion failed", ex);
                return null;
            }
        }

        private void ShowErrorMessage(string message, Exception ex = null)
        {
            MessageBox.Show(
                ex == null ? message : $"{message}\n{ex.Message}",
                "Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }

        #region Event Handlers
        private void dragimage_DragDrop(object sender, DragEventArgs e) => SharedDragDropLogic(e);
        private void dragimage_DragEnter(object sender, DragEventArgs e) => SharedDragEnterLogic(e);
        private void dragimage_DoubleClick(object sender, EventArgs e) => SharedDoubleClickLogic();
        private void panel2_DragDrop(object sender, DragEventArgs e) => SharedDragDropLogic(e);
        private void panel2_DragEnter(object sender, DragEventArgs e) => SharedDragEnterLogic(e);
        private void panel2_DoubleClick(object sender, EventArgs e) => SharedDoubleClickLogic();
        #endregion
    }

    public class GenerationParameters
    {
        public string Language { get; set; }
        public string Type { get; set; }
        public int Difficulty { get; set; }
        public int QuestionCount { get; set; }
        public string ContentDomain { get; set; }
    }
}