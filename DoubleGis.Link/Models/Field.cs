using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoubleGis.Link.Models
{
	public class Field
	{
		public Field(Contact contact)
		{
			Value = contact.Value;
			Alias = contact.Alias;
		}

		public string Value { get; private set; }
		public string Alias { get; private set; }
	}
}