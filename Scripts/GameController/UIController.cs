using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using AssemblyCSharp;

class Bool {
	public static string visible = "Visible";
	public static string glow = "Glow";
}


public class UIController : MonoBehaviour {

    public float timeDisplayingCorrect = 1.0f;

    public Color colorNeutral = new Color(0.3f, 0.4f, 0.6f, 0.3f);
    public Color colorCorrect = new Color(0.3f, 0.4f, 0.6f, 0.3f);
    public Color colorIncorrect = new Color(0.3f, 0.4f, 0.6f, 0.3f);
    public Color colorDisabled = new Color(0.3f, 0.4f, 0.6f, 0.3f);

    public Button startButton;
    public List<Button> replyButton;
    public Text questionText;

    public GameObject endScreen;
    public GameObject homeScreen;

    public GameObject progressionTool;

    public Image progressionBar;

    List<Text> replyText;

    int correctAnswerIdx;

    bool userReplied;

    Reply reply;

    GameController gameController;

    // -------------- Inherited from MonoBehavior ---------------------------- //

    void Awake () {

        // uiProgressBars = GetComponent<UIProgressBars> ();
        gameController = GetComponent<GameController>();
        replyText = new List<Text>();
        startButton.onClick.AddListener(UserStart);

        for (int i = 0; i < replyButton.Count; i++)
        {
            int v = i;
            replyButton[i].onClick.AddListener(delegate { Reply(v); });
            replyButton[i].interactable = false;
            replyText.Add(GetText(replyButton[i]));
        }

        reply = new Reply();
    }

	void Start () {
        Anim(homeScreen);
        Anim(startButton);
    }

	void Update () {}

	//// ----------------- //

	void Anim (GameObject go, bool visible=true, bool glow=false) {
		Animator anim = go.GetComponent<Animator> ();
		anim.SetBool (Bool.visible, visible);
		anim.SetBool(Bool.glow, glow);
	}

	void Anim (Button btn, bool visible=true, bool glow=false) {
		Anim (btn.gameObject, visible, glow);
	}

	void Anim (Slider slider, bool visible=true, bool glow=false) {
		Anim (slider.gameObject, visible, glow);
	}

	void Anim (Text text, bool visible=true, bool glow=false) {
		Anim (text.gameObject, visible, glow);
	}

	void Anim (Image image, bool visible=true, bool glow=false) {
		Anim (image.gameObject, visible, glow);
	}

    // ---------------------------------- //


    void UserStart()
    {
        Debug.Log("[UIController] User clicked on starting button.");

        startButton.interactable = false;
        Anim(startButton, visible: false);
        Anim(homeScreen, visible: false);

        gameController.UserReplied(reply);
    }

    void Reply(int idx)
    {
        if (!userReplied)
        {
            // Get the time of reply
            reply.timeReply = TimeStamp.Get();
            reply.reply = replyText[idx].text;

            // Disable the buttons
            for (int i = 0; i < replyButton.Count; i++)
            {
                replyButton[i].interactable = false;
                replyButton[i].image.color = colorDisabled;
            }

            // Put in green the correct answer
            replyButton[correctAnswerIdx].image.color = colorCorrect;

            // Put in red the wrong answer if applicable
            if (idx != correctAnswerIdx)
            {
                replyButton[idx].image.color = colorIncorrect;
                replyButton[correctAnswerIdx].interactable = true;
                Anim(replyButton[correctAnswerIdx], glow: true);
                userReplied = true;
            }
            else
            {
                Invoke("UserReplied", timeDisplayingCorrect);
            }

        }
        else
        {
            replyButton[idx].interactable = false;
            UserReplied();
        }
    }

    void UserReplied()
    {
        gameController.UserReplied(reply);
    }

    // ----------------------------------- //

    Text GetText(Button button)
    {
        return button.gameObject.GetComponentInChildren<Text>();
    }

    // -------------------------------- //

    public void SetQuestion(Question question)
    {
        if (question.t != -1)
        {
            userReplied = false;
            correctAnswerIdx = question.correctAnswerIdx;

            // Update question
            questionText.text = question.question;
            Anim(questionText);


            // Update progression bar
            Anim(progressionTool);
            UpdateProgression(question.t, question.tMax);

            // Update reply buttons
            for (int i = 0; i < replyButton.Count; i++)
            {
                replyText[i].text = question.possibleReplies[i];
                replyButton[i].image.color = colorNeutral;
                replyButton[i].interactable = true;
                Anim(replyButton[i], glow: false);
            }

            // Prepare sending of the reply to the server
            reply.userId = question.userId;
            reply.t = question.t;
            reply.timeDisplay = TimeStamp.Get();
        }

        else
        {
            Anim(endScreen);
        }

    }

    // ------------ Progress bar ----------- //

    public void UpdateProgression (int value, int maxValue) 
    {
      progressionBar.fillAmount = (float) value / maxValue;
    }
}




//// --------------- Communication with gameController ---------- //

//public void Init (string pseudo, int nGoods) {

//	uiProgressBars.Init (pseudo);
//	uiButtons.Init (nGoods);
//	uiTutorial.Init (nGoods, pseudo);
//}

//// ---------------------- //

//public void SetTitle (string value) {
//	title.text = value;
//}

//public void SetVersion (string value) {
//    version.text = value;
//}

//// ------- //

//public void ShowTitle (bool visible=true) {
//	Anim (title, visible);
//}

//public void ShowLogo (bool visible=true, bool glow=false) {
//	Anim (logo, visible, glow);
//}

//   public void ShowVersion (bool visible = true, bool glow = false) {
//       Anim(version, visible, glow);
//   }

//   public void ShowConnectionIndicator (bool visible = true, bool glow=false) {
//       Anim(connectionIndicator, visible, glow);
//   }

//   public void ShowConnected (bool visible=true) {
//       connected.gameObject.SetActive (visible);
//       nonConnected.gameObject.SetActive (!visible);
//   }

//// ------- HOME VIEW --------- //

//public void HomeWU (string version) {

//	ShowLogo ();
//	ShowTitle ();
//	SetTitle (Title.title);
//       SetVersion (version);
//	// uiButtons.ShowNext (glow: true);

//       ShowVersion ();
//       ShowConnectionIndicator ();
//       ShowConnected (false);
//}

//public void HomeWS () {

//	ShowLogo (glow: true);
//       ShowVersion (false);
//}

//// -------- TRAINING VIEW ----- //

//public void TrainingBegin () {

//	Anim (logo, visible: true);
//    // uiButtons.ShowNext (glow: true);
//	uiTutorial.SetText (Texts.training);
//}

//public void HideTrainingMsg () {

//	ShowLogo (visible: false);
//	uiTutorial.ShowText (false);
//}

//public void TrainingReady () {

//	Anim (woodInBox, false);
//	Anim (stoneInBox, false);
//	Anim (clayInBox, false);

//	Anim (character, false);
//	Anim (scoreFinal, false);

//	Anim (wheatDesired, false);

//	title.text = Title.title;
//	Anim (title);

//	Anim (logo);
//	uiTutorial.SetText(Texts.ready);
//	uiTutorial.ShowText ();
//	uiButtons.ShowNext (glow: true);
//}

//// ---------------------------------- //


//public void ShowProgress (bool visible=true) {

//  Anim (radialProgressBar, visible: visible);
//  Anim (pseudo, visible: visible);
//}

//public void ShowWaitingMessage (int progress) {

//  UpdateStatus (progress);
//  ShowStatus (true);
//  StatusMessage (Texts.waitingOtherPlayers);
//}

//// ------------------------------ //

//public void UpdateRadial (int value, int maxValue) {
//  radialProgressBar.fillAmount = (float) value / maxValue;
//}

//public void UpdateStatus (int value, int maxValue=100) {
//  statusProgressBar.value = (float) value / maxValue;
//}

//// ------------------------- //

//public void StatusMessage (string msg, Color color=default(Color), bool glow=false) {

//  Anim (statusText, visible: true, glow: glow);
//  statusText.text = msg;
//  if (color == default(Color)) {
//      statusText.color = Color.black;
//  } else {
//      statusText.color = color;
//  }
//}

//public void ResultView (bool success, int goodInHand, int goodDesired) {

//	uiButtons.ShowStone (false);
//	uiButtons.ShowWood (false);
//	uiButtons.ShowClay (false);

//	if (success) {

//		Anim (pictureSuccess);
//		Anim (arrow);

//		// Depending of good in hand
//		switch (goodInHand) {

//		case Good.wood:
//			Anim (woodInBox, false);
//			Anim (woodInHand);
//			break;

//		case Good.stone:
//			Anim (stoneInBox, false);
//			Anim (stoneInHand);
//			break;

//		case Good.clay:
//			Anim (clayInBox, false);
//			Anim (clayInHand);
//			break;

//		default:
//			throw new Exception ("Good " + goodInHand + " doesn't exist");
//		}

//		// Depending of the desired good
//		switch (goodDesired) {
//		case Good.wheat:
//			Anim (scoreToAdd, glow: true);
//			Anim (wheatDesired);
//			break;

//		case Good.wood:
//			Anim (woodDesired);
//			break;

//		case Good.stone:
//			Anim (stoneDesired);
//			break;

//		case Good.clay:
//			Anim (clayDesired);
//			break;

//		default:
//			throw new Exception ("Good " + goodDesired + " doesn't exist");
//		}

//	// if not a success...
//	} else {
//		Anim (pictureLost);
//	}

//	uiButtons.ShowNext (glow: true);
//}

//public void HideResults () {

//	uiButtons.ShowNext (false);

//	foreach (Image img in new Image[] {
//		pictureSuccess, pictureLost,
//		arrow,
//		woodInHand, stoneInHand, clayInHand,
//		woodDesired, wheatDesired, stoneDesired, clayDesired
//	}) {
//		Anim (img, false);
//	}

//	Anim (scoreToAdd, false);
//}

//public void ChoiceMadeView (int goodInHand, int goodDesired) {

//	uiButtons.ShowCorrespondingButton (goodInHand: goodInHand, goodDesired: goodDesired);

//	switch (goodInHand) {

//	case Good.wood:
//		Anim (woodInBox);
//		break;

//	case Good.stone:
//		Anim (stoneInBox);
//		break;

//	case Good.clay:
//		Anim (clayInHand);
//		break;
//	default:
//		throw new Exception ("Good " + goodDesired + " doesn't exist");
//	}
//}

//public void EndView (int scoreValue, int tMax) {

//	uiProgressBars.UpdateRadial (tMax, tMax);
//	ShowScore (false);
//	scoreFinal.text = scoreValue.ToString ();
//	Anim (scoreFinal, glow: true);
//	Anim (wheatDesired);
//	title.text = Title.end;
//	Anim (title);
//}

