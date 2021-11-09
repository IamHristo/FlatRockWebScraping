using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebScraping
{
    class HtmlParser
    {
        /// <summary>
        /// Gets or sets value indicating data list.
        /// </summary>
        public List<Data> DataList { get; set; }

        /// <summary>
        /// Method that parse html string to <see cref="Data"/> object.
        /// </summary>
        /// <param name="htmlData">Html as string.</param>
        public void ParseHtml(string htmlData)
        {
            DataList = new List<Data>();
            Data data;
            HtmlDocument htmlDocument = new HtmlAgilityPack.HtmlDocument();
            htmlDocument.LoadHtml(htmlData);

            HtmlNodeCollection firstImageInFigure = htmlDocument.DocumentNode.SelectNodes("//figure/a/img[1]");

            string ClassToGet = "price-display formatted";
            HtmlNodeCollection firstSpanInPriceClass = htmlDocument.DocumentNode.SelectNodes("//span[@class='" + ClassToGet + "']/span[1]");

            ClassToGet = "item";
            HtmlNodeCollection items = htmlDocument.DocumentNode.SelectNodes("//div[@class='" + ClassToGet + "']");

            for(int i=0; i < items.Count;i++)
            {
                data = new Data();
                decimal rating = RemoveZeroFromDecimal(items[i].Attributes["rating"].Value.Replace(".", ","));
                data.Rating = (rating > 5 ? rating / 2 : rating);
                data.Price = firstSpanInPriceClass[i].InnerText.Replace("$", "").Replace(",", "").Replace(".", ",");
                data.ProductName = System.Net.WebUtility.HtmlDecode(firstImageInFigure[i].Attributes["alt"].Value.Replace("\t", "  "));
                DataList.Add(data);
            }
        }

        /// <summary>
        /// If we keep data in decimal format we can not format it like 3, 3.6 and 4.2
        /// We must convert it to string and use string format.
        /// The rating should be decimal?
        /// </summary>
        /// <returns>Same ...</returns>
        private decimal RemoveZeroFromDecimal(string dec)
        {
            string decimalWithoutTrailingZeros = decimal.Parse(dec).ToString("G29");

            return Convert.ToDecimal(decimalWithoutTrailingZeros);
        }
    }
}
