using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace DoubleGis.Link.Models
{
	public class CardModel
	{
		public string RegisterBcUrl { get; set; }
		public string Name { get; set; }
		public string Type { get; set; }
		public IEnumerable<string> Rubrics { get; set; }
		public string Address { get; set; }
		public IEnumerable<Field> Websites { get; set; }
		public IEnumerable<string> Phones { get; set; }
		public IEnumerable<string> Vkontakte { get; set; }
		public IEnumerable<string> Emails { get; set; }
		public IEnumerable<string> Twitter { get; set; }
		public IEnumerable<string> Instagram { get; set; }
		public IEnumerable<string> Facebook { get; set; }

		public CardModel(ProfileResponse profileResponse)
		{
			RegisterBcUrl = profileResponse.RegisterBcUrl;
			Rubrics = profileResponse.Rubrics;
			Address = profileResponse.Address;

			var nameParts = profileResponse.Name.Split(',');
			Name = nameParts.First();
			if (nameParts.Last().Length > 0)
			{
				Type = nameParts.Last().Trim();
			}

			var contacts = (from cg in profileResponse.Contacts
							from c in cg.Contacts
							select c).ToLookup(c => c.Type);

			Websites = contacts["website"].Select(c => new Field(c));
			Phones = contacts["phone"].Select(c => FormatPhone(c.Value));
			Vkontakte = contacts["vkontakte"].Select(c => c.Value);
			Emails = contacts["email"].Select(c => c.Value);
			Twitter = contacts["twitter"].Select(c => c.Value);
			Instagram = contacts["instagram"].Select(c => c.Value);
			Facebook = contacts["facebook"].Select(c => c.Value);
		}

		#region Private

		private static string FormatPhone(string value)
		{
			value = string.Format("{0}-{1}-{2}-{3}-{4}",
				value.Substring(0, 2),
				value.Substring(2, 3),
				value.Substring(5, value.Length - 9),
				value.Substring(value.Length - 4, 2),
				value.Substring(value.Length - 2, 2));
			return value;
		}

		#endregion
	}
}