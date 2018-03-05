using Bookshelf.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Bookshelf.WebClient
{
    class Client
    {
        public async static Task<string> SearchBooks(string searchString)
        {
            UriBuilder builder = new UriBuilder("https://www.goodreads.com/search/index.xml");
            var query = HttpUtility.ParseQueryString(builder.Query);
            query["key"] = "key";
            query["q"] = searchString;
            builder.Query = query.ToString();
            string url = builder.ToString();

            HttpClient client = new HttpClient();

            var response = await client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var doc = XDocument.Parse(content);

                var books = new List<Book>();

                foreach(XElement element in doc.Descendants("best_book"))
                {
                    var title = element.Element("title").Value;
                    XElement author = element.Element("author");
                    var name = author.Element("name").Value;
                    var book = new Book { Title = title, Author = name };

                    books.Add(book);
                }

                StringBuilder ret = new StringBuilder();
                foreach (var book in books)
                {
                    ret.Append(book.Author);
                    ret.Append(", ");
                    ret.Append(book.Title);
                    ret.Append("\n");

                }
                Console.WriteLine(ret.ToString());

                return ret.ToString();
            }
            return "0";
        }
    }
}
