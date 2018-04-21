using Bookshelf.Auth;
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
        public async static Task<List<Book>> SearchBooks(string searchString)
        {
            UriBuilder builder = new UriBuilder("https://www.goodreads.com/search/index.xml");
            var query = HttpUtility.ParseQueryString(builder.Query);
            query["key"] = "K7gUv8myuMHUFxeNnDjfDQ";
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

                foreach(XElement element in doc.Descendants("work"))
                {
                    var year = Int32.Parse( element.Element("original_publication_year").Value);
                    var rating = Double.Parse( element.Element("average_rating").Value );

                    XElement b = element.Element("best_book");
                    var title = b.Element("title").Value;
                    XElement author = b.Element("author");
                    var name = author.Element("name").Value;

                    var sImg = b.Element("small_image_url").Value;
                    var img = b.Element("image_url").Value;

                    var book = new Book {
                        BookTitle = title,
                        Author = name,
                        Rating = rating,
                        PublicationYear = year,
                        SmallImageURL = sImg,
                        ImageURL = img
                    };

                    books.Add(book);
                }
                return books;
            }
            return new List<Book>();
        }

        public async static Task<GrUser> GetUserInfoAsync()
        {
            UriBuilder builder = new UriBuilder("https://www.goodreads.com/user/show/" + AuthorizationService.Instance.UserID.ToString() + ".xml");
            var query = HttpUtility.ParseQueryString(builder.Query);
            query["key"] = "K7gUv8myuMHUFxeNnDjfDQ";
            builder.Query = query.ToString();
            string url = builder.ToString();

            HttpClient client = new HttpClient();

            var response = await client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var doc = XDocument.Parse(content);
                XElement user = doc.Element("GoodreadsResponse").Element("user");

                var name = user.Element("name").Value;
                var image = user.Element("image_url").Value;
                var joined = user.Element("joined").Value;
                var friends = Int32.Parse(user.Element("friends_count").Value);
                var shelves = user.Descendants("user_shelf").Count();

                var gruser = new GrUser
                {
                    Name = name,
                    Image = image,
                    Joined = joined,
                    NumberOfFriends = friends,
                    NumberOfShelves = shelves
                };
                return gruser;
            }
            return new GrUser();
        }
    }
}
