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
using Xamarin.Auth;

namespace Bookshelf.WebClient
{
    class Client
    {
        public async static Task<List<Book>> SearchBooksAsync(string searchString)
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


        public async static Task<List<Book>> ListBooksAsync(string shelf)
        {
            string uri = "https://www.goodreads.com/review/list?id=" + AuthorizationService.Instance.UserID.ToString() 
                + "&v=2&key=K7gUv8myuMHUFxeNnDjfDQ&shelf=" + shelf;

            var request = new OAuth1Request("GET",
                              new Uri(uri),
                              null,
                              AuthorizationService.Instance.CurrentUser);

            var response = await request.GetResponseAsync();
            if (response != null)
            {
                var content = response.GetResponseText();
                var doc = XDocument.Parse(content);

                var books = new List<Book>();

                foreach (XElement element in doc.Descendants("book"))
                {
                    //var year = Int32.Parse(element.Element("original_publication_year").Value);
                    //var rating = Double.Parse(element.Element("average_rating").Value);

                    var title = element.Element("title").Value;
                    XElement author = element.Element("authors").Element("author");
                    var name = author.Element("name").Value;

                    var sImg = element.Element("small_image_url").Value;
                    var img = element.Element("image_url").Value;

                    var book = new Book
                    {
                        BookTitle = title,
                        Author = name,
                       // Rating = rating,
                       // PublicationYear = year,
                        SmallImageURL = sImg,
                        ImageURL = img
                    };

                    books.Add(book);
                }
                return books; 
            }
            return new List<Book>();
        }

        public async static Task<List<string>> ListShelvesAsync()
        {
            var dict = new Dictionary<string, string>();
            dict.Add("user_id", AuthorizationService.Instance.UserID.ToString());

            var request = new OAuth1Request("GET",
                              new Uri("https://www.goodreads.com/shelf/list.xml"),
                              dict,
                              AuthorizationService.Instance.CurrentUser);

            var response = await request.GetResponseAsync();
            if (response != null)
            {
                var data = response.GetResponseText();
                var doc = XDocument.Parse(data);
                var list = new List<string>();

                foreach(XElement e in doc.Descendants("user_shelf"))
                {
                    list.Add(e.Element("name").Value);
                }
                return list;
            }
            return new List<string>();
        }
    }
}
