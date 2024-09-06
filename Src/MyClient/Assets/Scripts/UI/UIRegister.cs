using SkillBridge.Message;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIRegister : MonoBehaviour
{
    public GameObject loginPanel;

    public InputField inputEmail;
    public InputField inputPassword;
    public InputField inputConfirmPassowrd;
    public Toggle checkboxRemUser;
    public Toggle checkboxAgree;
    public Button btnEnterGame;

    // Start is called before the first frame update
    void OnEnable()
    {
        Services.UserService.Instance.OnRegister += onRegister;
    }

    private void OnDisable()
    {
        Services.UserService.Instance.OnRegister -= onRegister;
    }

    private void onRegister(Result result, string error)
    {
        if (result == Result.Failed)
        {
            MessageBox.Show(error, type: MessageBoxType.Error);
        }
        else
        {
            MessageBox.Show("注册成功");
            gameObject.SetActive(false);
            loginPanel.SetActive(true);
        }
    }

    public void onEnterGameClicked()
    {
        if (string.IsNullOrEmpty(inputEmail.text))
        {
            MessageBox.Show("请输入账号");
            return;
        }
        if (string.IsNullOrEmpty(inputPassword.text))
        {
            MessageBox.Show("请输入密码");
            return;
        }
        if (string.IsNullOrEmpty(inputConfirmPassowrd.text))
        {
            MessageBox.Show("请输入重复密码");
            return;
        }
        if (inputPassword.text!=inputConfirmPassowrd.text)
        {
            MessageBox.Show("两次输入的密码不一致");
            return;
        }
        if (!checkboxAgree.isOn) 
        {
            MessageBox.Show("请阅读并同意《用户协议》");
        }
        Services.UserService.Instance.SendRegister(inputEmail.text, inputPassword.text);
    }
}
