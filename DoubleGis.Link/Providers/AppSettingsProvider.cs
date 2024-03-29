﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Web;

namespace DoubleGis.Link.Providers
{
	public class AppSettingsProvider
	{
		private readonly NameValueCollection _settings;

		public AppSettingsProvider()
		{
			_settings = ConfigurationManager.AppSettings;
		}

		public int PageSize { get { return 10; } }
		public string ApiKey { get { return _settings["apiKey"]; } }
		public string ApiVersion { get { return "1.3"; } }
		public string OverridedIp { get { return _settings["overridedIp"]; } }
		public string EsAddress { get { return _settings["esAddress"]; } }
	}
}