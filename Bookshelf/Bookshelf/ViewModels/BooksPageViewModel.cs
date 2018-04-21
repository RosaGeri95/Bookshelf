using Bookshelf.Models;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Bookshelf.ViewModels
{
	public class BooksPageViewModel : ViewModelBase
	{
        private ObservableCollection<Book> _books;
        public ObservableCollection<Book> Books
        {
            get { return _books; }
            set { SetProperty(ref _books, value); }
        }

        public BooksPageViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            Books = new ObservableCollection<Book>();
        }

        public override async void OnNavigatingTo(NavigationParameters parameters)
        {
            base.OnNavigatingTo(parameters);

            Title = (string)parameters["item"];
            Books.Clear();

            var result = await WebClient.Client.ListBooksAsync((string)parameters["item"]);

            ObservableCollection<Book> temp = new ObservableCollection<Book>(result);

            foreach (Book b in temp)
            {
                Books.Add(b);
            }
        }
    }
}
