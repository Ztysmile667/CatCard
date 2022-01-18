using UnityEngine;
using UnityEngine.UI;
using Z.Frame;
using Z.Tool;

namespace Z.Game
{
	public class NewAwardItemPanel : IPanel
	{
		private InputField describe;
		private InputField coin;
		private InputField count;
		private InputField time;

		public void OnOpen(GameObject root)
		{
			this.describe = root.Find("describe").GetComponentInChildren<InputField>();
			this.coin = root.Find("coin").GetComponentInChildren<InputField>();
			this.count = root.Find("count").GetComponentInChildren<InputField>();
			this.time = root.Find("time").GetComponentInChildren<InputField>();

			root.Find("btn_Cancel").GetComponent<Button>().onClick.AddListener(OnClickCancel);
			root.Find("btn_Create").GetComponent<Button>().onClick.AddListener(OnClickCreate);
		}

		public void OnClose()
		{

		}

		void OnClickCancel()
		{
			ScriptConst.UIM.ClosePanel(this);
		}

		void OnClickCreate()
		{
			Debug.Log($"添加新的奖励：{this.describe.text}，{this.coin.text}，{this.count.text}，{this.time.text}");
			DataManager.HabitData.AddItem(this.describe.text, int.Parse(this.coin.text), HabitType.Award, int.Parse(this.time.text), int.Parse(this.count.text));
			ScriptConst.UIM.ClosePanel(this);
			Message.SendMsg(ScriptConst.MSG_OnChangeHabit);
		}

	}

}
