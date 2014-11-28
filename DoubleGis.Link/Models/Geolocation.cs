namespace DoubleGis.Link.Models
{
	public class Geolocation
	{
		public string Ip { get; set; }
		public string Country { get; set; }
		public string City { get; set; }
		public double Lat { get; set; }
		public double Lon { get; set; }
		public short Confidence { get; set; }
	}
}