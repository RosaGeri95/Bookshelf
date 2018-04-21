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

        public BookshelfMasterDetailPageViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            Title = "Bookshelf";
            NavigateCommand = new DelegateCommand<string>(Navigate);
        }

        private async void Navigate(string obj)
        {
            await NavigationService.NavigateAsync(obj);
        }
    }
}
