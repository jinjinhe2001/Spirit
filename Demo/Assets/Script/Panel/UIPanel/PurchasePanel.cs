﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PurchasePanel : MonoBehaviour
{
    public List<GameObject> purchaseObject1 = new List<GameObject>();
    public List<GameObject> purchaseObject2 = new List<GameObject>();
    public GameObject buy;
    public GameObject panel;
    public GameObject fatherObj1;
    public GameObject fatherObj2;
    public Shader shader;
    public Material mt;
    public List<Material> mats1;
    public List<Material> mats2;
    Text text;
    int num = 3;
    public void Switch()
    {
        if(fatherObj1.activeSelf)
        {
            fatherObj1.SetActive(false);
            fatherObj2.SetActive(true);
            text.text = "查看主料";
        }
        else
        {
            fatherObj1.SetActive(true);
            fatherObj2.SetActive(false);
            text.text = "查看辅料";
        }
    }
    //更新物品list
    public void UpdateList()
    {
        //purchaseObject1.Clear();
    }
    public void Clear()
    {
        for (int i = 0; i < fatherObj1.transform.childCount; i++)
            Destroy(fatherObj1.transform.GetChild(i).gameObject);
    }
    public void UpdateObjects()
    {
        if (!buy.GetComponent<UIObject>().isConfirm)
        {
            //Clear();
            
        }
    }
    public void Purchase()
    {
        for(int i=0;i<purchaseObject1.Count;i++)
        {
            if (fatherObj1.transform.GetChild(i).GetComponent<UIObject>().isUse)
            {
                Destroy(fatherObj1.transform.GetChild(i).gameObject);
            }
        }
        buy.GetComponent<UIObject>().isConfirm = true;
        buy.SetActive(false);
    }
    public void Select()
    {
        if (!buy.GetComponent<UIObject>().isConfirm)
        {
            for(int i=0;i<num;i++)
            {
                GameObject temp = fatherObj1.transform.GetChild(i).gameObject;
                temp.GetComponent<Outline>().enabled = false;
                temp.GetComponent<UIObject>().isUse = false;
            }
            var btn = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
            btn.GetComponent<UIObject>().isUse = !btn.GetComponent<UIObject>().isUse;
            btn.GetComponent<Outline>().enabled = !btn.GetComponent<Outline>().enabled;
        }
    }
    public void InstantiateObj(Sprite sprite, string name, GameObject fatherObj)
    {
        GameObject obj = new GameObject();
        obj.AddComponent<UIObject>();
        Button btn = obj.AddComponent<Button>();
        Image img = obj.AddComponent<Image>();
        Outline outLine = obj.AddComponent<Outline>();
        outLine.enabled = false;
       // outLine.
        //outLine.OutlineColor = Color.yellow;
        img.sprite = sprite;
        obj.name = name;
        obj.transform.SetParent(fatherObj.transform);
        btn.targetGraphic = img;
        btn.onClick.AddListener(Select);
        obj.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
    }
    void Instance()
    {
        for(int i=0;i<fatherObj2.transform.childCount;i++)
        {
            Material mat = Instantiate(mt);
            mat.SetFloat("_Flag", 0);
            mat.SetFloat("_MinOffset", 8f);
            mat.SetColor("_OutLineCol", Color.yellow);
            try
            {
                purchaseObject1.Add(fatherObj1.transform.GetChild(i).gameObject);
                purchaseObject1[i].AddComponent<UIObject>();
                purchaseObject1[i].GetComponent<Image>().material = mat;
                mats1.Add(purchaseObject1[i].GetComponent<Image>().material);
            }
            catch { }
            purchaseObject2.Add(fatherObj2.transform.GetChild(i).gameObject);
            purchaseObject2[i].AddComponent<UIObject>();
            purchaseObject2[i].GetComponent<Image>().material = mat;
            mats2.Add(purchaseObject1[i].GetComponent<Image>().material);
        }
    }
    void Start()
    {
        text = GameObject.Find("Switch").transform.GetChild(0).GetComponent<Text>();
        UpdateList();
        UpdateObjects();
        gameObject.SetActive(false);
    }

    void Update()
    {
        
    }
}
