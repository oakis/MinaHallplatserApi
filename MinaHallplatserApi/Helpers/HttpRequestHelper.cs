using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace MinaHallplatserApi.Helpers
{
    public static class HttpRequestHelper
    {
        public static void ThrowIfNotOk(HttpResponseMessage response)
        {
            if (response.StatusCode != HttpStatusCode.OK)
            {
                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    throw new UnauthorizedAccessException();
                }
                else
                {
                    throw new HttpRequestException();
                }
            }
        }
    }
}
