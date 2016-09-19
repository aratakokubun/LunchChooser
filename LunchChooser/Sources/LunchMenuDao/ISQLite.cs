using System;
using SQLite.Net;

namespace LunchChooser
{
	public interface ISQLite
	{
		SQLiteConnection getConnection();
	}
}

