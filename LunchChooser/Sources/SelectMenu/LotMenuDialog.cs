using Android.App;
using Android.Views;
using Android.OS;
using Android.Widget;
using Android.Util;

using LunchChooser;
using System.Collections.Generic;

namespace LunchChooser
{
	public class LotMenuDialog : DialogFragment
	{
		private readonly Activity activity;
		private View view;
		private SlotPicker<LunchMenuPickerItem> slotPicker;

		public LotMenuDialog(Activity activity) {
			this.activity = activity;
		}

		public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			// Use this to return your custom view for this Fragment
			view =  inflater.Inflate(Resource.Layout.LotMenuDialog, container, false);

			var picker = view.FindViewById<NumberPicker> (Resource.Id.menuPicker);
			slotPicker = new SlotPicker<LunchMenuPickerItem> (
				picker, activity, 
				// new TouchPickerRoller (),
				new DiscretePickerRoller(0),
				// new ConcretePickerRoller(),
				getMenuItems (),
				6.0f, //rps
				0.1f, //acc
				-0.05f //deacc
			);

			var slotSwitch = view.FindViewById<Button> (Resource.Id.slotSwitch);
			slotSwitch.Click += delegate {
				if (slotPicker.isRolling()) {
					slotPicker.stopRolling();
				} else {
					slotPicker.startRolling();
				}
			};

			var lottedView = view.FindViewById<LinearLayout> (Resource.Id.lotMenuLayoutView);
			lottedView.Visibility = ViewStates.Gone;

			// Stop event handler
			this.slotPicker.StopPickerEvent += new SlotPicker<LunchMenuPickerItem>.StopPickerHandler (stopPickerCallback);

			// TODO
			// Not dismissable without confirm or cancel
			return view;
		}

		/// <summary>
		/// Get the menu candidates.
		/// </summary>
		/// <returns>The menu items.</returns>
		private LunchMenuPickerItem[] getMenuItems() 
		{
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

		/// <summary>
		/// Callback for picker stop.
		/// Update UI on UI thread.
		/// </summary>
		/// <param name="selectedItem">Selected item in picker.</param>
		private void stopPickerCallback(LunchMenuPickerItem selectedItem) {
			Application.SynchronizationContext.Post(_ => {
				showConfirmationView(selectedItem);
			}, null);
		}

		/// <summary>
		/// Show confirmation view to specify todays menu.
		/// </summary>
		/// <param name="selectedItem">Selected item.</param>
		private void showConfirmationView(LunchMenuPickerItem selectedItem)
		{
			var lottedView = view.FindViewById<LinearLayout>(Resource.Id.lotMenuLayoutView);
			lottedView.Visibility = ViewStates.Visible;

			var lottedItemValue = view.FindViewById<TextView>(Resource.Id.selectedMenuValue);
			lottedItemValue.Text = selectedItem.name;

			var confirmButton = view.FindViewById<Button>(Resource.Id.confirmButton);
			confirmButton.Click += delegate
			{
				regsiterLottedLunchMenu(selectedItem);
			};

			var cancelButton = view.FindViewById<Button>(Resource.Id.cancelButton);
			cancelButton.Click += delegate
			{
				this.Dismiss();
			};
		}

		/// <summary>
		/// Regsiters the lotted lunch menu.
		/// </summary>
		/// <param name="selectedItem">Selected item.</param>
		private void regsiterLottedLunchMenu(LunchMenuPickerItem selectedItem) 
		{
			var newMenuLog = new MenuLog (selectedItem.id, selectedItem.calory, selectedItem.cost);

			// Dismiss dialog on succeeding insert.
			if (LunchMenuDao.getInstance().insertMenuLog((newMenuLog))) {
				// TODO
				/*
				string successText = String.Format(Resources.GetString (Resource.String.success_add_log), name);
				Toast.MakeText(Activity ,successText , ToastLength.Short).Show();
				*/
			} else {
				// TODO
				// Show failure Toast and Highlight invalid field.
				/*
				string successText = String.Format(Resources.GetString (Resource.String.fail_add_menu), name);
				Toast.MakeText(Activity ,successText , ToastLength.Short).Show();
				*/
			}
			Dismiss();
		}
	}
}

