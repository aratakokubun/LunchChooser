using System;

namespace LunchChooser
{
	public static class DateTimeUtils
	{

		public static DateTime getStartOfDay(DateTime time) {
			return new DateTime (
	            time.Year,
	            time.Month,
	            time.Day,
	            0,
	            0,
	            0,
	            0);
		}

		public static DateTime getEndOfDay(DateTime time) {
			return new DateTime (
				time.Year,
				time.Month,
				time.Day,
				23,
				59,
				59,
				0);
		}
	}
}

