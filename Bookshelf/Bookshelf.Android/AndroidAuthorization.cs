using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Bookshelf.Auth;
using Bookshelf.Droid;
using Xamarin.Auth;
using Xamarin.Forms;

[assembly: Dependency(typeof(AndroidAuthorization))]
namespace Bookshelf.Droid
{
    public class AndroidAuthorization : IAuthorization
    {
        public void Authorize(OAuth1Authenticator auth)
        {
            Intent intent = auth.GetUI((Activity)Forms.Context);
            Forms.Context.StartActivity(intent);
        }
    }
}