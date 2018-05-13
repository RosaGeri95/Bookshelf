using Bookshelf.Models;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Bookshelf.ViewModels
{
	public class AddToShelfPopupPageViewModel : ViewModelBase
    {
        private IPageDialogService _dialogService;
        public DelegateCommand AddCommand { get; set; }
        public DelegateCommand CancelCommand { get; set; }

        private List<string> _vmShelves;
        public List<string> VmShelves
        {
            get { return _vmShelves; }
            set { SetProperty(ref _vmShelves, value); }
        }

        private string _bookId;
        public string BookId
        {
            get { return _bookId; }
            set { SetProperty(ref _bookId, value); }
        }

        private string _selection;
        public string Selection
        {
            get { return _selection; }
            set { SetProperty(ref _selection, value); }
        }

        public AddToShelfPopupPageViewModel(INavigationService navigationService, IPageDialogService service)
            : base(navigationService)
        {
            _dialogService = service;
            AddCommand = new DelegateCommand(Add);
            CancelCommand = new DelegateCommand(Cancel);
        }
        
        private async void Add()
        {
            await WebClient.Client.AddBookToShelfAsync(Int32.Parse(BookId), Selection);
            await _dialogService.DisplayAlertAsync("Success", "Book added to shelf", "OK");
            await PopupNavigation.Instance.PopAsync(true);
        }

        private async void Cancel()
        {
            await PopupNavigation.Instance.PopAsync(true);
        }
    }
}
