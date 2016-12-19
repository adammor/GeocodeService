using System;
using System.Net.Http;

namespace RESTHelpers
{
    public static class RESTHelper
    {
        public static string CallProvider(string requestUri)
        {
            var _jsonText = "";
            using (HttpClient _client = new HttpClient())
            {
                string _str = Uri.EscapeUriString(requestUri);

                HttpResponseMessage _response = _client.GetAsync(new Uri(_str)).Result;   //Get 
                _jsonText = _response.Content.ReadAsStringAsync().Result;        //Response
            }

            return _jsonText;
        }

    }
}
