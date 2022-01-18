using Z.Game;

namespace Z.Frame
{
    public static class ScriptConst
    {
        //屏幕基础比例
        public static int ScreenWidth = 1080;
        public static int ScreenHeight = 1920;

        //游戏组件
        public static CameraStage CameraStage;

        //UI钻进
        public static UIManager UIM;

        //Game
        public static GameManager Game;

        //Msg
        public static Message MSG;



        //MSG消息
        public const string MSG_OnChangeHabit = "OnChangeHabit";//当习惯发生改变
        public const string MSG_OnLevelChange = "OnLevelChange";//当等级发生改变
        public const string MSG_OnCoinChange = "OnCoinChange";//当金币发生改变
        public const string MSG_OnChangeCalender = "OnChangeCalender";//当改变日历
    }

}
