using System;
using Android.Graphics.Drawables;

namespace LunchChooser
{
	public class LunchMenuListItem
	{
		public LunchMenuListItem (int id, string name, float calory, float cost, int selectedCount, Drawable image)
		{
			this.id = id;
			this.name = name;
			this.calory = calory;
			this.cost = cost;
			this.selectedCount = selectedCount;
			this.image = image;
		}

		public LunchMenuListItem (LunchMenu menu, int count, Drawable image) {
			this.id = menu.id;
			this.name = menu.name;
			this.calory = menu.calory;
			this.cost = menu.cost;
			this.selectedCount = count;
			this.image = image;
		}

		public int id {
			get;
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

		public int selectedCount {
			get;
			set;
		}

		public Drawable image {
			get;
			set;
		}
	}
}

