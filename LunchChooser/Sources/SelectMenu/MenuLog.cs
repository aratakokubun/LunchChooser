using System;
using SQLite.Net.Attributes;

namespace LunchChooser
{
	public class MenuLog
	{
		public MenuLog(int menuId, float calory, float cost) {
			this.menuId = menuId;
			this.calory = calory;
			this.cost = cost;
			this.created = DateTime.Now;
		}

		public MenuLog ()
		{
		}

		[PrimaryKey, AutoIncrement]
		public int id {
			get;
			set;
		}

		public int menuId {
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
	}
}

