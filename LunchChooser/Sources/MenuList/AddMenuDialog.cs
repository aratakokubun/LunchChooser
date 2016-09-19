using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using System.Globalization;

namespace LunchChooser
{
	public class AddMenuDialog : DialogFragment
	{
		public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			// Use this to return your custom view for this Fragment
			View view =  inflater.Inflate(Resource.Layout.AddMenuDialog, container, false);

			// Lunch Menu Text
			EditText nameText = view.FindViewById<EditText> (Resource.Id.nameText);
			// Lunch Calory Text
			EditText caloryText = view.FindViewById<EditText> (Resource.Id.caloryText);
			// Lunch Cost Text
			EditText costText = view.FindViewById<EditText> (Resource.Id.costText);

			// Add menu
			Button button = view.FindViewById<Button> (Resource.Id.addButton);
			button.Click += delegate {
				addLunchMenu(nameText.Text, float.Parse(caloryText.Text), float.Parse(costText.Text));
			};

			// Cancel
			Button cancelButton = view.FindViewById<Button> (Resource.Id.cancelButton);
			cancelButton.Click += delegate {
				this.Dismiss();
			};

			return view;
		}

		/**
		 * Add new lunch menu.
		 */
		private void addLunchMenu(String name, float calory, float cost) {
			var newMenu = new LunchMenu(name, calory, cost);

			// Dismiss dialog on succeeding insert.
			if (LunchMenuDao.getInstance().insertMenu(newMenu)) {
				Dismiss();
				string successText = String.Format(Resources.GetString (Resource.String.success_add_menu), name);
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

