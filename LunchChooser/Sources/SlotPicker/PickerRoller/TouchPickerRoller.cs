using System;
using Android.Views;
using Android.OS;

namespace LunchChooser
{
	public class TouchPickerRoller : PickerRoller
	{
		#region PickerRoller implementation

		public void roll (Android.Widget.NumberPicker picker, int scrollX, int scrollY, long actionTime)
		{
			// Execute dummy touch to scroll specified length
			long now = SystemClock.UptimeMillis ();
			int touchX = picker.Width / 2;
			int scrollStartY = (picker.Height - scrollY) / 2;
			int scrollEndY = scrollStartY + scrollY;
			const int metaState = 0;

			MotionEvent e = MotionEvent.Obtain(
				now,
				now + actionTime/2,
				MotionEventActions.Down,
				touchX,
				scrollStartY,
				metaState);
			picker.DispatchTouchEvent (e);

			Android.Util.Log.Error ("Motion", scrollStartY.ToString());

			e = MotionEvent.Obtain(
				now + actionTime/2,
				now + actionTime,
				MotionEventActions.Move,
				touchX,
				scrollEndY,
				metaState);
			picker.DispatchTouchEvent (e);

			e.Action = MotionEventActions.Up;
			picker.DispatchTouchEvent (e);

			Android.Util.Log.Error ("Motion", scrollEndY.ToString());
		}

		public void stop (Android.Widget.NumberPicker picker, long elapsedTime)
		{
			// Nothing to do
		}

		#endregion
	}
}

