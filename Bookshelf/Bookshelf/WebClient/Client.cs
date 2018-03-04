using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Bookshelf.WebClient
{
    class Client
    {
        public async static Task<string> SearchBooks()
        {
            HttpClient client = new HttpClient();

            var uri = new Uri("url");

            var response = await client.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return content;
            }
            return "0";
        }
    }
}
