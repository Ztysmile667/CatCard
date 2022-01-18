using UnityEngine;

namespace Z.Tool 
{
	public static class GameObjectEx
	{
		public static GameObject Find(this GameObject go, string name)
		{
			Transform obj = go.transform.Find(name);
			if (string.IsNullOrEmpty(name))
			{
				Debug.Log("GameObject Find Error. name is null. ");
				return null;
			}
			if (obj != null)
				return obj.gameObject;

			for (int i = 0; i < go.transform.childCount; i++)
			{
				var obj1 = go.transform.GetChild(i).gameObject.Find(name);
				if(obj1 == null)
					continue;

				obj = obj1.transform;
				if (obj != null)
					return obj.gameObject;
			}
			return null;
		}
	}
}


