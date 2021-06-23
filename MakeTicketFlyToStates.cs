using Lean.Gui;
using Lean.Transition.Method;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeTicketFlyToStates : MonoBehaviour
{

    public GameObject StateUP;
    public GameObject StatePunjab;
    public GameObject StateDelhi;
    public GameObject Ticket;
    public LeanTransformPosition_xy leanTransformPosition_xy;

    public LeanToggle highlightUP;
    public LeanToggle highlightDelhi;
    public LeanToggle highlightPunjab;
    public LeanToggle highlightAllUnlockedStates;

    public void MakeTicketFly(GameObject state)
    {
        GameObject currentState = null;

        switch (StatesGenralScript.currentState)
        {
            case StatesGenralScript.States.Delhi:
                currentState = StateDelhi;
                break;

            case StatesGenralScript.States.UttarPradesh:
                currentState = StateUP;
                break;

            case StatesGenralScript.States.Punjab:
                currentState = StatePunjab;
                break;
        }

        leanTransformPosition_xy.Data.Value.x = currentState.transform.position.x;
        leanTransformPosition_xy.Data.Value.y = currentState.transform.position.y;
        leanTransformPosition_xy.BeginAllTransitions();

        StepCounter.isResetUP = false;
    }

    public void SelectHighlight()
    {
        if(!StepCounter.isfinalStateCompleted && !StatesGenralScript.notInCurrentState)
        {
            switch (StatesGenralScript.currentState)
            {
                case StatesGenralScript.States.Delhi:
                    highlightDelhi.TurnOn();
                    break;

                case StatesGenralScript.States.UttarPradesh:
                    highlightUP.TurnOn();
                    break;

                case StatesGenralScript.States.Punjab:
                    highlightPunjab.TurnOn();
                    break;
            }
        }
        else if (StepCounter.isfinalStateCompleted)
        {
            highlightAllUnlockedStates.TurnOn();
        }
        else
        {
            switch (StatesGenralScript.previousState)
            {
                case StatesGenralScript.States.Delhi:
                    highlightDelhi.TurnOn();
                    break;

                case StatesGenralScript.States.UttarPradesh:
                    highlightUP.TurnOn();
                    break;

                case StatesGenralScript.States.Punjab:
                    highlightPunjab.TurnOn();
                    break;
            }
        }
       
    }


}
