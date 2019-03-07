using System.Collections;
using System;
using UnityEngine;
using WebSocketSharp;
using System.Collections.Generic;
using System.Text;
using AssemblyCSharp;


    public class Client : MonoBehaviour
{

    public string url = "<url>";

    // ------------------------------------------------------ //

    WebSocketSharp.WebSocket w;
    GameController gameController;

    // ------------------------------------------------------ //

    void Start()
    {
        // deviceId = SystemInfo.deviceUniqueIdentifier;
        gameController = GetComponent<GameController>();
        StartWebSocket();
    }

    void StartWebSocket()
    {
        Debug.Log("[Client] Connecting...");

        float timeConnection = Time.time;

        w = new WebSocketSharp.WebSocket(url);

        w.OnMessage += (sender, e) => OnMessage(e);
        w.OnOpen += (sender, e) => OnOpen();
        w.OnError += (sender, e) => OnError(e);
        w.OnClose += (sender, e) => OnClose(e);
        w.ConnectAsync();
    }

    void OnApplicationQuit()
    {
        w.Close();
    }

    void OnOpen()
    {
        Debug.Log("[Client] Connected!");
        gameController.ServerConnected();
    }

    void OnError(ErrorEventArgs e)
    {
        Debug.Log(string.Format(
            "[Client] Using url '{0}', I got error '{1}'.",
            new object[] { url, e.Message }));
    }

    void OnClose(CloseEventArgs e)
    {
        Debug.Log(string.Format(
            "[Client] WebSocket has been closed with reason: '{0}' (code {1}).",
            new object[] { e.Reason, e.Code }));
    }

    void OnMessage(MessageEventArgs e)
    {
        string msg = e.Data;

        Debug.Log("[Client] Received msg: " + msg);

        Question r = Question.CreateFromJson(msg);

        gameController.ServerNewQuestion(r);
    }

    void Disconnect()
    {
        if (w == null)
        {
            Debug.Log("[Client] No websocket to disconnect.");
        }
        else
        {
            Debug.Log("[Client] Disconnect.");
            w.CloseAsync();
        }
    }

    public void AskNewQuestion(Reply reply)
    {
        string toSend = JsonUtility.ToJson(reply);
        w.Send(toSend);
    }
}

//    public int reconnectTime = 1000;
//    public int timeOut = 30000;
//    public int delayRequest = 2000;
//    public int delayRequestNoResponse = 10000;
//    //  public int delayPing = 1000;
// string error;
//string currentJSONRequest = "";

// bool connected;
// bool justDisconnect;
// bool justConnect;
// bool receivedResponse;
// bool receivedPing;
// bool readyToSend = true;

// bool responseTreated;

// float timeLastReconnection = 0;
//void HandleResponse(string response)
//{
//    Debug.Log("Receive response");
//}

//    IEnumerator ReConnect()
//    {
//        yield return new WaitWhile(
//            () => Time.time - timeLastReconnection < reconnectTime / 1000.0f
//        );

//        if (!connected)
//        {
//            timeLastReconnection = Time.time;
//            StartCoroutine(StartWebSocket());
//        }
//    }

//IEnumerator SendRequest()
//{
//    bool sending = false;
//    try
//    {
//        if (!connected || w == null)
//        {
//            Debug.Log("[Client] I'm not connected so I couldn't send a message.");
//        }
//        else
//        {
//            if (currentJSONRequest == "")
//            {
//                Debug.Log("[Client] I send a ping");
//                RequestPing req = new RequestPing
//                {
//                    deviceId = deviceId
//                };
//                w.Send(JsonUtility.ToJson(req));
//            }
//            else
//            {
//                Debug.Log("[Client] I send a json request");
//                w.Send(currentJSONRequest);
//            }
//            sending = true;
//        }
//    }
//    catch (Exception)
//    {
//        Debug.Log("[Client ] I got an exception during sending.");
//    }
//    if (sending)
//    {
//        float sendingTime = Time.time;
//        receivedPing = false;
//        receivedResponse = false;
//        yield return new WaitUntil(
//            () => Time.time - sendingTime > delayRequestNoResponse / 1000.0f
//            || receivedResponse || receivedPing);
//    }
//    yield return new WaitForSeconds(delayRequest / 1000.0f);
//    readyToSend = true;
//}

//    void Update()
//    {

//        if (justConnect)
//        {
//            justConnect = false;
//            gameController.OnConnection();
//        }
//        else if (justDisconnect)
//        {
//            justDisconnect = false;
//            Debug.Log("[Client] I've been disconnected!");

//            // Try to reconnect
//            Debug.Log("[Client] I will try to reconnect...");
//            StartCoroutine(ReConnect());
//            gameController.OnDisconnection();
//        }
//        else if (responseTreated)
//        {
//            responseTreated = false;
//            gameController.ServerReplied();
//        }
//        else if (readyToSend)
//        {
//            readyToSend = false;
//            StartCoroutine(SendRequest());
//        }
//    }

//    // ------------------------------------------------------------------------------------- //

//    public void Init()
//    {

//        demand = Demand.init;

//        RequestInit req = new RequestInit
//        {
//            deviceId = deviceId
//        };

//        Debug.Log(String.Format(
//            "[Client] [RequestInit] deviceId: {0}",
//            new object[] { req.deviceId }
//        ));

//        currentJSONRequest = JsonUtility.ToJson(req);
//    }

//    // ------------------ General methods for communicating with the server --------- //

//    void HandleResponse(string response)
//    {

//        ResponseShared rs = ResponseShared.CreateFromJson(response);

//        if ((rs.demand != demand) ||
//            ((rs.demand == Demand.choice && t != rs.t) ||
//            (rs.demand == Demand.trainingChoice && trainingT != rs.t))
//           )
//        {

//            Debug.LogWarning(String.Format("[Client] Not expected server response (content='{0}', expected response = ').", response));
//            return;
//        }

//        wait = rs.wait;
//        progress = rs.progress;

//        Debug.Log(String.Format(
//            "[Client] Received response for demand: '{0}' with wait: {1}, " +
//            "progress: {2}.", new object[] { rs.demand, wait, progress }
//            ));

//        if (demand == Demand.init)
//        {

//            ResponseInit ri = ResponseInit.CreateFromJson(response);

//            userId = ri.userId;
//            pseudo = ri.pseudo;

//            step = ri.step;

//            trainingT = ri.trainingT;
//            trainingTMax = ri.trainingTMax;
//            trainingGoodInHand = ri.trainingGoodInHand;
//            trainingGoodDesired = ri.trainingGoodDesired;
//            trainingChoiceMade = ri.trainingChoiceMade;
//            trainingScore = ri.trainingScore;

//            t = ri.t;
//            tMax = ri.tMax;
//            goodInHand = ri.goodInHand;
//            goodDesired = ri.goodDesired;
//            choiceMade = ri.choiceMade;
//            score = ri.score;

//            nGood = ri.nGood;

//            Debug.Log(String.Format(
//                "[Client] [ResponseInit] wait: {0}, progress: {1}, userId: {2}, pseudo: {3}, step: {4}, \n" +
//                "trainingT: {5}, trainingTMax: {6}, trainingGoodInHand: {7}, trainingGoodDesired: {8}, trainingChoiceMade: {9}, trainingScore: {10}, \n" +
//                "t: {11}, tMax: {12}, goodInHand: {13}, goodDesired: {14}, choiceMade: {15}, score: {16}, nGood: {17}.",
//                new object[] {wait, progress, userId, pseudo, step, trainingT, trainingTMax, trainingGoodInHand, trainingGoodDesired, trainingChoiceMade, trainingScore,
//                t, tMax, goodInHand, goodDesired, choiceMade, score, nGood}
//            ));

//        }
//        else if (demand == Demand.survey)
//        {

//            wait = rs.wait;
//            progress = rs.progress;

//            Debug.Log(String.Format(
//                "[ResponseSurvey] wait: {0}, progress: {1}", new object[] { wait, progress }
//            ));

//        }
//        else if (demand == Demand.choice && !wait)
//        {

//            ResponseChoice rc = ResponseChoice.CreateFromJson(response);

//            success = rc.success;
//            score = rc.score;
//            end = rc.end;

//            t += 1;

//            Debug.Log(String.Format(
//                "[Client] [ResponseChoice] wait: {0}, progress: {1}, success: {2}, t: {3}, score: {4}, end: {5}.",
//                new object[] { wait, progress, success, t, score, end }
//            ));

//        }
//        else if (demand == Demand.trainingChoice && !wait)
//        {

//            ResponseTrainingChoice rtc = ResponseTrainingChoice.CreateFromJson(response);

//            trainingSuccess = rtc.trainingSuccess;
//            trainingScore = rtc.trainingScore;
//            trainingEnd = rtc.trainingEnd;

//            trainingT += 1;

//            Debug.Log(String.Format(
//                "[Client] [ResponseTrainingChoice] wait: {0}, progress: {1}, trainingSuccess: {2}, trainingT: {3}, trainingScore: {4}, trainingEnd: {5}.",
//                new object[] { wait, progress, trainingSuccess, trainingT, trainingScore, trainingEnd }
//            ));

//        }

//        if (!wait)
//        {
//            demand = null;
//            currentJSONRequest = "";
//        }

//        responseTreated = true;
//    }
