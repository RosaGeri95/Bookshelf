using Bookshelf.Models;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;

namespace Bookshelf.Views
{
    public partial class ManageReviewPopupPage : PopupPage
    {
        public ManageReviewPopupPage(Review review, int bookId)
        {
            InitializeComponent();

            idHolder.Text = review.ID.ToString();
            idHolder2.Text = bookId.ToString();
            slider.Value = review.Rating;
            editor.Text = review.Comment;
        }
    }
}
