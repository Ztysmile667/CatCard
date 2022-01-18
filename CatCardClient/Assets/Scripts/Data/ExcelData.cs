using OfficeOpenXml;
using System.Collections.Generic;
using UnityEngine;
using Z.Frame;
using static ZipUtility;

namespace Z.Game
{
	/// <summary>
	/// 管理游戏生成的Excel文件
	/// </summary>
	public class ExcelDataFile
	{
		//所有游戏数据
		private List<ExcelData> allExcelData = new List<ExcelData>();
			
		private ExcelPackage currentExcel;  //当前Excel文件数据

		//外界访问
		public List<ExcelData> AllData { get => allExcelData; }
		//是否是新的一天
		public bool IsNewDay = false;

		//用户数据
		public ExcelPackage PlayerExcel;
		public PlayerExcelData PlayerExcelData;

		public ExcelDataFile()
		{
			FileData.CreateFile();
		}

		public void Init()
		{
			var fileName = FileData.PathStr(FileData.Path, DataManager.calender.ToString());
			this.InitPlayerExcel();
			this.InitExcelList(fileName);
		}

		/// <summary>
		/// 初始化用户表格
		/// </summary>
		public void InitPlayerExcel()
		{
			this.PlayerExcel = FileData.LoadExcelFile(FileData.PathStr(FileData.Path, "PlayerData"), typeof(PlayerExcelData));
			if(PlayerExcel == null)
			{
				Debug.LogError("Load PlayerExcel Fail.");
				return;
			}

			this.PlayerExcelData = new PlayerExcelData();
			var cells = PlayerExcel.Workbook.Worksheets[1].Cells;
			PlayerExcelData.Coin = cells[2, 1].Value == null ? 0 :int.Parse(cells[2, 1].Value.ToString());
			PlayerExcelData.Level = cells[2, 2].Value == null ? 0 : int.Parse(cells[2, 2].Value.ToString());
			PlayerExcelData.Exp = cells[2, 3].Value == null ? 0 : int.Parse(cells[2, 3].Value.ToString());
			PlayerExcelData.Save(this.PlayerExcel);
		}

		/// <summary>
		/// 保存用户数据
		/// </summary>
		public void SavePlayerData()
		{
			if (PlayerExcelData != null)
				PlayerExcelData.Save(this.PlayerExcel);
		}

		/// <summary>
		/// 初始化所有游戏数据
		/// </summary>
		public void InitExcelList(string name)
		{
			this.currentExcel = FileData.LoadExcelFile(name, typeof(ExcelData));
			
			if (this.currentExcel == null)
				return;
			this.allExcelData.Clear();
			if(DataManager.HabitData!=null)
				DataManager.HabitData.Clear();

			//保存上一次的习惯和奖励
			if (DataManager.calender.IsToday && !string.IsNullOrEmpty(PlayerExcelData.LastExcelName) && !PlayerExcelData.LastExcelName.Equals(name))
			{
				//复制上一个表格的数据过来到新表中
				this.copyLastExcel();
				PlayerExcelData.LastExcelName = name;
				this.IsNewDay = true;
			}

			var sheet = currentExcel.Workbook.Worksheets[1];
			var start = sheet.Dimension.Start;
			var end = sheet.Dimension.End;

			for (int i = start.Row + 1; i <= end.Row; i++)
			{
				var data = new ExcelData();
				data.ID = int.Parse(sheet.Cells[i, 1].Value.ToString());
				data.Descirbe = sheet.Cells[i, 2].Value.ToString();
				data.CoinNum = int.Parse(sheet.Cells[i, 3].Value.ToString());
				data.Type = (HabitType)(int.Parse(sheet.Cells[i, 4].Value.ToString()));
				data.Target = int.Parse(sheet.Cells[i, 5].Value.ToString());
				data.TargetProgress = int.Parse(sheet.Cells[i, 6].Value.ToString());
				data.AwardDoneTime = long.Parse(sheet.Cells[i, 7].Value.ToString());
				data.AwardGetCount = int.Parse(sheet.Cells[i, 8].Value.ToString());
				data.AwardCount = int.Parse(sheet.Cells[i, 9].Value.ToString());

				this.allExcelData.Add(data);
			}
			Debug.Log("初始化allExcelData ：count = "+allExcelData.Count);
			Message.SendMsg(ScriptConst.MSG_OnChangeCalender);
		}

		//赋值上一次存储的Excel数据
		private void copyLastExcel()
		{
			var lastExcel = FileData.LoadExcelFile(PlayerExcelData.LastExcelName, typeof(ExcelData));
			if (lastExcel != null)
			{
				currentExcel.Workbook.Worksheets.Delete(1);
				currentExcel.Workbook.Worksheets.Add("first", lastExcel.Workbook.Worksheets[1]);

				//删除临时的的数据
				var startRow = currentExcel.Workbook.Worksheets[1].Dimension.Start.Row;
				var endRow = currentExcel.Workbook.Worksheets[1].Dimension.End.Row;
				if (startRow != endRow)
				{
					for (int i = startRow + 1; i <= endRow; i++)
					{
						var type = (HabitType)(int.Parse(currentExcel.Workbook.Worksheets[1].Cells[i, 4].Value.ToString()));
						if (type == HabitType.Interim)
						{
							currentExcel.Workbook.Worksheets[1].DeleteRow(i);
							i--;
						}
					}
				}
				currentExcel.Save();
			}
		}

		
		/// <summary>
		/// 添加表格数据
		/// </summary>
		/// <param name="data"></param>
		public void AddExcelData(ExcelData data)
		{
			if (data == null)
				return;

			if (allExcelData.Contains(data))
				return;

			allExcelData.Add(data);
			data.Save(currentExcel);
		}

		/// <summary>
		/// 保存数据
		/// </summary>
		public void SaveExcelData(ExcelData data)
		{
			if (data == null) return;
			data.Save(currentExcel);
		}

		/// <summary>
		/// 移除表格数据
		/// </summary>
		public void RemoveExcelData(ExcelData data)
		{
			if (data == null)
				return;

			if (!allExcelData.Contains(data))
				return;

			//删除表格数据
			currentExcel.Workbook.Worksheets[1].DeleteRow(GetRowByExcel(data));
			currentExcel.Save();
			allExcelData.Remove(data);
		}



		/// <summary>
		/// 通过ID找到ExcelData
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public ExcelData FindExcelDataByID(int id)
		{
			for (int i = 0; i < allExcelData.Count; i++)
			{
				if (allExcelData[i].ID == id)
					return allExcelData[i];
			}

			return null;
		}

		/// <summary>
		/// 得到指定数据在表中的行数
		/// </summary>
		/// <returns></returns>
		public int GetRowByExcel(ExcelData data)
		{
			var sheet = currentExcel.Workbook.Worksheets[1];
			var start = sheet.Dimension.Start;
			var end = sheet.Dimension.End;
			var index = 2;

			Debug.Log("end.Row = "+ end.Row);
			Debug.Log("data.id = "+ data.ID);
			for (int i = start.Row + 1; i <= end.Row; i++)
			{
				if (sheet.Cells[i, 1].Value.ToString() == data.ID.ToString())
					return index;
				index++;
			}
			return index;
		}

		/// <summary>
		/// 导入Zip数据
		/// </summary>
		public void ImportData()
		{
			FileData.ParseZipFile(new ImportZipCallback());
		}
	}

	public interface IExcelData
	{
		void Save(ExcelPackage excel);
	}
	/// <summary>
	/// 表格属性
	/// </summary>
	public class ExcelData: IExcelData
	{
		/// <summary>
		/// ID
		/// </summary>
		public int ID { get; set; }

		/// <summary>
		/// 描述
		/// </summary>
 		public string Descirbe { get; set; }

		/// <summary>
		/// 规定金币数量
		/// </summary>
		public int CoinNum { get; set; }

		/// <summary>
		/// 类型
		/// </summary>
		public HabitType Type { get; set; }

		/// <summary>
		/// 完成目标
		/// </summary>
		public int Target { get; set; }

		/// <summary>
		/// 目标进度
		/// </summary>
		public int TargetProgress { get; set; }

		/// <summary>
		/// 奖励到期时间
		/// </summary>
		public long AwardDoneTime { get; set; }

		/// <summary>
		/// 奖励以领取次数
		/// </summary>
		public int AwardGetCount { get; set; }

		/// <summary>
		/// 可领取奖励数量
		/// </summary>
		public int AwardCount { get; set; }

		public void Save(ExcelPackage excel)
		{
			if (excel == null)
				return;

			//ID!=0说明ID已经被赋值过了
			var row = DataManager.ExcelData.GetRowByExcel(this);
			Debug.Log("添加数据到第"+row+"行");
			Debug.Log("ExcelData id = "+this.ID);

			var sheet = excel.Workbook.Worksheets[1];
			sheet.Cells[row, 1].Value = ID;
			sheet.Cells[row, 2].Value = Descirbe;
			sheet.Cells[row, 3].Value = CoinNum;
			sheet.Cells[row, 4].Value = (int)Type;
			sheet.Cells[row, 5].Value = Target;
			sheet.Cells[row, 6].Value = TargetProgress;
			sheet.Cells[row, 7].Value = AwardDoneTime;
			sheet.Cells[row, 8].Value = AwardGetCount;
			sheet.Cells[row, 9].Value = AwardCount;

			excel.Save();
		}
	}

	public class PlayerExcelData: IExcelData
	{
		/// <summary>
		/// 金币数量
		/// </summary>
		public int Coin { get; set; }

		/// <summary>
		/// 人物等级
		/// </summary>
		public int Level { get; set; }

		/// <summary>
		/// 人物当前经验
		/// </summary>
		public int Exp { get; set; }

		/// <summary>
		/// 上一次获取的Excel名字
		/// </summary>
		public string LastExcelName
		{
			get { return PlayerPrefs.GetString("date"); }
			set { PlayerPrefs.SetString("date", value); Save(DataManager.ExcelData.PlayerExcel); }
		}

		public void Save(ExcelPackage excel)
		{
			if (excel == null)
				return;

			var row = 2;

			var sheet = excel.Workbook.Worksheets[1];
			sheet.Cells[row, 1].Value = Coin;
			sheet.Cells[row, 2].Value = Level;
			sheet.Cells[row, 3].Value = Exp;
			sheet.Cells[row, 4].Value = LastExcelName;

			excel.Save();
		}
	}

	/// <summary>
	/// 导入zip包之后的回调
	/// </summary>
	public class ImportZipCallback: UnzipCallback
	{
		public override void OnFinished(bool _result)
		{
			base.OnFinished(_result);
			if (!_result) return;

			var fileName = FileData.PathStr(FileData.Path, DataManager.calender.ToString());
			DataManager.ExcelData.InitPlayerExcel();
			DataManager.ExcelData.InitExcelList(fileName);
		}
	}
}

