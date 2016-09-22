using System;

namespace LunchChooser
{
	/// <summary>
	/// Arguments to pass motion parameters
	/// </summary>
	public class MotionArgs : EventArgs
	{
		private readonly float distance;
		private readonly float velocity;
		private readonly long elapsedTime;

		public MotionArgs (float distance, float velocity, long elapsedTime)
		{
			this.distance = distance;
			this.velocity = velocity;
			this.elapsedTime = elapsedTime;
		}

		public float getDistance() {
			return distance;
		}

		public float getVelocity() {
			return velocity;
		}

		public long getElapsedTime() {
			return elapsedTime;
		}
	}
}

