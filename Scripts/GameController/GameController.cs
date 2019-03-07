using System.Collections;
using UnityEngine;
using System;
using AssemblyCSharp;


public class GameController : MonoBehaviour
{

    public string version;

    UIController uiController;

    Client client;

    bool newQuestion;
    Question question;

    bool newReply;
    Reply reply;

    bool serverConnected;

    // -------------- Inherited from MonoBehavior ---------------------------- //

    void Awake()
    {

        uiController = GetComponent<UIController>();
        client = GetComponent<Client>();

        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }

    void Start() {}

    void Update()
    {
        if (serverConnected)
        {
            if (newQuestion)
            {
                uiController.SetQuestion(question);
                newQuestion = false;
            } 
            else if (newReply) 
            {
                client.AskNewQuestion(reply);
                newReply = false;
            }
            else
            {

            }
        }
    }

    // ----------- From UIManager ---------------- //

    public void UserReplied(Reply reply)
    {
        Debug.Log("[GameController] User replied");
        this.reply = reply;
        newReply = true;
    }

    // ---------- From Client ---------- //

    public void ServerConnected()
    {
        //Reply initReply = new Reply
        //{
        //    timeReply = TimeStamp.Get()
        //};
        serverConnected = true;
    }

    public void ServerNewQuestion(Question question)
    {
        this.question = question;
        newQuestion = true;
    }

}

//public void UserReplied(Reply reply)
//{
//    Debug.Log("[GameController] User replied");
//    client.AskNewQuestion(reply);
//    // AskNewQuestion();

//    //switch (state) {

//    //case TL.AskWU:
//    //  //uiController.HomeWS ();
//    //  // client.Init ();
//    //  state = TL.InitWS;
//    //  break;

//    //case TL.ReplyWU:

//    ////if (survey.EvaluateUserData ()) {
//    ////    survey.View (false);
//    ////             uiController.ShowLogo(visible: true, glow: true);
//    ////    client.Survey (age: survey.GetAge(), sex:survey.GetSex());
//    ////    state = TL.SurveyWS;
//    ////} else {
//    ////    uiButtons.ShowNext ();
//    ////}
//    //break;
//}

// ------------------------------ //

//void LogState() {
//  Debug.Log ("[GameController] My state is '" + state + "'.");
//}

//IEnumerator ActionWithDelay (Action methodName, float seconds) {

//  yield return new WaitForSeconds(seconds);
//  methodName ();
//}

//void AskNewQuestion()
//{
//    string question = "愛";

//    List<string> reply = new List<string>
//    {
//        "Love",
//        "Hatred",
//        "Car",
//        "Dog",
//        "Go",
//        "Knowledge"
//    };


//    reply.Shuffle();

//    int correctAnswerIdx = reply.IndexOf("Love");

//    uiController.SetQuestion(question, correctAnswerIdx, reply);
//}

//// ----------- From Client ----------- //

//public void ServerReplied () {

//	Debug.Log ("[GameController] Received response from server.");

//	if (client.GetWait ()) {

//		if (state == TL.SurveyWS || state == TL.TrainingDoneWS)  {
//			uiController.ShowLogo (glow: true);
//			uiProgressBars.StatusMessage (Texts.waitingOtherPlayers, glow: true);
//		}

//		uiProgressBars.ShowWaitingMessage (client.GetProgress ());

//	} else {

//		uiProgressBars.ShowStatus (false);
//		uiController.ShowLogo(false);

//		switch (state) {

//		case TL.InitWS:

//			// Initialize things
//			uiController.Init (client.GetPseudo (), client.GetNGoods ());

//			if (client.GetStep () == GameStep.training) {
//				BeginTutorial ();
//			} else if (client.GetStep () == GameStep.survey) {
//				BeginSurvey ();
//			} else if (client.GetStep () == GameStep.game) {
//				BeginGame ();
//			} else {
//				throw new Exception (String.Format(
//                           "[GameController] Step '{0}' was not expected.", 
//                           client.GetStep()));
//			}

//			break;

//		case TL.SurveyWS:

//			BeginTutorial ();
//			break;

//		case TL.TrainingChoiceWS:

//			success = client.GetTrainingSuccess ();
//			t = client.GetTrainingT ();
//			score = client.GetTrainingScore ();
//			end = client.GetTrainingEnd ();

//			uiProgressBars.UpdateRadial (t, tMax);
//			uiController.ResultView (success, goodInHand, goodDesired);

//			state = TL.TrainingResultWU;
//			break;

//		case TL.TrainingDoneWS:

//			BeginGame ();
//			break;

//		case TL.GameChoiceWS:

//			success = client.GetSuccess ();
//			t = client.GetT ();
//			score = client.GetScore ();
//			end = client.GetEnd ();

//			uiProgressBars.UpdateRadial (t, tMax);
//			uiController.ResultView (success, goodInHand, goodDesired);

//			state = TL.GameResultWU;
//			break;
//		}
//	}
//}

//   public void OnDisconnection () {
//       uiController.ShowConnected(false);
//   }

//   public void OnConnection () {
//       uiController.ShowConnected ();
//   }

//   // -------------------------- //

//   void BeginTutorial () {

//	uiController.ShowLogo (false);
//	uiProgressBars.ShowStatus (false);
//	uiTutorial.Begin ();
//	state = TL.TutorialWU;
//}

//void BeginGame (bool training=false) {

//	if (training) {
//		choiceMade = client.GetTrainingChoiceMade ();
//		t = client.GetTrainingT ();
//		tMax = client.GetTrainingTMax (); 
//		goodInHand = client.GetTrainingGoodInHand (); 
//		goodDesired = client.GetTrainingGoodDesired (); 
//		score = client.GetTrainingScore ();

//		uiController.ShowTitle ();
//		uiController.SetTitle (Title.training);
//	} else {
//		choiceMade = client.GetChoiceMade ();
//		t = client.GetT ();
//		tMax = client.GetTMax (); 
//		goodInHand = client.GetGoodInHand (); 
//		goodDesired = client.GetGoodDesired (); 
//		score = client.GetScore ();

//		uiController.ShowTitle (visible: false);
//	}

//	uiController.SetScore (score);

//	uiController.ShowScore ();

//	uiProgressBars.ShowProgress ();
//	uiProgressBars.UpdateRadial (t, tMax);
//	uiProgressBars.ShowStatus (false);

//	uiController.ShowLogo (false);
//	uiController.ShowCharacter ();

//	if (choiceMade) {
//		uiController.ChoiceMadeView (goodInHand, goodDesired);
//		if (training) {
//			client.TrainingChoice (goodDesired);
//			state = TL.TrainingChoiceWS;
//		} else {
//			client.Choice (goodDesired);
//			state = TL.GameChoiceWS;
//		}
//	} else {
//		uiController.ChoiceView (goodInHand);
//		if (training) {
//			state = TL.TrainingChoiceWU;
//		} else {
//			state = TL.GameChoiceWU;		
//		}
//	}
//}

//void BeginSurvey () {

//	survey.View ();
//	state = TL.SurveyWU;
//}

//void UpdateGoodInHand () {

//	if (success) {
//		if (goodDesired == Good.wheat) {
//			goodInHand = Good.wood;
//		} else {
//			goodInHand = goodDesired;
//		}		
//	} 
//}

//void BeginTurn (bool training=false) {

//	uiController.SetScore (score);
//	uiController.HideResults ();

//	if (end) {
//		uiController.EndView (score, tMax);

//		if (training) {
//			uiButtons.ShowNext (glow: true);
//			state = TL.TrainingEndWU;
//		} else {
//			state = TL.End;
//		}
//	} else {
//		UpdateGoodInHand ();
//		uiController.ChoiceView (goodInHand);
//		if (training) {
//			state = TL.TrainingChoiceWU;
//		} else {
//			state = TL.GameChoiceWU;	
//		}
//	}
//}
