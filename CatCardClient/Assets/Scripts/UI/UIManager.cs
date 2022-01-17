using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class UIManager
{
	public GameObject Bg;//黑色遮罩
	public GameObject Root;//根物体
	public List<UIPanelInfo> PlanesList = new List<UIPanelInfo>();//所有打开的界面

	public Queue<IPanelCMD> PanelCmds = new Queue<IPanelCMD>();//界面命令队列

	public UIManager()
	{
		InitRoot();
		InitBG();
	}

	/// <summary>
	/// 初始化黑色背景
	/// </summary>
	public void InitBG()
	{
		var obj = Resources.Load<GameObject>("Data/UI/BlackBGPanel");
		this.Bg = GameObject.Instantiate(obj, this.Root.transform);
		this.Bg.name = "BlackBGPanel";

		this.Bg.SetActive(false);
	}

	/// <summary>
	/// 初始化根物体
	/// </summary>
	public void InitRoot()
	{
		var obj = Resources.Load<GameObject>("Data/UI/UI");
		var go = GameObject.Instantiate(obj);
		go.name = "UI";
		go.transform.position = Vector3.zero;
		go.transform.localScale = Vector3.one;

		this.Root = go.transform.Find("Canvas").gameObject;
		this.Root.GetComponent<Canvas>().worldCamera = ScriptConst.CameraStage.Camera;
	}

	/// <summary>
	/// 执行界面命令
	/// </summary>
	/// <param name="cmd"></param>
	public void Excute(IPanelCMD cmd)
	{
		PanelCmds.Enqueue(cmd);
		//TODO: 放进update中
		
	}

	/// <summary>
	/// 打开一个界面
	/// </summary>
	/// <param name="panel"></param>
	/// <param name="isCloseLast"></param>
	/// <param name="needBg"></param>
	public void OpenPanel(object panel, bool isCloseLast = true, bool needBg = true)
	{
		if (panel == null) return;

		UIPanelInfo uiInfo = new UIPanelInfo(panel, isCloseLast, needBg);
		if (IsOpen(uiInfo)) return;

		this.Excute(new OpenPanelCmd(uiInfo));
	}

	/// <summary>
	/// 是否已经打开了界面
	/// </summary>
	public bool IsOpen(UIPanelInfo info)
	{
		for (int i = 0; i < PlanesList.Count; i++)
		{
			if (PlanesList[i].Name == info.Name)
				return true;
		}

		if (PanelCmds.Count > 0)
		{
			var it = this.PanelCmds.GetEnumerator();
			while (it.MoveNext())
			{
				if (it.Current.IsSame(info))
					return true;
			}
		}

		return false;
	}

	/// <summary>
	/// 关闭一个界面
	/// </summary>
	/// <param name="panel"></param>
	public void ClosePanel(object panel)
	{
		if (panel == null) return;

		this.Excute(new ClosePanelCmd(panel));
	}

	/// <summary>
	/// 找到指定类型的界面信息
	/// </summary>
	/// <param name="obj"></param>
	/// <returns></returns>
	public UIPanelInfo FindPanelInfo(object obj)
	{
		if (PlanesList == null || PlanesList.Count <= 0)
			return null;

		for (int i = 0; i < PlanesList.Count; i++)
		{
			if (PlanesList[i].Obj == obj)
				return PlanesList[i];
		}

		return null;
	}

	/// <summary>
	/// 找到最后一个界面
	/// </summary>
	/// <returns></returns>
	public UIPanelInfo FindLastPanelInfo()
	{
		if (PlanesList == null || PlanesList.Count <= 0)
			return null;

		return PlanesList[PlanesList.Count -1];
	}

	/// <summary>
	/// 加载UI预制件
	/// </summary>
	/// <returns></returns>
	public GameObject LoadUIRes(string name)
	{
		if(string.IsNullOrEmpty(name))
		{
			Debug.LogError("Load UIPanel Error.Name is NUll.");
			return null;
		}
		return Resources.Load<GameObject>("Data/UI/" + name);
	}
}

#region UI信息类

/// <summary>
/// 界面分类
/// </summary>
public enum PanelType
{
	/// <summary>
	/// 背景
	/// </summary>
	BG,
	/// <summary>
	/// 界面
	/// </summary>
	Panel,
	/// <summary>
	/// 提示
	/// </summary>
	Tip,
	/// <summary>
	/// 效果
	/// </summary>
	Effect
}

/// <summary>
/// 界面信息管理
/// </summary>
public class UIPanelInfo : IPanel
{
	public GameObject PanelObj { get; private set; }
	public object Obj { get; private set; }

	public string Name;
	public bool IsCloseLast;
	public bool NeedBg;

	public UIPanelInfo(object obj, bool isCloseLast, bool needBg)
	{
		this.Obj = obj;
		this.IsCloseLast = isCloseLast;
		this.NeedBg = needBg;
		this.Name = ((System.Type)Obj).Name;
	}

	public void OnOpen(GameObject root)
	{
		this.PanelObj = root;

		if (this.Obj is IPanel)
		{
			((IPanel)this.Obj).OnOpen(root);
		}
		else
		{
			Debug.LogError("Cannot execute IDialog.OnCreate() with " + this.Obj);
		}
	}

	public void OnClose()
	{
		if (this.Obj is IPanel)
		{
			((IPanel)this.Obj).OnClose();
		}
		else
		{
			Debug.LogError("Cannot execute IDialog.OnClose() with " + this.Obj);
		}
	}

	public void Show(UIManager uim)
	{
		if (this.PanelObj != null)
			this.PanelObj.SetActive(true);

		if (NeedBg)
			uim.Bg.SetActive(true);
	}

	public void Hide(UIManager uim)
	{
		if (this.PanelObj != null)
			this.PanelObj.SetActive(false);

		if (NeedBg)
			uim.Bg.SetActive(false);

	}

	public override string ToString()
	{
		return $"Panel {this.Obj} Info : Name = {this.Name}, IsCloseLast = {this.IsCloseLast}, NeedBg = {this.NeedBg}";
	}
}
#endregion

#region 指令
/// <summary>
/// 页面相关命令接口
/// </summary>
public interface IPanelCMD
{
	/// <summary>
	/// 执行
	/// </summary>
	void Excute(UIManager uim);

	/// <summary>
	/// 检查是否处理完成
	/// </summary>
	bool Beat();

	/// <summary>
	/// 检查是否是对同一个界面进行操作
	/// </summary>
	/// <returns></returns>
	bool IsSame(UIPanelInfo info);
}

/// <summary>
/// 打开界面命令
/// </summary>
class OpenPanelCmd : IPanelCMD
{
	private UIPanelInfo info;
	private bool isEnd = false;

	public OpenPanelCmd(UIPanelInfo info)
	{
		this.info = info;
	}

	public void Excute(UIManager uim)
	{
		//关闭前一个界面
		var lastPanel = uim.FindLastPanelInfo();
		if(lastPanel != null && info.IsCloseLast)
		{
			lastPanel.Hide(uim);
		}

		//生成界面预制体
		var obj = uim.LoadUIRes(info.Name);
		if(obj == null)
		{
			Debug.LogError("Load UIRes Fail.Name = "+ info.Name);
			isEnd = true;
			return;
		}

		var go = GameObject.Instantiate(obj, uim.Root.transform);
		go.transform.localScale = Vector3.zero;

		info.OnOpen(go);
		uim.PlanesList.Add(this.info);

		//播放音效
		//播放效果
		go.transform.DOScale(1, .2f).OnComplete(()=> { isEnd = true; });
	}

	public bool Beat()
	{
		if (info.PanelObj == null)
			return true;

		return isEnd;
	}

	public bool IsSame(UIPanelInfo info)
	{
		if (info.Name == this.info.Name)
			return true;

		return false;
	}
}

/// <summary>
/// 关闭界面命令
/// </summary>
class ClosePanelCmd : IPanelCMD
{
	private bool isEnd = false;
	private object obj;

	public ClosePanelCmd(object obj)
	{
		this.obj = obj;
	}

	public void Excute(UIManager uim)
	{
		if (uim == null || obj == null)
			return;

		var info = uim.FindPanelInfo(obj);
		if (info == null || info.PanelObj == null)
			return;

		info.OnClose();
		uim.PlanesList.Remove(info);

		info.PanelObj.transform.DOScale(0, .2f).OnComplete(() =>
		 {
			 GameObject.Destroy(info.PanelObj);
			 isEnd = true;
		 });

		var lastPanel = uim.FindLastPanelInfo();
		if (lastPanel != null)
		{
			lastPanel.Show(uim);
		}
	}

	public bool Beat()
	{
		return isEnd;
	}

	public bool IsSame(UIPanelInfo info)
	{
		return false;
	}
}
#endregion
