using OfficeOpenXml;
using System.IO;
using System.Linq;
using UnityEngine;

public class ExcelTest : MonoBehaviour
{
    private ExcelPackage excelData;
    // Start is called before the first frame update
    void Start()
    {
        FileInfo fileinfo = new FileInfo(@"C:\Users\82362\Desktop\test");
        if (!File.Exists(fileinfo.FullName+".xlsx"))
        {
            excelData = ExcelTool.CreateExcel(@"C:\Users\82362\Desktop\test", new string[] { "first" });
        }
		else
		{
            excelData = ExcelTool.LoadExcel(@"C:\Users\82362\Desktop\test");

        }
        ExcelTool.AddData(excelData, 1, 1, "first1");
        ExcelTool.AddData(excelData, 1, 2, "first2");
        ExcelTool.AddData(excelData, 1, 3, "first3");
        ExcelTool.AddData(excelData, 1, 4, "first4");
        ExcelTool.AddData(excelData, 2, 1, "first11");
        ExcelTool.SaveExcel(excelData);

        var cells = excelData.Workbook.Worksheets[1].Cells;
        Debug.Log(excelData.Workbook.Worksheets[1].Dimension.Start.Column +"   "+ excelData.Workbook.Worksheets[1].Dimension.End.Column);
        Debug.Log(cells.Count());

        var start = excelData.Workbook.Worksheets[1].Dimension.Start.Column;
        var end = excelData.Workbook.Worksheets[1].Dimension.End.Column;


        for (int i = start; i <= end; i++)
		{
            Debug.Log(cells[1,i].Value);
		}

        //删除操作
        excelData.Workbook.Worksheets[1].DeleteRow(1);
        excelData.Save();

        var data = ExcelTool.CreateExcel(@"C:\Users\82362\Desktop\test1", new string[] { "first" });
        data.Workbook.Worksheets.Delete(1);
        data.Workbook.Worksheets.Add("first", excelData.Workbook.Worksheets[1]);
        data.Save();
    }
}
