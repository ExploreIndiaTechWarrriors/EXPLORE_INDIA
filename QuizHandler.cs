using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Lean.Gui;

public class QuizHandler : MonoBehaviour
{
    Image answerNext_img;
    int problemNumber;
    int numberOfQuestionAnsweredCorrect;

    public UnityEvent tryAgainQuiz;
    public UnityEvent quizComplete;
    public UnityEvent FinalStateComplete;

    public QuestionAndAnswerManager questionAndAnswerManager;


    public TextMeshProUGUI questionText;
    public TextMeshProUGUI option1Text;
    public TextMeshProUGUI option2Text;
    public TextMeshProUGUI option3Text;
    public TextMeshProUGUI option4Text;


    public bool optionA;
    public bool optionB;
    public bool optionC;
    public bool optionD;

    public bool buttonA;
    public bool buttonB;
    public bool buttonC;
    public bool buttonD;

    public static int stateIndex;

    public Button answerNext_btn;

    Color color;

    public List<int> randomQuestionNumbers;
    int r;

    public TextMeshProUGUI stateInformationText;
    public TextMeshProUGUI quizResultText;
    public LeanToggle quizResultPanel;

    public bool isFinalQuizCompleted, isQuizCompleted, isTryAgainQuiz; 

    private void OnEnable()
    {

        bool isPresent(int i)
        {
            for (int t = 0; t < randomQuestionNumbers.Count; t++)
            {
                if (i == randomQuestionNumbers[t])
                    return true;
            }
            return false;
        }

        int z = 0;
        problemNumber = -1;
        randomQuestionNumbers = new List<int>();
        while (z < 3)
        {
            r = Random.Range(0, 10);
            if (!isPresent(r))
            {
                randomQuestionNumbers.Add(r);
                z++;
            }

        }



        answerNext_btn.onClick.AddListener(delegate
        {
            if (problemNumber > -1)
            {
                if (optionA)
                {
                    if (string.Compare(option1Text.text, questionAndAnswerManager.numberOfStates[stateIndex].problemHolder[randomQuestionNumbers[problemNumber]].answer) == 0)
                    {
                        numberOfQuestionAnsweredCorrect++;
                    }

                }
                else if (optionB)
                {
                    if (string.Compare(option2Text.text, questionAndAnswerManager.numberOfStates[stateIndex].problemHolder[randomQuestionNumbers[problemNumber]].answer) == 0)
                    {
                        numberOfQuestionAnsweredCorrect++;
                    }
                }
                else if (optionC)
                {

                    if (string.Compare(option3Text.text, questionAndAnswerManager.numberOfStates[stateIndex].problemHolder[randomQuestionNumbers[problemNumber]].answer) == 0)
                    {
                        numberOfQuestionAnsweredCorrect++;
                    }
                }
                else if (optionD)
                {
                    if (string.Compare(option4Text.text, questionAndAnswerManager.numberOfStates[stateIndex].problemHolder[randomQuestionNumbers[problemNumber]].answer) == 0)
                    {
                        numberOfQuestionAnsweredCorrect++;
                    }
                }

            }



            problemNumber++;

            DiaplayQuestionAndOptions();
        });

        problemNumber++;
        DiaplayQuestionAndOptions();


    }

    public void MakeAllOptionsFlase()
    {
        optionA = false;
        optionB = false;
        optionC = false;
        optionD = false;
    }

    public void DiaplayQuestionAndOptions()
    {
        Debug.Log(problemNumber + " :NO: " + stateIndex + " :Si");

        if (problemNumber >= 3)
        {
            if (numberOfQuestionAnsweredCorrect >= 3)
            {
                stateIndex++;
                
                quizResultText.text = "Congratulations! You have answered all 3 questions correctly";
                if (stateIndex > 2)
                {
                    stateIndex = 2;
                    isFinalQuizCompleted = true;
                   // FinalStateComplete.Invoke();
                }
                else
                {
                    StatesGenralScript.currentState = (StatesGenralScript.States)stateIndex;
                    Debug.Log(stateIndex);
                    isQuizCompleted = true;
                   // quizComplete.Invoke();
                }
                
            }
            else
            {

                if(numberOfQuestionAnsweredCorrect >= 2)
                {
                    quizResultText.text = "You have answered (2/3) questions correctly";
                }
                else if(numberOfQuestionAnsweredCorrect >=1)
                {
                    quizResultText.text = "You have answered (1/3) question correctly";
                }
                else
                {
                    quizResultText.text = "You have answered (0/3) questions correctly";
                }

                 stateInformationText.text = " Walk 35 steps to reach the nearest check point.";

                isTryAgainQuiz = true;
                print("try Again" + numberOfQuestionAnsweredCorrect);
               // tryAgainQuiz.Invoke();
            }

            quizResultPanel.TurnOn();

            gameObject.SetActive(false);
            return;
        }

        ColorUtility.TryParseHtmlString("#FBDC9B", out color);



        answerNext_img = answerNext_btn.transform.GetComponent<Image>();
        answerNext_img.color = color;
        answerNext_btn.interactable = false;


        optionA = false;
        optionB = false;
        optionC = false;
        optionD = false;


        questionText.text = questionAndAnswerManager.numberOfStates[stateIndex].problemHolder[randomQuestionNumbers[problemNumber]].question;
        option1Text.text = questionAndAnswerManager.numberOfStates[stateIndex].problemHolder[randomQuestionNumbers[problemNumber]].options[0];
        option2Text.text = questionAndAnswerManager.numberOfStates[stateIndex].problemHolder[randomQuestionNumbers[problemNumber]].options[1];
        option3Text.text = questionAndAnswerManager.numberOfStates[stateIndex].problemHolder[randomQuestionNumbers[problemNumber]].options[2];
        option4Text.text = questionAndAnswerManager.numberOfStates[stateIndex].problemHolder[randomQuestionNumbers[problemNumber]].options[3];


    }


    public void OnButtonClick(int i)
    {
        MakeAllOptionsFlase();

        switch (i)
        {
            case 1:
                optionA = true;
                break;
            case 2:
                optionB = true;
                break;
            case 3:
                optionC = true;
                break;
            case 4:
                optionD = true;
                break;
        }

        ColorUtility.TryParseHtmlString("#F1B73E", out color);

        answerNext_img.color = color;
        answerNext_btn.interactable = true;

    }

    private void OnDisable()
    {
        //answerNext_btn.onClick.RemoveListener(delegate
        //{
        //    if (problemNumber > -1)
        //    {
        //        if (optionA)
        //        {
        //            if (string.Compare(option1Text.text, questionAndAnswerManager.numberOfStates[stateIndex].problemHolder[randomQuestionNumbers[problemNumber]].answer) == 0)
        //            {
        //                numberOfQuestionAnsweredCorrect++;
        //            }

        //        }
        //        else if (optionB)
        //        {
        //            if (string.Compare(option2Text.text, questionAndAnswerManager.numberOfStates[stateIndex].problemHolder[randomQuestionNumbers[problemNumber]].answer) == 0)
        //            {
        //                numberOfQuestionAnsweredCorrect++;
        //            }
        //        }
        //        else if (optionC)
        //        {

        //            if (string.Compare(option3Text.text, questionAndAnswerManager.numberOfStates[stateIndex].problemHolder[randomQuestionNumbers[problemNumber]].answer) == 0)
        //            {
        //                numberOfQuestionAnsweredCorrect++;
        //            }
        //        }
        //        else if (optionD)
        //        {
        //            if (string.Compare(option4Text.text, questionAndAnswerManager.numberOfStates[stateIndex].problemHolder[randomQuestionNumbers[problemNumber]].answer) == 0)
        //            {
        //                numberOfQuestionAnsweredCorrect++;
        //            }
        //        }

        //    }



        //    problemNumber++;

        //    DiaplayQuestionAndOptions();
        //});

        answerNext_btn.onClick.RemoveAllListeners();

        numberOfQuestionAnsweredCorrect = 0;
        problemNumber = -1;
    }

    public void QuizResultEvent()
    {
        if(isTryAgainQuiz)
        {
            tryAgainQuiz.Invoke();
            isTryAgainQuiz = false;
        }
        else if(isQuizCompleted)
        {
            quizComplete.Invoke();
            isQuizCompleted = false;
        }
        else if(isFinalQuizCompleted)
        {
            FinalStateComplete.Invoke();
            isFinalQuizCompleted = false;
        }
    }

}
