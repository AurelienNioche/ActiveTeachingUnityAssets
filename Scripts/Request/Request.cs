using System.Collections;
using System;
using UnityEngine;
using WebSocketSharp;
using System.Collections.Generic;
using System.Text;

namespace AssemblyCSharp
{

    [Serializable]
    class Reply
    {
        public string reply = "<empty>";
        public string timeDisplay = "<empty>";
        public string timeReply = "<empty>";
        public int questionIdx = -1;
        // public string deviceId = "";
    }

    [Serializable]
    public class Question
    {
        public string question = "<empty>";
        public string correctAnswer = "<empty>";
        public List<string> possibleReplies = new List<string>();

        public static Question CreateFromJson(string jsonString)
        {
            return JsonUtility.FromJson<Question>(jsonString);
        }
    }
}

