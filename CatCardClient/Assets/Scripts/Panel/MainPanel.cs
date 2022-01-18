using UnityEngine;
using UnityEngine.UI;
using Z.Frame;
using Z.Tool;

namespace Z.Game
{
	public class MainPanel : IPanel
	{
		private GameObject root;
		public void OnOpen(GameObject root)
		{
			this.root = root;
			loadBG();
			ScriptConst.Game.DelayInvoke(3, () =>
			 {
				 ScriptConst.UIM.OpenPanel(new DiaryPanel());
			 });
		}

		public void OnClose()
		{

		}

		void loadBG()
		{
			var img = this.root.GetComponent<Image>();
			var range = Random.Range(1, 8);
			img.sprite = Resources.Load<Sprite>("Data/Img/bg" + range);
		}

	}
}

