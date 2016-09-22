using System;
using SQLite.Net.Attributes;

namespace LunchChooser
{
	public class LunchMenu
	{
		public LunchMenu(string name, float calory, float cost) {
			// Default id
			this.id = 0;
			this.name = name;
			this.calory = calory;
			this.cost = cost;
			this.created = DateTime.Now;
			this.deleted = false;
		}

		public LunchMenu() {}

		[PrimaryKey, AutoIncrement]
		public int id {
			get;
			set;
		}

		public string name {
			get;
			set;
		}

		public float calory {
			get;
			set;
		}

		public float cost {
			get;
			set;
		}

		public DateTime created {
			get;
			set;
		}

		// Delete flag
		public bool deleted {
			get;
			set;
		}
	}
}

