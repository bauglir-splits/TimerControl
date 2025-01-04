using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimerControl
{
    public static class RESTClient
    {
        public static readonly HttpClient Client = new HttpClient();

        public static HttpResponseMessage? Post(string url, IEnumerable<KeyValuePair<string, string?>> values)
        {
            var content = new FormUrlEncodedContent(values);
            var task = Task.Run(() => Client.PostAsync(url, content));
            task.Wait();
            return task.Result;
        }
    }
}
