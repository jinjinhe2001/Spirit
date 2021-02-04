﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public List<GameObject> buttonList = new List<GameObject>();
    public List<GameObject> panelList = new List<GameObject>();
    public List<GameObject> confirmList = new List<GameObject>();
    public List<GameObject> quickSetList = new List<GameObject>();
    public List<Image> gridList = new List<Image>();
    public List<BaseFragment> fragmentsOnDisc;
    public Slider status;
    public Slider statusSet;
    public float statusValue, setValue;//参数/调整初值（连续变化）
    public float setValue2;//调整数值（离散）
    public float parameter;//倍数（连续变化）
    float valueChange;
    int currentUI;//当前打开UI在list中的序号
    int num = 12;//UI总数
    bool isOpen = false;
    bool isLast = false;

    UIManager instance = null;
    private void Awake()
    {
        if (instance != null)
            Destroy(this);
        instance = this;
    }
    public void CloseFactory()//关闭工厂panel
    {
        panelList[13].SetActive(false);
    }
    public void OpenFactory()//打开工厂panel
    {
        panelList[13].SetActive(true);
    }
    //打开/关闭panel
    public void OpenUI()
    {
        ///获取当前button并打开相应panel/打开总览
        if (!isOpen)
        {
            var btn = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
            if (btn.name == "Overview")
                panelList[currentUI].SetActive(true);
            else if (btn.name == "Storage")
            {
                panelList[12].SetActive(true);
            }
            else if (btn.GetComponent<UIObject>().isUse)
            {
                for (int i = 0; i < num; i++)
                {
                    if (btn.name + "Panel" == panelList[i].name)
                    {
                        panelList[i].SetActive(true);
                        currentUI = i;
                    }
                }
            }
            isOpen = true;
            btn = null;
        }
        else
        {
            panelList[currentUI].SetActive(true);
        }
        ///若参数未确认，则设置参数初值
        if (!buttonList[currentUI].GetComponent<UIObject>().isConfirm && !panelList[12].activeSelf)
        {
            status = GameObject.FindWithTag("Status").GetComponent<Slider>();
            statusSet = GameObject.FindWithTag("StatusSet").GetComponent<Slider>();
            gridList.Clear();
            for (int i = 0; i < 10; i++)
            {
                Image tempImage = GameObject.FindWithTag("Grid").transform.GetChild(i).GetComponent<Image>();
                gridList.Add(tempImage);
                if (i < setValue2 * 10)
                {
                    gridList[i].color = new Color(0, 0, 0);
                }
            }
            status.value = statusValue;
            statusSet.value = setValue;
        }
    }
    public void CloseUI()
    {
        if (panelList[currentUI].activeSelf)
            panelList[currentUI].SetActive(false);
        else
        {
            panelList[12].SetActive(false);
        }
        isOpen = false;
    }
    //
    //确认并检查是否完成设置
    public void Confirm()
    {

        buttonList[currentUI].transform.GetComponent<UIObject>().isConfirm = true;
        buttonList[currentUI].GetComponent<Outline>().enabled = false;
        confirmList[currentUI].SetActive(false);
        quickSetList[currentUI].SetActive(false);
        CloseUI();
    }
    //
    //上一个/下一个panel
    public void Next()
    {
        panelList[currentUI].SetActive(false);
        currentUI++;
        if (currentUI > num - 1)
            currentUI = 0;
        while (!buttonList[currentUI].transform.GetComponent<UIObject>().isUse)
        {
            currentUI++;
            if (currentUI > num - 1)
                currentUI = 0;
        }
        OpenUI();
    }
    public void Previous()
    {
        panelList[currentUI].SetActive(false);
        currentUI--;
        if (currentUI < 0)
            currentUI = 11;
        while (!buttonList[currentUI].transform.GetComponent<UIObject>().isUse)
        {
            currentUI--;
            if (currentUI < 0)
                currentUI = 11;
        }
        OpenUI();
    }

    #region 初始化相关
    /// <summary>
    /// 初始化函数
    /// </summary>
    public void InitializeFactory(List<BaseFragment> newList)
    {
        panelList[13].SetActive(true);
        fragmentsOnDisc = newList;
        ResetState();
        ResetOutline();
    }
    public void ResetState()///初始化工厂
    {
        for (int i = 0; i < num; i++)
        {
            buttonList[i].GetComponent<UIObject>().isUse = false;
            buttonList[i].GetComponent<UIObject>().isConfirm = false;
        }
        for (int i = 0; i < num - 1; i++)
        {
            confirmList[i].SetActive(true);
            quickSetList[i].SetActive(true);
        }
        for (int i = 0; i < fragmentsOnDisc.Count; i++)
        {
            switch (fragmentsOnDisc[i].name)
            {
                case "原、辅料准备":
                    buttonList[0].GetComponent<UIObject>().isUse = true;
                    break;
                case "粉碎、润料":
                    buttonList[1].GetComponent<UIObject>().isUse = true;
                    break;
                case "配料":
                    buttonList[2].GetComponent<UIObject>().isUse = true;
                    break;
                case "蒸煮、摊凉":
                    buttonList[3].GetComponent<UIObject>().isUse = true;
                    break;
                case "修窖":
                case "制曲、入曲":
                case "发酵":
                case "加原/辅料":
                    buttonList[4].GetComponent<UIObject>().isUse = true;
                    break;
                case "上甑":
                    buttonList[5].GetComponent<UIObject>().isUse = true;
                    break;
                case "蒸馏":
                case "看花摘酒":
                    buttonList[6].GetComponent<UIObject>().isUse = true;
                    break;
                case "陈酿":
                    buttonList[7].GetComponent<UIObject>().isUse = true;
                    break;
                case "勾兑/勾调":
                    buttonList[8].GetComponent<UIObject>().isUse = true;
                    break;
                case "离心杀菌":
                    buttonList[9].GetComponent<UIObject>().isUse = true;
                    break;
                case "灌装":
                    buttonList[10].GetComponent<UIObject>().isUse = true;
                    break;
                case "鉴酒":
                    buttonList[11].GetComponent<UIObject>().isUse = true;
                    break;
            }
        }
    }
    public void ResetOutline()///初始化高亮
    {
        for (int i = 0; i < num; i++)
        {
            if (buttonList[i].GetComponent<UIObject>().isUse) buttonList[i].GetComponent<Outline>().enabled = true;
            else buttonList[i].GetComponent<Outline>().enabled = false;
        }
    }
    #endregion

    //快速设置
    public void QuickSet()
    {

    }
    //
    public void NextDay()
    {
        panelList[13].SetActive(false);
    }
    //
    void Start()
    {
        GameObject temp;
        int i;
        temp = GameObject.Find("FactoryPanel").transform.GetChild(1).gameObject;
        var text = temp.transform.Find("Text");
        text.GetComponent<Text>().text = temp.name;
        buttonList.Add(temp);
        //
        temp = GameObject.Find("FactoryPanel").transform.GetChild(1 + num + 1).gameObject;
        text = temp.transform.Find("Title");
        text.GetComponent<Text>().text = temp.name;
        panelList.Add(temp);
        for (i = 2; i < num + 2; i++) /// 初始化list
        {
            temp = GameObject.Find("FactoryPanel").transform.GetChild(i).gameObject;
            text = temp.transform.Find("Text");
            text.GetComponent<Text>().text = temp.name;
            buttonList.Add(temp);
            //
            temp = GameObject.Find("FactoryPanel").transform.GetChild(i + num + 1).gameObject;
            text = temp.transform.Find("Title");
            text.GetComponent<Text>().text = temp.name;
            panelList.Add(temp);
            //
            temp = panelList[i - 2].transform.Find("Confirm").gameObject;
            confirmList.Add(temp);
            temp = panelList[i - 2].transform.Find("QuickSet").gameObject;
            quickSetList.Add(temp);
        }
        temp = GameObject.Find("FactoryPanel");
        panelList.Add(temp);
        //ResetState();
        ResetOutline();
        temp = null;
        gameObject.SetActive(false);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) CloseUI();
        if (status != null && !buttonList[currentUI].GetComponent<UIObject>().isConfirm && !panelList[12].activeSelf) //连续变化
        {
            valueChange = statusSet.value - setValue;
            status.value = statusValue + parameter * valueChange;
        }
        if (gridList.Count > 1 && !buttonList[currentUI].GetComponent<UIObject>().isConfirm && !panelList[12].activeSelf)//离散变化
        {
            valueChange = statusSet.value - setValue2;
            for (int i = 0; i < 10; i++)
            {
                gridList[i].color = new Color(255, 255, 255);
            }
            for (int i = 0; i < 5 + 10 * valueChange * 0.4; i++)
            {
                gridList[i].color = new Color(0, 0, 0);
            }
        }
    }
}
