using Bookshelf.Auth;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bookshelf.ViewModels
{
	public class BookshelfMasterDetailPageViewModel : ViewModelBase
	{
        private IPageDialogService _dialogService;
        public DelegateCommand<string> NavigateCommand { get; set; }

        public BookshelfMasterDetailPageViewModel(INavigationService navigationService, IPageDialogService service)
            : base(navigationService)
        {
            Title = "Bookshelf";
            NavigateCommand = new DelegateCommand<string>(Navigate);

            _dialogService = service;
        }

        private async void Navigate(string obj)
        {
            if (obj == "NavigationPage/ShelvesPage" && AuthorizationService.Instance.CurrentUser == null)
            {
                await _dialogService.DisplayAlertAsync("Error", "You need to login in order to navigate to the requested page", "OK");
                return;
            }
            else if( obj == "NavigationPage/ProfilePage" && AuthorizationService.Instance.CurrentUser == null)
            {
                await _dialogService.DisplayAlertAsync("Warning", "You are not logged in", "OK");
            }

            await NavigationService.NavigateAsync(obj);
        }
    }
}
