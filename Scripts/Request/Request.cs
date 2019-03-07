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
        public int userId = -1;
        public int t = -1;
        public string reply = "<empty>";
        public string timeDisplay = "<empty>";
        public string timeReply = "<empty>";
        // public string deviceId = "";
    }

    [Serializable]
    public class Question
    {
        public int userId = -1;
        public int t = -1;
        public int tMax = -1;
        public int correctAnswerIdx = -1;
        public string question = "<empty>";
        public string correctAnswer = "<empty>";
        public List<string> possibleReplies = new List<string>();

        public static Question CreateFromJson(string jsonString)
        {
            return JsonUtility.FromJson<Question>(jsonString);
        }
    }
}

