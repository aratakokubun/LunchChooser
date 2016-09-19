using System;
using SQLite.Net;
using System.Collections.Generic;
using Xamarin.Forms;
using Android.Util;

namespace LunchChooser
{
	/**
	 * Accessing Lunch Menu Database object<br>
	 * To access this class, use {@link getInstance} method.
	 */
	public class LunchMenuDao
	{
		private static LunchMenuDao instance = null;
		private static readonly object locker = new object();
		private static SQLiteConnection dbConn;

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

		/**
		 * Get lunch menus.
		 */
		public IEnumerable<LunchMenu> getLunchMenus()
		{
			lock (locker) {
				// Except deleted
				return dbConn.Table<LunchMenu>().Where(m => !m.deleted);
			}
		}

		/**
		 * Update lunch menu database element.
		 */
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

		/**
		 * Check if the menu is valid to update.
		 */
		private readonly String CHECK_EXIST_FORMAT = @"select count(id) from LunchMenu where id=?";
		private readonly String CHECK_DUPLICATE_FORMAT = @"select count(id) from LunchMenu where id<>? and name=?";
		public bool validateUpdateMenu(LunchMenu menu) {
			lock (locker) {
				SQLiteCommand cmd = dbConn.CreateCommand(CHECK_EXIST_FORMAT, menu.id);
				int existCount = Convert.ToInt32 (cmd.ExecuteScalar<Int32> ());

				cmd = dbConn.CreateCommand(CHECK_DUPLICATE_FORMAT, menu.id, menu.name);
				int dupliateCount = Convert.ToInt32 (cmd.ExecuteScalar<Int32> ());

				return (validateMenu (menu) && existCount == 1 && dupliateCount == 0);
			}
		}

		/**
		 * Insert new menu.
		 */
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

		/**
		 * Check if the new menu is valid to insert.
		 */
		private readonly String CHECK_NEW_FORMAT = @"select count(id) from LunchMenu where name=?";
		public bool validateNewMenu(LunchMenu menu) {
			lock (locker) {
				SQLiteCommand cmd = dbConn.CreateCommand(CHECK_NEW_FORMAT, menu.name);
				int count = Convert.ToInt32 (cmd.ExecuteScalar<Int32> ());

				return (validateMenu (menu) && count == 0);
			}
		}

		/**
		 * Check if the content of the menu is valid.
		 */
		public bool validateMenu(LunchMenu menu)
		{
			return menu.calory >= 0 && menu.cost >= 0 && !menu.deleted;
		}

		/**
		 * Insert Today's selected menu
		 */
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

		/**
		 * Check if the content of the new menu log is valid to insert.
		 */
		private readonly String CHECK_NEW_LOG_FORMAT = @"select count(id) from LunchMenu where created>=? and created<=?";
		public bool validateNewMenuLog(MenuLog menuLog) {
			lock (locker) {
				DateTime startDay = DateTimeUtils.getStartOfDay(menuLog.created);
				DateTime endDay = DateTimeUtils.getEndOfDay(menuLog.created);

				SQLiteCommand cmd = dbConn.CreateCommand(CHECK_NEW_LOG_FORMAT, startDay, endDay);
				int count = Convert.ToInt32 (cmd.ExecuteScalar<Int32> ());

				return (validateMenuLog (menuLog) && count == 0);
			}
		}

		/**
		 * Check if the content of the menu log is valid.
		 */
		private bool validateMenuLog(MenuLog menuLog) {
			return menuLog.calory >= 0 && menuLog.cost >= 0;
		}
	}
}

