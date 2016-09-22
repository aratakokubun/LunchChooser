using System;
using Android.Widget;

namespace LunchChooser
{
	/// <summary>
	/// Roll picker with specified Index.
	/// Picker is rolling and appears discrete index.
	/// </summary>
	public class DiscretePickerRoller : PickerRoller
	{
		private readonly int initIndex;

		private int currentPositionX;
		private int currentPositionY;

		public DiscretePickerRoller(int initIndex = 0) {
			this.initIndex = initIndex;
			this.currentPositionX = 0;
			this.currentPositionY = 0;
		}

		#region PickerRoller implementation

		public void roll (NumberPicker picker, int scrollX, int scrollY, long actionTime)
		{
			calcCurrentPosition (scrollX, scrollY);
			var index = calcCurrentIndex (picker);
			picker.Value = index;
		}

		public void stop (NumberPicker picker, long elapsedTime)
		{
			// Nothing to do
		}

		private void calcCurrentPosition(int distanceX, int distanceY) {
			this.currentPositionX += distanceX;
			this.currentPositionY += distanceY;
		}

		private int calcCurrentIndex(NumberPicker picker) {
			var itemHeight = picker.Height / 3;
			var items = picker.MaxValue - picker.MinValue;
			return (currentPositionY / itemHeight) % items + initIndex;
		}

		#endregion
	}
}

