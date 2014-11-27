using System.Collections.Generic;
using System.Linq;

namespace DoubleGis.Link.Models
{
	public class SearchModel
	{
		public int Total { get; set; }
		public string What { get; set; }
		public string Where { get; set; }
		public IEnumerable<CardModel> Cards { get; set; }
		public int Page { get; set; }
		public bool HasNextPage { get; set; }
		
		public SearchModel(SearchResponse response, IEnumerable<ProfileResponse> cards)
		{
			Total = response.Total;
			What = response.What;
			Where = response.Where;
			Cards = cards.Select(c => new CardModel(c)).ToArray();
		}
	}
}