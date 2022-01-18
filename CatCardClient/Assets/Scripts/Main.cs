using UnityEngine;
using Z.Frame;

namespace Z.Game
{
    public class Main : MonoBehaviour
    {
        private void Awake()
        {
            InitManager();

        }

        // Start is called before the first frame update
        void Start()
        {
            ScriptConst.UIM.OpenPanel(new MainPanel(), false, false);
        }

        public void InitManager()
        {
            ScriptConst.MSG = GameObject.Find("Message").GetComponent<Message>();
            ScriptConst.Game = GameObject.Find("[Game]").GetComponent<GameManager>();

            DataManager.Init();

            ScriptConst.CameraStage = new CameraStage();

            ScriptConst.UIM = new UIManager();
        }
    }

}
