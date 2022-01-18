using UnityEngine;
using UnityEngine.UI;
using Z.Frame;
using Z.Tool;

namespace Z.Game
{
	public class DiaryPanel : IPanel
	{
		private Transform content;
		private Transform group;
		public void OnOpen(GameObject root)
		{
			var calender = root.Find("btn_Calender");
			calender.GetComponent<Button>().onClick.AddListener(this.onClickCalender);
			calender.Find("Text").GetComponent<Text>().text = DataManager.calender.Day.ToString();
			root.Find("btn_Account").GetComponent<Button>().onClick.AddListener(this.onClickAccount);

			this.content = root.Find("Content").transform;
			this.content.Find("btn_Add").GetComponent<ButtonEx>().OnClick.AddListener(this.onClickAdd);

			this.group = root.Find("toggleGroup").transform;
			this.setToggleEvent();

			this.loadItems();
			Message.AddListener(ScriptConst.MSG_OnChangeHabit,onHabitChange);
		}

		private void onHabitChange(Msg obj)
		{
			this.loadItems();
		}

		public void OnClose()
		{
			Message.RemoveListener(ScriptConst.MSG_OnChangeHabit, onHabitChange);
		}

		void onClickCalender()
		{
			ScriptConst.UIM.ClosePanel(this);
			ScriptConst.UIM.OpenPanel(new CalendarDatePanel());
		}

		void onClickAccount()
		{
			ScriptConst.UIM.ClosePanel(this);
			ScriptConst.UIM.OpenPanel(new PlayerPanel());
		}

		void onClickAdd()
		{
			if (!DataManager.calender.IsToday)
				return;

			if (DataManager.HabitData.Type == HabitType.Award)
			{
				ScriptConst.UIM.OpenPanel(new NewAwardItemPanel(), false, true);
			}
			else if(DataManager.HabitData.Type == HabitType.Interim)
			{
				ScriptConst.UIM.OpenPanel(new NewInterimItemPanel(), false, true);
			}
			else
				ScriptConst.UIM.OpenPanel(new NewHabitItemPanel(),false,true);
		}

		/// <summary>
		/// 设置toggle切换设置
		/// </summary>
		void setToggleEvent()
		{
			if (this.group == null)
				return;
			
			for (int i = 0; i < this.group.childCount; i++)
			{
				var toggle = this.group.GetChild(i).GetComponent<Toggle>();
				var index = i;
				toggle.onValueChanged.AddListener((bol)=> 
				{
					var type = (HabitType)(index+1);
					if (bol)
					{
						if (DataManager.HabitData.ChangeHabitType(type))
						{
							//刷新界面
							loadItems();
						}
					}

				});
			}
		}

		void loadItems()
		{
			//先删除所有Item
			for (int i = 0; i < content.childCount -1; i++)
			{
				GameObject.Destroy(content.GetChild(i).gameObject);
			}

			//加载item
			var items = DataManager.HabitData.GetItems();
			for (int i = 0; i < items.Count; i++)
			{
				var itemData = items[i];
				if(DataManager.HabitData.Type == HabitType.Award)
				{
					loadAwardItem((AwardItemData)itemData);
				}
				else
				{
					loadHabitItem((HabitItemData)itemData);
				}
			}
		}

		///加载习惯Item
		void loadHabitItem(HabitItemData habit)
		{
			var item = (GameObject)GameObject.Instantiate(Resources.Load("Data/UI/Com/habit_item"), this.content);
			item.transform.SetAsFirstSibling();

			//属性赋值
   			item.Find("txt_desc").GetComponent<Text>().text = habit.Descirbe;
			item.Find("txt_coin").GetComponent<Text>().text = habit.CoinNum.ToString();
			item.Find("txt_pro").GetComponent<Text>().text = habit.TargetProgress.ToString() + "/" + habit.Target.ToString();

			item.GetComponent<ButtonEx>().OnLongPress.AddListener(() =>
			{
				if (!DataManager.calender.IsToday)
					return;

				ScriptConst.UIM.OpenPanel(new RemoveHabitPanel(habit), false, true);
			});
			item.GetComponent<ButtonEx>().OnDoubleClick.AddListener(() =>
			{
				if (!DataManager.calender.IsToday)
					return;
				habit.AddProgress();
				this.loadItems();
			});
		}

		///加载奖励Item
		void loadAwardItem(AwardItemData award)
		{
			var item = (GameObject)GameObject.Instantiate(Resources.Load("Data/UI/Com/award_item"), this.content);
			item.transform.SetAsFirstSibling();

			//属性赋值
			item.Find("txt_desc").GetComponent<Text>().text = award.Descirbe;
			item.Find("txt_coin").GetComponent<Text>().text = award.CoinNum.ToString();
			item.Find("txt_pro").GetComponent<Text>().text = award.AwardGetCount.ToString() + "/" + award.AwardCount.ToString();
			item.Find("txt_time").GetComponent<Text>().text = $"剩余时间:{award.GetRemineDay()}";

			item.GetComponent<ButtonEx>().OnLongPress.AddListener(() =>
			{
				if (!DataManager.calender.IsToday)
					return;
				ScriptConst.UIM.OpenPanel(new RemoveAwardItemPanel(award), false, true);
			});
			item.GetComponent<ButtonEx>().OnDoubleClick.AddListener(() =>
			{
				if (!DataManager.calender.IsToday)
					return;
				award.GetAward();
				this.loadItems();
			});
		}
	}

}
