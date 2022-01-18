using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Z.Frame;
using Z.Tool;

namespace Z.Game
{
	public class QuitPanel : IPanel
	{
		public void OnOpen(GameObject root)
		{
			root.Find("btn_Cancel").GetComponent<Button>().onClick.AddListener(onClickCancel);
			root.Find("btn_Quit").GetComponent<Button>().onClick.AddListener(onClickQuit);
		}

		public void OnClose()
		{
			
		}

		void onClickQuit()
		{
			QuitManager.Quit();
		}

		void onClickCancel()
		{
			ScriptConst.UIM.ClosePanel(this);
		}
	}
}
