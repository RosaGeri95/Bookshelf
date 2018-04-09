using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bookshelf.ViewModels
{
	public class BooksPageViewModel : ViewModelBase
	{
        public BooksPageViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            Title = "Books";
        }
	}
}
