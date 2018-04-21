using Bookshelf.Auth;
using Bookshelf.Models;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bookshelf.ViewModels
{
	public class ProfilePageViewModel : ViewModelBase
	{
        private GrUser _user;
        public GrUser User
        {
            get { return _user; }

            set
            {
                SetProperty(ref _user, value);
            }
        }

        public ProfilePageViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            Title = "Profile";
        }


        public override async void OnNavigatingTo(NavigationParameters parameters)
        {
            base.OnNavigatingTo(parameters);

            User = await WebClient.Client.GetUserInfoAsync();
        }
    }
}
