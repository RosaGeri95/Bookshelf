using Bookshelf.Auth;
using Bookshelf.Models;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Bookshelf.ViewModels
{
	public class PlaceholderViewModel : ViewModelBase
	{
        public DelegateCommand ListCommand { get; private set; }

        private ObservableCollection<string> _shelves;
        public ObservableCollection<string> Shelves
        {
            get { return _shelves; }
            set { SetProperty(ref _shelves, value); }
        }

        private string _retval;
        public string Retval
        {
            get { return _retval; }
            set { SetProperty(ref _retval, value); }
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

        public PlaceholderViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            Title = "Books";
            ListCommand = new DelegateCommand(ListBooks);
            Shelves = new ObservableCollection<string>();
        }

        private async void ListBooks()
        {
            Shelves.Clear();

            var shelf = await WebClient.Client.ListShelvesAsync();
            ObservableCollection<string> temp = new ObservableCollection<string>(shelf);
            foreach (string s in temp)
            {
                Shelves.Add(s);
            }

            //Retval = await WebClient.Client.ListBooksAsync();
            //Retval = AuthorizationService.Instance.ListedBooks;
            /* var listedBooks = await WebClient.Client.ListBooksAsync();


             ObservableCollection<Book> temp = new ObservableCollection<Book>(listedBooks);
             foreach (Book b in temp)
             {
                 Books.Add(b);
             }*/
        }

        private async void Select()
        {
            var parameter = new NavigationParameters();
            parameter.Add("item", SelectedItem);

            await NavigationService.NavigateAsync("./DetailsPage", parameter);
        }
    }
}
