using System.Collections.Generic;
using Newtonsoft.Json;

namespace DoubleGis.Link.Models
{
	public class SearchResponse
	{
		[JsonProperty("response_code")]
		public int ResponseCode { get; set; }
		public int Total { get; set; }
		public string What { get; set; }
		public string Where { get; set; }
		public IEnumerable<SearchResponseResultElem> Result { get; set; } 
	}
}