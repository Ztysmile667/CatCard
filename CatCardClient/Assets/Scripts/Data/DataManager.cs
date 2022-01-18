namespace Z.Game 
{
	/// <summary>
	/// 习惯类型
	/// </summary>
	public enum HabitType
	{
		None = 0,
		/// <summary>
		/// 习惯
		/// </summary>
		Habit = 1,
		/// <summary>
		/// 临时
		/// </summary>
		Interim = 2,
		/// <summary>
		/// 奖励
		/// </summary>
		Award =3
	}
	/// <summary>
	/// 管理游戏中用到的数据
	/// </summary>
	public static class DataManager
	{
		/// <summary>
		/// 日历
		/// </summary>
		public static Calender calender;

		public static ExcelDataFile ExcelData;

		public static HabitData HabitData;

		public static PlayerData PlayerData;

		public static void Init()
		{
			calender = new Calender();
			calender.Init();

			ExcelData = new ExcelDataFile();
			ExcelData.Init();

			HabitData = new HabitData();

			PlayerData = new PlayerData();
		}
	}

}

