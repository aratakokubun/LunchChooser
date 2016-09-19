using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace LunchChooser
{
	[Activity (Label = "SelectMenu")]			
	public class SelectMenu : Activity
	{
		private bool isTodayMenuSelected;

		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);
			RequestWindowFeature(WindowFeatures.NoTitle);
			Xamarin.Forms.Forms.Init (this, savedInstanceState);

			SetContentView (Resource.Layout.SelectMenu);

			// TODO
			// Get if today's menu is already selected.
			isTodayMenuSelected = false;
			switchSelectedViewOptions ();

			// Buttons before select
			Button menuLot = this.FindViewById<Button> (Resource.Id.menuLotButton);
			menuLot.Click += (_, __) => lotMenu ();

			Button menuSelect = this.FindViewById<Button> (Resource.Id.menuSelectButton);
			menuSelect.Click += (_, __) => selectMenu ();
		}

		/**
		 * Switch View if selected or not
		 */
		private void switchSelectedViewOptions() {
			LinearLayout before = this.FindViewById<LinearLayout> (Resource.Id.beforeSelect);
			LinearLayout after  = this.FindViewById<LinearLayout> (Resource.Id.afterSelect);

			if (isTodayMenuSelected) {
				before.Visibility = ViewStates.Gone;
				after.Visibility = ViewStates.Visible;
			} else {
				before.Visibility = ViewStates.Visible;
				after.Visibility = ViewStates.Gone;
			}
		}

		/**
		 * Show select menu dialog
		 */
		private void selectMenu() {
			FragmentTransaction ft = FragmentManager.BeginTransaction();
			//Remove fragment else it will crash as it is already added to backstack
			Fragment prev = FragmentManager.FindFragmentByTag("dialog");
			if (prev != null) {
				ft.Remove(prev);
			}
			ft.AddToBackStack(null);

			//Add fragment
			var dialog = new SelectMenuDialog(this, (dialogResult) =>
				{
					switch (dialogResult) {
					}
				});
			dialog.Show(ft, "dialog");
		}

		/**
		 * Show lot menu dialog
		 */
		private void lotMenu() {
			FragmentTransaction ft = FragmentManager.BeginTransaction();
			//Remove fragment else it will crash as it is already added to backstack
			Fragment prev = FragmentManager.FindFragmentByTag("dialog");
			if (prev != null) {
				ft.Remove(prev);
			}
			ft.AddToBackStack(null);

			//Add fragment
			var dialog = new LotMenuDialog(this);
			dialog.Show(ft, "dialog");
		}
	}
}
