using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using DoubleGis.Link.Models;
using MelissaData;

namespace DoubleGis.Link.Providers
{
	public class GeolocationProvider
	{
		private readonly EsClient _esClient;

		public GeolocationProvider(EsClient esClient)
		{
			_esClient = esClient;
		}

		public IEnumerable<Geolocation> GetLocationSorted(string ip)
		{
			var locations = _esClient.FindGeolocationSorted(ip);
			if (locations.Any())
			{
				return locations;
			}

			var client = GetIpCheckClient();
			foreach (var loc in client.SuggestIPAddresses(ip, 5, 0.7).Execute())
			{
				_esClient.IndexGeolocation(new Geolocation
				{
					Country = loc.Country,
					City = loc.City,
					Ip = ip,
					Lat = loc.Latitude,
					Lon = loc.Longitude,
					Confidence = loc.Confidence
				});
			}

			return _esClient.FindGeolocationSorted(ip);
		}

		#region Private

		private static IPCheckContainer GetIpCheckClient()
		{
			var client = new IPCheckContainer(new Uri("https://api.datamarket.azure.com/Data.ashx/MelissaData/IPCheck/v1/"));
			client.Credentials = new NetworkCredential("accountKey", ConfigurationManager.AppSettings["ipCheckKey"]);
			return client;
		}

		#endregion

	}
}