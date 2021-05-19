namespace LatinoNetOnline.Backend.Modules.Links.Core.Entities
{
    public class Link
    {
        public Link()
        {
        }

        public Link(string name, string url)
        {
            Name = name;
            Url = url;
        }

        public string Name { get; set; }
        public string Url { get; set; }
    }
}
