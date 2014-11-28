using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using DoubleGis.Link.Models;
using Newtonsoft.Json;

namespace DoubleGis.Link.Providers
{
	public class ApiClient
	{
		private readonly AppSettingsProvider _appSettings;

		public ApiClient(AppSettingsProvider appSettings)
		{
			_appSettings = appSettings;
		}

		public SearchResponse Search(string what, string where, int page)
		{
			var request = new UriBuilder("http://catalog.api.2gis.ru/search");
			request.Query = ToQueryString(new NameValueCollection()
			{
				{"key", _appSettings.ApiKey},
				{"version", _appSettings.ApiVersion},
				{"what", what},
				{"where", where},
				{"pagesize", _appSettings.PageSize.ToString()},
				{"page", page.ToString()},
			});

			using (var client = new WebClient())
			{
				var response = client.DownloadData(request.Uri);
				return JsonConvert.DeserializeObject<SearchResponse>(Encoding.UTF8.GetString(response));
			}
		}

		public IReadOnlyCollection<ProfileResponse> LoadProfiles(IEnumerable<SearchResultElem> resultElems)
		{
			var result = new List<ProfileResponse>();
			var request = "http://catalog.api.2gis.ru/profile?" + ToQueryString(new NameValueCollection
			{
				{"key", _appSettings.ApiKey},
				{"version", _appSettings.ApiVersion},
			});
		
			using (var client = new WebClient())
			{
				foreach (var elem in resultElems)
				{
					var data = client.DownloadData(request + "&" + ToQueryString(new NameValueCollection
					{
						{ "id", elem.Id },
						{ "hash", elem.Hash }
					}));

					var profile = JsonConvert.DeserializeObject<ProfileResponse>(Encoding.UTF8.GetString(data));
					result.Add(profile);
				}
			}

			return result.AsReadOnly();
		}

		#region Private

		private string ToQueryString(NameValueCollection nvc)
		{
			var array = (from key in nvc.AllKeys
						 from value in nvc.GetValues(key)
						 select string.Format("{0}={1}", HttpUtility.UrlEncode(key), HttpUtility.UrlEncode(value)))
						 .ToArray();

			return string.Join("&", array);
		}

		#endregion

	}
}