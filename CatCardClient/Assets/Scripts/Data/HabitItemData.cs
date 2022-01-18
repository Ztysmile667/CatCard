using UnityEngine;
using Z.Frame;

namespace Z.Game
{
	public interface IItem
	{
		public ExcelData Data { get; set; }
		public void Save();
		public void Delect();
	}

	/// <summary>
	/// 习惯、临时数据结构
	/// </summary>
	public class HabitItemData: IItem
	{
		/// <summary>
		/// ID
		/// </summary>
		public int ID { get; set; }

		/// <summary>
		/// 描述
		/// </summary>
		public string Descirbe 
		{ 
 			get { return this.Data.Descirbe; } 
			set { this.Data.Descirbe = value; Save(); } 
		}

		/// <summary>
		/// 规定金币数量
		/// </summary>
		public int CoinNum
		{
			get { return this.Data.CoinNum; }
			set { this.Data.CoinNum = value; Save(); }
		}

		/// <summary>
		/// 类型
		/// </summary>
		public HabitType Type
		{
			get { return this.Data.Type; }
			set { this.Data.Type = value; Save(); }
		}

		/// <summary>
		/// 完成目标
		/// </summary>
		public int Target
		{
			get { return this.Data.Target; }
			set { this.Data.Target = value; Save(); }
		}

		/// <summary>
		/// 目标进度
		/// </summary>
		public int TargetProgress
		{
			get { return this.Data.TargetProgress; }
			set { this.Data.TargetProgress = value; Save(); }
		}

		/// <summary>
		/// 是否完成
		/// </summary>
		public bool IsEnd { get => TargetProgress >= Target; } 

		public ExcelData Data { get; set; }

		public HabitItemData(string descirbe, int coinNum, HabitType type, int target)
		{
			if (this.Data == null)
				this.Data = new ExcelData();

			this.Data.ID = this.Data.GetHashCode();
			this.Descirbe = descirbe;
			this.CoinNum = coinNum;
			this.Type = type;
			this.Target = target;
			this.TargetProgress = 0;

			DataManager.ExcelData.AddExcelData(this.Data);
		}

		public HabitItemData(ExcelData data)
		{
			this.Data = data;

			if (DataManager.ExcelData.IsNewDay)
				this.TargetProgress = 0;
		}

		/// <summary>
		/// 添加进度
		/// </summary>
		public void AddProgress()
		{
			if (TargetProgress >= Target)
				return;

			TargetProgress++;
			if (TargetProgress >= Target)
			{
				DataManager.PlayerData.ChangeExp(1);
				DataManager.PlayerData.ChangeCoin(this.CoinNum);
			}
		}

		public void Save()
		{
			if (this.Data == null)
				return;

			DataManager.ExcelData.SaveExcelData(this.Data);
		}

		public void Delect()
		{
			if (this.Data == null)
				return;

			DataManager.ExcelData.RemoveExcelData(this.Data);
		}
	}

	/// <summary>
	/// 奖励数据结构
	/// </summary>
	public class AwardItemData:IItem
	{
		/// <summary>
		/// ID
		/// </summary>
		public int ID { get; set; }

		/// <summary>
		/// 描述
		/// </summary>
		public string Descirbe
		{
			get { return this.Data.Descirbe; }
			set { this.Data.Descirbe = value; Save(); }
		}

		/// <summary>
		/// 规定金币数量
		/// </summary>
		public int CoinNum
		{
			get { return this.Data.CoinNum; }
			set { this.Data.CoinNum = value; Save(); }
		}

		/// <summary>
		/// 类型
		/// </summary>
		public HabitType Type
		{
			get { return this.Data.Type; }
			set { this.Data.Type = value; Save(); }
		}

		/// <summary>
		/// 奖励到期时间
		/// </summary>
		public long AwardDoneTime
		{
			get { return this.Data.AwardDoneTime; }
			set { this.Data.AwardDoneTime = value; Save(); }
		}

		/// <summary>
		/// 奖励以领取次数
		/// </summary>
		public int AwardGetCount
		{
			get { return this.Data.AwardGetCount; }
			set { this.Data.AwardGetCount = value; Save(); }
		}

		/// <summary>
		/// 可领取奖励数量
		/// </summary>
		public int AwardCount
		{
			get { return this.Data.AwardCount; }
			set { this.Data.AwardCount = value; Save(); }
		}

		/// <summary>
		/// 是否完成
		/// </summary>
		public bool IsEnd { get => AwardGetCount >= AwardCount; }

		/// <summary>
		/// 是否时间到了
		/// </summary>
		public bool IsTimeOver { get => AwardDoneTime <= TimeStamp.GetTimeStamp(); }

		public ExcelData Data { get; set; }

		public AwardItemData(string descirbe, int coinNum, HabitType type, int awardDoneTime, int awardCount)
		{
			if (this.Data == null)
				this.Data = new ExcelData();

			this.Data.ID = this.Data.GetHashCode();
			this.Descirbe = descirbe;
			this.CoinNum = coinNum;
			this.Type = type;
			this.AwardDoneTime = TimeStamp.GetTimeStamp()+(TimeStamp.OneDayToSecond * awardDoneTime);
			this.AwardGetCount = 0;
			this.AwardCount = awardCount;

			DataManager.ExcelData.AddExcelData(this.Data);
		}

		public AwardItemData(ExcelData data)
		{
			this.Data = data;
		}

		/// <summary>
		/// 领取奖励
		/// </summary>
		public void GetAward()
		{
			if (AwardGetCount >= AwardCount)
				return;

			if (DataManager.PlayerData.ChangeCoin(-this.CoinNum))
			{
				AwardGetCount++;
				if (AwardGetCount >= AwardCount)
					DataManager.PlayerData.ChangeExp(1);
			}
		}

		public void Save()
		{
			if (this.Data == null)
				return;

			DataManager.ExcelData.SaveExcelData(this.Data);
		}

		public void Delect()
		{
			if (this.Data == null)
				return;

			DataManager.ExcelData.RemoveExcelData(this.Data);
		}

		/// <summary>
		/// 得到剩余时间（大于一天显示天，小于显示小时）
		/// </summary>
		/// <returns></returns>
		public string GetRemineDay()
		{
			var remineTime = this.AwardDoneTime - TimeStamp.GetTimeStamp();
			var day = remineTime / TimeStamp.OneDayToSecond;
			if(day == 0)
			{
				var hour = remineTime / 3600;
				if(hour == 0)
				{
					var min = remineTime / 60;
					return $"{min}分";
				}
				else
				{
					return $"{hour}时";
				}
			}
			return $"{day}天";
		}
	}
}
