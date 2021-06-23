using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionAndAnswerManager : MonoBehaviour
{

    public List<StateProblem> numberOfStates;








    [System.Serializable]
    public class StateProblem
    {
        public string stateName;
        public List<ProblemHolder> problemHolder;
    }

    [System.Serializable]
    public class ProblemHolder
    {
        public int problemNumber;
        public string question;
        public string answer;
        public List<string> options;
    }
}
