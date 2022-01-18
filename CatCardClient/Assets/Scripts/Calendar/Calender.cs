using System;
using UnityEngine;
using Z.Tool;

namespace Z.Game 
{
    public class Calender
    {
        /// <summary>
        /// 当前选择年份
        /// </summary>
        public int Year { get; private set; }

        /// <summary>
        /// 当前选择月份
        /// </summary>
        public int Month { get; private set; }

        /// <summary>
        /// 当前选择日
        /// </summary>
        public int Day { get; private set; }

        /// <summary>
        /// 当前月份天数
        /// </summary>
        public int CurrentMaxDay { get; private set; }

        /// <summary>
        /// 当前数据中的时间是否是今天
        /// </summary>
        public bool IsToday { get => this.Year == DateTime.UtcNow.Year && this.Month == DateTime.UtcNow.Month && this.Day == DateTime.UtcNow.Day; }


        public void Init()
        {
            Year = DateTime.UtcNow.Year;
            Month = DateTime.UtcNow.Month;
            Day = DateTime.UtcNow.Day;
            this.RefreshMaxDayCount();
        }

        /// <summary>
        /// 修改年份
        /// </summary>
        public bool ChangeYear(int value)
        {
            if (this.Year + value <= 0 || this.Year + value >= 10000)
                return false;

            if (this.Year + value > DateTime.UtcNow.Year)
                return false;

            this.Year += value;
            this.RefreshMaxDayCount();
            //发送消息通知别人改变了

            return true;
        }

        /// <summary>
        /// 改变月份
        /// </summary>
        public bool ChangeMonth(int value)
        {
            if (this.Month + value < 1 || this.Month > 12)
                return false;

            if (this.Month + value > DateTime.UtcNow.Month)
                return false;

            this.Month += value;
            this.RefreshMaxDayCount();
            //发送消息通知别人改变了

            return true;
        }

        /// <summary>
        /// 改变日期
        /// </summary>
        public bool ChangeDay(int value)
        {
            if (value < 1 || value > this.CurrentMaxDay)
                return false;

            if (value > DateTime.UtcNow.Day)
                return false;

            this.Day = value;

            //发送消息通知别人改变了
            return true;
        }

        /// <summary>
        /// 刷新当前月中的最大天数
        /// </summary>
        public void RefreshMaxDayCount()
        {
            this.CurrentMaxDay = CalenderTool.GetDaysToMonth(this.Year, this.Month);
        }

		public override string ToString()
		{
            return @$"{this.Year}/{this.Month}/{this.Day}";

        }

	}
}


