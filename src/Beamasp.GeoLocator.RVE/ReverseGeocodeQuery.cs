namespace Beamasp.GeoLocator.RVE
{
    public class ReverseGeocodeQuery
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public int? Limit { get; set; }
        public int? Radius { get; set; }
        public bool? WideSearch { get; set; }

        protected bool Equals(ReverseGeocodeQuery other)
        {
            return Latitude.Equals(other.Latitude) &&
                   Longitude.Equals(other.Longitude) &&
                   Limit == other.Limit &&
                   Radius == other.Radius &&
                   WideSearch.Equals(other.WideSearch);
        }

        public override bool Equals(object obj)
        {
            return !(obj is null) && (ReferenceEquals(this, obj) || (obj.GetType() == GetType() && Equals((ReverseGeocodeQuery)obj)));
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = Latitude.GetHashCode();
                hashCode = (hashCode * 397) ^ Longitude.GetHashCode();
                hashCode = (hashCode * 397) ^ Limit.GetHashCode();
                hashCode = (hashCode * 397) ^ Radius.GetHashCode();
                hashCode = (hashCode * 397) ^ WideSearch.GetHashCode();
                return hashCode;
            }
        }
    }
}