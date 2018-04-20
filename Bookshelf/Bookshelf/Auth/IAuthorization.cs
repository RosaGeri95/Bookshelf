using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Auth;

namespace Bookshelf.Auth
{
    public interface IAuthorization
    {
        void Authorize(OAuth1Authenticator auth);
    }
}
