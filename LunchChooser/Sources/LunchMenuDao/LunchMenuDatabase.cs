using System;
using SQLite.Net;
using System.IO;
using Xamarin.Forms;

[assembly: Dependency (typeof(LunchChooser.LunchMenuDatabase))]
namespace LunchChooser
{
	public class LunchMenuDatabase : ISQLite
	{
		private const string SQLITE_FILE_NAME = "LunchMenu.db3";

		public LunchMenuDatabase ()
		{
		}
			
		#region ISQLite implementation

		public SQLiteConnection getConnection() {
			string documentsPath = System.Environment.GetFolderPath (System.Environment.SpecialFolder.Personal);
			var path = Path.Combine (documentsPath, SQLITE_FILE_NAME);
			var plat = new SQLite.Net.Platform.XamarinAndroid.SQLitePlatformAndroid();
			var conn = new SQLiteConnection (plat, path);
			return conn;
		}

		#endregion
	}
}

