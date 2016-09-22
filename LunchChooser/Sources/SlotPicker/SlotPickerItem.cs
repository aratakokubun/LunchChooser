/**
 * Copyright © 2016 Arata Kokubun. All rights reserved.
 * Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
 * The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
 */
using System;

namespace LunchChooser {
	/**
	 * <summary>
	 * <para>Interface which must be implemented by items used in SlotPicker class</para>
	 * <para><see cref="SlotPicker"/>for more information about the functions.</para>
	 * </summary>
	 */
	public interface SlotPickerItem
	{
		/**
		 * <summary>
		 * Specify the string displayed in SlotPicker.
		 * </summary>
		 * <returns>Returns displayed string</returns>
		 */
		string getDisplayedValue ();
	}
}