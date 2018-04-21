using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Xamarin.Auth;
using Xamarin.Forms;

namespace Bookshelf.Auth
{
    public class AuthorizationService
    {
        private static readonly object locked = new object();
        private static AuthorizationService _instance;

        public int UserID { get; set; }
        public Account CurrentUser { get; set; }

        private AuthorizationService() { }

        public static AuthorizationService Instance
        {
            get
            {
                lock (locked)
                {
                    if (_instance == null)
                    {
                        _instance = new AuthorizationService();
                    }
                    return _instance;
                }
            }
        }

        public void StartAuthorization()
        {
            OAuth1Authenticator auth = new OAuth1Authenticator
            (
                consumerKey: "K7gUv8myuMHUFxeNnDjfDQ",
                consumerSecret: "5Gn7aJAMNYU8L6kcdUiic5debIFqw2m4RqwoQa9ys8",
                requestTokenUrl: new Uri("https://www.goodreads.com/oauth/request_token"),
                authorizeUrl: new Uri("https://www.goodreads.com/oauth/authorize"),
                accessTokenUrl: new Uri("https://www.goodreads.com/oauth/access_token"),
                callbackUrl: new Uri("http://127.0.0.1")
            );

            auth.AllowCancel = true;

            auth.Completed += async (sender, eventArgs) =>
            {
                if (eventArgs.IsAuthenticated)
                {

                    if (eventArgs.Account != null && eventArgs.Account.Properties != null)
                    {
                        CurrentUser = eventArgs.Account;

                        var request = new OAuth1Request("GET",
                                          new Uri("https://www.goodreads.com/api/auth_user"),
                                          null,
                                          CurrentUser);

                        var response = await request.GetResponseAsync();
                        if (response != null)
                        {
                            var xmlData = response.GetResponseText();
                            var doc = XDocument.Parse(xmlData);
                            UserID = Int32.Parse(doc.Element("GoodreadsResponse").Element("user").Attribute("id").Value);
                        }
                    }
                    else
                    {
                        Console.WriteLine("Account properties does not exists");
                    }

                }
                else
                {
                    Console.WriteLine("Cancelled");
                }
            };

            auth.Error += (s, ea) =>
            {
                Console.WriteLine("Cancelled");
            };

            DependencyService.Get<IAuthorization>().Authorize(auth);
        }
    }
}
