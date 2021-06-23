using Lean.Gui;
using Lean.Transition;
using Lean.Transition.Method;
using PedometerU;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using HighlightPlus;
using UnityEngine.Android;


public class StepCounter : MonoBehaviour
{
    public TextMeshProUGUI stepText;
    TextMeshProUGUI distanceText;

    public TextMeshProUGUI t1;
    public TextMeshProUGUI t2;
    public TextMeshProUGUI t3;
    public TextMeshProUGUI t4;
    public TextMeshProUGUI instruction;

    public int stepCountForEachDestination;

    bool checkedInAR;

    public Color light_Blue;
    public Color blue;
    public Color green;
    public Color red;

    public LeanToggle instuctionPannel;

    public Button ticketBtn;

    Pedometer pedometer;

    public float timer;
    public int count;

    public float previousLatitudeValue, previousLongitudeValue;
    public float latitudeDistance, longitudeDistance;

    public Text inputX, inputY, differencelat, differenceLong;

    int punjabStartCount;
    public LeanToggle punjabProgressBar;

    public static int stage;
    bool colliderEnabled;                                   // To enable the collider in marker for AR view
    bool isStageCompleted;

    public StateInfoSliderAndPin[] stateInfo;
    public int Sindex;                                      // StateIndex (0-UP , 1-Delhi, 2-Punjab)
    public static bool isfinalStateCompleted;

    int debugWalk;
    int st, rst;                                        // Temporary variable for storing stage values

    public Sprite greenSprite;
    public Sprite redSprite;

    public static bool isResetUP, isResetUserDelhi, isResetUserPunjab;
    public GameObject Startbutton;

    public HighlightEffect highlightEffectUP, highlightEffectDelhi, highlightEffectPunjab;

    public HighlightProfile unlockedStateProfile;
    public HighlightProfile currentProfile;
    public HighlightProfile lockedStateProfile;

    public bool isUserValuetaken;
    bool isLocationStarted;

    public LeanToggle UPunlockedHighlight, DelhiUnlockedHighlight, PunjabUnlockedHighlight, AllUnlockedHighlight;
    public LeanToggle ResetHighlight;

    private void Awake()
    {
        Debug.unityLogger.logEnabled = false;
    }

    private void Start()
    {
        
        Input.location.Start(5,0.1f);
       
        if (PlayerPrefs.HasKey("scoreIndex"))
        {
            stage = PlayerPrefs.GetInt("scoreIndex");
            st = stage;

            for (int i = 0, j = 0; i < st && j < 3; i++)
            {
                stateInfo[j].SwalkingSliderArray[i].Data.Value = 0;
                stateInfo[j].SwalkingSliderArray[i].Data.Duration = 0;

                Debug.Log(i + " :i:j: " + j);
                stateInfo[j].SwalkingSliderArray[i].BeginAllTransitions();
                if (i % 4 != 3)
                {
                    stateInfo[j].ScheckPoints[i].color = green;

                    stateInfo[j].SlPinArray[i].GetComponent<BoxCollider>().enabled = true;
                    stateInfo[j].SlPinArray[i].GetComponent<SpriteRenderer>().sprite = greenSprite;
                    stateInfo[j].animatedPin[i] = true;
                    stateInfo[j].ticketBtn.interactable = false;
                    debugWalk += 20;
                    if (j == 2 && i == 1)
                    {
                        stateInfo[j].animatedPin[1] = true;
                    }
                    else if (j == 2 && i == 1)
                    {
                        stateInfo[j].animatedPin[2] = true;
                    }
                }
                else
                {
                    stateInfo[j].animatedPin[i] = true;

                    if (stage >= 8 && Sindex == 1)
                    {
                        StatesGenralScript.currentState = StatesGenralScript.States.Punjab;
                        stateInfo[j].ScheckPoints[i].color = green;
                        debugWalk = 20;
                        j = 2;
                        i = -1;
                        st -= 4;
                        Sindex = 2;
                        QuizHandler.stateIndex = 2;
                    }
                    else
                    {
                        StatesGenralScript.currentState = StatesGenralScript.States.Delhi;
                        stateInfo[j].ScheckPoints[i].color = green;
                        debugWalk = 0;
                        j = 1;                                              // Next state
                        i = -1;
                        st -= 4;
                        Sindex = 1;
                        QuizHandler.stateIndex = 1;
                    }
                }

            }

            if (stage >= 9)
            {
                isStageCompleted = true;
            }

            if (stage == 0)
            {
                isUserValuetaken = false;
            }
            else
            {
                isUserValuetaken = true;
            }

            OnStep1(debugWalk, 0.222, Sindex);
        }
    }

    public void SelectHighlightStart()
    {
        if (stage >= 4 && stage <= 7)
        {
            highlightEffectUP.ProfileLoad(unlockedStateProfile);
            UPunlockedHighlight.TurnOn();
        }
        else if (stage >= 8 && stage < 11)
        {
            highlightEffectUP.ProfileLoad(unlockedStateProfile);
            highlightEffectDelhi.ProfileLoad(unlockedStateProfile);
            DelhiUnlockedHighlight.TurnOn();
        }
        else if (stage == 11)
        {
            highlightEffectUP.ProfileLoad(unlockedStateProfile);
            highlightEffectDelhi.ProfileLoad(unlockedStateProfile);
            highlightEffectPunjab.ProfileLoad(unlockedStateProfile);
            AllUnlockedHighlight.TurnOn();
        }

    }

    
    public void LocationStart()
    {
        if (!isLocationStarted)
        {
            isLocationStarted = true;
        }
    }

    public void LocationStop()
    {
        isLocationStarted = false;
        count = 0;
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer > 0.55f)    //.75                             // For location Enabled 
        {
            if (!StatesGenralScript.notInCurrentState && !isfinalStateCompleted)
            {
                if (Input.location.isEnabledByUser && isLocationStarted && Input.location.lastData.latitude > 0 && Input.location.lastData.longitude > 0)
                {
                    float lat = Input.location.lastData.latitude;
                    float lon = Input.location.lastData.longitude;

                    if (count == 0 && lat > 0 && lon > 0)
                    {
                        previousLatitudeValue = lat;
                        previousLongitudeValue = lon;
                        Debug.Log(previousLatitudeValue + " :lat:lon: " + previousLongitudeValue);
                        count += 1;
                    }

                    inputX.text = lat.ToString();
                    inputY.text = lon.ToString();
                  //  differencelat.text = Mathf.Abs(previousLatitudeValue - lat).ToString();
                   // differenceLong.text = Mathf.Abs(previousLongitudeValue - lon).ToString();
                    latitudeDistance = Mathf.Abs(previousLatitudeValue - lat);
                    longitudeDistance = Mathf.Abs(previousLongitudeValue - lon);

                    if ((lat > (previousLatitudeValue + 0.00013)) || (lat < (previousLatitudeValue - 0.00013)))//25         //11         // if (latitudeDistance > 0.00009f)
                    {
                        debugWalk += 10;
                        if (stage >= 8 && StatesGenralScript.currentState == StatesGenralScript.States.Punjab && punjabStartCount == 0)
                        {
                            Sindex = 2;
                            // debugWalk = 25;
                            OnStep1(debugWalk, 0.222, 2);
                            punjabStartCount = 1;
                        }
                        else if (stage >= 8)
                        {
                            Sindex = 2;
                            OnStep1(debugWalk, 0.222, 2);
                        }
                        else if (stage >= 4)
                        {
                            Sindex = 1;
                            print("stage2: " + stage);

                            if (!firsttime)
                            {
                                // debugWalk = 0;
                                firsttime = true;
                            }
                            else
                            {
                                // OnStep1(debugWalk, 0.222, 1);
                            }

                            OnStep1(debugWalk, 0.222, 1);
                            //Sindex = 1;

                        }
                        else
                        {
                            print("stage1: " + stage);
                            Sindex = 0;
                            OnStep1(debugWalk, 0.222, 0);
                        }

                        previousLatitudeValue = lat;
                        previousLongitudeValue = lon;
                    }
                    else if ((lon > (previousLongitudeValue + 0.00016)) || (lon < (previousLongitudeValue- 0.00016)))  //20 //16 //10 //09  //12
                    {

                        debugWalk += 10;
                        if (stage >= 8 && StatesGenralScript.currentState == StatesGenralScript.States.Punjab && punjabStartCount == 0)
                        {
                            Sindex = 2;
                            // debugWalk = 25;
                            OnStep1(debugWalk, 0.222, 2);
                            punjabStartCount = 1;
                        }
                        else if (stage >= 8)
                        {
                            Sindex = 2;
                            OnStep1(debugWalk, 0.222, 2);
                        }
                        else if (stage >= 4)
                        {
                            Sindex = 1;
                            print("stage2: " + stage);

                            if (!firsttime)
                            {
                                // debugWalk = 0;
                                firsttime = true;
                            }
                            else
                            {
                                // OnStep1(debugWalk, 0.222, 1);
                            }

                            OnStep1(debugWalk, 0.222, 1);
                            //Sindex = 1;

                        }
                        else
                        {
                            print("stage1: " + stage);
                            Sindex = 0;
                            OnStep1(debugWalk, 0.222, 0);
                        }
                      
                        previousLongitudeValue = lon;
                        previousLatitudeValue = lat;
   
                    }

                }
                else
                {
                    inputX.text = "TurnON GPS";
                    inputY.text = " ";
                }
            }

            timer = 0;
        }

    }

    bool firsttime;

    public void onStepMove11()                      // ButtonClick Function for Walk
    {
        if (!StatesGenralScript.notInCurrentState && !isfinalStateCompleted)
        {
            debugWalk += 5;
            if (stage >= 8 && StatesGenralScript.currentState == StatesGenralScript.States.Punjab && punjabStartCount == 0)
            {
                Sindex = 2;
                // debugWalk = 25;
                OnStep1(debugWalk, 0.222, 2);
                punjabStartCount = 1;
            }
            else if (stage >= 8)
            {
                Sindex = 2;
                OnStep1(debugWalk, 0.222, 2);
            }
            else if (stage >= 4)
            {
                Sindex = 1;
                print("stage2: " + stage);

                if (!firsttime)
                {
                    // debugWalk = 0;
                    firsttime = true;
                }
                else
                {
                    // OnStep1(debugWalk, 0.222, 1);
                }

                OnStep1(debugWalk, 0.222, 1);
                //Sindex = 1;
            }
            else
            {
                print("stage1: " + stage);
                Sindex = 0;
                OnStep1(debugWalk, 0.222, 0);
            }
        }
    }

    void OnStep1(int steps, double distance, int SindexVal)
    {
        // Display the values
        stepText.text = steps.ToString();

        if (stepCountForEachDestination * 3 <= steps)
        {
            stateInfo[SindexVal].SwalkingSliderArray[3].Data.Value = -550 + ((steps - (stepCountForEachDestination * 3f)) / (stepCountForEachDestination * 1f)) * 550;

            stateInfo[SindexVal].SwalkingSliderArray[0].Data.Value = 0;
            stateInfo[SindexVal].SwalkingSliderArray[0].Data.Duration = 0;
            stateInfo[SindexVal].ScheckPoints[0].color = green;
            //    leanTransformLocalScale_xy[0].BeginAllTransitions();

            stateInfo[SindexVal].SwalkingSliderArray[1].Data.Value = 0;
            stateInfo[SindexVal].SwalkingSliderArray[1].Data.Duration = 0;
            stateInfo[SindexVal].ScheckPoints[1].color = green;
            //    leanTransformLocalScale_xy[1].BeginAllTransitions();


            stateInfo[SindexVal].SwalkingSliderArray[2].Data.Value = 0;
            stateInfo[SindexVal].SwalkingSliderArray[2].Data.Duration = 0;
            stateInfo[SindexVal].ScheckPoints[2].color = green;
            //  leanTransformLocalScale_xy[2].BeginAllTransitions();



            stateInfo[SindexVal].ScheckPoints[3].color = light_Blue;
           // stateInfo[SindexVal].animatedPin[3] = false;

            if (stateInfo[SindexVal].SwalkingSliderArray[3].Data.Value >= 0)
            {
                stateInfo[SindexVal].SwalkingSliderArray[3].Data.Value = 0;
                stateInfo[SindexVal].ScheckPoints[3].color = blue;
                stateInfo[SindexVal].ticketBtn.interactable = true;
                if (!stateInfo[SindexVal].animatedPin[3] && !isfinalStateCompleted)
                {
                    colliderEnabled = true;

                    instruction.text = "Congratulation you have unlocked the test, click on the ticket to take the test.";

                    if (!isStageCompleted)
                    {
                        instuctionPannel.TurnOn();
                    }
                    isStageCompleted = false;
                    // instuctionPannel.TurnOn();
                    // onTicketReached();
                    stateInfo[SindexVal].SleanTransformLocalScale_xy[3].BeginAllTransitions();
                    stateInfo[SindexVal].animatedPin[3] = true;
                }
            }
            stateInfo[SindexVal].SwalkingSliderArray[3].Data.Duration = 0.5f;

            stateInfo[SindexVal].SwalkingSliderArray[0].BeginAllTransitions();
            stateInfo[SindexVal].SwalkingSliderArray[1].BeginAllTransitions();
            stateInfo[SindexVal].SwalkingSliderArray[2].BeginAllTransitions();
            stateInfo[SindexVal].SwalkingSliderArray[3].BeginAllTransitions();


            if (!stateInfo[SindexVal].animatedPin[0])
            {
                stateInfo[SindexVal].SleanTransformLocalScale_xy[0].BeginAllTransitions();
                stateInfo[SindexVal].animatedPin[0] = true;
            }
            if (!stateInfo[SindexVal].animatedPin[1])
            {
                stateInfo[SindexVal].SleanTransformLocalScale_xy[1].BeginAllTransitions();
                stateInfo[SindexVal].animatedPin[1] = true;

            }
            if (!stateInfo[SindexVal].animatedPin[2])
            {
                colliderEnabled = true;

                stateInfo[SindexVal].SleanTransformLocalScale_xy[2].BeginAllTransitions();
                if (StatesGenralScript.currentState == StatesGenralScript.States.Punjab)
                {
                    instruction.text = "Congratulation you have unlocked the 2nd location, click on the marker to view." + "\n" + "Walk another 40 steps to unlock the test.";
                }
                else
                {
                    instruction.text = "Congratulation you have unlocked the 3rd location, click on the marker to view." + "\n" + "Walk another 40 steps to unlock the test.";
                }

                if (!isStageCompleted)
                {
                    instuctionPannel.TurnOn();
                }
                isStageCompleted = false;
                stateInfo[SindexVal].animatedPin[2] = true;

            }


        }
        else if (stepCountForEachDestination * 2 <= steps)
        {
            stateInfo[SindexVal].SwalkingSliderArray[2].Data.Value = -550 + ((steps - (stepCountForEachDestination * 2f)) / (stepCountForEachDestination * 1f)) * 550;

            stateInfo[SindexVal].SwalkingSliderArray[0].Data.Value = 0;
            stateInfo[SindexVal].SwalkingSliderArray[0].Data.Duration = 0;
            stateInfo[SindexVal].ScheckPoints[0].color = green;

            stateInfo[SindexVal].SwalkingSliderArray[1].Data.Value = 0;
            stateInfo[SindexVal].SwalkingSliderArray[1].Data.Duration = 0;
            stateInfo[SindexVal].ScheckPoints[1].color = green;

            stateInfo[SindexVal].ScheckPoints[2].color = red;

            if (stateInfo[SindexVal].SwalkingSliderArray[2].Data.Value > 0)
            {

                stateInfo[SindexVal].SwalkingSliderArray[2].Data.Value = 0;
                stateInfo[SindexVal].ScheckPoints[2].color = green;
                stateInfo[SindexVal].SleanTransformLocalScale_xy[2].BeginAllTransitions();
                if (!stateInfo[SindexVal].animatedPin[2])
                {
                    colliderEnabled = true;

                    stateInfo[SindexVal].SleanTransformLocalScale_xy[2].BeginAllTransitions();
                    if (StatesGenralScript.currentState == StatesGenralScript.States.Punjab)
                    {
                        instruction.text = "Congratulation you have unlocked the 2nd location, click on the marker to view." + "\n" + "Walk another 40 steps to unlock the test.";
                    }
                    else
                    {
                        instruction.text = "Congratulation you have unlocked the 3rd location, click on the marker to view." + "\n" + "Walk another 40 steps to unlock the test.";
                    }
                    
                    instuctionPannel.TurnOn();
                    stateInfo[SindexVal].animatedPin[2] = true;

                }

            }

            stateInfo[SindexVal].SwalkingSliderArray[2].Data.Duration = 0.5f;


            t3.text = stateInfo[SindexVal].SwalkingSliderArray[2].Data.Value.ToString();


            stateInfo[SindexVal].SwalkingSliderArray[3].Data.Value = -550;
            stateInfo[SindexVal].SwalkingSliderArray[3].Data.Duration = 0;
            stateInfo[SindexVal].ScheckPoints[3].color = light_Blue;



            stateInfo[SindexVal].SwalkingSliderArray[0].BeginAllTransitions();
            stateInfo[SindexVal].SwalkingSliderArray[1].BeginAllTransitions();
            stateInfo[SindexVal].SwalkingSliderArray[2].BeginAllTransitions();
            stateInfo[SindexVal].SwalkingSliderArray[3].BeginAllTransitions();

            if (!stateInfo[SindexVal].animatedPin[0])
            {
                stateInfo[SindexVal].SleanTransformLocalScale_xy[0].BeginAllTransitions();
                stateInfo[SindexVal].animatedPin[0] = true;
            }
            if (!stateInfo[SindexVal].animatedPin[1])
            {

                colliderEnabled = true;

                stateInfo[SindexVal].SleanTransformLocalScale_xy[1].BeginAllTransitions();
                if (StatesGenralScript.currentState == StatesGenralScript.States.Punjab)
                {
                    instruction.text = "Congratulation you have unlocked the 1st location, click on the marker to view." + "\n" + "Walk another 40 steps to unlock the next location.";
                    Debug.Log("tt0:");
                }
                else
                {
                    instruction.text = "Congratulation you have unlocked the 2nd location, click on the marker to view." + "\n" + "Walk another 40 steps to unlock the next location.";
                    Debug.Log("tt1:");
                }
                // instruction.text = "Congratulation you have unlock the 2nd location, click on the marker to view.";

                // instuctionPannel.TurnOn();
                if (!isStageCompleted)
                {
                    Debug.Log("tt2:");
                    instuctionPannel.TurnOn();
                }
                isStageCompleted = false;

                //  stage = PlayerPrefs.GetInt("scoreIndex");
                //  Debug.Log(stage);
                stateInfo[SindexVal].animatedPin[1] = true;

            }



        }
        else if (stepCountForEachDestination * 1 <= steps)
        {
            stateInfo[SindexVal].SwalkingSliderArray[1].Data.Value = -550 + ((steps - (stepCountForEachDestination * 1f)) / (stepCountForEachDestination * 1f)) * 550;

            stateInfo[SindexVal].SwalkingSliderArray[0].Data.Value = 0;
            stateInfo[SindexVal].SwalkingSliderArray[0].Data.Duration = 0;
            stateInfo[SindexVal].ScheckPoints[0].color = green;
            //  leanTransformLocalScale_xy[0].BeginAllTransitions();

            stateInfo[SindexVal].ScheckPoints[1].color = red;


            if (stateInfo[SindexVal].SwalkingSliderArray[1].Data.Value > 0)
            {
                stateInfo[SindexVal].SwalkingSliderArray[1].Data.Value = 0;
                stateInfo[SindexVal].ScheckPoints[1].color = green;
                stateInfo[SindexVal].SleanTransformLocalScale_xy[1].BeginAllTransitions();
                if (!stateInfo[SindexVal].animatedPin[1])
                {
                    colliderEnabled = true;

                    stateInfo[SindexVal].SleanTransformLocalScale_xy[1].BeginAllTransitions();
                    if (StatesGenralScript.currentState == StatesGenralScript.States.Punjab)
                    {
                        instruction.text = "Congratulation you have unlocked the 1st location, click on the marker to view." + "\n" +"Walk another 40 steps to unlock the next location.";
                        Debug.Log("sss:");
                    }
                    else
                    {
                        instruction.text = "Congratulation you have unlocked the 2nd location, click on the marker to view." + "\n" + "Walk another 40 steps to unlock the next location.";

                    }
                    instuctionPannel.TurnOn();
                    isStageCompleted = false;
                    //  stage = PlayerPrefs.GetInt("scoreIndex");
                    //   Debug.Log(stage);
                    stateInfo[SindexVal].animatedPin[1] = true;

                }

            }
                       
            stateInfo[SindexVal].SwalkingSliderArray[1].Data.Duration = 0.5f;
            t2.text = stateInfo[SindexVal].SwalkingSliderArray[1].Data.Value.ToString();

            stateInfo[SindexVal].SwalkingSliderArray[2].Data.Value = -550;
            stateInfo[SindexVal].SwalkingSliderArray[2].Data.Duration = 0;
            stateInfo[SindexVal].ScheckPoints[2].color = red;


            stateInfo[SindexVal].SwalkingSliderArray[3].Data.Value = -550;
            stateInfo[SindexVal].SwalkingSliderArray[3].Data.Duration = 0;
            stateInfo[SindexVal].ScheckPoints[3].color = light_Blue;


            stateInfo[SindexVal].SwalkingSliderArray[0].BeginAllTransitions();
            stateInfo[SindexVal].SwalkingSliderArray[1].BeginAllTransitions();
            stateInfo[SindexVal].SwalkingSliderArray[2].BeginAllTransitions();
            stateInfo[SindexVal].SwalkingSliderArray[3].BeginAllTransitions();

            if (!stateInfo[SindexVal].animatedPin[0] && StatesGenralScript.currentState != StatesGenralScript.States.Punjab)
            {
                colliderEnabled = true;

                stateInfo[SindexVal].SleanTransformLocalScale_xy[0].BeginAllTransitions();
                instruction.text = "Congratulation you have unlocked the 1st location, click on the marker to view." + "\n" + "Walk another 40 steps to unlock the next location.";

                instuctionPannel.TurnOn();
                isStageCompleted = false;
                // stage = PlayerPrefs.GetInt("scoreIndex");
                //  Debug.Log(stage);
                stateInfo[SindexVal].animatedPin[0] = true;
            }
            else if (StatesGenralScript.currentState == StatesGenralScript.States.Punjab)
            {
                isStageCompleted = false;

            }

        }
        else if (stepCountForEachDestination * 0 <= steps && StatesGenralScript.currentState != StatesGenralScript.States.Punjab)
        {

            stateInfo[SindexVal].SwalkingSliderArray[0].Data.Value = -550 + ((steps - (stepCountForEachDestination * 0f)) / (stepCountForEachDestination * 1f)) * 550;

            stateInfo[SindexVal].ScheckPoints[0].color = red;
            if (stateInfo[SindexVal].SwalkingSliderArray[0].Data.Value >= 0)
            {
                stateInfo[SindexVal].SwalkingSliderArray[0].Data.Value = 0;
                stateInfo[SindexVal].ScheckPoints[0].color = green;
                if (!stateInfo[SindexVal].animatedPin[0])
                {
                    colliderEnabled = true;

                    stateInfo[SindexVal].SleanTransformLocalScale_xy[0].BeginAllTransitions();
                    instruction.text = "Congratulation you have unlocked the 1st location, click on the marker to view." + "\n" + "Walk another 40 steps to unlock the next location.";
                   // onPin1reached?.Invoke();
                    instuctionPannel.TurnOn();
                    //  stage = PlayerPrefs.GetInt("scoreIndex");
                    //   Debug.Log(stage);

                    isStageCompleted = false;
                    stateInfo[SindexVal].animatedPin[0] = true;
                }


            }




            stateInfo[SindexVal].SwalkingSliderArray[1].Data.Value = -550;
            stateInfo[SindexVal].SwalkingSliderArray[1].Data.Duration = 0;
            stateInfo[SindexVal].ScheckPoints[1].color = red;



            stateInfo[SindexVal].SwalkingSliderArray[2].Data.Value = -550;
            stateInfo[SindexVal].SwalkingSliderArray[2].Data.Duration = 0;
            stateInfo[SindexVal].ScheckPoints[2].color = red;


            stateInfo[SindexVal].SwalkingSliderArray[3].Data.Value = -550;
            stateInfo[SindexVal].SwalkingSliderArray[3].Data.Duration = 0;
            stateInfo[SindexVal].ScheckPoints[3].color = light_Blue;



            t1.text = stateInfo[SindexVal].SwalkingSliderArray[0].Data.Value.ToString();

            stateInfo[SindexVal].SwalkingSliderArray[0].Data.Duration = 0.5f;

            stateInfo[SindexVal].SwalkingSliderArray[0].BeginAllTransitions();
            stateInfo[SindexVal].SwalkingSliderArray[1].BeginAllTransitions();
            stateInfo[SindexVal].SwalkingSliderArray[2].BeginAllTransitions();
            stateInfo[SindexVal].SwalkingSliderArray[3].BeginAllTransitions();

        }

        if (colliderEnabled)
        {
            if (StatesGenralScript.currentState == StatesGenralScript.States.UttarPradesh)
            {
                if (stateInfo[0].SwalkingSliderArray[2].Data.Value == 0)
                {
                    PlayerPrefs.SetInt("scoreIndex", 3);

                    stateInfo[0].SlPinArray[2].GetComponent<BoxCollider>().enabled = true;
                    stateInfo[0].SlPinArray[2].GetComponent<SpriteRenderer>().sprite = greenSprite;

                    Debug.Log(PlayerPrefs.GetInt("scoreIndex"));
                    CurrentLocation.instance.CurrentLocationInfo(2);

                    userInformationDetails();

                }
                else if (stateInfo[0].SwalkingSliderArray[1].Data.Value == 0)
                {
                    PlayerPrefs.SetInt("scoreIndex", 2);
                    stateInfo[0].SlPinArray[1].GetComponent<BoxCollider>().enabled = true;
                    stateInfo[0].SlPinArray[1].GetComponent<SpriteRenderer>().sprite = greenSprite;

                    Debug.Log(PlayerPrefs.GetInt("scoreIndex"));
                    CurrentLocation.instance.CurrentLocationInfo(1);

                    userInformationDetails();

                }
                else if (stateInfo[0].SwalkingSliderArray[0].Data.Value == 0)
                {
                    PlayerPrefs.SetInt("scoreIndex", 1);

                    stateInfo[0].SlPinArray[0].GetComponent<BoxCollider>().enabled = true;
                    stateInfo[0].SlPinArray[0].GetComponent<SpriteRenderer>().sprite = greenSprite;
                    Debug.Log(PlayerPrefs.GetInt("scoreIndex"));

                    CurrentLocation.instance.CurrentLocationInfo(0);

                    userInformationDetails();
                }

            }

            if (StatesGenralScript.currentState == StatesGenralScript.States.Delhi)
            {
                if (stateInfo[1].SwalkingSliderArray[2].Data.Value == 0)
                {
                    PlayerPrefs.SetInt("scoreIndex", 7);

                    stateInfo[1].SlPinArray[2].GetComponent<BoxCollider>().enabled = true;
                    stateInfo[1].SlPinArray[2].GetComponent<SpriteRenderer>().sprite = greenSprite;

                    Debug.Log(PlayerPrefs.GetInt("scoreIndex"));

                    CurrentLocation.instance.CurrentLocationInfo(5);

                    userInformationDetails();

                }
                else if (stateInfo[1].SwalkingSliderArray[1].Data.Value == 0)
                {
                    PlayerPrefs.SetInt("scoreIndex", 6);
                    stateInfo[1].SlPinArray[1].GetComponent<BoxCollider>().enabled = true;
                    stateInfo[1].SlPinArray[1].GetComponent<SpriteRenderer>().sprite = greenSprite;

                    Debug.Log(PlayerPrefs.GetInt("scoreIndex"));

                    CurrentLocation.instance.CurrentLocationInfo(4);

                    userInformationDetails();

                }
                else if (stateInfo[1].SwalkingSliderArray[0].Data.Value == 0)
                {
                    PlayerPrefs.SetInt("scoreIndex", 5);

                    stateInfo[1].SlPinArray[0].GetComponent<BoxCollider>().enabled = true;
                    stateInfo[1].SlPinArray[0].GetComponent<SpriteRenderer>().sprite = greenSprite;

                    Debug.Log(PlayerPrefs.GetInt("scoreIndex"));

                    CurrentLocation.instance.CurrentLocationInfo(3);

                    userInformationDetails();
                }


            }

            if (StatesGenralScript.currentState == StatesGenralScript.States.Punjab)
            {
                if (stage == 11)
                {
                    stateInfo[2].SwalkingSliderArray[3].Data.Value = 0;
                    stateInfo[2].SwalkingSliderArray[3].Data.Duration = 0;

                    Debug.Log(stage + " :i:j: " + stateInfo[2].animatedPin[3]);
                    stateInfo[2].SwalkingSliderArray[3].BeginAllTransitions();
                    stateInfo[2].animatedPin[3] = true;

                    isfinalStateCompleted = true;
                    stateInfo[2].ticketBtn.interactable = false;
                    stateInfo[2].ScheckPoints[3].color = green;
                    instruction.text = "<align=center>Successfully Completed";

                    userInformationDetails();
                }
                else if (stateInfo[2].SwalkingSliderArray[2].Data.Value == 0)
                {
                    PlayerPrefs.SetInt("scoreIndex", 10);

                    stateInfo[2].SlPinArray[2].GetComponent<BoxCollider>().enabled = true;
                    stateInfo[2].SlPinArray[2].GetComponent<SpriteRenderer>().sprite = greenSprite;

                    Debug.Log(PlayerPrefs.GetInt("scoreIndex"));

                    CurrentLocation.instance.CurrentLocationInfo(7);
                    userInformationDetails();

                }
                else if (stateInfo[2].SwalkingSliderArray[1].Data.Value == 0)
                {
                    PlayerPrefs.SetInt("scoreIndex", 9);
                    stateInfo[2].SlPinArray[1].GetComponent<BoxCollider>().enabled = true;
                    stateInfo[2].SlPinArray[1].GetComponent<SpriteRenderer>().sprite = greenSprite;

                    Debug.Log(PlayerPrefs.GetInt("scoreIndex"));

                    CurrentLocation.instance.CurrentLocationInfo(6);
                    userInformationDetails();
                }
                else if (stateInfo[2].SwalkingSliderArray[0].Data.Value == 0)
                {
                    /* PlayerPrefs.SetInt("scoreIndex", 5);

                     stateInfo[2].SlPinArray[0].GetComponent<BoxCollider>().enabled = true;
                     stateInfo[2].SlandmarkArray[0].Data.Value = green;
                     stateInfo[2].SlandmarkArray[0].BeginAllTransitions();
                     Debug.Log(PlayerPrefs.GetInt("scoreIndex"));*/

                }

            }

            colliderEnabled = false;
        }

    }

    public void ResetCounterToLast()
    {

        stateInfo[Sindex].animatedPin[0] = true;
        stateInfo[Sindex].animatedPin[1] = true;
        stateInfo[Sindex].animatedPin[2] = true;
        stateInfo[Sindex].animatedPin[3] = false;


        stateInfo[Sindex].SwalkingSliderArray[0].Data.Value = 0;
        stateInfo[Sindex].SwalkingSliderArray[0].Data.Duration = 0;
        stateInfo[Sindex].ScheckPoints[0].color = green;
        //    leanTransformLocalScale_xy[0].BeginAllTransitions();


        stateInfo[Sindex].SwalkingSliderArray[1].Data.Value = 0;
        stateInfo[Sindex].SwalkingSliderArray[1].Data.Duration = 0;
        stateInfo[Sindex].ScheckPoints[1].color = green;
        //    leanTransformLocalScale_xy[1].BeginAllTransitions();


        stateInfo[Sindex].SwalkingSliderArray[2].Data.Value = 0;
        stateInfo[Sindex].SwalkingSliderArray[2].Data.Duration = 0;
        stateInfo[Sindex].ScheckPoints[2].color = green;


        debugWalk = 65;
        stateInfo[Sindex].SwalkingSliderArray[3].Data.Value = -400;
        stateInfo[Sindex].SwalkingSliderArray[3].Data.Duration = 0;


        stateInfo[Sindex].SwalkingSliderArray[0].BeginAllTransitions();
        stateInfo[Sindex].SwalkingSliderArray[1].BeginAllTransitions();
        stateInfo[Sindex].SwalkingSliderArray[2].BeginAllTransitions();
        stateInfo[Sindex].SwalkingSliderArray[3].BeginThisTransition();

        stateInfo[Sindex].ScheckPoints[3].color = light_Blue;
        stateInfo[Sindex].ticketBtn.interactable = false;

    }

    public void ResetCounter()
    {
        stage = PlayerPrefs.GetInt("scoreIndex");                     // Disabled
        rst = stage;

        Debug.Log("wert: " + stage);
        debugWalk = 0;
        // isStageCompleted = true;

        for (int i = 0, j = 0; i < rst && j < 3; i++)
        {
            stateInfo[j].SwalkingSliderArray[i].Data.Value = 0;
            stateInfo[j].SwalkingSliderArray[i].Data.Duration = 0;

            Debug.Log(i + " :i:j: " + j);
            stateInfo[j].SwalkingSliderArray[i].BeginAllTransitions();

            if (i % 4 != 3)
            {
                stateInfo[j].ScheckPoints[i].color = green;

                // stateInfo[j].SlandmarkArray[i].Data.Value = green;
                //  stateInfo[j].SlandmarkArray[i].BeginAllTransitions();
                stateInfo[j].SlPinArray[i].GetComponent<BoxCollider>().enabled = true;
                stateInfo[j].SlPinArray[i].GetComponent<SpriteRenderer>().sprite = greenSprite;
                debugWalk += 20;
                stateInfo[j].StatePanelLean.TurnOff();
                stateInfo[j].animatedPin[i] = true;
                stateInfo[j].ticketBtn.interactable = false;

            }
            else
            {
                stateInfo[j].animatedPin[i] = true;
                // stateInfo[j].ticketBtn.interactable = true;
                if (stage >= 8 && Sindex == 1)
                {
                    StatesGenralScript.currentState = StatesGenralScript.States.Punjab;
                    stateInfo[j].ScheckPoints[i].color = green;
                    debugWalk = 20;
                    j = 2;
                    i = -1;
                    rst -= 4;
                    Sindex = 2;
                    stateInfo[j].StatePanelLean.TurnOff();
                }
                else
                {
                    StatesGenralScript.currentState = StatesGenralScript.States.Delhi;
                    stateInfo[j].ScheckPoints[i].color = green;
                    debugWalk = 0;
                    j = 1;                                              // Next state
                    i = -1;
                    rst -= 4;
                    stateInfo[j].StatePanelLean.TurnOff();
                    Sindex = 1;
                }
            }
            
        }

        if(stage ==3)
        {
            stateInfo[0].animatedPin[3] = false;
        }
        else if(stage == 7)
        {
            stateInfo[1].animatedPin[3] = false;
        }

        OnStep1(debugWalk, 0.222, Sindex);

        if (stage == 11)
        {
            stateInfo[2].ticketBtn.interactable = false;
            stateInfo[2].ScheckPoints[3].color = green;
            instruction.text = "<align=center>Successfully Completed";
        }
    }

    public void FinalStateCompleted()
    {
        isfinalStateCompleted = true;
        stateInfo[2].ticketBtn.interactable = false;
        stateInfo[2].ScheckPoints[3].color = green;

        PlayerPrefs.SetInt("scoreIndex", 11);
        stage = PlayerPrefs.GetInt("scoreIndex");
        //  Debug.Log("st: " + stage);

        stateInfo[2].animatedPin[0] = true;
        stateInfo[2].animatedPin[1] = true;
        stateInfo[2].animatedPin[2] = true;
        stateInfo[2].animatedPin[3] = true;
        Sindex = 3;

        userInformationDetails();
    }

    /// <summary>
    ///     Once unlocked all pins in one state and sucessfully completed quiz, this function will be called and setting the next state.
    /// </summary>
    public void TickedReachedIndex()
    {
        if (StatesGenralScript.currentState == StatesGenralScript.States.Punjab)
        {
            // PlayerPrefs.SetInt("scoreIndex", 11);
            stateInfo[1].ticketBtn.interactable = false;
            stateInfo[1].ScheckPoints[3].color = green;
            // instruction.text = "Completed:";
            PlayerPrefs.SetInt("scoreIndex", 8);
            stage = PlayerPrefs.GetInt("scoreIndex");
            // Debug.Log("st: " + stage);
            debugWalk = 20;

            stateInfo[1].animatedPin[0] = true;
            stateInfo[1].animatedPin[1] = true;
            stateInfo[1].animatedPin[2] = true;
            stateInfo[1].animatedPin[3] = true;
            Sindex = 2;

            isResetUserPunjab = false;

            highlightEffectUP.ProfileLoad(unlockedStateProfile);

            DelhiUnlockedHighlight.TurnOn();

            userInformationDetails();

        }
        else if (StatesGenralScript.currentState == StatesGenralScript.States.Delhi)
        {
            // PlayerPrefs.SetInt("scoreIndex", 8);
            stateInfo[0].ticketBtn.interactable = false;
            stateInfo[0].ScheckPoints[3].color = green;
            // instruction.text = "Completed:";

            PlayerPrefs.SetInt("scoreIndex", 4);
            stage = PlayerPrefs.GetInt("scoreIndex");
            // Debug.Log("st: " + stage);
            debugWalk = 0;

            stateInfo[0].animatedPin[0] = true;
            stateInfo[0].animatedPin[1] = true;
            stateInfo[0].animatedPin[2] = true;
            stateInfo[0].animatedPin[3] = true;
            Sindex = 1;

            isResetUserDelhi = false;

            highlightEffectUP.ProfileLoad(unlockedStateProfile);
            UPunlockedHighlight.TurnOn();

            userInformationDetails();

        }
        else if (StatesGenralScript.currentState == StatesGenralScript.States.UttarPradesh)
        {
            // PlayerPrefs.SetInt("scoreIndex", 4);
            // stage = PlayerPrefs.GetInt("scoreIndex");
            // Debug.Log("st: " + stage);
        }

        //Sindex += 1;
    }

    int previousStateVal, firstCount;
    int scoreIndexVal;

    /// <summary>
    ///    User values are updated after unlocking each LandmarkPin
    /// </summary>
    public void userInformationDetails()
    {
        scoreIndexVal = PlayerPrefs.GetInt("scoreIndex");

      //  CurrentLocation.instance.user.record.name = CurrentLocation.instance.currentUser;

        CurrentLocation.instance.user.record.stepCount = scoreIndexVal * 40;

        if (scoreIndexVal == 11)
        {
            CurrentLocation.instance.user.record.currentLocation = " ";
            CurrentLocation.instance.user.record.unlockedStates[2] = CurrentLocation.instance.user.record.currentState;

        }
        else
        {
            if (CurrentLocation.instance.user.record.currentState != StatesGenralScript.currentState.ToString() && firstCount == 1)
            {
                CurrentLocation.instance.user.record.unlockedStates[previousStateVal] = CurrentLocation.instance.user.record.currentState;
            }

            if ((scoreIndexVal % 4) != 0)
            {
                CurrentLocation.instance.user.record.currentLocation = CurrentLocation.instance.currentLocationText.text;
            }
            else
            {
                CurrentLocation.instance.user.record.currentLocation = " ";
            }
        }

        CurrentLocation.instance.user.record.currentState = StatesGenralScript.currentState.ToString();
        previousStateVal = (int)StatesGenralScript.currentState;

        firstCount = 1;

        if (!isUserValuetaken)
        {
            CurrentLocation.instance.PostUserData();

        }

        isUserValuetaken = false;

    }

    public void ResetCurrentUserInfo()
    {
        CurrentLocation.instance.user.record.name = CurrentLocation.instance.currentUser;
        CurrentLocation.instance.user.record.stepCount = 0;
        CurrentLocation.instance.user.record.currentState = "";
        CurrentLocation.instance.user.record.currentLocation = "";

        for (int i = 0; i < 3; i++)         //CurrentLocation.instance.user.record.unlockedStates.Count
        {
            CurrentLocation.instance.user.record.unlockedStates[i] = "";
        }
        CurrentLocation.instance.PostUserData();


        for (int j = 0; j < stateInfo.Length; j++)
        {
            for (int k = 0; k < stateInfo[j].SwalkingSliderArray.Length; k++)
            {
                stateInfo[j].SwalkingSliderArray[k].Data.Value = -550;
                stateInfo[j].SwalkingSliderArray[k].Data.Duration = 0;

                Debug.Log(k + " :i:j: " + j);
                stateInfo[j].SwalkingSliderArray[k].BeginAllTransitions();

                if (k % 4 != 3)
                {
                    stateInfo[j].ScheckPoints[k].color = red;

                    stateInfo[j].SlPinArray[k].GetComponent<BoxCollider>().enabled = false;
                    stateInfo[j].SlPinArray[k].GetComponent<SpriteRenderer>().sprite = redSprite;
                    // debugWalk += 20;
                    stateInfo[j].StatePanelLean.TurnOff();
                    stateInfo[j].animatedPin[k] = false;
                    stateInfo[j].ticketBtn.interactable = false;

                }
                else
                {
                    stateInfo[j].ScheckPoints[k].color = light_Blue;
                    stateInfo[j].animatedPin[k] = false;
                    stateInfo[j].ticketBtn.interactable = false;
                }
            }
        }

        debugWalk = 0;
        Sindex = 0;
        StatesGenralScript.currentState = StatesGenralScript.States.UttarPradesh;
        PlayerPrefs.SetInt("scoreIndex", 0);
        isfinalStateCompleted = false;
        stage = 0;
        Startbutton.SetActive(true);

        QuizHandler.stateIndex = 0;
        isResetUserDelhi = true;
        isResetUserPunjab = true;
        isResetUP = true;

        isUserValuetaken = false;

        ResetHighlight.TurnOn();

       // LocationStop();
    }

    public void HighlightEffectProfileController()
    {
        if (Sindex == 0 && (PlayerPrefs.GetInt("scoreIndex") != 0))
        {
            highlightEffectUP.ProfileLoad(currentProfile);
            highlightEffectDelhi.ProfileLoad(lockedStateProfile);
            highlightEffectPunjab.ProfileLoad(lockedStateProfile);
        }
        else if (Sindex == 1)
        {
            highlightEffectUP.ProfileLoad(unlockedStateProfile);
            highlightEffectDelhi.ProfileLoad(currentProfile);
            highlightEffectPunjab.ProfileLoad(lockedStateProfile);
        }
        else if (Sindex == 2)
        {
            highlightEffectUP.ProfileLoad(unlockedStateProfile);
            highlightEffectDelhi.ProfileLoad(unlockedStateProfile);
            highlightEffectPunjab.ProfileLoad(currentProfile);
        }
    }
}

[System.Serializable]
public class StateInfoSliderAndPin
{
    public LeanRectTransformSizeDelta_x[] SwalkingSliderArray;                  // Progress Bar
    public LeanSpriteRendererColor[] SlandmarkArray;
    public GameObject[] SlPinArray;                                            // State LandmarkPins -(AR view Pin)
    public Image[] ScheckPoints;
    public LeanTransformLocalScale_xy[] SleanTransformLocalScale_xy;            // Scale animation for each pin once reached the checkpoint
    public Button ticketBtn;
    public LeanToggle StatePanelLean;
    public bool[] animatedPin;
}