using System.Collections.Generic;
using UnityEngine;

namespace Z.Game
{
	public class Message : MonoBehaviour
	{
		public delegate void MsgDelegate(Msg obj);
		private static Dictionary<string, MsgDelegate> m_Msg = new Dictionary<string, MsgDelegate>();

		private void Awake()
		{
			DontDestroyOnLoad(this.gameObject);
		}

		/// <summary>
		/// 添加消息
		/// </summary>
		/// <param name="key">消息名称</param>
		/// <param name="action">消息体</param>
		public static void AddListener(string key, MsgDelegate action)
		{
			if (!m_Msg.ContainsKey(key))
			{
				m_Msg.Add(key, action);
			}
			else
			{
				m_Msg[key] += action;
			}
		}

		/// <summary>
		/// 移除消息
		/// </summary>
		/// <param name="key">消息名称</param>
		/// <param name="action">消息体</param>
		public static void RemoveListener(string key, MsgDelegate action)
		{
			if (m_Msg.ContainsKey(key))
			{
				if (!m_Msg.ContainsValue(action))
				{
					Debug.LogError("不存在当前传递消息体，请检查");
					return;
				}
				m_Msg[key] -= action;
			}
		}

		/// <summary>
		/// 移除所有消息
		/// </summary>
		public static void RemoveAllListener()
		{
			m_Msg.Clear();
		}

		/// <summary>
		/// 发送消息
		/// </summary>
		/// <param name="key"></param>
		public static void SendMsg(string key)
		{
			if (m_Msg.ContainsKey(key))
			{
				m_Msg[key]?.Invoke(null);
			}
		}

		public static void SendMsg(string key, Msg msg)
		{
			if (m_Msg.ContainsKey(key))
			{
				m_Msg[key]?.Invoke(msg);
			}
		}
	}

	public class Msg
	{
		private Dictionary<string, object> data = new Dictionary<string, object>();

		/// <summary>
		/// 得到数据
		/// </summary>
		/// <returns></returns>
		public object GetData(string key)
		{
			if (data.ContainsKey(key))
				return data[key];

			return null;
		}

		/// <summary>
		/// 设置数据
		/// </summary>
		/// <param name="key"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		public Msg SetData(string key, object value)
		{
			if (data.ContainsKey(key))
			{
				Debug.LogError("重复添加消息Key！！");
			}
			else
			{
				data.Add(key, value);
				return this;
			}
			return null;
		}
	}
}

