﻿using System;
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
		private readonly ElasticStorage _elasticStorage;

		public GeolocationProvider(ElasticStorage elasticStorage)
		{
			_elasticStorage = elasticStorage;
		}

		public IEnumerable<Geolocation> GetLocationSorted(string ip)
		{
			var locations = _elasticStorage.FindGeolocationSorted(ip);
			if (locations.Any())
			{
				return locations;
			}

			var client = GetIpCheckClient();
			var result = new List<Geolocation>();

			foreach (var loc in client.SuggestIPAddresses(ip, 5, 0.7).Execute())
			{
				var geolocation = new Geolocation
				{
					Country = loc.Country,
					City = loc.City,
					Ip = ip,
					Lat = loc.Latitude,
					Lon = loc.Longitude,
					Confidence = loc.Confidence
				};

				result.Add(geolocation);
				_elasticStorage.IndexGeolocation(geolocation);
			}

			return result.AsReadOnly();
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