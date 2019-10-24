using System.Collections;
using System;
using UnityEngine;
using WebSocketSharp;
using System.Collections.Generic;
using System.Text;

namespace AssemblyCSharp
{

    [Serializable]
    public class Reply
    {
        // User info
        public int userId = -1;
        // Settings
        public int nIteration = -1;
        public bool registerReplies;
        public string teacher = "<empty>";
        // Question info
        // Before
        public int t = -1;
        public int idQuestion = -1;
        public List<int> idPossibleReplies = new List<int>();
        public string timeDisplay = "<empty>";
        // After
        public int idReply = -1;
        public bool success;
        public string timeReply = "<empty>";
    }

    [Serializable]
    public class Question
    {
        public int userId = -1;
        public int t = -1;
        public int nIteration = -1;
        public string question = "<empty>";
        public List<string> possibleReplies = new List<string>();
        public int idCorrectAnswer = -1;
        public int idQuestion = -1;
        public int idReply = -1;
        public bool newQuestion;
        public List<int> idPossibleReplies = new List<int>();

        public static Question CreateFromJson(string jsonString)
        {
            return JsonUtility.FromJson<Question>(jsonString);
        }
    }
}

