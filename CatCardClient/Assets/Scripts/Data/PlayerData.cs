using UnityEngine;
using Z.Frame;
using Z.Game;

public class PlayerData
{
	/// <summary>
	/// 金币数量
	/// </summary>
    public int Coin
	{
		get { return DataManager.ExcelData.PlayerExcelData.Coin; }
		set { DataManager.ExcelData.PlayerExcelData.Coin = value; DataManager.ExcelData.SavePlayerData(); }
	}

	/// <summary>
	/// 人物等级
	/// </summary>
	public int Level
	{
		get { return DataManager.ExcelData.PlayerExcelData.Level; }
		set { DataManager.ExcelData.PlayerExcelData.Level = value; DataManager.ExcelData.SavePlayerData(); }
	}

	/// <summary>
	/// 人物当前经验
	/// </summary>
	public int Exp
	{
		get { return DataManager.ExcelData.PlayerExcelData.Exp; }
		set { DataManager.ExcelData.PlayerExcelData.Exp = value; DataManager.ExcelData.SavePlayerData(); }
	}

	/// <summary>
	/// 用户名称
	/// </summary>
	public string Name
	{
		get 
		{
			var name = PlayerPrefs.GetString("playerName");
			if (string.IsNullOrEmpty(name))
				return "喵";
			return name;
		}
		set { PlayerPrefs.SetString("playerName", value); }
	}

	private int levelBase = 2;

	//得到当前等级下的最大经验
	private int getMaxExp()
	{
		return levelBase + Level * levelBase;
	}

	//升级
	private void levelUp()
	{
		Level++;
		Message.SendMsg(ScriptConst.MSG_OnLevelChange);
	}

	/// <summary>
	/// 添加经验
	/// </summary>
	/// <returns></returns>
	public bool ChangeExp(int value = 1)
	{
		if (value <= 0)
			return false;

		Exp += value;
		if(Exp>= getMaxExp())
		{
			var remineExp = Exp - getMaxExp();
			levelUp();
			Exp = 0;
			ChangeExp(remineExp);
		}
		Debug.Log("获得经验："+ value + ", 当前等级： "+Level);

		return true;
	}

	/// <summary>
	/// 添加经验
	/// </summary>
	/// <returns></returns>
	public bool ChangeCoin(int value)
	{
		if (Coin + value < 0)
		{
			Debug.Log("金币不足");
			return false;
		}

		Coin += value;
		Message.SendMsg(ScriptConst.MSG_OnCoinChange);
		Debug.Log("获得金币：" + value + ", 当前金币： " + Coin);
		return true;
	}
}
