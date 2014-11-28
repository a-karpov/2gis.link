using System;
using System.Collections.Generic;
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
	    private readonly AppSettingsProvider _appSettings;
	    private readonly ApiClient _apiClient;

	    public HomeController()
	    {
		    _appSettings = new AppSettingsProvider();
			_apiClient = new ApiClient(_appSettings);
	    }

        public ActionResult Index()
        {
	        return View();
        }

	    public ActionResult Search(string what, string where, int page = 1)
	    {
		    var searchResponse = _apiClient.Search(what, where, page);

			if (searchResponse.ResponseCode == 404)
			{
				return View("EmptyResult");
			}

		    var profiles = _apiClient.LoadProfiles(searchResponse.Result);

		    return View(new SearchModel(searchResponse, profiles)
			{
				Page = page,
				HasNextPage = _appSettings.PageSize*page < searchResponse.Total
			});
			
	    }

	    public ActionResult Recognize(string query)
	    {

			string where = "";


		    var ip = _appSettings.OverridedIp ?? FindLanIpAddress(HttpContext.Request);

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