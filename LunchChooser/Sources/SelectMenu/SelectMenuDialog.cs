using System;
using Android.App;
using Android.Views;
using Android.OS;
using Android.Widget;
using System.Collections.Generic;
using Android.Content;

namespace LunchChooser
{
	public class SelectMenuDialog : DialogFragment
	{
		private static readonly int DEFAULT_VALUE = 0;

		private readonly Activity activity;
		private readonly Action<int> dialogResultAction;
		private bool isOptionalSelected;

		public SelectMenuDialog (Activity activity, Action<int> dialogResultAction) : base() {
			this.activity = activity;
			this.dialogResultAction = dialogResultAction;
		}

		public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
			// Use this to return your custom view for this Fragment
			View view =  inflater.Inflate(Resource.Layout.SelectMenuDialog, container, false);

			// Select menu name (id) spinner
			Spinner menuSpinner = view.FindViewById<Spinner> (Resource.Id.selectMenuSpinner);
			// Apply menu items adapter from db
			var adapter = new LunchMenuAdapter (activity, getMenuItems());
			menuSpinner.Adapter = adapter;
			menuSpinner.Selected = false;

			// Select optional inputs
			isOptionalSelected = false;
			Button optionalButton = view.FindViewById<Button> (Resource.Id.optionalButton);
			optionalButton.Click += delegate {
				isOptionalSelected = !isOptionalSelected;
				switchOptions(view);
			};

			// Lunch Calory Text with default value
			EditText caloryText = view.FindViewById<EditText> (Resource.Id.caloryText);
			caloryText.Text = DEFAULT_VALUE.ToString ();
			// Lunch Cost Text with default value
			EditText costText = view.FindViewById<EditText> (Resource.Id.costText);
			costText.Text = DEFAULT_VALUE.ToString ();

			// Confirm Today's Menu
			Button registerButton = view.FindViewById<Button> (Resource.Id.registerButton);
			registerButton.Click += delegate {
				int selectedPos = menuSpinner.SelectedItemPosition;
				var selectedItem = adapter[selectedPos];
				// If optional values are set, use them values on log.
				float calory = selectedItem.calory;
				float cost = selectedItem.cost;
				if (isOptionalSelected) {
					float optionalCalory = float.Parse(caloryText.Text);
					if (optionalCalory > 0) {
						calory = optionalCalory;
					}

					float optionalCost = float.Parse(costText.Text);
					if (optionalCost > 0) {
						cost = optionalCost;
					}
				}
					
				regsiterLunchMenu(selectedItem.id, selectedItem.name, calory, cost);
			};

			// Cancel
			Button cancelButton = view.FindViewById<Button> (Resource.Id.cancelButton);
			cancelButton.Click += delegate {
				this.Dismiss();
			};

			return view;
		}


		/**
		 * Switch View if selected or not
		 */
		private void switchOptions(View view) {
			LinearLayout optional = view.FindViewById<LinearLayout> (Resource.Id.optionalInputs);

			optional.Visibility = isOptionalSelected ? ViewStates.Visible : ViewStates.Gone;
		}

		private List<LunchMenuListItem> getMenuItems() {
			IEnumerable<LunchMenu> menus = LunchMenuDao.getInstance ().getLunchMenus ();
			var menuItems = new List<LunchMenuListItem> ();

			foreach (var menu in menus)
			{
				// TODO
				// Fetch count from db
				menuItems.Add (new LunchMenuListItem(menu.id, menu.name, menu.calory, menu.cost, 0, 
					activity.GetDrawable(Resource.Drawable.ic_media_route_on_0_mono_dark)));
			}
			return menuItems;
		}

		/**
		 * Add new lunch menu log.
		 */
		private void regsiterLunchMenu(int menuId, string name, float calory, float cost) {
			var newMenuLog = new MenuLog (menuId, calory, cost);

			// Dismiss dialog on succeeding insert.
			if (LunchMenuDao.getInstance().insertMenuLog((newMenuLog))) {
				Dismiss();
				string successText = String.Format(Resources.GetString (Resource.String.success_add_log), name);
				Toast.MakeText(Activity ,successText , ToastLength.Short).Show();
			} else {
				// TODO
				// Show failure Toast and Highlight invalid field.
				string successText = String.Format(Resources.GetString (Resource.String.fail_add_menu), name);
				Toast.MakeText(Activity ,successText , ToastLength.Short).Show();
			}
		}
	}
}

