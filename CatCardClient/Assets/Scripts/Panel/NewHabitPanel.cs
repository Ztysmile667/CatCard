using UnityEngine;
using UnityEngine.UI;
using Z.Frame;
using Z.Tool;

namespace Z.Game
{
	public class NewHabitPanel : IPanel
	{
		protected InputField describe;
		protected InputField coin;
		protected InputField count;

		public void OnOpen(GameObject root)
		{
			this.describe = root.Find("describe").GetComponentInChildren<InputField>();
			this.coin = root.Find("coin").GetComponentInChildren<InputField>();
			this.count = root.Find("count").GetComponentInChildren<InputField>();

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

		protected virtual void OnClickCreate()
		{
		}
	}
}
