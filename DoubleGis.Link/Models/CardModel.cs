using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI.HtmlControls;

namespace DoubleGis.Link.Models
{
	public class CardModel
	{
		private readonly Dictionary<string, string> _fieldNamesMap = new Dictionary<string, string>
		{
			{"phone","Телефон"},
			{"website","Сайт"},
			{"email","Почта"},
			{"vkontakte","ВКонтакте"},
		};

		private readonly HashSet<string> _excludedContacts = new HashSet<string>
		{
			"jabber", "fax", "icq"
		};
		
		public string RegisterBcUrl { get; set; }
		public string Name { get; set; }
		public string Type { get; set; }
		public IEnumerable<string> Rubrics { get; set; }
		public IEnumerable<IGrouping<string, string>> Fields { get; set; } 

		public CardModel(Card card)
		{
			RegisterBcUrl = card.RegisterBcUrl;
			var nameParts = card.Name.Split(',');
			Name = nameParts.First();
			Type = nameParts.Last();
			Rubrics = card.Rubrics;

			var fields = new List<Field>();
			fields.Add(new Field{Name = "Адрес", Value = card.Address});
			fields.AddRange(from cg in card.Contacts 
							from c in cg.Contacts
							where FilterContacts(c)
							select ContactToField(c));
			Fields = fields.GroupBy(f => f.Name, field => field.Value);
		}

		#region Private

		public bool FilterContacts(Contact contact)
		{
			return !_excludedContacts.Contains(contact.Type);
		}

		private Field ContactToField(Contact contact)
		{
			var name = _fieldNamesMap.ContainsKey(contact.Type)
				? _fieldNamesMap[contact.Type]
				: contact.Type;

			string value = contact.Value;

			if (contact.Type == "phone")
			{
				value = string.Format("{0}-{1}-{2}-{3}-{4}",
					value.Substring(0, 2),
					value.Substring(2, 3),
					value.Substring(5, value.Length - 9),
					value.Substring(value.Length - 4, 2),
					value.Substring(value.Length - 2, 2));
			}
			if (contact.Type == "website")
			{
				value = string.Format(@"<a href=""{0}"">{1}</a>", contact.Value, contact.Alias);
			}

			return new Field { Name = name, Value = value };
		}

		#endregion

	}
}