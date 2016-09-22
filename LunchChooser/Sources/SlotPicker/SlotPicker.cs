/**
 * Copyright © 2016 Arata Kokubun. All rights reserved.
 * Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
 * The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
 */
using System;
using Android.App;
using Android.Widget;

namespace LunchChooser {
	/// <summary>
	/// <para>Android Picker library to use it as a slot.</para>
	/// <para>Inject NumberPicker object to this class.</para>
	/// <para>You can specify slot item which implments SlotPickerItem.</para>
	/// <para><see cref="SlotPickerItem"/>for more information to implment item. </para>
	/// </summary>
	public class SlotPicker<Type> where Type : SlotPickerItem
	{
		/// <summary>
		/// Picker object which this class depends on
		/// </summary>
		private readonly NumberPicker basePicker;

		/// <summary>
		/// Parent activity object displaying this picker
		/// </summary>
		private readonly Activity activity;

		/// <summary>
		/// Array of items displayed on the slot
		/// </summary>
		private readonly Type[] pickerItems;

		/// <summary>
		/// Revolution per second in float
		/// </summary>
		/// <value>The rps.</value>
		public float rps {
			set;
			get;
		}

		/// <summary>
		/// Acceleration of revolution per second in float
		/// </summary>
		/// <value>The accel rps.</value>
		public float accelRps {
			set;
			get;
		}

		/// <summary>
		/// Deacceleration of revolution per second in float
		/// Gets or sets the deaccel rps.
		/// </summary>
		/// <value>The deaccel rps.</value>
		public float deaccelRps {
			set;
			get;
		}

		/// <summary>
		/// Gets or sets the max velocity.
		/// </summary>
		/// <value>The max velocity.</value>
		public float maxVelocity {
			set;
			get;
		}

		/// <summary>
		/// Controller for rolling motion of this picker
		/// </summary>
		private readonly AcceleratedMotionController motionController;

		/// <summary>
		/// Controller for roll picker
		/// </summary>
		private readonly PickerRoller roller;

		/// <summary>
		/// Initializes a new instance of the SlotPicker class.
		/// </summary>
		/// <param name="basePicker">Base number picker</param>
		/// <param name="activity">Activity.</param>
		/// <param name="roller">Roll controller.</param>
		/// <param name="pickerItems">Picker items.</param>
		/// <param name="rps">Rps, MUST BE POSITIVE</param>
		/// <param name="accelRps">Accelerate rps, MUST BE POSITIVE</param>
		/// <param name="deaccelRps">Deaccelerate rps, MUST BE NEGATIVE</param>
		public SlotPicker (NumberPicker basePicker, Activity activity, PickerRoller roller, Type[] pickerItems, float rps = 2.0f, float accelRps = 0.4f, float deaccelRps = -0.2f)
		{
			this.basePicker = basePicker;
			this.activity = activity;

			// Motion controller
			this.motionController = new AcceleratedMotionController ();
			this.motionController.RollingEvent += new AcceleratedMotionController.MotionEventHandler (rollingMotionCallback);
			this.motionController.StopEvent += new AcceleratedMotionController.StopEventHandler (stopMotionCallback);

			this.roller = roller;
			this.pickerItems = pickerItems;
			this.rps = rps;
			this.accelRps = accelRps;
			this.deaccelRps = deaccelRps;

			// Allocate each index of picker values to base picker.
			this.basePicker.MinValue = 0;
			this.basePicker.MaxValue = pickerItems.Length-1;
			this.basePicker.SetDisplayedValues (getDisplayedFromPickerItems());
			// Set infinite loop
			this.basePicker.WrapSelectorWheel = true;
		}

		/// <summary>
		/// Gets the displayed from picker items.
		/// </summary>
		/// <returns>The displayed strings from picker items.</returns>
		private string[] getDisplayedFromPickerItems () {
			var displayedValues = new string[pickerItems.Length];
			for (int i = 0; i < pickerItems.Length; i++) {
				displayedValues [i] = pickerItems [i].getDisplayedValue();			
			}
			return displayedValues;
		}

		/// <summary>
		/// Starts the rolling.
		/// </summary>
		/// <returns><c>true</c>, if rolling was started, <c>false</c> otherwise.</returns>
		public bool startRolling() {
			// 1 revolution => picker.length = n
			// 1 dispaly => 3
			// displayed range => l = Height
			// if rps = r, velocity = v
			// r * 1/1000 = 3/n * v/l
			// ∴ y = r*n*l*s/1000/3
			float convertRate = pickerItems.Length * basePicker.Height / 1000.0f / 3.0f;
			float maxLenPerFrame = rps * convertRate;
			float acceleratePerFrame = accelRps * convertRate;
			float deacceleratePerFrame = deaccelRps * convertRate;

			return motionController.startMotion (acceleratePerFrame, deacceleratePerFrame, maxLenPerFrame);
		}

		/// <summary>
		/// Stops the roll.
		/// </summary>
		/// <returns><c>true</c>, if roll was stoped, <c>false</c> otherwise.</returns>
		public bool stopRolling() {
			return motionController.stopMotion ();
		}

		/// <summary>
		/// Get if the ptcker is rolling.
		/// </summary>
		/// <returns><c>true</c>, if rolling was ised, <c>false</c> otherwise.</returns>
		public bool isRolling() {
			return motionController.isMoving ();
		}

		/// <summary>
		/// Callback event from RevolutionController
		/// </summary>
		/// <param name="eventArgs">Event arguments.</param>
		private void rollingMotionCallback(MotionArgs eventArgs) {
			activity.RunOnUiThread (() => roller.roll (
				basePicker, 0, (int)eventArgs.getDistance (), AcceleratedMotionController.SLEEP_TIME));
		}

		/// <summary>
		/// Stops the motion callback.
		/// </summary>
		/// <param name="elapsedTime">Elapsed time.</param>
		private void stopMotionCallback(long elapsedTime) {
			roller.stop(basePicker, elapsedTime);
			sendStopEvent (pickerItems[basePicker.Value]);
		}

		/// <summary>
		/// Delegate of event handler called when rolling motion completely stopped
		/// </summary>
		public delegate void StopPickerHandler(Type selectedItem);
		/// <summary>
		/// Stop motion event handler.
		/// </summary>
		public event StopPickerHandler StopPickerEvent;
		/// <summary>
		/// Sends rolling stopped event.
		/// </summary>
		private void sendStopEvent(Type selectedItem) {
			StopPickerEvent (selectedItem);
		}
	}
}
