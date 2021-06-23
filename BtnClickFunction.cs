using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Lean.Gui;
using Lean.Transition.Method;

public class BtnClickFunction : MonoBehaviour
{

    public ARSpawner arsp;

  //  public ARTaptoPlace1 ARP;
  //  public LeanToggle ARMode;

    public UnityEvent OnclickonButton;
    // public GameObject Prefab1;

    [Tooltip("Name of the spawned place")]
    public string NameText;                     // Name of the current place

    public string contentDetailsText;

    public AudioClip contentClip;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnEnable()
    {
        arsp.isContentInfoBtnClicked = false;
        arsp.replayButton.interactable = false;
       // iscontentInfoButtonClicked = false;
        OnclickonButton.Invoke();
    }


    public void OnClickFunction(GameObject prefab1)
    {
        arsp.ARMode.TurnOn();
        arsp.ARP.SessionTrue();

        arsp.Ctext.text = NameText;
        arsp.ARP.gameObjToInstantiate = prefab1;

        arsp.contentText.text = contentDetailsText;
        arsp.contentAudio.clip = contentClip;

        arsp.replayImage.color = arsp.grey;

       // arsp.contentAudio.Play();
        arsp.helpInfoAR.TurnOn();


    }

    public void OnBackClicked()
    {
        arsp.ARP.ToggleFunction1();
        arsp.ARP.spawnedObj = null;
        arsp.ARMode.TurnOff();

        /*  ARP.ToggleFunction1();
          // ARP.gameObjToInstantiate.SetActive(false);
          ARP.spawnedObj = null;
          ARMode.TurnOff();*/

        // Destroy(ARP.gameObjToInstantiate);
        //  Destroy(PlaceOnPlane.instance.spawnedObject);
    }


    private void OnDisable()
    {
       
    }
    // Update is called once per frame
    void Update()
    {
        if(arsp.isContentInfoBtnClicked && !arsp.contentAudio.isPlaying && !arsp.isClipReplayed)
        {
            arsp.replayButton.interactable = true;
            arsp.replayImage.color = arsp.white;
            arsp.isClipReplayed = true;
        }
    }
}
