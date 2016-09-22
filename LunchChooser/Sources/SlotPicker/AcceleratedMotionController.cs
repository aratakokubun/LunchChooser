using System;
using System.Threading.Tasks;
using Android.OS;

namespace LunchChooser
{
	/// <summary>
	/// Accelerated motion controller.
	/// You must add callback function to <code>RollingEvent</code> and <code>StopEvent</code>.
	/// </summary>
	public class AcceleratedMotionController
	{
		public static readonly short SLEEP_TIME = 20;

		// Motion parameters
		private float accel;
		private float deaccel;
		private float maxVelocity;

		// State of the motion
		enum MotionState : byte {
			stopped = 0,
			accelerating = 1,
			uniformed = 2,
			deaccelerating = 3
		};
		private volatile MotionState motionState = MotionState.stopped;

		/// <summary>
		/// Initializes a new instance of the <see cref="SlotPicker.AcceleratedMotionController"/> class.
		/// </summary>
		public AcceleratedMotionController ()
		{
		}

		/// <summary>
		/// Start accelerated motion specifying acceleration, deacceleration and max velocity.
		/// </summary>
		/// <returns><c>true</c>, if motion can be started (stopped state), <c>false</c> otherwise.</returns>
		/// <param name="accel">Acceleration on starting. MUST BE POSITIVE</param>
		/// <param name="deaccel">Deacceleration on stopping. MUST BE NEGATIVE</param>
		/// <param name="maxVelocity">Max velocity. MUST BE POSITIVE</param>
		/// <param name="initVelocity">Initialize velocity.</param>
		public bool startMotion(float accel, float deaccel, float maxVelocity, float initVelocity = 0.0f) {
			if (motionState.Equals(MotionState.stopped)) {
				this.accel = accel;
				this.deaccel = deaccel;
				this.maxVelocity = maxVelocity;
				if (!validateParameter (initVelocity)) {
					// Invalid Parameter
					return false;
				}

				this.motionState = MotionState.accelerating;

				startMotionTask(initVelocity);
				return true;
			} else {
				return false;
			}
		}

		/// <summary>
		/// Starts the motion task.
		/// </summary>
		/// <param name="initVelocity">Init velocity.</param>
		private void startMotionTask(float initVelocity) {
			Task.Run(() => motionLoop(initVelocity));
		}

		private bool validateParameter(float initVelocity) {
			return accel > 0.0f && deaccel < 0.0f && maxVelocity > initVelocity;
		}

		/// <summary>
		/// Stops the motion.
		/// </summary>
		/// <returns><c>true</c>, if motion can be stoped (not accelerating nor deaccelerating), <c>false</c> otherwise.</returns>
		public bool stopMotion() {
			if (motionState.Equals(MotionState.uniformed)) {
				this.motionState = MotionState.deaccelerating;
				return false;
			} else {
				return true;
			}
		}

		/// <summary>
		/// Execute motion loop thread
		/// </summary>
		/// <param name="initVelocity">Initialize velocity.</param>
		private void motionLoop(float initVelocity)
		{
			float velocity = initVelocity;
			float sumDistance = 0.0f;
			long preFrameTime = SystemClock.CurrentThreadTimeMillis();
			long elapsedTime = 0L;

			while (!motionState.Equals(MotionState.stopped)) {
				// Sleep
				long now = SystemClock.CurrentThreadTimeMillis ();
				short sleep = (short)Math.Max(SLEEP_TIME - (now - preFrameTime), 0);
				System.Threading.Thread.Sleep (sleep);
				preFrameTime = now;

				// Update
				accelerate (ref velocity);
				deaccelerate (ref velocity);

				float distance = velocity * SLEEP_TIME;
				sumDistance += distance;
				elapsedTime += SLEEP_TIME;
				sendMotionEvent (distance, velocity, elapsedTime);
			}

			sendStopEvent (elapsedTime);
		}

		/// <summary>
		/// Accelerate the specified velocity.
		/// </summary>
		/// <param name="velocity">Reference to current Velocity.</param>
		private void accelerate(ref float velocity) {
			if (motionState.Equals(MotionState.accelerating)) {
				velocity += accel;
				if (velocity >= maxVelocity) {
					velocity = maxVelocity;
					motionState = MotionState.uniformed;
				}
			}
		}

		/// <summary>
		/// Deaccelerate the specified velocity.
		/// </summary>
		/// <param name="velocity">Velocity.</param>
		private void deaccelerate(ref float velocity) {
			if (motionState.Equals(MotionState.deaccelerating)) {
				velocity += deaccel;
				if (velocity <= 0) {
					velocity = 0;
					motionState = MotionState.stopped;
				}
			}
		}

		/// <summary>
		/// Get if the motion is moving.
		/// </summary>
		/// <returns><c>true</c>, if moving, <c>false</c> otherwise.</returns>
		public bool isMoving() {
			return !motionState.Equals (MotionState.stopped);
		}
			
		/// <summary>
		/// Delegate of event handler called on each motipn frame
		/// </summary>
		public delegate void MotionEventHandler(MotionArgs eventArgs);
		/// <summary>
		/// Motion event handler
		/// </summary>
		public event MotionEventHandler RollingEvent;
		/// <summary>
		/// Send motion event.
		/// </summary>
		/// <param name="distance">Moved distance in this frame.</param>
		/// <param name="velocity">Velocity in this frame</param>
		/// <param name="elapsedTime">Elapsed time from start.</param>
		private void sendMotionEvent(float distance, float velocity, long elapsedTime) {
			RollingEvent(new MotionArgs(distance, velocity, elapsedTime));
		}

		/// <summary>
		/// Delegate of event handler called when motion completely stopped
		/// </summary>
		public delegate void StopEventHandler(long elapsedTime);
		/// <summary>
		/// Stop motion event handler.
		/// </summary>
		public event StopEventHandler StopEvent;
		/// <summary>
		/// Sends stopped event.
		/// </summary>
		/// <param name="elapsedTime">Elapsed time.</param>
		private void sendStopEvent(long elapsedTime) {
			StopEvent (elapsedTime);
		}
	}
}
