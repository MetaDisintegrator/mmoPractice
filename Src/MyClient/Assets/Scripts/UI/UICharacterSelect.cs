using Common;
using Models;
using Services;
using SkillBridge.Message;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class UICharacterSelect : MonoBehaviour
{
    public GameObject panelCreate;
    public GameObject panelSelect;

    public Text textDesc;
    public Image[] titleClass;
    public GameObject[] models;
    public InputField inputName;

    public Transform content;
    public GameObject elementCharInfo;

    CharacterClass selectingClass = CharacterClass.Warrior;
    int selectingCharId = -1;

    void Start()
    {
        Init();
    }

    void Init()
    {
        UserService.Instance.OnCreate += onCreate;
        panelCreate.SetActive(false);
        panelSelect.SetActive(true);
        CharListUpdate();
        elementCharInfo.SetActive(false);
        MapService.Instance.currentMapId = -1;
    }

    public void onSelectClass(int selection)
    {
        for (int i = 0; i < models.Length; i++)
        {
            titleClass[i].gameObject.SetActive(i == selection);
            models[i].gameObject.SetActive(i == selection);
        }
        textDesc.text = DataManager.Instance.Characters[selection + 1].Description;
        selectingClass = (CharacterClass)(selection + 1);
    }

    private void ShowModel(int id)
    {
        for (int i = 0; i < models.Length; i++)
        {
            models[i].gameObject.SetActive(i == id);
        }
    }

    private void onCreate(Result result, string error)
    {
        if (result == Result.Failed)
        {
            MessageBox.Show(error, type: MessageBoxType.Error);
        }
        else
        {
            Log.InfoFormat("Create Successed");

            CharListUpdate();
            
            onBackClicked();
        }
    }

    public void onBackClicked()
    {
        panelCreate.SetActive(false);
        panelSelect.SetActive(true);
        if (selectingCharId >= 0)
        {
            content.GetChild(selectingCharId).GetComponent<UICharInfo>().Selected = false;
        }
        selectingCharId = -1;
        ShowModel(-1);
    }

    public void onCreateClicked()
    {
        if (string.IsNullOrEmpty(inputName.text)) 
        {
            MessageBox.Show("角色名不能为空");
            return;
        }
        if (selectingClass == CharacterClass.None)
        {
            MessageBox.Show("请选择职业");
            return;
        }
        UserService.Instance.SendCreateCharacter(inputName.text, selectingClass);
    }

    void CharListUpdate()
    {
        int i;
        for (i = 0; i < content.childCount; i++)
        {
            Destroy(content.GetChild(i).gameObject);
        }
        UICharInfo sc;
        i = 0;
        foreach (var info in User.Instance.Info.Player.Characters)
        {
            int listId = i;
            GameObject e = Instantiate(elementCharInfo);
            e.transform.SetParent(content);
            sc = e.GetComponent<UICharInfo>();
            sc.InfoElementSetup(info);
            sc.onClickInfo += () => this.SelectChar(listId);
            i++;
        }
        GameObject e1 = Instantiate(elementCharInfo);
        e1.transform.SetParent(content);
        e1.GetComponent<UICharInfo>().CreateElementSetup();
    }

    public void SelectChar(int listId)
    {
        if (selectingCharId >= 0)
        {
            content.GetChild(selectingCharId).GetComponent<UICharInfo>().Selected = false;
        }
        UICharInfo sc = content.GetChild(listId).GetComponent<UICharInfo>();
        sc.Selected = true;
        ShowModel((int)sc.characterClass - 1);
        selectingCharId = listId;
    }

    public void onAddClicked()
    {
        panelCreate.SetActive(true);
        panelSelect.SetActive(false);
        textDesc.text = DataManager.Instance.Characters[(int)selectingClass].Description;
        ShowModel((int)selectingClass - 1);
    }

    public void onEnterClicked()
    {
        if (selectingCharId < 0)
        {
            MessageBox.Show("未选择角色");
        }
        else
        { 
            UserService.Instance.SendGameEnter(selectingCharId);
        }
    }
}
