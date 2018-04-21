using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Bookshelf.ViewModels
{
	public class ShelvesPageViewModel : ViewModelBase
	{
        public DelegateCommand SelectCommand { get; private set; }

        private ObservableCollection<string> _shelves;
        public ObservableCollection<string> Shelves
        {
            get { return _shelves; }
            set { SetProperty(ref _shelves, value); }
        }

        private string _selectedItem;
        public string SelectedItem
        {
            get { return _selectedItem; }

            set
            {
                _selectedItem = value;

                Select();
            }
        }

        public ShelvesPageViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            Title = "Shelves";
            SelectCommand = new DelegateCommand(Select);
            Shelves = new ObservableCollection<string>();
        }

        private async void Select()
        {
            var parameter = new NavigationParameters();
            parameter.Add("item", SelectedItem);

            await NavigationService.NavigateAsync("./BooksPage", parameter);
        }

        public override async void OnNavigatingTo(NavigationParameters parameters)
        {
            base.OnNavigatingTo(parameters);

            Shelves.Clear();

            var shelf = await WebClient.Client.ListShelvesAsync();
            ObservableCollection<string> temp = new ObservableCollection<string>(shelf);
            foreach (string s in temp)
            {
                Shelves.Add(s);
            }
        }
    }
}

