using System;
using Android.Graphics.Drawables;
using LunchChooser;

namespace LunchChooser
{
	public class LunchMenuPickerItem : LunchMenuListItem, SlotPickerItem
	{
		public LunchMenuPickerItem (int id, string name, float calory, float cost, int selectedCount, Drawable image)
			: base(id, name, calory, cost, selectedCount, image)
		{
		}

		public LunchMenuPickerItem(LunchMenu menu, int count, Drawable image) 
			: base(menu, count, image) {
		}

		#region SlotPickerItem implementation

		public string getDisplayedValue ()
		{
			return name;
		}

		#endregion
	}
}

