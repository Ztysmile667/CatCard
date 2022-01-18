using UnityEngine;
using UnityEngine.UI;
using Z.Frame;
using Z.Tool;

namespace Z.Game
{
	public class RemoveHabitPanel : IPanel
	{
		private InputField describe;
		private InputField coin;
		private InputField count;
		private HabitItemData item;

		public RemoveHabitPanel(HabitItemData item)
		{
			this.item = item;
		}

		public void OnOpen(GameObject root)
		{
			this.describe = root.Find("describe").GetComponentInChildren<InputField>();
			this.describe.text = this.item.Descirbe;
			this.coin = root.Find("coin").GetComponentInChildren<InputField>();
			this.coin.text = this.item.CoinNum.ToString();
			this.count = root.Find("count").GetComponentInChildren<InputField>();
			this.count.text = this.item.Target.ToString();

			root.Find("btn_Cancel").GetComponent<Button>().onClick.AddListener(OnClickCancel);
			root.Find("btn_Change").GetComponent<Button>().onClick.AddListener(OnClickChange);
			root.Find("btn_Delect").GetComponent<Button>().onClick.AddListener(OnClickDelect);
		}

		public void OnClose()
		{

		}

		void OnClickCancel()
		{
			ScriptConst.UIM.ClosePanel(this);
		}

		void OnClickDelect()
		{
			if (this.item != null)
			{
				DataManager.HabitData.RemoveItem(this.item);
			}
				
			ScriptConst.UIM.ClosePanel(this);
		}

		void OnClickChange()
		{
			if(this.item != null) 
			{
				var data = (HabitItemData)this.item;
				data.Descirbe = this.describe.text;
				data.CoinNum = int.Parse(this.coin.text);
				data.Target = int.Parse(this.count.text);
			}
			
			ScriptConst.UIM.ClosePanel(this);
			Message.SendMsg(ScriptConst.MSG_OnChangeHabit);
		}
	}

}
