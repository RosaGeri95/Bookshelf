using Bookshelf.Auth;
using Bookshelf.Models;
using Bookshelf.Views;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bookshelf.ViewModels
{
	public class DetailsPageViewModel : ViewModelBase
	{
        private IPageDialogService _dialogService;

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

        public DetailsPageViewModel(INavigationService navigationService, IPageDialogService service) 
            : base (navigationService)
        {
            Title = "Details";
            AddToShelfCommand = new DelegateCommand(Add);
            ShowReviewsCommand = new DelegateCommand(Show);
            ManageReviewCommand = new DelegateCommand(ManageReview);

            _dialogService = service;
        }

        private async void ManageReview()
        {
            if (AuthorizationService.Instance.CurrentUser == null)
            {
                await _dialogService.DisplayAlertAsync("Error", "You need to login in order to navigate to the requested page", "OK");
                return;
            }

            Review review = await WebClient.Client.GetUserReviewAsync(DetailedBook.ID);
            await PopupNavigation.Instance.PushAsync(new ManageReviewPopupPage(review, DetailedBook.ID));
        }

        private async void Show()
        {
            if (AuthorizationService.Instance.CurrentUser == null)
            {
                await _dialogService.DisplayAlertAsync("Error", "You need to login in order to navigate to the requested page", "OK");
                return;
            }

            ReviewUrl = await WebClient.Client.GetReviewsAsync(DetailedBook.ID);
        }

        private async void Add()
        {
            if (AuthorizationService.Instance.CurrentUser == null)
            {
                await _dialogService.DisplayAlertAsync("Error", "You need to login in order to navigate to the requested page", "OK");
                return;
            }

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
