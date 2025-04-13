using System;
using System.IO;
using System.Threading.Tasks;
using GenerativeAI.Core;
using GenerativeAI.Types;
using GenerativeAI;
using System.Windows.Forms;

namespace LMS
{
    public static class AI
    {
        public static decimal counter;
        public static readonly GeminiModel GeminiModel = CreateGeminiModel();
        public static string LanguageAI;
        public static DateTime DateTime = DateTime.Now;
        public static DateTime expirationDate = DateTime.Now;
        private static GeminiModel CreateGeminiModel()
        {
            var modelParams = new ModelParams { Model = GoogleAIModels.Gemini2FlashLatest };
            return new GeminiModel(Properties.Settings.Default.googleAI, modelParams);
        }

        public static async Task<string> GenerateContentAsync(RemoteFile file, GenerationParameters parameters)
        {
            var request = new GenerateContentRequest();
            request.AddText(BuildPrompt(parameters, file.DisplayName));
            request.AddRemoteFile(file);

            var response = await GeminiModel.GenerateContentAsync(request);
            return response.Text;
        }

        public static async Task<string> GenerateContentAsync(RemoteFile file1, RemoteFile file2, GenerationParameters parameters)
        {
            var request = new GenerateContentRequest();
            request.AddText(BuildPrompt(parameters, file1.DisplayName, file2.DisplayName));
            request.AddRemoteFile(file1);
            request.AddRemoteFile(file2);

            var response = await GeminiModel.GenerateContentAsync(request);
            return response.Text;
        }

        private static string BuildPrompt(GenerationParameters parameters, string sourceDocument, string exampleExam = null)
        {
            LanguageAI = parameters.Language;
            string template = GetTemplate(parameters.Type, exampleExam != null);
            template = template
                .Replace(PromptTemplates.DifficultyPlaceholder, parameters.Difficulty.ToString())
                .Replace(PromptTemplates.MinimumQuestionsPlaceholder, parameters.QuestionCount.ToString())
                .Replace(PromptTemplates.LanguagePlaceholder, parameters.Language)
                .Replace(PromptTemplates.SourceDocument, sourceDocument)
                .Replace(PromptTemplates.ContentDomainPlaceholder, parameters.ContentDomain)
                .Replace(PromptTemplates.ExampleExam, exampleExam ?? string.Empty);
            return template;
        }

        private static string GetTemplate(string type, bool hasExample)
        {
            switch (type)
            {
                case "Yes No Questions":
                    return PromptTemplates.YesNoTemplate;
                case "Regular Multiple Choice":
                    return hasExample ? PromptTemplates.OptionsTemplateTwoPDF : PromptTemplates.OptionsTemplateOnePDF;
                case "Math Multiple Choice":
                    return hasExample ? PromptTemplates.MathOptionsTemplateTwoPDF : PromptTemplates.MathOptionsTemplateOnePDF;
                case "Programming Multiple Choice":
                    return hasExample ? PromptTemplates.ProgrammingOptionsTemplateTwoPDF : PromptTemplates.ProgrammingOptionsTemplateOnePDF;
                default:
                    throw new ArgumentException("Invalid question type");
            }
        }
    }
}