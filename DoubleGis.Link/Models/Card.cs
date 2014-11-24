using System.Collections.Generic;

namespace DoubleGis.Link.Models
{
	public class Card
	{
		public string Name { get; set; }
		public string Address { get; set; }
		public IEnumerable<string> Rubrics { get; set; }
	}
}