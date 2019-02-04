using System.IO;
using System.Text;

namespace CRISPR.Anotations.Helpers.Zipping
{
    public class ZipItem
    {
        public string Name { get; set; }
        public Stream Content { get; set; }

        public ZipItem(string name, Stream content)
        {
            Name = name;
            Content = content;
        }

        public ZipItem(string name, string contentStr, Encoding encoding)
        {
            var byteArray = encoding.GetBytes(contentStr);
            var memoryStream = new MemoryStream(byteArray);

            Name = name;
            Content = memoryStream;
        }
    }
}