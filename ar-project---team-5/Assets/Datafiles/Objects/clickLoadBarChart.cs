﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class clickLoadBarChart : MonoBehaviour
{

    void OnMouseDown()
    {
        GameObject DeptTargetObj = this.transform.parent.parent.parent.gameObject;
        GameObject DeptOverview = DeptTargetObj.transform.Find("DeptOverview").gameObject;
        GameObject backContainer = DeptTargetObj.transform.Find("BackBarContainer").gameObject;

        string name = "";
        //if (DeptTargetObj.name.Contains("HR"))
        //{
        //    name = "HR";
        //}
        //else if (DeptTargetObj.name.Contains("Engineering"))
        //{
        //    name = "Eng";

        //}
        //else if (DeptTargetObj.name.Contains("Marketing"))
        //{
        //    name = "Mar";
        //}

        //name = "Pie3D" + name;

        GameObject BarChart = DeptTargetObj.transform.Find("Bar3DMultiple").gameObject;
        setCanvasGroup(0, DeptOverview);
        setToView(BarChart);
        setToView(backContainer);
        setToHidden(DeptOverview);

    }



    void setCanvasGroup(float alpha, GameObject o)
    {
        CanvasGroup canvas = o.GetComponent("CanvasGroup") as CanvasGroup;
        canvas.alpha = alpha;
    }

    void setToView(GameObject o)
    {
        if (o.name == "BackBarContainer")
        {
            o.transform.localPosition = new Vector3(-0.5f, 0, 0.3f);
        }
        else
        {
            o.transform.localPosition = new Vector3(0, 0, -0.04f);
        }
    }

    void setToHidden(GameObject o)
    {
        o.transform.localPosition = new Vector3(0, 0, -4);
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
