using System;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
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
			string where = null;

			var ip = _appSettings.OverridedIp ?? FindIpAddress(HttpContext.Request);
		    if (!string.IsNullOrEmpty(ip))
		    {
				var geolocationProvider = new GeolocationProvider(new EsClient(_appSettings));
				var location = geolocationProvider.GetLocationSorted(ip);
				
				if (location.Any())
				{
					 where = location.First().City;
				}
		    }

		    var queryParts = query.Split(',');
		    var what = queryParts.First();

		    if (queryParts.Length > 1)
		    {
			    where = (where ?? string.Empty) + query.Substring(what.Length);
		    }

		    if (string.IsNullOrEmpty(what))
		    {
			    return View("CannotFindYou");
		    }

			return Redirect(string.Format("~/{0}/{1}", what, where));
	    }

	    public string Geolocation()
	    {
		    var request = HttpContext.Request;

		    var forwardedFor = request.ServerVariables["HTTP_X_FORWARDED_FOR"];
		    var remoteAddr = request.ServerVariables["REMOTE_ADDR"];
		    var dns = Dns.GetHostAddresses(Dns.GetHostName());

		    return
			    "forwardedFor: " + forwardedFor + "<br />" +
			    "remoteAddr: " + remoteAddr + "<br />" +
				"dns name: " + Dns.GetHostName() + "<br />" +
			    "dns ips: " + string.Join(", ", dns.Select(ip => ip.ToString()));
	    }

	    #region Private

	    private String FindIpAddress(HttpRequestBase request)
	    {
		    return request.ServerVariables["REMOTE_ADDR"];
	    }

	    #endregion

    }
}