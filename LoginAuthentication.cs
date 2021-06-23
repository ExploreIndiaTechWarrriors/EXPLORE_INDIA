using Lean.Gui;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class LoginAuthentication : MonoBehaviour
{
    public static event Action<string> OnLogin;

    public PredefinedUserInfo[] userInfos;

    public PredefinedUserInfo currentUserinfo;

    private string password = "12345";
    Color color;

    public Button loginButton;
    public Image loginBtnImage;
    public TMP_InputField userNameEnterd;
    public TMP_InputField passwordEnterd;
    public LeanToggle loginSuccessfullLean;
    public UnityEvent onSuccessfulLogin;

    private void OnEnable()
    {
        loginButton.onClick.AddListener(CheckLoginAuthentications);
        loginBtnImage = loginButton.gameObject.GetComponent<Image>();

        ColorUtility.TryParseHtmlString("#94C9FF", out color);
        loginBtnImage.color = color;
        loginButton.interactable = false;
    }

    private void OnDisable()
    {
        loginButton.onClick.RemoveListener(CheckLoginAuthentications);

    }

    public void MakeLoginButtonIntractable()
    {

        if (userNameEnterd.text.Length == 0 || passwordEnterd.text.Length == 0)
        {
            ColorUtility.TryParseHtmlString("#94C9FF", out color);
            loginBtnImage.color = color;
            loginButton.interactable = false;
        }
        else
        {
            ColorUtility.TryParseHtmlString("#28A7FF", out color);
            loginBtnImage.color = color;
            loginButton.interactable = true;
        }

    }

    public void CheckLoginAuthentications()
    {
        currentUserinfo = getUserName();

        if (currentUserinfo != null && !string.IsNullOrEmpty(currentUserinfo.userName) && string.Compare(passwordEnterd.text, password) == 0)
        {
            print("login Successfully");
            // allow 
            loginSuccessfullLean.TurnOn();
            onSuccessfulLogin.Invoke();
            OnLogin?.Invoke(currentUserinfo.url);
        }

    }

    PredefinedUserInfo getUserName()
    {
        for (int i = 0; i < userInfos.Length; i++)
        {
            if (userInfos[i].userName.Equals(userNameEnterd.text))
            {
                return userInfos[i];
            }
        }
        return null;
    }
}

[System.Serializable]
public class PredefinedUserInfo
{
    public string userName;
    public string url;
}
