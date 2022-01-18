using UnityEngine;
using UnityEngine.UI;
using Z.Frame;
using Z.Tool;

namespace Z.Game
{
	public class PlayerPanel : IPanel
	{
		private Text name;
		public void OnOpen(GameObject root)
		{
			this.name = root.Find("txt_name").GetComponent<Text>();
			this.name.text = DataManager.PlayerData.Name;
			var input = root.Find("input_name");
			input.GetComponent<InputField>().onEndEdit.AddListener((str) =>
			{
				this.name.text = str;
				input.SetActive(false);
			});
			root.Find("btn_name").GetComponent<Button>().onClick.AddListener(() =>
			{
				input.SetActive(!input.activeInHierarchy);
			});

			root.Find("txt_coin").GetComponent<Text>().text = DataManager.PlayerData.Coin.ToString();
			root.Find("txt_level").GetComponent<Text>().text = DataManager.PlayerData.Level.ToString();

			root.Find("btn_Import").GetComponent<Button>().onClick.AddListener(onImport);
			root.Find("btn_export").GetComponent<Button>().onClick.AddListener(onExport);
			root.Find("btn_exit").GetComponent<Button>().onClick.AddListener(() =>
			{
				QuitManager.OnQuit();
			});

			root.Find("btn_back").GetComponent<Button>().onClick.AddListener(() =>
			{
				ScriptConst.UIM.ClosePanel(this);
				ScriptConst.UIM.OpenPanel(new DiaryPanel());
			});
		}

		public void OnClose()
		{

		}

		/// <summary>
		/// 导入数据
		/// </summary>
		void onImport()
		{
			DataManager.ExcelData.ImportData();
		}

		/// <summary>
		/// 导出数据
		/// </summary>
		void onExport()
		{
			var fileName = FileData.Path.Substring(0, FileData.Path.Length - 1);
			ZipUtility.Zip(new string[] { fileName }, FileData.OutPath);
		}
	}

}
