using OfficeOpenXml;
using System;
using System.IO;
using UnityEngine;
using static ZipUtility;

namespace Z.Game
{
    /// <summary>
    /// 管理游戏文件结构
    /// </summary>
	public static class FileData
    {
        public static string Path =
#if UNITY_EDITOR
		@"../CatExcelData/";
#elif UNITY_ANDROID
        @"../"+ Application.persistentDataPath+ "/CatExcelData/";
#else
		"";
#endif

		/// <summary>
		/// 导出目录
		/// </summary>
		public static string OutPath =
#if UNITY_EDITOR
		@"../CatData.zip";
#elif UNITY_ANDROID
        @"../"+ Application.persistentDataPath+ "/CatData.zip";
#else
		"";
#endif

		public static string PutPath =
#if UNITY_EDITOR
		@"../";
#elif UNITY_ANDROID
       @"../"+ Application.persistentDataPath;
#else
		"";
#endif

		/// <summary>
		/// 创建目录
		/// </summary>
		public static void CreateFile()
		{
			if(string.IsNullOrEmpty(Path))
			{
				Debug.LogError("File Path is Null.");
				return;
			}

			//年份目录
			var yearDir = PathStr(Path, DataManager.calender.Year.ToString());
			if (!Directory.Exists(yearDir))
				Directory.CreateDirectory(yearDir);

			//月份目录
			var MonthDir = PathStr(Path, DataManager.calender.Year.ToString(), DataManager.calender.Month.ToString());
			if (!Directory.Exists(MonthDir))
				Directory.CreateDirectory(MonthDir);
		}

		/// <summary>
		/// 加载Excel文件
		/// </summary>
		/// <returns></returns>
        public static ExcelPackage LoadExcelFile(string name, Type type)
		{
			if (!File.Exists(name+".xlsx"))
			{
				return CreateExcelFile(name, type);
			}
			else
				return ExcelTool.LoadExcel(name);
		}

		public static ExcelPackage CreateExcelFile(string name, Type type)
		{
			var excel = ExcelTool.CreateExcel(name, new string[] { "info" });
			var props = type.GetProperties();
			for (int i = 0; i < props.Length; i++)
			{
				ExcelTool.AddData(excel, 1, i+1, props[i].Name);
			}
			ExcelTool.SaveExcel(excel);
			return excel;
		}

		/// <summary>
		/// 拼接字符串
		/// </summary>
		/// <param name="path"></param>
		/// <returns></returns>
		public static string PathStr(params string[] path)
		{
			string str = "";
			for (int i = 0; i < path.Length; i++)
			{
				if (i != path.Length - 1)
					str += path[i] + "/";
				else
					str += path[i];
			}
			return @str;
		}

		/// <summary>
		/// 解压Zip文件
		/// </summary>
		public static void ParseZipFile(UnzipCallback _unzipCallback = null)
		{
			//先删除现有的文件
			var path = Path.Substring(0, Path.Length-1);
			if (Directory.Exists(path))
				Directory.Delete(path,true);

			UnzipFile(OutPath, PutPath, null, _unzipCallback);
		}
	}
}
