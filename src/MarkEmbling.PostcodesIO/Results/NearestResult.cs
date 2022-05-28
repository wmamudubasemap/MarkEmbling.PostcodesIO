using System;

namespace Beamasp.PostcodesIO.Results
{
    [Serializable]
    public class NearestResult : PostcodeResult
    {
        public double Distance { get; set; }
    }
}
