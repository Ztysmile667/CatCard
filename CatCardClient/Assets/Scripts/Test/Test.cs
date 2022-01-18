using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Z.Game;
using Z.Tool;

public class Test : MonoBehaviour
{
    GameObject go;
    // Start is called before the first frame update
    void Start()
    {
        //go = new GameObject("132");
        //go.transform.DORotate(new Vector3(120, 0, 0), 2f);
        //Destroy(go);

        //Debug.Log(CalenderTool.GetWeekToDay(2021,12,15));

        //Debug.Log(new DateTime(2021,10,31).ToString());

        var props = typeof(ExcelData).GetProperties();
        for (int i = 0; i < props.Length; i++)
        {
            Debug.Log(props[i].Name);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(go == null);
    }
}
