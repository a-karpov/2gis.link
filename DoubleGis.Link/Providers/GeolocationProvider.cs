using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using MelissaData;

namespace DoubleGis.Link.Providers
{
	public class GeolocationProvider
	{
		public IEnumerable<IPCheckEntity> GetLocation()
		{
			var client = new IPCheckContainer(new Uri("https://api.datamarket.azure.com/Data.ashx/MelissaData/IPCheck/v1/"));
			
			client.Credentials = new NetworkCredential("accountKey", ConfigurationManager.AppSettings["ipCheckKey"]);
			return  client.SuggestIPAddresses("216.231.3.166",5,0.7).Execute();
		}
	}
}