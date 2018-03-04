using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bookshelf.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        private IPageDialogService _pageDialog;
        public DelegateCommand SearchCommand { get; private set; }
        public MainPageViewModel(INavigationService navigationService, IPageDialogService PageDialog) 
            : base (navigationService)
        {
            Title = "Main Page";
            _pageDialog = PageDialog;
            SearchCommand = new DelegateCommand(Search);
        }

        private void Search()
        {
            _pageDialog.DisplayAlertAsync("Book search", "Not implemented yet", "Ok");
        }
    }
}
