using Android.App;
using Android.Widget;
using Android.OS;
using System.Collections.Generic;
using System;
using Android.Content;
using Android.Views;

namespace LunchChooser
{
	[Activity (Label = "LunchChooser", MainLauncher = true, Icon = "@mipmap/icon")]
	public class MainActivity : Activity
	{

		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);
			RequestWindowFeature(WindowFeatures.NoTitle);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);

			// Shift to select menu activity
			Button selectMenuButton = FindViewById<Button> (Resource.Id.selectMenuButton);
			selectMenuButton.Click += (_, __) => {
				var intent = new Intent (this, typeof(SelectMenu));
				StartActivity(intent);
			};

			// Shift to lunch menu list activity
			Button menuListButton = FindViewById<Button> (Resource.Id.menuListButton);
			menuListButton.Click += (_, __) => {
				var intent = new Intent (this, typeof(MenuListView));
				StartActivity(intent);
			};
		}

		private void lotMenu() {
			FragmentTransaction ft = FragmentManager.BeginTransaction();
			//Remove fragment else it will crash as it is already added to backstack
			Fragment prev = FragmentManager.FindFragmentByTag("dialog");
			if (prev != null) {
				ft.Remove(prev);
			}
			ft.AddToBackStack(null);

			//Add fragment
			new AddMenuDialog().Show(ft, "dialog");

			// FIXME inform item is added on dismissing dialog
		}
	}
}


