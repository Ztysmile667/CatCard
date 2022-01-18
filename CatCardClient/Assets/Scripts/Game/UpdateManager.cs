using System;
using System.Collections.Generic;
using UnityEngine;

namespace Z.Frame 
{
	public static class UpdateManager
	{
		/// <summary>
		/// 添加删除队列的时候用到，所有操作都在下一次调用时处理
		/// </summary>
		public static Dictionary<int, OnUpdateItem> AddOrRemoveQueue = new Dictionary<int, OnUpdateItem>();

		/// <summary>
		/// 处理帧调用对象
		/// </summary>
		public static Dictionary<int, OnUpdateItem> OnUpdateObjs = new Dictionary<int, OnUpdateItem>();

		static UpdateManager()
		{
			GameManager.BEAT_BY_UPDATE += OnUpdate;
		}

		static void OnUpdate()
		{
			if (AddOrRemoveQueue.Count > 0)
			{
				var it = AddOrRemoveQueue.GetEnumerator();
				while (it.MoveNext())
				{
					var value = it.Current.Value;
					//清理移除的对象
					if (value == null)
					{
						OnUpdateObjs.Remove(it.Current.Key);
					}
					else
					{
						if (value != null)
							OnUpdateObjs[it.Current.Key] = value;
					}
				}
				AddOrRemoveQueue.Clear();
			}

			if (OnUpdateObjs.Count > 0)
			{
				var it = OnUpdateObjs.GetEnumerator();
				while (it.MoveNext())
				{
					var item = it.Current.Value;
					if(item == null)
					{
						Debug.LogError("Update Item is Error."+ item.GetType());
						continue;
					}

					if (item.callTime > 0)
					{
						item.passTime += Time.deltaTime;
						if (item.passTime >= item.callTime)
						{
							item.callTime = 0;
							item.passTime = 0;
						}
					}

					if (item.passTime >= item.callTime)
					{
						try
						{
							float r = item.Excute();

							if (r >= 0)
								item.callTime = r;
							else
								AddOrRemoveQueue[it.Current.Key] = null;
						}
						catch (Exception ex)
						{
							//出错的话从列表中删除
							Debug.LogError("Execute OnUpdate() Error : " + item + "." + ex);
							AddOrRemoveQueue[it.Current.Key] = null;
						}
					}
				}
			}
		}

		/// <summary>
		/// 添加Update对象
		/// </summary>
		/// <param name="obj"></param>
		static public void Add(object obj)
		{
			if (obj == null)
				return;

			if (AddOrRemoveQueue.ContainsKey(obj.GetHashCode()))
				return;

			if (obj is IOnUpdate)
			{
				AddOrRemoveQueue[obj.GetHashCode()] = new MonoUpdateItem((IOnUpdate)obj);
			}
			else
				Debug.LogError("Unknow onUpdate() type: " + obj.GetType());
		}

		/// <summary>
		/// 移除Update对象
		/// </summary>
		/// <param name="obj"></param>
		static public void Remove(object obj)
		{
			if (obj == null)
				return;

			if (AddOrRemoveQueue.ContainsKey(obj.GetHashCode()))
				AddOrRemoveQueue[obj.GetHashCode()] = null;
		}
	}

	/// <summary>
	/// 每帧调用接口
	/// </summary>
	public interface IOnUpdate
	{
		/// <summary>
		/// 处理心跳的接口
		/// </summary>
		/// <returns></returns>
		float OnUpdate();
	}

	/// <summary>
	/// 用于管理OnFrame中的对象
	/// </summary>
	public class OnUpdateItem
	{
		public float callTime = -1; //下次要调用的时间
		public float passTime = 0;  //已经经过的时间

		virtual public float Excute()
		{
			return -1;
		}
	}

	public class MonoUpdateItem : OnUpdateItem
	{
		private WeakReference<IOnUpdate> _ref;
		public MonoUpdateItem(IOnUpdate obj)
		{
			this._ref = new WeakReference<IOnUpdate>(obj);
		}

		public override float Excute()
		{
			IOnUpdate obj;
			this._ref.TryGetTarget(out obj);
			if (obj == null)
				return -1;

			//如果是不可用的MonoBehaviour，不处理心跳
			//1秒后再进行检查
			if (obj is MonoBehaviour)
			{
				if ((MonoBehaviour)obj == null || ((MonoBehaviour)obj).gameObject == null)
					return -1;

				if (!((MonoBehaviour)obj).isActiveAndEnabled)
					return 1;
			}

			return obj.OnUpdate();
		}
	}

}


