using Bookshelf.Models;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bookshelf.ViewModels
{
	public class DetailsPageViewModel : ViewModelBase
	{
        public DelegateCommand AddToShelfCommand { get; set; }

        private Book _detailedBook;
        public Book DetailedBook
        {
            get { return _detailedBook; }
            set { SetProperty(ref _detailedBook, value); }
        }

        public DetailsPageViewModel(INavigationService navigationService) 
            : base (navigationService)
        {
            Title = "Details";
            AddToShelfCommand = new DelegateCommand(Add);
        }

        private async void Add()
        {
            await WebClient.Client.AddBookToShelfAsync(DetailedBook.ID);
        }

        public override void OnNavigatedTo(NavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            DetailedBook = (Book) parameters["item"];
        }
    }
}
