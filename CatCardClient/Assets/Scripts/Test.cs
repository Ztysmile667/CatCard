using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    GameObject go;
    // Start is called before the first frame update
    void Start()
    {
        go = new GameObject("132");
        //go.transform.DORotate(new Vector3(120, 0, 0), 2f);
        //Destroy(go);
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(go == null);
    }
}
