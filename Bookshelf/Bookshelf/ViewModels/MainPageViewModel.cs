using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookshelf.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        private IPageDialogService _pageDialog;
        public DelegateCommand SearchCommand { get; private set; }

        private string _temporary;
        public string Temporary
        {
            get { return _temporary; }
            set { SetProperty(ref _temporary, value); }
        }
        public MainPageViewModel(INavigationService navigationService, IPageDialogService PageDialog) 
            : base (navigationService)
        {
            Title = "Main Page";
            _pageDialog = PageDialog;
            SearchCommand = new DelegateCommand(Search);
            Temporary = "-1";
        }

        private async void Search()
        {
            Temporary = await WebClient.Client.SearchBooks();
        }
    }
}
