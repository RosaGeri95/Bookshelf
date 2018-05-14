using Bookshelf.Models;
using Bookshelf.Views;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bookshelf.ViewModels
{
	public class DetailsPageViewModel : ViewModelBase
	{
        public DelegateCommand AddToShelfCommand { get; set; }
        public DelegateCommand ShowReviewsCommand { get; set; }
        public DelegateCommand ManageReviewCommand { get; set; }

        private Book _detailedBook;
        public Book DetailedBook
        {
            get { return _detailedBook; }
            set { SetProperty(ref _detailedBook, value); }
        }

        private string _reviewUrl;
        public string ReviewUrl
        {
            get { return _reviewUrl; }
            set { SetProperty(ref _reviewUrl, value); }
        }

        public DetailsPageViewModel(INavigationService navigationService) 
            : base (navigationService)
        {
            Title = "Details";
            AddToShelfCommand = new DelegateCommand(Add);
            ShowReviewsCommand = new DelegateCommand(Show);
            ManageReviewCommand = new DelegateCommand(ManageReview);
        }

        private async void ManageReview()
        {
            Review review = await WebClient.Client.GetUserReviewAsync(DetailedBook.ID);
            await PopupNavigation.Instance.PushAsync(new ManageReviewPopupPage(review, DetailedBook.ID));
        }

        private async void Show()
        {
            ReviewUrl = await WebClient.Client.GetReviewsAsync(DetailedBook.ID);
        }

        private async void Add()
        {
            var shelves = await WebClient.Client.ListShelvesAsync();
            await PopupNavigation.Instance.PushAsync(new AddToShelfPopupPage(shelves, DetailedBook.ID));
        }

        public override void OnNavigatedTo(NavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            DetailedBook = (Book) parameters["item"];
        }
    }
}
