using System.Collections;
using System.Globalization;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

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
    public List<Button> reply;
    public Text question;

    // Bottom: status bar
    // public Slider statusProgressBar;
    // public Text statusText;

    // Top left: radial progress bar
    // public Image radialProgressBar;
    // public Text pseudo;

    List<Text> replyText;

   // string replyUserStr;

    int correctAnswerIdx;

    bool userReplied;

    string timeDisplay;
    string timeReply;

    GameController gameController;

    // -------------- Inherited from MonoBehavior ---------------------------- //

    void Awake () {

        // uiProgressBars = GetComponent<UIProgressBars> ();
        gameController = GetComponent<GameController>();
        replyText = new List<Text>();
        startButton.onClick.AddListener(StartButton);

        for (int i = 0; i < reply.Count; i++)
        {
            int v = i;
            reply[i].onClick.AddListener(delegate { Reply(v); });
            reply[i].interactable = false;
            replyText.Add(GetText(reply[i]));
        }
    }

	void Start () {}

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


    void StartButton()
    {
        Debug.Log("[UIController] User clicked on starting button.");

        startButton.interactable = false;
        Anim(startButton, visible: false);

        gameController.UserReplied();
    }

    void Reply(int idx)
    {
        if (!userReplied)
        {
            // Get the time of reply
            timeReply = GetTime();

            // Disable the buttons
            for (int i = 0; i < reply.Count; i++)
            {
                reply[i].interactable = false;
                reply[i].image.color = colorDisabled;
            }

            // Put in green the correct answer
            reply[correctAnswerIdx].image.color = colorCorrect;

            // Put in red the wrong answer if applicable
            if (idx != correctAnswerIdx)
            {
                reply[idx].image.color = colorIncorrect;
                reply[correctAnswerIdx].interactable = true;
                Anim(reply[correctAnswerIdx], glow: true);
                userReplied = true;
            }
            else
            {
                Invoke("UserReplied", timeDisplayingCorrect);
            }

        }
        else
        {
            reply[idx].interactable = false;
            UserReplied();
        }
    }

    void UserReplied()
    {
        gameController.UserReplied();
    }

    // ----------------------------------- //

    Text GetText(Button button)
    {
        return button.gameObject.GetComponentInChildren<Text>();
    }

    // -------------------------------- //

    public void SetQuestion(string questionStr, int correctAnswerIdx, 
        List<string> answerTextStr)
    {
        userReplied = false;
        this.correctAnswerIdx = correctAnswerIdx;

        question.text = questionStr;

        Anim(question);

        for (int i = 0; i < reply.Count; i++)
        {
            replyText[i].text = answerTextStr[i];
            reply[i].image.color = colorNeutral; 
            reply[i].interactable = true;
            Anim(reply[i], glow: false);
        }

        timeDisplay = GetTime();
    }

    static string GetTime()
    {
        return DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff",
                                            CultureInfo.InvariantCulture);
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

