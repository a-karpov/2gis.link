using System;
using System.Collections.Generic;
using DoubleGis.Link.Models;
using Nest;

namespace DoubleGis.Link.Providers
{
	public class IndexStorage
	{
		public IEnumerable<Geolocation> FindGeolocation(string ip)
		{
			var node = new Uri("http://23.101.8.162:9200/");
			var settings = new ConnectionSettings(node, "2gislink");
			var client = new ElasticClient(settings);

			var found = client.Search<Geolocation>(s => s.Size(1).Query(q => q.Term(g => g.Ip, ip)).Sort(sort => sort.OnField(g => g.Confidence).Descending()));
			return found.Documents;
		}

		public void IndexGeolocation(Geolocation geolocation)
		{
			var node = new Uri("http://23.101.8.162:9200/");
			var settings = new ConnectionSettings(node, "2gislink");
			var client = new ElasticClient(settings);

			client.Index(geolocation);
		}
	}
}