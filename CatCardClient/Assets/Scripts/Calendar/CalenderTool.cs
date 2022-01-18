using System;

namespace Z.Tool 
{
	/// <summary>
	/// 日历工具类
	/// </summary>
	public static class CalenderTool
	{
		/// <summary>
		/// 得到某个月有多少天
		/// </summary>
		/// <param name="year"></param>
		/// <param name="month"></param>
		/// <returns></returns>
		public static int GetDaysToMonth(int year, int month)
		{
			return DateTime.DaysInMonth(year, month);
		}

		/// <summary>
		/// 获取某一天是星期几
		/// </summary>
		/// <returns></returns>
		public static int GetWeekToDay(int year, int month, int day)
		{
			DateTime date = new DateTime(year,month,day);
			return (int)date.DayOfWeek;
		}
	}
}
