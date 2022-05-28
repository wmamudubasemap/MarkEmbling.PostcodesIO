﻿using System;

namespace Beamasp.PostcodesIO.Exceptions
{
    public class PostcodesIOApiException : Exception
    {
        public PostcodesIOApiException(Exception innerException)
            : base("Error retrieving response. Please check inner exception for details.", innerException) { }
    }
}