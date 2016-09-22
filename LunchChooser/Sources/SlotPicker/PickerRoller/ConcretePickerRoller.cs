using System;
using Android.Widget;
using Android.Views;
using Android.OS;

namespace LunchChooser
{
	/// <summary>
	/// Roll picker with specified value.
	/// </summary>
	public class ConcretePickerRoller : PickerRoller
	{
		#region PickerRoller implementation

		public void roll (NumberPicker picker, int scrollX, int scrollY, long actionTime)
		{
			picker.ScrollBy (scrollX, scrollY);
		}

		public void stop (NumberPicker picker, long elapsedTime)
		{
			// Execute dummy touch to adjust the position of the picker
			long now = SystemClock.UptimeMillis ();
			int touchX = picker.Width / 2;
			int topTouchY = picker.Height / 5;
			int bottomTouchY = picker.Height * 4 / 5;
			const int metaState = 0;

			// Dummy touch of upper side of the picker
			MotionEvent e = MotionEvent.Obtain(
				now,
				now,
				MotionEventActions.Down,
				touchX,
				topTouchY,
				metaState);
			picker.DispatchTouchEvent (e);

			e.Action = MotionEventActions.Up;
			picker.DispatchTouchEvent (e);

			// Dummy touch of bottom side of the picker
			e = MotionEvent.Obtain(
				now,
				now,
				MotionEventActions.Down,
				touchX,
				bottomTouchY,
				metaState);
			picker.DispatchTouchEvent (e);

			e.Action = MotionEventActions.Up;
			picker.DispatchTouchEvent (e);

		}

		#endregion
	}
}

