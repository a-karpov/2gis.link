using System;
using System.Collections.Generic;
using DoubleGis.Link.Models;
using Nest;

namespace DoubleGis.Link.Providers
{
	public class EsClient
	{
		private readonly AppSettingsProvider _settings;
		private const string IntexName = "2gislink";

		public EsClient(AppSettingsProvider settings)
		{
			_settings = settings;
		}

		public IEnumerable<Geolocation> FindGeolocationSorted(string ip)
		{
			var client = GetElasticClient();
			var found = client.Search<Geolocation>(s => s.Size(1).Query(q => q.Term(g => g.Ip, ip)).Sort(sort => sort.OnField(g => g.Confidence).Descending()));
			return found.Documents;
		}

		public void IndexGeolocation(Geolocation geolocation)
		{
			var client = GetElasticClient();
			client.Index(geolocation);
		}

		#region Private

		private ElasticClient GetElasticClient()
		{
			var node = new Uri(_settings.EsAddress);
			var settings = new ConnectionSettings(node, IntexName);
			return new ElasticClient(settings);;
		}

		#endregion

	}
}