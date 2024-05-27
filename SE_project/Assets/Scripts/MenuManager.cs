using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
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
    void Update()
    {
        if(Input.GetKey(KeyCode.KeypadEnter) || Input.GetKey(KeyCode.Space)){
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
