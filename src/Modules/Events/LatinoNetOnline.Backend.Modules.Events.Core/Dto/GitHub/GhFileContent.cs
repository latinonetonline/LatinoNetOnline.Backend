namespace LatinoNetOnline.Backend.Modules.Events.Core.Dto.GitHub
{
    public class GhFileContent
    {
        public GhFileContent(string name, string downloadUrl)
        {
            Name = name;
            DownloadUrl = downloadUrl;
        }

        public GhFileContent(string name, string content, string downloadUrl)
        {
            Name = name;
            Content = content;
            DownloadUrl = downloadUrl;
        }

        public string Name { get; set; }
        public string? Content { get; set; }
        public string DownloadUrl { get; set; }
    }
}
