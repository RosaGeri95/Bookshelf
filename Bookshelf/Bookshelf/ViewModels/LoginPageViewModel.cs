using Bookshelf.Auth;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Auth;
using Xamarin.Forms;

namespace Bookshelf.ViewModels
{
	public class LoginPageViewModel : ViewModelBase
	{
        public DelegateCommand LoginCommand { get; private set; }

        public LoginPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            Title = "Login Page";
            LoginCommand = new DelegateCommand(Login);
        }

        private void Login()
        {
            StringBuilder sb = new StringBuilder();

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
           

            auth.Completed += (sender, eventArgs) =>
            {
                if (eventArgs.IsAuthenticated)
                {

                    if (eventArgs.Account != null && eventArgs.Account.Properties != null)
                    {
                        sb.Append("Token = ").AppendLine($"{eventArgs.Account.Properties["oauth_token"]}");
                    }
                    else
                    {
                        sb.Append("Not authenticated ").AppendLine($"Account.Properties does not exist");
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

            var parameter = new NavigationParameters();
            parameter.Add("token", sb.ToString());
            NavigationService.NavigateAsync("../MainPage", parameter);
        }
        
    }
}
