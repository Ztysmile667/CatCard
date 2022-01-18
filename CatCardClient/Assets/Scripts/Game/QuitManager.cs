using UnityEngine;
using Z.Game;

namespace Z.Frame
{
    public class QuitManager
    {
        static QuitManager()
        {
        }

        public static void OnQuit()
		{
            ScriptConst.UIM.OpenPanel(new QuitPanel(),false,true);
        }

        /// <summary>
        /// 退出游戏
        /// </summary>
        public static void Quit()
		{
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
			Application.Quit();
#endif
        }
    }

}
