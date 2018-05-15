using Bookshelf.Models;
using Rg.Plugins.Popup.Pages;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Bookshelf.Views
{
    public partial class AddToShelfPopupPage : PopupPage
    {
        public AddToShelfPopupPage( List<string> shelves, int id)
        {
            InitializeComponent();

            picker.ItemsSource = shelves;
            picker.SelectedIndex = 0;

            idHolder.Text = id.ToString();
        }
    }
}
