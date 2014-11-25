﻿using System.Collections.Generic;

namespace DoubleGis.Link.Models
{
	public class CardLink
	{
		public string Id { get; set; }
		public string Hash { get; set; }
		public string Name { get; set; }
		public string Address { get; set; }
		public IEnumerable<string> Rubrics { get; set; }
	}
}