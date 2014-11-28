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
		private readonly IndexStorage _indexStorage;

		public GeolocationProvider(IndexStorage indexStorage)
		{
			_indexStorage = indexStorage;
		}

		public IEnumerable<Geolocation> GetLocation(string ip)
		{
			var locations = _indexStorage.FindGeolocation(ip);

			if (locations.Any())
			{
				return locations;
			}

			var client = new IPCheckContainer(new Uri("https://api.datamarket.azure.com/Data.ashx/MelissaData/IPCheck/v1/"));
			client.Credentials = new NetworkCredential("accountKey", ConfigurationManager.AppSettings["ipCheckKey"]);

			locations =  client.SuggestIPAddresses(ip,5,0.7).Execute()
				.OrderByDescending(e => e.Confidence)
				.Select(e => new Geolocation
				{
					Country = e.Country,
					City = e.City,
					Ip = ip,
					Lat = e.Latitude,
					Lon = e.Longitude,
					Confidence = e.Confidence
				});

			foreach (var geolocation in locations)
			{
				_indexStorage.IndexGeolocation(geolocation);
			}

			return locations;
		}
	}
}