using UnityEngine;
using UnityEngine.UI;
using Z.Frame;
using Z.Tool;

namespace Z.Game 
{
	public class CalendarDatePanel : IPanel
	{
		private Text txt_Year;
		private Text txt_Month;
		private ScrollRect scroll;

		public void OnOpen(GameObject root)
		{
			root.Find("btn_Ok").GetComponent<Button>().onClick.AddListener(onClickOk);

			var year = root.Find("txt_Year");
			this.txt_Year = year.GetComponent<Text>();
			this.txt_Year.text = DataManager.calender.Year.ToString();
			year.Find("btn_left").GetComponent<Button>().onClick.AddListener(onClickYearLeft);
			year.Find("btn_right").GetComponent<Button>().onClick.AddListener(onClickYearRight);

			var month = root.Find("txt_Month");
			this.txt_Month = month.GetComponent<Text>();
			this.txt_Month.text = DataManager.calender.Month.ToString();
			month.Find("btn_left").GetComponent<Button>().onClick.AddListener(onClickMonthLeft);
			month.Find("btn_right").GetComponent<Button>().onClick.AddListener(onClickMonthRight);

			var scrollObj = root.Find("list_days");
			this.scroll = scrollObj.GetComponent<ScrollRect>();
			this.refreshDayList();
		}

		public void OnClose()
		{

		}

		/// <summary>
		/// 刷新日期列表
		/// </summary>
		void refreshDayList()
		{
			for (int i = 0; i < this.scroll.content.childCount; i++)
			{
				GameObject.Destroy(this.scroll.content.GetChild(i).gameObject);
			}

			for (int i = 0; i < DataManager.calender.CurrentMaxDay; i++)
			{
				var itemObj = Resources.Load("Data/UI/Com/day_item");
				if (itemObj == null) continue;

				var item = (GameObject)GameObject.Instantiate(itemObj, this.scroll.content.transform);
				item.GetComponentInChildren<Text>().text = (i + 1).ToString();
				var day = i + 1;
				item.GetComponent<Button>().onClick.AddListener(()=> 
				{
					DataManager.calender.ChangeDay(day);
				});
			}
		}

		void onClickOk()
		{
			//刷新文件数据
			var fileName = FileData.PathStr(FileData.Path, DataManager.calender.ToString());
			DataManager.ExcelData.InitExcelList(fileName);
			ScriptConst.UIM.ClosePanel(this);
			ScriptConst.UIM.OpenPanel(new DiaryPanel());
		}

		void onClickYearLeft()
		{
			if(DataManager.calender.ChangeYear(-1))
			{
				this.txt_Year.text = DataManager.calender.Year.ToString();
				this.txt_Month.text = DataManager.calender.Month.ToString();
				refreshDayList();
			}
		}

		void onClickYearRight()
		{
			if (DataManager.calender.ChangeYear(1))
			{
				this.txt_Year.text = DataManager.calender.Year.ToString();
				this.txt_Month.text = DataManager.calender.Month.ToString();
				refreshDayList();
			}
		}

		void onClickMonthLeft()
		{
			if (DataManager.calender.ChangeMonth(-1))
			{
				this.txt_Month.text = DataManager.calender.Month.ToString();
				refreshDayList();
			}
		}

		void onClickMonthRight()
		{
			if(DataManager.calender.ChangeMonth(1))
			{
				this.txt_Month.text = DataManager.calender.Month.ToString();
				refreshDayList();
			}
		}
	}
}


