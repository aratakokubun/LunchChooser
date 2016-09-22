using System;
using Android.Widget;

namespace LunchChooser
{
	/// <summary>
	/// Controller interface to roll <see>NumberPicker</see> class.
	/// </summary>
	public interface PickerRoller
	{
		/// <summary>
		/// Roll <see>SlotPicker</see>.
		/// </summary>
		void roll(NumberPicker picker, int scrollX, int scrollY, long actionTime);

		/// <summary>
		/// Stop <see>SlotPicker</see>.
		/// </summary>
		void stop(NumberPicker picker, long elapsedTime);
	}
}

