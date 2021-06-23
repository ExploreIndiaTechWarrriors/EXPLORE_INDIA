using System.Collections;
using System.Collections.Generic;
using Lean.Gui;
using Lean.Transition.Method;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.Video;

public class StatesGenralScript : MonoBehaviour
{
    public enum States
    {
        UttarPradesh = 0,
        Delhi = 1,
        Punjab = 2
    }

    public static States currentState;

    Ray ray;
    RaycastHit hit;
    bool fingerPlacedOnState;
    public Camera cam;

    public TextMeshProUGUI ticketTextUI;
    public TextMeshPro ticketText3d;
    public TextMeshProUGUI contentText;
    public string contentUP;
    public string contentDelhi;
    public string contentPunjab;

    public GameObject states_UP;
    public GameObject zoomedUP;
    public LeanToggle upPinAnimationToggle;
    public bool clickedStateUP;
    public UnityEvent onClickedOnStateUP;

    public GameObject states_Punjab;
    public GameObject zoomedPunjab;
    public LeanToggle punjabPinAnimationToggle;
    public bool clickedStatePunjab;
    public UnityEvent onClickedOnStatePunjab;

    public GameObject states_Delhi;
    public GameObject zoomedDelhi;
    public LeanToggle delhiPinAnimationToggle;
    public bool clickedStateDelhi;
    public UnityEvent onClickedOnStateDelhi;

    public LeanEvent goBackToAllStates;
    public LeanEvent instructionPanel;

    public static States previousState;
    public static bool notInCurrentState;           // If user not in current state, it is set to true.

    public TextMeshProUGUI stateInstruction;
    public LeanToggle stateInstructionPanel;

    public Vector3 vector3location, UPmoveupward;
    public GameObject UP;

    public LeanToggle UPHighlight, DelhiHighlight, PunjabHighlight ,AllStatesHighlight;

    public RenderTexture UPInfoTexture, PunjabInfoTexture, DelhiInfoTexture, DefaultTexture;
    public RawImage stateInfoImage;

    public VideoPlayer videoPlayer;
    public VideoClip UPInfoClip, DelhiInfoClip, PunjabInfoClip;

    public bool isVideoPlaying;

    private void OnEnable()
    {
        clickedStateUP = false;
        clickedStatePunjab = false;
        clickedStateDelhi = false;
        fingerPlacedOnState = false;
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began)
        {
            print(11);
            ray = cam.ScreenPointToRay(Input.touches[0].position);
            if (Physics.Raycast(ray, out hit))
            {
                if (currentState == States.UttarPradesh && hit.collider.gameObject == states_UP)
                {
                    fingerPlacedOnState = true;
                    notInCurrentState = false;
                }
                else if (currentState == States.Punjab && hit.collider.gameObject == states_Punjab)
                {
                    fingerPlacedOnState = true;
                    notInCurrentState = false;
                }
                else if (currentState == States.Delhi && hit.collider.gameObject == states_Delhi)
                {
                    fingerPlacedOnState = true;
                    notInCurrentState = false;
                }
                else if(currentState == States.Delhi && hit.collider.gameObject == states_UP)
                {
                    fingerPlacedOnState = true;
                    notInCurrentState = true;
                }
                else if(previousState == States.Delhi && currentState == States.UttarPradesh && hit.collider.gameObject == states_Delhi)
                {
                    fingerPlacedOnState = true;
                    notInCurrentState = false;
                }
                else if(currentState == States.Punjab && (hit.collider.gameObject == states_UP || hit.collider.gameObject == states_Delhi))
                {
                    fingerPlacedOnState = true;
                    notInCurrentState = true;
                }
                else if(previousState == States.Punjab && (currentState == States.UttarPradesh || currentState == States.Delhi) && hit.collider.gameObject == states_Punjab)
                {
                    fingerPlacedOnState = true;
                    notInCurrentState = false;
                }
            }
        }
        if (fingerPlacedOnState)
        {
            Debug.DrawRay(ray.origin, ray.direction);
        }
        if (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Ended && fingerPlacedOnState)
        {
            ray = cam.ScreenPointToRay(Input.touches[0].position);
            if (Physics.Raycast(ray, out hit))
            {
                if (currentState == States.UttarPradesh && hit.collider.gameObject == states_UP)
                {
                    stateInstruction.text = "Walk 40 steps to reach the nearest check point.";

                    fingerPlacedOnState = false;
                    clickedStateUP = true;
                    OnClickedOnState();
                }
                else if (currentState == States.Punjab && hit.collider.gameObject == states_Punjab)
                {
                    if(!StepCounter.isfinalStateCompleted)
                    {
                        stateInstruction.text = "Walk 40 steps to reach the nearest check point.";
                    }
                    else
                    {
                        stateInstruction.text = "<align=center> Successfully Completed ";
                        AllStatesHighlight.TurnOff();
                    }

                    fingerPlacedOnState = false;
                    clickedStatePunjab = true;
                    OnClickedOnState();
                }
                else if (currentState == States.Delhi && hit.collider.gameObject == states_Delhi)
                {
                    stateInstruction.text = "Walk 40 steps to reach the nearest check point.";

                    Debug.Log("Delhi:");
                    fingerPlacedOnState = false;
                    clickedStateDelhi = true;
                    OnClickedOnState();
                }
                else if(currentState == States.Delhi && hit.collider.gameObject == states_UP)
                {
                    Debug.Log("upp:");
                    stateInstruction.text = "<align=center>Successfully Completed ";

                    fingerPlacedOnState = false;
                    previousState = States.Delhi;
                    currentState = States.UttarPradesh;
                    clickedStateUP = true;
                    OnClickedOnState();
                    DelhiHighlight.TurnOff();
                }
                else if(previousState == States.Delhi && currentState == States.UttarPradesh && hit.collider.gameObject == states_Delhi)
                {
                    stateInstruction.text = "Walk 40 steps to reach the nearest check point.";

                    currentState = States.Delhi;
                    fingerPlacedOnState = false;
                    clickedStateDelhi = true;
                    OnClickedOnState();
                }
                else if (currentState == States.Punjab && hit.collider.gameObject == states_UP)
                {
                    Debug.Log("upp:");
                    stateInstruction.text = "<align=center>Successfully Completed ";

                    previousState = States.Punjab;
                    currentState = States.UttarPradesh;
                    clickedStateUP = true;
                    OnClickedOnState();
                    fingerPlacedOnState = false;
                    PunjabHighlight.TurnOff();
                    if(StepCounter.isfinalStateCompleted)
                    {
                        AllStatesHighlight.TurnOff();
                    }
                 
                }
                else if (previousState == States.Punjab && (currentState == States.UttarPradesh || currentState == States.Delhi)&& hit.collider.gameObject == states_Punjab)
                {

                    stateInstruction.text = "Walk 40 steps to reach the nearest check point.";

                    currentState = States.Punjab;
                    fingerPlacedOnState = false;
                    clickedStatePunjab = true;
                    OnClickedOnState();
                }
                else if(currentState == States.Punjab && hit.collider.gameObject == states_Delhi)
                {
                    stateInstruction.text = "<align=center>Successfully Completed ";

                    previousState = States.Punjab;
                    currentState = States.Delhi;
                    clickedStateDelhi = true;
                    OnClickedOnState();
                    fingerPlacedOnState = false;
                    PunjabHighlight.TurnOff();

                    if (StepCounter.isfinalStateCompleted)
                    {
                        AllStatesHighlight.TurnOff();
                    }

                }
            }
        }
    }

    /// <summary>
    ///      To display video and state information in the Panel
    /// </summary>
   
    public void SetStateContentText()                               
    {
        switch (currentState)
        {
            case States.Delhi:
                contentText.text = contentDelhi;
                videoPlayer.clip = DelhiInfoClip;
                videoPlayer.targetTexture = DelhiInfoTexture;
                stateInfoImage.texture = DelhiInfoTexture;
                videoPlayer.enabled = true;
                break;

            case States.UttarPradesh:
                contentText.text = contentUP;
                videoPlayer.clip = UPInfoClip;
                videoPlayer.targetTexture = UPInfoTexture;
                stateInfoImage.texture = UPInfoTexture;
                videoPlayer.enabled = true;
                break;

            case States.Punjab:
                contentText.text = contentPunjab;
                videoPlayer.clip = PunjabInfoClip;
                videoPlayer.targetTexture = PunjabInfoTexture;
                stateInfoImage.texture = PunjabInfoTexture;
                videoPlayer.enabled = true;
                break;
        }
    }

    public void OnClickedOnState()
    {
        goingBack = false;

        if (clickedStateUP && !StepCounter.isResetUP)
        {
            onClickedOnStateUP.Invoke();
            clickedStateUP = false;
        }
        else if (clickedStateDelhi && !StepCounter.isResetUserDelhi)
        {
            onClickedOnStateDelhi.Invoke();
            clickedStateDelhi = false;
        }
        else if (clickedStatePunjab && !StepCounter.isResetUserPunjab)
        {
            onClickedOnStatePunjab.Invoke();
            clickedStatePunjab = false;
        }
    }

    public void MakeVisibleAppropriateCloseUpState()
    {
        zoomedDelhi.SetActive(false);
        zoomedPunjab.SetActive(false);
        zoomedUP.SetActive(false);

        switch (currentState)
        {
            case States.Delhi:
                zoomedDelhi.SetActive(true);
                break;

            case States.UttarPradesh:
                zoomedUP.SetActive(true);
                break;

            case States.Punjab:
                zoomedPunjab.SetActive(true);
                break;
        }
    }

    Coroutine instructionCoroutine;

    public void PlayPinAnimation()
    {
        switch (currentState)
        {
            case States.Delhi:
                delhiPinAnimationToggle.TurnOn();
                break;

            case States.UttarPradesh:
                upPinAnimationToggle.TurnOn();
                break;

            case States.Punjab:
                punjabPinAnimationToggle.TurnOn();
                break;
        }

        instructionCoroutine = StartCoroutine(ShowInstructionCR());
    }

    public void ShowInstruction()
    {

    }

    IEnumerator ShowInstructionCR()
    {
        yield return new WaitForSeconds(1.3f); //1.9f
        if (!goingBack)
        stateInstructionPanel.TurnOn();
        //instructionPanel.BeginAllTransitions();
    }
    bool goingBack;
    public void GoBackToAllStates()
    {
        goingBack = true;
        stateInstructionPanel.TurnOff();
        if (instructionCoroutine != null)
        {
            StopCoroutine(instructionCoroutine);
            instructionCoroutine = null;
        }

        goBackToAllStates.BeginAllTransitions();
    }

    public void CheckForCurrentTicket()
    {
        var currentstateName = "";
        switch (currentState)
        {
            case States.Delhi:
                currentstateName = "Delhi & Haryana";
                break;

            case States.UttarPradesh:
                currentstateName = "Uttar Pradesh";
                break;

            case States.Punjab:
                currentstateName = "Punjab";
                break;
        }

        ticketTextUI.text = currentstateName;
        ticketText3d.text = currentstateName;
    }

    public void PlayPauseVideo()
    {
        if(!isVideoPlaying)
        {
            videoPlayer.Pause();
            isVideoPlaying = true;
        }
        else
        {
            videoPlayer.Play();
            isVideoPlaying = false;
        }

    }

    public void ReleaseTexture()
    {
        videoPlayer.targetTexture.Release();
        videoPlayer.enabled = false;
       // videoPlayer.Stop();
        stateInfoImage.texture = DefaultTexture;
    }
    
}
