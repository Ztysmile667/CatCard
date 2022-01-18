using UnityEngine;
using Z.Frame;

namespace Z.Game
{
	public class NewHabitItemPanel : NewHabitPanel
	{
		protected override void OnClickCreate()
		{
			Debug.Log($"添加新的习惯：{this.describe.text}，{this.coin.text}，{this.count.text}");
			DataManager.HabitData.AddItem(this.describe.text, int.Parse(this.coin.text), HabitType.Habit, int.Parse(this.count.text));
			ScriptConst.UIM.ClosePanel(this);
			Message.SendMsg(ScriptConst.MSG_OnChangeHabit);
		}
	}

}
