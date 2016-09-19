
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
	[Activity (Label = "MenuListView")]			
	public class MenuListView : Activity
	{
		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);
			RequestWindowFeature(WindowFeatures.NoTitle);
			Xamarin.Forms.Forms.Init (this, savedInstanceState);

			SetContentView (Resource.Layout.MenuListView);

			// Add lunch menu button
			Button addMenuButton = FindViewById<Button> (Resource.Id.addMenu);
			addMenuButton.Click += delegate {
				addMenu ();
			};

			ListView listView = FindViewById<ListView> (Resource.Id.lunchMenuList);

			List<LunchMenuListItem> menuItems = getMenuItems ();
			LunchMenuAdapter adapter = new LunchMenuAdapter (this, menuItems);
			listView.Adapter = adapter;
		}

		private List<LunchMenuListItem> getMenuItems() {
			IEnumerable<LunchMenu> menus = LunchMenuDao.getInstance ().getLunchMenus ();
			List<LunchMenuListItem> menuItems = new List<LunchMenuListItem> ();

			foreach (var menu in menus)
			{
				// TODO
				// Fetch count from db
				menuItems.Add (new LunchMenuListItem(menu.id, menu.name, menu.calory, menu.cost, 0, 
					GetDrawable(Resource.Drawable.ic_media_route_on_0_mono_dark)));
			}
			return menuItems;
		}

		private void addMenu() {
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

