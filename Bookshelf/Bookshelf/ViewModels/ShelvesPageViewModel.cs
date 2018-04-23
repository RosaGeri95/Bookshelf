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
        public DelegateCommand AddCommand { get; private set; }

        private ObservableCollection<string> _shelves;
        public ObservableCollection<string> Shelves
        {
            get { return _shelves; }
            set { SetProperty(ref _shelves, value); }
        }

        private string _newShelf;
        public string NewShelf
        {
            get { return _newShelf; }
            set { SetProperty(ref _newShelf, value); }
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
            AddCommand = new DelegateCommand(Add);
            Shelves = new ObservableCollection<string>();
        }
        
        private async void Add()
        {
            if( NewShelf.Trim() != "")
            {
                await WebClient.Client.AddNewShelf(NewShelf);
                NewShelf = "";

                var shelf = await WebClient.Client.ListShelvesAsync();
                ObservableCollection<string> temp = new ObservableCollection<string>(shelf);
                Shelves.Clear();
                foreach (string s in temp)
                {
                    Shelves.Add(s);
                }
            }
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

            var shelf = await WebClient.Client.ListShelvesAsync();
            ObservableCollection<string> temp = new ObservableCollection<string>(shelf);
            Shelves.Clear();
            foreach (string s in temp)
            {
                Shelves.Add(s);
            }
        }
    }
}

