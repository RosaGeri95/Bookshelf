﻿using Bookshelf.Auth;
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

        private async void Login()
        {
            var authService = AuthorizationService.Instance;
            authService.StartAuthorization();
            await NavigationService.NavigateAsync("../BookshelfMasterDetailPage");
        }
    }
}
