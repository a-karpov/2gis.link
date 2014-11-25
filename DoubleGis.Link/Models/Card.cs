using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace DoubleGis.Link.Models
{
	public class Card
	{
		[JsonProperty("register_bc_url")]
		public string RegisterBcUrl { get; set; }
		public string Name { get; set; }
		public string Address { get; set; }
		public IEnumerable<string> Rubrics { get; set; }
	}
}