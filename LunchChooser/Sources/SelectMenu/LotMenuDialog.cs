using Android.App;
using Android.Views;
using Android.OS;
using Android.Widget;
using Android.Util;

using SlotPicker;
using System.Collections.Generic;

namespace LunchChooser
{
	public class LotMenuDialog : DialogFragment
	{
		private readonly Activity activity;
		private SlotPicker<LunchMenuPickerItem> slotPicker;

		public LotMenuDialog(Activity activity) {
			this.activity = activity;
		}

		public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
			// Use this to return your custom view for this Fragment
			View view =  inflater.Inflate(Resource.Layout.LotMenuDialog, container, false);

			var picker = view.FindViewById<NumberPicker> (Resource.Id.menuPicker);
			slotPicker = new SlotPicker<LunchMenuPickerItem> (
				picker, activity, 
				new TouchPickerRoller (),
				// new DiscretePickerRoller(),
				getMenuItems ());

			var slotSwitch = view.FindViewById<Button> (Resource.Id.slotSwitch);
			slotSwitch.Click += delegate {
				if (slotPicker.isRolling()) {
					slotPicker.stopRolling();
					Log.Error("LotMenuDialog", "rolling");
				} else {
					slotPicker.startRolling();
					Log.Error("LotMenuDialog", "stopped");
				}
			};

			return view;
		}

		private LunchMenuPickerItem[] getMenuItems() {
			IEnumerable<LunchMenu> menus = LunchMenuDao.getInstance ().getLunchMenus ();
			var menuItems = new List<LunchMenuPickerItem> ();

			foreach (var menu in menus)
			{
				// TODO
				// Fetch count from db
				// Fetch image from db
				menuItems.Add (new LunchMenuPickerItem(menu, 0, 
					activity.GetDrawable(Resource.Drawable.ic_media_route_on_0_mono_dark)));
			}
			return menuItems.ToArray();
		}
	}
}

