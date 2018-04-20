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
        public DelegateCommand<string> NavigateCommand { get; set; }


        IPageDialogService pageDialogService;

        public BookshelfMasterDetailPageViewModel(INavigationService navigationService, IPageDialogService pageDialog)
            : base(navigationService)
        {
            Title = "Master-Detail";
            pageDialogService = pageDialog;
            NavigateCommand = new DelegateCommand<string>(Navigate);
        }

        private async void Navigate(string obj)
        {
            await NavigationService.NavigateAsync(obj);
        }

        public override void OnNavigatedTo(NavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            pageDialogService.DisplayAlertAsync("Token", (string) parameters["token"], "Ok");
        }
    }
}
