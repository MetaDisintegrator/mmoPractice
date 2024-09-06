using Models;
using Services;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMain : MonoSingleton<UIMain>
{
    public Transform canvas;
    public Text labName;
    public Text labLv;
    protected override void OnStart()
    {
        UpdateInfo();
        UIManager.Instance.Canvas = canvas;
    }

    void UpdateInfo()
    { 
        labName.text = User.Instance.CurrentCharacter.Name;
        labLv.text = User.Instance.CurrentCharacter.Level.ToString();
    }

    public void Back2CharSelect()
    {
        SceneManager.Instance.LoadScene("CharSelect");
        UserService.Instance.SendGameLeave();
    }

    public void OpenTestUI()
    {
        UITest ui = UIManager.Instance.Show<UITest>();
        ui.SetTitle("≤‚ ‘≤‚ ‘≤‚ ‘");
        ui.closeHandler += (sender, res) => MessageBox.Show("≤‚ ‘UIπÿ±’" + res.ToString("G"), "Ã· æ", MessageBoxType.Information);
    }
}
