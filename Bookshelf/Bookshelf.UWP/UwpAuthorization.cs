using Bookshelf.Auth;
using Bookshelf.UWP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Auth;
using Xamarin.Forms;
using Xamarin.Forms.Platform.UWP;

[assembly:Dependency(typeof(UwpAuthorization))]
namespace Bookshelf.UWP
{
    public class UwpAuthorization : IAuthorization
    {
        public void Authorize(OAuth1Authenticator auth)
        {
           /* Windows.UI.Xaml.Controls.Frame frame = Windows.UI.Xaml.Window.Current.Content as Windows.UI.Xaml.Controls.Frame;
            frame.Navigate(auth.GetUI(), auth);
            Windows.UI.Xaml.Window.Current.Content = frame;
            Windows.UI.Xaml.Window.Current.Activate();*/
        }
    }
}
