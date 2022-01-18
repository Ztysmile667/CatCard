using UnityEngine;
using Z.Frame;

namespace Z.Game
{
	public class NewInterimItemPanel : NewHabitPanel
	{
		protected override void OnClickCreate()
		{
			Debug.Log($"添加新的临时任务：{this.describe.text}，{this.coin.text}，{this.count.text}");
			DataManager.HabitData.AddItem(this.describe.text, int.Parse(this.coin.text), HabitType.Interim, int.Parse(this.count.text));
			ScriptConst.UIM.ClosePanel(this);
			Message.SendMsg(ScriptConst.MSG_OnChangeHabit);
		}
	}
}
