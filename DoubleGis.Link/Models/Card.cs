using System.Collections.Generic;
using Newtonsoft.Json;

namespace DoubleGis.Link.Models
{
	public class Card
	{
		[JsonProperty("response_code")]
		public int ResponseCode { get; set; }
		[JsonProperty("register_bc_url")]
		public string RegisterBcUrl { get; set; }
		public string Name { get; set; }
		public string Address { get; set; }
		public IEnumerable<string> Rubrics { get; set; }
		public IEnumerable<ContactsGroup>  Contacts { get; set; }
	}

	public class Contact
	{
		public string Type { get; set; }
		public string Value { get; set; }
		public string Alias { get; set; }
	}

	public class ContactsGroup
	{
		public string Name { get; set; }
		 public IEnumerable<Contact> Contacts { get; set; }
	}
}