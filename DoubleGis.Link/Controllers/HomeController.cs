﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using DoubleGis.Link.Models;
using DoubleGis.Link.Providers;
using Newtonsoft.Json;

namespace DoubleGis.Link.Controllers
{
    public class HomeController : Controller
    {
	    private readonly string _apiKey = ConfigurationManager.AppSettings["apiKey"];
	    private const int Pagesize = 7;

        public ActionResult Index()
        {
	        return View();
        }

	    public ActionResult Search(string what, string where, int page = 1)
	    {
		using (var client = new WebClient())
			{
				var response = client.DownloadData(new Uri(string.Format("http://catalog.api.2gis.ru/search?key={0}&version=1.3&what={1}&where={2}&pagesize={3}&page={4}", _apiKey, what, where, Pagesize, page)));
				var searchResponse = JsonConvert.DeserializeObject<SearchResponse>(Encoding.UTF8.GetString(response));

				if (searchResponse.ResponseCode == 404)
				{
					return View("EmptyResult");
				}

				var cards = new List<ProfileResponse>();
				foreach (var cardLink in searchResponse.Result)
				{
					var data = client.DownloadData(new Uri(string.Format("http://catalog.api.2gis.ru/profile?key={0}&version=1.3&id={1}&hash={2}", _apiKey, cardLink.Id, cardLink.Hash)));
					var card = JsonConvert.DeserializeObject<ProfileResponse>(Encoding.UTF8.GetString(data));
					cards.Add(card);
				}

				return View(new SearchModel(searchResponse, cards){Page = page, HasNextPage = Pagesize * page < searchResponse.Total});
			}
	    }

	    public ActionResult Recognize(string query)
	    {

			string where = "";


		    var ip = ConfigurationManager.AppSettings["overrideIp"] ?? FindLanIpAddress(HttpContext.Request);

		    var geolocationProvider = new GeolocationProvider(new IndexStorage());
		    var location = geolocationProvider.GetLocationSorted(ip);

		  

		    if (location.Any())
		    {
			     where = location.First().City;
		    }

		    var queryParts = query.Split(',');
		    var what = queryParts.First();

		    if (queryParts.Length > 1)
		    {
			    where += query.Substring(what.Length);
		    }

		    if (string.IsNullOrEmpty(what))
		    {
			    throw new Exception("Can not Recognize where.");
		    }

			return Redirect(string.Format("~/{0}/{1}", what, where));
	    }

	    #region Private

	    private String FindLanIpAddress(HttpRequestBase request)
	    {
		    //The X-Forwarded-For (XFF) HTTP header field is a de facto standard for identifying the originating IP address of a 
		    //client connecting to a web server through an HTTP proxy or load balancer
		    String ip = request.ServerVariables["HTTP_X_FORWARDED_FOR"];

		    if (string.IsNullOrEmpty(ip))
		    {
			    ip = request.ServerVariables["REMOTE_ADDR"];
		    }

		    return ip.Split(':').First();
	    }

	    #endregion

    }
}