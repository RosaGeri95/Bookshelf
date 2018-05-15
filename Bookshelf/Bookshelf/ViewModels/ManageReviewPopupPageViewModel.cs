using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Bookshelf.ViewModels
{
	public class ManageReviewPopupPageViewModel : ViewModelBase
	{
        public DelegateCommand OkCommand { get; set; }
        public DelegateCommand CancelCommand { get; set; }

        private string _reviewId;
        public string ReviewId
        {
            get { return _reviewId; }
            set { SetProperty(ref _reviewId, value); }
        }

        private string _bookId;
        public string BookId
        {
            get { return _bookId; }
            set { SetProperty(ref _bookId, value); }
        }
        private int _reviewRating;
        public int ReviewRating
        {
            get { return _reviewRating; }
            set { SetProperty(ref _reviewRating, value); }
        }

        private string _reviewComment;
        public string ReviewComment
        {
            get { return _reviewComment; }
            set { SetProperty(ref _reviewComment, value); }
        }


        public ManageReviewPopupPageViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            OkCommand = new DelegateCommand(Accept);
            CancelCommand = new DelegateCommand(Cancel);
        }

        private async void Cancel()
        {
            await PopupNavigation.Instance.PopAsync(true);
        }

        private async void Accept()
        {
            if(ReviewId == "0")
            {
                await WebClient.Client.AddReviewAsync(BookId, ReviewRating, ReviewComment);
            }
            else
            {
                await WebClient.Client.EditReviewAsync(ReviewId, ReviewRating, ReviewComment);
            }
            await PopupNavigation.Instance.PopAsync(true);
        }
    }
}
