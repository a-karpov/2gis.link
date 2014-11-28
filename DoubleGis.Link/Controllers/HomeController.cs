using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DoubleGis.Link.Models;
using DoubleGis.Link.Providers;

namespace DoubleGis.Link.Controllers
{
    public class HomeController : Controller
    {
	    private readonly AppSettingsProvider _appSettings;

	    public HomeController()
	    {
		    _appSettings = new AppSettingsProvider();
	    }

        public ActionResult Index()
        {
	        return View();
        }

	    public ActionResult Search(string what, string where, int page = 1)
	    {
			var apiClient = new ApiClient(_appSettings);

		    var searchResponse = apiClient.Search(what, where, page);
			if (searchResponse.ResponseCode == 404)
			{
				return View("EmptyResult");
			}

		    var profiles = apiClient.LoadProfiles(searchResponse.Result);

		    return View(new SearchModel(searchResponse, profiles)
			{
				Page = page,
				HasNextPage = _appSettings.PageSize*page < searchResponse.Total
			});
	    }

	    public ActionResult Recognize(string query)
	    {
			var ip = _appSettings.OverridedIp ?? FindIpAddress(HttpContext.Request);
		    if (string.IsNullOrEmpty(ip))
		    {
			    throw new Exception("Ip not found.");
		    }

		    var geolocationProvider = new GeolocationProvider(new EsClient(_appSettings));
		    var location = geolocationProvider.GetLocationSorted(ip);

			string where = "";
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
			    throw new Exception("Can not recognize where.");
		    }

			return Redirect(string.Format("~/{0}/{1}", what, where));
	    }

	    #region Private

	    private String FindIpAddress(HttpRequestBase request)
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