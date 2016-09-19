using System;
using Android.Widget;
using System.Collections.Generic;
using Android.App;
using Android.Views;
using Android.Util;

namespace LunchChooser
{
	public class LunchMenuAdapter : BaseAdapter<LunchMenuListItem>
	{
		private readonly List<LunchMenuListItem> items;
		private readonly Activity context;

		public LunchMenuAdapter (Activity context, List<LunchMenuListItem> items) : base()
		{
			this.context = context;
			this.items = items;
		}

		public override long GetItemId(int position)
		{
			return position;
		}

		public override LunchMenuListItem this[int position]
		{
			get { return items [position]; }
		}

		public override int Count
		{
			get { return items.Count; }
		}

		public void addItem(LunchMenuListItem item) {
			items.Add (item);
			this.NotifyDataSetChanged ();
		}

		public void updateItem(int position, LunchMenuListItem item) {
			items [position] = item;
			this.NotifyDataSetChanged ();
		}

		public override View GetView (int position, View convertView, ViewGroup parent)
		{
			var item = items [position];
			// If no view to re-use exist, create new one
			View view = convertView;
			if (view == null) {
				view = context.LayoutInflater.Inflate (Resource.Layout.LunchMenuListItem, null);
			}
			// Adapt value to list item
			view.FindViewById<ImageView>(Resource.Id.menuImage).SetImageDrawable(item.image);
			view.FindViewById<TextView>(Resource.Id.nameValue).Text = item.name;
			view.FindViewById<TextView>(Resource.Id.selectedTimesValue).Text = item.selectedCount.ToString();
			view.FindViewById<TextView>(Resource.Id.caloryValue).Text = item.calory.ToString();
			view.FindViewById<TextView>(Resource.Id.costValue).Text = item.cost.ToString();

			return view;
		}
	}
}

