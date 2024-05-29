using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : BaseManager
{
    GameObject canvas;
    GameObject titleUI;
    GameObject rulesUI;

    void Awake()
    {
        canvas = GameObject.Find("Canvas");
        titleUI = canvas.transform.GetChild(1).gameObject;
        rulesUI = canvas.transform.GetChild(2).gameObject;

        BackButton();
    }
    override protected void Update()
    {
        base.Update();
        if(Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space)){
            StartButton();
        }
    }
    public void StartButton(){
        SceneManager.LoadScene(1);
    }
    public void RulesButton(){
        titleUI.SetActive(false);
        rulesUI.SetActive(true);
    }
    public void BackButton(){
        titleUI.SetActive(true);
        rulesUI.SetActive(false);
    }  
}
