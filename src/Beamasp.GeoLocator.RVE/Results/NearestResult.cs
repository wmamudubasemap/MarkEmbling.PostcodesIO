using System;

namespace Beamasp.GeoLocator.RVE.Results
{
    [Serializable]
    public class NearestResult : PostcodeResult
    {
        public double Distance { get; set; }
    }
}
