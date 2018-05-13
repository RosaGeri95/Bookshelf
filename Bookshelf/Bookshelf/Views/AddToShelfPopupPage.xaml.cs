using Bookshelf.Models;
using Rg.Plugins.Popup.Pages;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Bookshelf.Views
{
    public partial class AddToShelfPopupPage : PopupPage
    {
        public List<string> Shelves { get; set; }
        public int ID { get; set; }

        public AddToShelfPopupPage( List<string> shelves, int id)
        {
            InitializeComponent();

            Shelves = shelves;
            picker.ItemsSource = Shelves;
            picker.SelectedIndex = 0;

            ID = id;
            idHolder.Text = ID.ToString();
        }
    }
}
