﻿using System;
using System.Configuration;
using System.Net;
using System.Text;
using System.Web.Mvc;
using DoubleGis.Link.Models;
using Newtonsoft.Json;

namespace DoubleGis.Link.Controllers
{
    public class HomeController : Controller
    {
	    private object ApiKey { get { return ConfigurationManager.AppSettings["apiKey"]; } }

        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

	    public ActionResult Search(string what, string where)
	    {
			using (var client = new WebClient())
			{
				var response = client.DownloadData(new Uri(string.Format("http://catalog.api.2gis.ru/search?key={0}&version=1.3&what={1}&where={2}", ApiKey, what, where)));
				var o = JsonConvert.DeserializeObject<SearchResponse>(Encoding.UTF8.GetString(response));
				return View(o);
			}
	    }

	    public ActionResult Card(string id, string hash)
	    {
			using (var client = new WebClient())
			{
				var response = client.DownloadData(new Uri(string.Format("http://catalog.api.2gis.ru/profile?key={0}&version=1.3&id={1}&hash={2}", ApiKey, id, hash)));
				var o = JsonConvert.DeserializeObject<Card>(Encoding.UTF8.GetString(response));
				return View(o);
			}
	    }
    }
}