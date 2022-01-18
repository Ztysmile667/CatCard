using UnityEngine;

namespace Z.Frame
{
	public class CameraStage
	{
		public Camera Camera;

		public CameraStage()
		{
			//创建相机
			var cameraGO = new GameObject("2DCamera");
			GameObject.DontDestroyOnLoad(cameraGO);
			this.Camera = cameraGO.AddComponent<Camera>();
			cameraGO.transform.position = Vector3.zero;
			cameraGO.transform.localScale = Vector3.one;

			//设置相机属性
			this.Camera.orthographic = true;
			this.Camera.orthographicSize = 5;
			this.Camera.clearFlags = CameraClearFlags.Depth;
			this.Camera.nearClipPlane = 0.3f;
			this.Camera.farClipPlane = 1000f;
			this.Camera.cullingMask = 1 << LayerMask.NameToLayer("UI"); //只渲染第x层   
		}

	}
}

