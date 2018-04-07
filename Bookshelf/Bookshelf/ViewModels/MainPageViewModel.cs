using Bookshelf.Models;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookshelf.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        public DelegateCommand SearchCommand { get; private set; }

        private string _tempToken;
        public string TempToken
        {
            get { return _tempToken; }
            set { SetProperty(ref _tempToken, value); }
        }

        private ObservableCollection<Book> _books;
        public ObservableCollection<Book> Books
        {
            get { return _books; }
            set { SetProperty(ref _books, value); }
        }

        private Book _selectedItem;
        public Book SelectedItem
        {
            get { return _selectedItem; }

            set
            {
                _selectedItem = value;

                Select();
            }
        }

        private string _searchText;
        public string SearchText
        {
            get { return _searchText; }
            set { SetProperty(ref _searchText, value); }
        }
        public MainPageViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            Title = "Main Page";
            SearchCommand = new DelegateCommand(Search);
            Books = new ObservableCollection<Book>();
        }

        private void Select()
        {
            var parameter = new NavigationParameters();
            parameter.Add("item", SelectedItem);

            NavigationService.NavigateAsync("DetailsPage", parameter);
        }

        private async void Search()
        {
            Books.Clear();
            if (SearchText != "")
            {
                SearchText.Trim().Replace(' ', '+');
                var searchedBooks = await WebClient.Client.SearchBooks(SearchText);

                ObservableCollection<Book> temp = new ObservableCollection<Book>(searchedBooks);
                foreach (Book b in temp)
                {
                    Books.Add(b);
                }
            }
        }

        public override void OnNavigatedTo(NavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            TempToken = (string)parameters["token"];
            _selectedItem = null;
        }
    }
}