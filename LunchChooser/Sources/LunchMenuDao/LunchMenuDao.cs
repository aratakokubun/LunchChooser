using System;
using SQLite.Net;
using System.Collections.Generic;
using Xamarin.Forms;
using Android.Util;

namespace LunchChooser
{
	/// <summary>
	/// Accessing Lunch Menu Database object<br>
	/// To access this class, use <see>getInstance</see> method.
	/// </summary>
	public class LunchMenuDao
	{
		private static LunchMenuDao instance = null;
		private static readonly object locker = new object();
		private static SQLiteConnection dbConn;

		/// <summary>
		/// Gets singleton instance of LunchMenuDao.
		/// </summary>
		/// <returns>Singleton instance</returns>
		public static LunchMenuDao getInstance() {
			lock (locker) {
				if (instance == null) {
					instance = new LunchMenuDao ();
				}
				return instance;
			}
		}

		private LunchMenuDao ()
		{
			dbConn = DependencyService.Get<ISQLite> ().getConnection ();
			dbConn.CreateTable<LunchMenu> ();
			dbConn.CreateTable<MenuLog> ();
		}

		/// <summary>
		/// Gets the lunch menus.
		/// </summary>
		/// <returns>Lunch menu Iterator</returns>
		public IEnumerable<LunchMenu> getLunchMenus()
		{
			lock (locker) {
				// Except deleted
				return dbConn.Table<LunchMenu>().Where(m => !m.deleted);
			}
		}

		/// <summary>
		/// Update lunch menu database element.
		/// </summary>
		/// <returns><c>true</c>, if menu format is correct. <c>false</c> otherwise.</returns>
		/// <param name="menu">Lunch menu data to update</param>
		public bool updateMenu(LunchMenu menu) {
			if (validateUpdateMenu (menu)) {
				lock (locker) {
					dbConn.Update (menu);
					return true;
				}
			} else {
				return false;
			}
		}

		private readonly String CHECK_EXIST_FORMAT = @"select count(id) from LunchMenu where id=?";
		private readonly String CHECK_DUPLICATE_FORMAT = @"select count(id) from LunchMenu where id<>? and name=?";
		/// <summary>
		/// Check if the menu is valid to update.
		/// </summary>
		/// <returns><c>true</c>, if update menu was validated, <c>false</c> otherwise.</returns>
		/// <param name="menu">Lunch Menu data to validate.</param>
		public bool validateUpdateMenu(LunchMenu menu) {
			lock (locker) {
				SQLiteCommand cmd = dbConn.CreateCommand(CHECK_EXIST_FORMAT, menu.id);
				int existCount = Convert.ToInt32 (cmd.ExecuteScalar<Int32> ());

				cmd = dbConn.CreateCommand(CHECK_DUPLICATE_FORMAT, menu.id, menu.name);
				int dupliateCount = Convert.ToInt32 (cmd.ExecuteScalar<Int32> ());

				return (validateMenu (menu) && existCount == 1 && dupliateCount == 0);
			}
		}

		/// <summary>
		/// Insert new menu.
		/// </summary>
		/// <returns><c>true</c>, if menu format is correct, <c>false</c> otherwise.</returns>
		/// <param name="menu">Lunch Menu data to insert.</param>
		public bool insertMenu(LunchMenu menu) {
			if (validateNewMenu (menu)) {
				lock (locker) {
					dbConn.Insert (menu);
					return true;
				}
			} else {
				return false;
			}
		}

		private readonly String CHECK_NEW_FORMAT = @"select count(id) from LunchMenu where name=?";
		/// <summary>
		/// Check if the menu is valid to insert.
		/// </summary>
		/// <returns><c>true</c>, if new menu was validated, <c>false</c> otherwise.</returns>
		/// <param name="menu">Lunch Menu data to validate.</param>
		public bool validateNewMenu(LunchMenu menu) {
			lock (locker) {
				SQLiteCommand cmd = dbConn.CreateCommand(CHECK_NEW_FORMAT, menu.name);
				int count = Convert.ToInt32 (cmd.ExecuteScalar<Int32> ());

				return (validateMenu (menu) && count == 0);
			}
		}

		/// <summary>
		/// Check if the content of the menu is valid.
		/// </summary>
		/// <returns><c>true</c>, if menu was validated, <c>false</c> otherwise.</returns>
		/// <param name="menu">Lunch menu data to validate.</param>
		public bool validateMenu(LunchMenu menu)
		{
			return menu.calory >= 0 && menu.cost >= 0 && !menu.deleted;
		}

		/// <summary>
		/// Check if the content of the menu is valid..
		/// </summary>
		/// <returns><c>true</c>, if new menu log format is correct, <c>false</c> otherwise.</returns>
		/// <param name="menuLog">Menu log data to insert.</param>
		public bool insertMenuLog(MenuLog menuLog) {
			if (validateNewMenuLog (menuLog)) {
				lock (locker) {
					dbConn.Insert (menuLog);
					return true;
				}
			} else {
				return false;
			}
		}

		private readonly String CHECK_NEW_LOG_FORMAT = @"select count(id) from LunchMenu where created>=? and created<=?";
		/// <summary>
		/// Check if the content of the new menu log is valid to insert.
		/// </summary>
		/// <returns><c>true</c>, if new menu log was validated, <c>false</c> otherwise.</returns>
		/// <param name="menuLog">Menu log data to validate.</param>
		public bool validateNewMenuLog(MenuLog menuLog) {
			lock (locker) {
				DateTime startDay = DateTimeUtils.getStartOfDay(menuLog.created);
				DateTime endDay = DateTimeUtils.getEndOfDay(menuLog.created);

				SQLiteCommand cmd = dbConn.CreateCommand(CHECK_NEW_LOG_FORMAT, startDay, endDay);
				int count = Convert.ToInt32 (cmd.ExecuteScalar<Int32> ());

				return (validateMenuLog (menuLog) && count == 0);
			}
		}

		/// <summary>
		/// Check if the content of the menu log is valid.
		/// </summary>
		/// <returns><c>true</c>, if menu log was validated, <c>false</c> otherwise.</returns>
		/// <param name="menuLog">Menu log data to validate.</param>
		private bool validateMenuLog(MenuLog menuLog) {
			return menuLog.calory >= 0 && menuLog.cost >= 0;
		}
	}
}

