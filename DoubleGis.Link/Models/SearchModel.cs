using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoubleGis.Link.Models
{
	public class SearchModel
	{
		public int Total { get; set; }
		public string What { get; set; }
		public string Where { get; set; }
		public IEnumerable<CardLink> Result { get; set; }
		public IEnumerable<CardModel> Cards { get; set; }
		public int Page { get; set; }

		public SearchModel(SearchResponse response, IEnumerable<Card> cards)
		{
			Total = response.Total;
			What = response.What;
			Where = response.Where;
			Cards = cards.Select(c => new CardModel(c)).ToArray();
		}
	}
}