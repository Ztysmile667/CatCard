using OfficeOpenXml;
using System.IO;
using UnityEngine;

public class ExcelTest : MonoBehaviour
{
    private ExcelPackage excelData;
    // Start is called before the first frame update
    void Start()
    {
        FileInfo fileinfo = new FileInfo("test");
        if (!Directory.Exists(fileinfo.DirectoryName))
        {
            excelData = ExcelTool.CreateExcel("test", new string[] { "first" });
        }
		else
		{
            excelData = ExcelTool.LoadExcel("test");

        }
        ExcelTool.AddData(excelData, 1, 1, 1, "first");
        ExcelTool.SaveExcel(excelData);
    }
}
