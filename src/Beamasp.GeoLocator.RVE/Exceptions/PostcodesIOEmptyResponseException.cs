using System;
using System.Net;

namespace Beamasp.GeoLocator.RVE.Exceptions
{
    public class PostcodesIOEmptyResponseException : Exception
    {
        public PostcodesIOEmptyResponseException(HttpStatusCode statusCode)
            : base(string.Format("No response was provided; HTTP status: {0}", (int)statusCode)) { }
    }
}