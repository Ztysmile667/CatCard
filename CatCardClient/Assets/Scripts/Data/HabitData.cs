using System.Collections.Generic;
using UnityEngine;
using Z.Frame;

namespace Z.Game
{
	public class HabitData :IOnUpdate
	{
		public List<IItem> ItemList = new List<IItem>();
		private HabitType currentType = HabitType.Habit;
		public HabitType Type => currentType;

		public HabitData()
		{
			init();
			UpdateManager.Add(this);
			GameManager.ON_EXITGAME += () => 
			{
				UpdateManager.Remove(this);
				Message.RemoveListener(ScriptConst.MSG_OnChangeCalender, onChangeCalender);
			};
			Message.AddListener(ScriptConst.MSG_OnChangeCalender,onChangeCalender);
		}

		private void onChangeCalender(Msg obj)
		{
			init();
		}

		public void init()
		{ 
			//加载Items列表
			for (int i = 0; i < DataManager.ExcelData.AllData.Count; i++)
			{
				var data = DataManager.ExcelData.AllData[i];
				if (data == null)
					return;

				IItem item = null;
				if(data.Type == HabitType.Award)
				{
					item = new AwardItemData(data);
				}
				else
				{
					item = new HabitItemData(data);
				}
				ItemList.Add(item);
			}
			DataManager.ExcelData.IsNewDay = false;
		}

		public void AddItem(string descirbe, int coinNum, HabitType type, int target)
		{
			HabitItemData data = new HabitItemData(descirbe,coinNum,type,target);
			if (data != null)
				ItemList.Add(data);
			else
				Debug.LogError("Create HabitItemData Fail.");
		}
		public void AddItem(string descirbe, int coinNum, HabitType type, int awardDoneTime, int awardCount)
		{
			AwardItemData data = new AwardItemData(descirbe, coinNum, type, awardDoneTime,awardCount);
			if (data != null)
				ItemList.Add(data);
			else
				Debug.LogError("Create HabitItemData Fail.");
		}

		/// <summary>
		/// 移除Item
		/// </summary>
		/// <param name="item"></param>
		public void RemoveItem(IItem item)
		{
			if (item != null)
			{
				item.Delect();
				ItemList.Remove(item);
				Message.SendMsg(ScriptConst.MSG_OnChangeHabit);
			}
		}

		/// <summary>
		/// 得到当前所显示的Item
		/// </summary>
		/// <returns></returns>
		public List<IItem> GetItems()
		{
			switch (currentType)
			{
				case HabitType.None:
					return null;
				case HabitType.Habit:
					return getHabits();
				case HabitType.Interim:
					return getInterimHabits();
				case HabitType.Award:
					return getAwards();
				default:
					break;
			}
			return null;
		}

		/// <summary>
		/// 得到习惯类型的所有数据
		/// </summary>
		/// <returns></returns>
		private List<IItem> getHabits()
		{
			List<IItem> datas = new List<IItem>();
			for (int i = 0; i < ItemList.Count; i++)
			{
				if (ItemList[i] is HabitItemData)
				{
					if (((HabitItemData)ItemList[i]).Type == HabitType.Habit)
						datas.Add(ItemList[i]);
				}
			}
			return datas;
		}

		/// <summary>
		/// 得到临时类型的所有数据
		/// </summary>
		/// <returns></returns>
		private List<IItem> getAwards()
		{
			List<IItem> datas = new List<IItem>();
  			for (int i = 0; i < ItemList.Count; i++)
			{
				if (ItemList[i] is AwardItemData)
				{
					datas.Add(ItemList[i]);
				}
			}
			return datas;
		}

		/// <summary>
		/// 得到临时类型的所有数据
		/// </summary>
		/// <returns></returns>
		private List<IItem> getInterimHabits()
		{
			List<IItem> datas = new List<IItem>();
			for (int i = 0; i < ItemList.Count; i++)
			{
				if (ItemList[i] is HabitItemData)
				{
					if (((HabitItemData)ItemList[i]).Type == HabitType.Interim)
						datas.Add(ItemList[i]);
				}
			}
			return datas;
		}

		/// <summary>
		/// 改变当前选择习惯类型
		/// </summary>
		/// <param name="type"></param>
		public bool ChangeHabitType(HabitType type)
		{
			if (type == this.currentType)
				return false;

			this.currentType = type;
			return true;
		}

		public void Clear()
		{
			ItemList.Clear();
			currentType = HabitType.Habit;
		}

		public float OnUpdate()
		{
			//检查奖励有没有到时间的，清除掉
			var awards = getAwards();
			for (int i = 0; i < awards.Count; i++)
			{
				if (awards[i] == null)
					continue;

				if (((AwardItemData)awards[i]).IsTimeOver)
				{
					RemoveItem(awards[i]);
					i = 0;
				}
			}
			return 1;
		}
	}

}
