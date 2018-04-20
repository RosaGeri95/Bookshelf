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
	public class ProfilePageViewModel : ViewModelBase
	{
        IPageDialogService pageDialogService;

        public DelegateCommand ChangeCommand { get; private set; }

        public ProfilePageViewModel(INavigationService navigationService, IPageDialogService pageDialog)
            : base(navigationService)
        {
            Title = "Profile";
            pageDialogService = pageDialog;
            ChangeCommand = new DelegateCommand(Change);
        }

        private void Change()
        {
            AuthorizationService authservice = AuthorizationService.Instance;
            pageDialogService.DisplayAlertAsync("Token", authservice.Token, "Ok");
        }
    }
}
