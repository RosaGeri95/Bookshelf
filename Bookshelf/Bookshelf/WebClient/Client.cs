﻿using Bookshelf.Auth;
using Bookshelf.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
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
                    int year;
                    if (element.Element("original_publication_year").Attribute("nil") == null)
                    {
                        year = Int32.Parse(element.Element("original_publication_year").Value);
                    }
                    else
                    {
                        year = -1;
                    }

                    var rating = Double.Parse( element.Element("average_rating").Value );

                    XElement b = element.Element("best_book");
                    var id = Int32.Parse(b.Element("id").Value);
                    var title = b.Element("title").Value;
                    XElement author = b.Element("author");
                    var name = author.Element("name").Value;

                    var sImg = b.Element("small_image_url").Value;
                    var img = b.Element("image_url").Value;

                    var book = new Book {
                        ID = id,
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


        public async static Task<List<Book>> ListBooksOfShelfAsync(string shelf)
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
                    var id = Int32.Parse(element.Element("id").Value);
                    int year;
                    if(  ! String.IsNullOrWhiteSpace(element.Element("publication_year").Value))
                    {
                        year = Int32.Parse(element.Element("publication_year").Value);
                    }
                    else
                    {
                        year = -1;
                    }
                    var rating = Double.Parse(element.Element("average_rating").Value);

                    var title = element.Element("title").Value;
                    XElement author = element.Element("authors").Element("author");
                    var name = author.Element("name").Value;

                    var sImg = element.Element("small_image_url").Value;
                    var img = element.Element("image_url").Value;

                    var book = new Book
                    {
                        ID = id,
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

        public async static Task AddNewShelf( string shelfName)
        {
            var dict = new Dictionary<string, string>();
            dict.Add("user_shelf[name]", shelfName);

            var request = new OAuth1Request("POST",
                              new Uri("https://www.goodreads.com/user_shelves.xml"),
                              dict,
                              AuthorizationService.Instance.CurrentUser);
            await request.GetResponseAsync();
        }

        public async static Task AddBookToShelfAsync(int id, string shelf)
        {
             var dict = new Dictionary<string, string>();
             dict.Add("book_id", id.ToString());
             dict.Add("name", shelf);

             var request = new OAuth1Request("POST",
                               new Uri("https://www.goodreads.com/shelf/add_to_shelf.xml"),
                               dict,
                               AuthorizationService.Instance.CurrentUser);

            await request.GetResponseAsync();
        }

        public async static Task<string> GetReviewsAsync(int id)
        {
            var dict = new Dictionary<string, string>();
            dict.Add("id", id.ToString());
            dict.Add("key", "K7gUv8myuMHUFxeNnDjfDQ");
            dict.Add("text_only", "true");

            var request = new OAuth1Request("GET",
                               new Uri("https://www.goodreads.com/book/show.xml"),
                               dict,
                               AuthorizationService.Instance.CurrentUser);

            var response = await request.GetResponseAsync();
            if (response != null)
            {
                var data = response.GetResponseText();
                var doc = XDocument.Parse(data);

                string cdata = doc.Element("GoodreadsResponse").Element("book").Element("reviews_widget").Value;

                /*Gets the url for the reviews that can be find in a string that contains ' src="<url here>" '
                This is a pretty bad solution, but the web api gave back a html structure ( in CDATA),
                which was also badly formatted as it had multiple roots*/
                var match = Regex.Match(cdata, "src=\"[^\"]*");
                string s = match.Value;

                //removes the first 5 character ( src=" ) to get the url
                return s.Substring(5);
               }
            return "";
        }


        public async static Task<Review> GetUserReviewAsync(int bookId)
        {
            var dict = new Dictionary<string, string>();
            dict.Add("user_id", AuthorizationService.Instance.UserID.ToString());
            dict.Add("book_id", bookId.ToString());
            dict.Add("key", "K7gUv8myuMHUFxeNnDjfDQ");

            var request = new OAuth1Request("GET",
                               new Uri("https://www.goodreads.com/review/show_by_user_and_book.xml"),
                               dict,
                               AuthorizationService.Instance.CurrentUser);

            var response = await request.GetResponseAsync();
            if (response != null)
            {
                var data = response.GetResponseText();
                var doc = XDocument.Parse(data);

                XElement root = doc.Element("GoodreadsResponse");

                if ( root.Value.Contains("review not found"))
                {
                    Review r =  new Review()
                    {
                        ID = 0,
                        Rating = 0,
                        Comment = ""
                    };
                    return r;
                }

                XElement review = root.Element("review");

                ulong id = UInt64.Parse(review.Element("id").Value);
                int rating = Int32.Parse(review.Element("rating").Value);
                string comment = review.Element("body").Value;

                Review rev = new Review()
                {
                    ID = id,
                    Rating = rating,
                    Comment = comment
                };
                return rev;
            }
            return new Review();
        }


        public async static Task EditReviewAsync(string reviewId, int rating, string comment)
        {
            var dict = new Dictionary<string, string>();
            dict.Add("review[rating]", rating.ToString());
            dict.Add("review[review]", comment.Trim());
            
            string uri = "https://www.goodreads.com/review/" + reviewId + ".xml";
            var request = new OAuth1Request("POST",
                              new Uri(uri),
                              dict,
                              AuthorizationService.Instance.CurrentUser);

            await request.GetResponseAsync();
        }

        public async static Task AddReviewAsync(string book_id, int rating, string comment)
        {
            var dict = new Dictionary<string, string>();
            dict.Add("book_id", book_id);
            dict.Add("review[rating]", rating.ToString());
            dict.Add("review[review]", comment);
            
            var request = new OAuth1Request("POST",
                              new Uri("https://www.goodreads.com/review.xml"),
                              dict,
                              AuthorizationService.Instance.CurrentUser);

            await request.GetResponseAsync();
        }
    }
}
