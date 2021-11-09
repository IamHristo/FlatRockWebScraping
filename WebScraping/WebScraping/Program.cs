using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;

namespace WebScraping
{
    class Program
    {

        static void Main(string[] args)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "WebScraping.index.txt";
            string html = "";
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                html = reader.ReadToEnd();
            }

            HtmlParser htmlParser = new HtmlParser();
            htmlParser.ParseHtml(html);

            string output = JsonConvert.SerializeObject(htmlParser.DataList, Formatting.Indented);
            Console.WriteLine(output);
        }
    }
}
