using System.Collections.Generic;
using Newtonsoft.Json;

namespace DoubleGis.Link.Models
{
	public class ProjectsResponse
	{
		[JsonProperty("response_code")]
		public int ResponseCode { get; set; }
		public int Total { get; set; }
		public IEnumerable<ProjectModel> Result { get; set; } 
	}
}