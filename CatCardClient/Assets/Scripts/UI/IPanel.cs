using UnityEngine;

namespace Z.Frame{
    /// <summary>
    /// panel基类
    /// </summary>
    public interface IPanel
    {
        /// <summary>
        /// 打开
        /// </summary>
        void OnOpen(GameObject root);

        /// <summary>
        /// 关闭
        /// </summary>
        void OnClose();
    }

}
