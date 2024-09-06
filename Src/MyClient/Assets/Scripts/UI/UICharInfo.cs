using SkillBridge.Message;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UICharInfo : MonoBehaviour
{
    public GameObject elementCreate;
    public GameObject elementInfo;
    public Button btnSelect;
    public Text labLv;
    public Text labName;
    public Image imgHead;

    public GameObject imgSelected;
    public bool Selected 
    {
        get => imgSelected.activeSelf;
        set => imgSelected.SetActive(value);
    }

    public event UnityAction onClickInfo;

    public CharacterClass characterClass;

    public void CreateElementSetup()
    {
        elementCreate.SetActive(true);
        elementInfo.SetActive(false);
        Setup();
    }

    public void InfoElementSetup(NCharacterInfo info)
    {
        elementCreate.SetActive(false);
        elementInfo.SetActive(true);
        Setup();

        labLv.text = "lv" + info.Level.ToString() + " " + info.Class.ToString("G");
        labName.text = info.Name;
        characterClass = info.Class;
        Selected = false;
    }

    void Setup()
    { 
        transform.localScale = Vector3.one;
        gameObject.SetActive(true);
    }

    public void onInfoElementClicked()
    {
        //Debug.Log("clicked");
        this.onClickInfo?.Invoke();
    }
}
