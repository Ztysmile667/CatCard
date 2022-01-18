using OfficeOpenXml;
using System.IO;
using UnityEngine;

/*
 * 用到的一些索引都是从1开始的  不是从0
 */
public static class ExcelTool
{
    /// <summary>
    /// 创建一个Excel文件
    /// </summary>
    /// <param name="fileName">文件名</param>
    /// <param name="sheetNames">工作表名称</param>
    public static ExcelPackage CreateExcel(string fileName, string[] sheetNames)
	{
        FileInfo fileinfo = new FileInfo(fileName+".xlsx");
        if (!Directory.Exists(fileinfo.DirectoryName))
        {
            Directory.CreateDirectory(fileinfo.DirectoryName);
        }
		else
		{
			if (File.Exists(fileinfo.FullName))
			{
                return new ExcelPackage(fileinfo);
            }
		}

        ExcelPackage excelPackage = new ExcelPackage(fileinfo);

        AddSheet(excelPackage, sheetNames);
        excelPackage.Save();
        Debug.Log("创建Excel成功>>"+ fileinfo.FullName);
        return excelPackage;
    }

    /// <summary>
    /// 加载Excel文件
    /// </summary>
    /// <param name="fileName">文件名</param>
    /// <returns></returns>
    public static ExcelPackage LoadExcel(string fileName)
	{
        FileInfo fileInfo = new FileInfo(fileName+".xlsx");
        if (!Directory.Exists(fileInfo.DirectoryName))
        {
            Debug.LogError("加载表格失败，不存在此文件 "+ fileName);
            return null;
        }
        //通过excel表格的文件信息打开excel表格
        ExcelPackage excelPackage = new ExcelPackage(fileInfo);
        if(excelPackage == null)
		{
            Debug.LogError("Excel文件读取失败。name = "+ fileName);
            return null;
		}
        Debug.Log("加载Excel成功>>" + fileInfo.FullName);
        return excelPackage;
    }

    /// <summary>
    /// 添加数据
    /// </summary>
    /// <param name="package">文件</param>
    /// <param name="sheetIndex">工作页索引</param>
    /// <param name="row">行数</param>
    /// <param name="col">列数</param>
    /// <param name="value">赋值内容</param>
    public static void AddData(ExcelPackage package,int row,int col,string value, int sheetIndex = 1)
	{
        if (package == null)
		{
            Debug.LogError("Excel文件添加数据失败。文件不存在 = " + package.File.Name);
            return;
        }

        if(package.Workbook.Worksheets.Count< sheetIndex)
		{
            Debug.LogError("Excel文件添加数据失败。Sheet索引不对 = " + package.File.Name+",index = "+ sheetIndex);
            return;
        }

        var sheet = package.Workbook.Worksheets[sheetIndex];
        sheet.Cells[row, col].Value = value;
        Debug.Log("添加数据成功"+value);
    }

    /// <summary>
    /// 保存数据
    /// </summary>
    /// <param name="package"></param>
    public static void SaveExcel(ExcelPackage package)
    {
        if (package == null)
        {
            Debug.LogError("Excel文件添加数据失败。文件不存在 = " + package.File.Name);
            return;
        }

        package.Save();
        Debug.Log("保存Excel成功>>" + package.File.Name);
    }

    /// <summary>
    /// 添加工作页
    /// </summary>
    /// <param name="package"></param>
    /// <param name="sheetNames"></param>
    public static void AddSheet(ExcelPackage package, string[] sheetNames)
	{
        if (sheetNames == null)
            return;

		for (int i = 0; i < sheetNames.Length; i++)
        {
            package.Workbook.Worksheets.Add(sheetNames[i]);
            Debug.Log("添加Excel工作页>>" + sheetNames[i]);
        }
	}
}
