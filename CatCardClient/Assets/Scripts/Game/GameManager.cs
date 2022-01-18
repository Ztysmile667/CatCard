using System;
using System.Collections;
using UnityEngine;

namespace Z.Frame
{
	public class GameManager : MonoBehaviour
	{
		//各种事件
		static public Action BEAT_BY_FIXED_FRAME;
		static public Action BEAT_BY_PRE_UPDATE;//最先执行的
		static public Action BEAT_BY_UPDATE;
		static public Action BEAT_BY_POST_UPDATE;//最后执行的
		static public Action<bool> ON_ACTIVE;  //活动状态判断
		static public Action ON_EXITGAME;  //离开游戏

		private static bool IsInited = false;  //是否已经初始化
		private static bool IsExitGame = false;  //是否已经退出游戏
		private static bool IsActive = true;  //是否处于活动状态


		void Awake()
		{
			DontDestroyOnLoad(this.gameObject);
		}

		void Start()
		{

		}

		void Update()
		{
			//每帧事件的调用
			BEAT_BY_PRE_UPDATE?.Invoke();
			BEAT_BY_UPDATE?.Invoke();

			//结束后的处理
			BEAT_BY_POST_UPDATE?.Invoke();

			if (Application.platform == RuntimePlatform.Android && (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Home)))
				QuitManager.OnQuit();
		}

		public void FixedUpdate()
		{
			BEAT_BY_FIXED_FRAME?.Invoke();
		}

		void OnApplicationPause(bool pause)
		{
			if (IsActive != !pause)
			{
				IsActive = !pause;
				ON_ACTIVE?.Invoke(!pause);
			}
		}

		void OnApplicationFocus(bool focus)
		{
			if (IsActive != focus)
			{
				IsActive = focus;
				ON_ACTIVE?.Invoke(focus);
			}
		}

		public void OnApplicationQuit()
		{
			if (!IsExitGame)
			{
				IsExitGame = true;
				ON_EXITGAME?.Invoke();
			}
		}

		public void OnDestroy()
		{
			if (!IsExitGame)
			{
				IsExitGame = true;
				ON_EXITGAME?.Invoke();
			}
		}


		/// <summary>
		/// 延迟执行
		/// </summary>
		/// <param name="time"></param>
		/// <param name="fun"></param>
		public void DelayInvoke(float time, Action fun)
		{
			StartCoroutine(_DelayInvoke(time, fun));
		}

		private static IEnumerator _DelayInvoke(float time, Action fun)
		{
			yield return new WaitForSeconds(time);
			fun();
		}
	}

}
