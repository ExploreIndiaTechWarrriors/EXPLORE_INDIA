using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Networking;

public class CurrentLocation : MonoBehaviour
{
    public string[] Locations;

    public TextMeshProUGUI currentLocationText;

    public static CurrentLocation instance;

    public string userID;

    public UserInformation user;

    public string userJsonString;

    private string APIKey = "$2b$10$.Tzp5zp/cIjI1xevkLBm/.k1Kxs73QBkyTYAHtMhf836f.yzC3tmW";

    string url = "https://api.jsonbin.io/v3/b/";
    


    public TextMeshProUGUI userContent;

    [Tooltip("Text holder for Displaying User Details")]
    public TextMeshProUGUI[] userContentss;

    public string[] userIDs;

    public int firstTime;

    public string currentUser;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }

    // Start is called before the first frame update
    void Start()
    {
        LoginAuthentication.OnLogin += (string _userID) =>
            {
                userID = _userID;
                StartCoroutine(GetRequest(url));
            };

    }

    IEnumerator GetRequest(string url)
    {
        UnityWebRequest uwr = UnityWebRequest.Get(url + userID + "/latest");
        uwr.SetRequestHeader("X-Master-Key", APIKey);
        yield return uwr.SendWebRequest();

        if (uwr.isNetworkError)
        {
            Debug.Log("Error While Sending: " + uwr.error);
        }
        else
        {
            Debug.Log("Received: " + uwr.downloadHandler.text);
            user = JsonUtility.FromJson<UserInformation>(uwr.downloadHandler.text);

            currentUser = user.record.name;
            Debug.Log(user.record.name + ": Stage:" + user.record.stepCount);
        }
    }

    IEnumerator PostRequest(string url, string json)
    {
        UnityWebRequest uwr = UnityWebRequest.Put(url + userID, json);

        uwr.SetRequestHeader("Content-Type", "application/json");
        uwr.SetRequestHeader("X-Master-Key", APIKey);
        yield return uwr.SendWebRequest();

        if (uwr.isNetworkError)
        {
            Debug.Log("Error While Sending: " + uwr.error);
        }
        else
        {
            Debug.Log("Received: " + uwr.downloadHandler.text);
        }
    }


    public void PostUserData()
    {
        userJsonString = JsonUtility.ToJson(user.record);
        StartCoroutine(PostRequest(url, userJsonString));
    }


    public void CurrentLocationInfo(int sIndex)
    {
        currentLocationText.text = Locations[sIndex];
        Debug.Log(currentLocationText.text);
    }


    IEnumerator GetRequest1(string url, string userid, System.Action<UserInformation> callback)
    {
        // firstTime = 0;

        UnityWebRequest uwr = UnityWebRequest.Get(url + userid + "/latest");
        uwr.SetRequestHeader("X-Master-Key", APIKey);
        yield return uwr.SendWebRequest();

        if (uwr.isNetworkError)
        {
            Debug.Log("Error While Sending: " + uwr.error);
        }
        else
        {
            Debug.Log("Received: " + uwr.downloadHandler.text);
            var tempUser = JsonUtility.FromJson<UserInformation>(uwr.downloadHandler.text);

            Debug.Log(tempUser.record.name + ": Stage:" + tempUser.record.stepCount);
            callback?.Invoke(tempUser);
        }

    }

    public void ShowUserDetails()
    {
        StartCoroutine(GetRequest(url));
        userContent.text = "User Name: " + user.record.name + "\n" + "Total Steps: " + user.record.stepCount + "\n" + "Current Location: " + user.record.currentLocation
            + "\n" + "Current State: " + user.record.currentState + "\n";
        if (user.record.unlockedStates.Count > 0)
        {
            userContent.text += "unlocked States: " + user.record.unlockedStates[0];
        }
    }


    public void ShowUserDetailsCollection()
    {

        for (int q = 0; q < userContentss.Length; q++)
        {
            userContentss[q].text = " ";
        }

        userContent.text = " ";
        for (int i = 0; i < userIDs.Length; i++)
        {
            int j = i;
            StartCoroutine(GetRequest1(url, userIDs[j], (UserInformation userInfo) =>
            {
                firstTime = 0;

                if (userInfo.record.name.Equals(currentUser))
                {
                    userContentss[j].text += "User Name: " + userInfo.record.name + "  (Me)" + "\n";
                }
                else
                {
                    userContentss[j].text += "User Name: " + userInfo.record.name + "\n";
                }
                userContentss[j].text += "Total Steps: " + userInfo.record.stepCount + "\n" + "Current Location: " + userInfo.record.currentLocation
                 + "\n" + "Current State: " + userInfo.record.currentState + "\n" + "unlocked States: ";


                for (int k = 0; k < userInfo.record.unlockedStates.Count; k++)
                {

                    if (string.IsNullOrEmpty(userInfo.record.unlockedStates[0]))
                    {
                        userContentss[j].text += " None";
                        break;
                    }
                    else if (!string.IsNullOrEmpty(userInfo.record.unlockedStates[k]) && firstTime == 0)
                    {
                        userContentss[j].text += userInfo.record.unlockedStates[k];
                        firstTime = 1;
                    }
                    else if (!string.IsNullOrEmpty(userInfo.record.unlockedStates[k]))
                    {
                        userContentss[j].text += ", " + userInfo.record.unlockedStates[k];
                    }
                    // firstTime = 1;

                }

            }));

        }
        
    }
   
}
