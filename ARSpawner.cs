using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Lean.Gui;
using Lean.Transition.Method;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Video;

public class ARSpawner : MonoBehaviour
{
    public ARTaptoPlace1 ARP;
    public LeanToggle ARMode;
    public TextMeshProUGUI Ctext;               // Text Holder - To display the name of the current place.(Title)

    public TextMeshProUGUI contentText;         // State Content Details

    public Image replayImage;
    public Button replayButton;
    public Button InfoButton;

    public AudioSource contentAudio;            //Audio   
    // public PlaceOnPlane placeOnPlane;

    public LeanToggle helpInfoAR;
    public LeanToggle ScrollAtInitialPosition;

    public Color grey;
    public Color white;

    public bool isContentInfoBtnClicked;
    public bool isClipReplayed;

    // Start is called before the first frame update
    void Start()
    {
       // ARTaptoPlace1.onObjectPlaced += PlayAudioOnClicked;
    }

    public void TurnOnAR()
    {
        ARMode.TurnOn();
        ARP.SessionTrue();
    }

    public void TurnoffAR()
    {
        ARP.ToggleFunction1();
        ARP.spawnedObj = null;
        ARMode.TurnOff();
    }

    public void PlayAudioOnClicked()
    {
        contentAudio.Play();
    }

    public void PlayAudioOnClickedReplay()
    {
        contentAudio.Play();
        replayButton.interactable = false;
        replayImage.color = grey;
        isClipReplayed = false;
    }

    public void OnContentButtonClicked()
    {
        ScrollAtInitialPosition.TurnOn();

        isContentInfoBtnClicked = true;

        InfoButton.interactable = false;

        var currentPrefab = ARP.spawnedObj.transform.GetChild(0).GetComponent<VideoPlayer>();

        if ((ARP.spawnedObj !=null) && (currentPrefab != null))
        {
            currentPrefab.Pause();
        }



        //iscontentInfoButtonClicked = true;
    }

    public void InformationPanelClosed()                                // To stop the Audio, on clicking the ok button in AR state information Panel
    {
        InfoButton.interactable = true;

        ScrollAtInitialPosition.TurnOff();

        isContentInfoBtnClicked = false;
        isClipReplayed = false;

        contentAudio.Stop();
        replayButton.interactable = false;
        replayImage.color = grey;

        var currentPrefab = ARP.spawnedObj.transform.GetChild(0).GetComponent<VideoPlayer>();

        if ((ARP.spawnedObj != null) && (currentPrefab != null))
        {
            currentPrefab.Play();
        }

    }
}
