using Common;
using Services;
using SkillBridge.Message;
using UnityEngine;
using UnityEngine.UI;

public class UILogin : MonoBehaviour
{
    public InputField inputUser;
    public InputField inputPassword;
    public Toggle checkboxRemUser;
    public Toggle checkboxAgree;
    public Button btnLogin;

    private void OnEnable()
    {
        UserService.Instance.OnLogin += onLogin;
    }

    private void OnDisable()
    {
        UserService.Instance.OnLogin -= onLogin;
    }

    private void onLogin(Result result, string error)
    {
        if (result == Result.Failed)
        {
            MessageBox.Show(error, type: MessageBoxType.Error);
        }
        else
        {
            Log.InfoFormat("Login Successed");
            SceneManager.Instance.LoadScene("CharSelect");
        }
    }

    public void onLoginClicked()
    {
        if (string.IsNullOrEmpty(inputUser.text))
        {
            MessageBox.Show("请输入账号");
            return;
        }
        if (string.IsNullOrEmpty(inputPassword.text))
        {
            MessageBox.Show("请输入密码");
            return;
        }
        if (!checkboxAgree.isOn)
        {
            MessageBox.Show("请阅读并同意《用户协议》");
            return;
        }
        UserService.Instance.SendLogin(inputUser.text, inputPassword.text);
    }
}
