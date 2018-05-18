//using UnityEngine;

//using System;
//using MiniJSON;
//using System.Collections;
//using System.Net;
//using System.Net.Sockets;
//using System.Threading;
//using System.Collections.Generic;
//#if WINDOWS_UWP
//    using Windows.Foundation;
//    using Windows.Networking.Sockets;
//    using Windows.Security.Cryptography.Certificates;
//    using Windows.Storage.Streams;
//    using System.Threading.Tasks;
//#else

//#endif

//public class CubeCommands : MonoBehaviour
//{
//    Transform himax;
//    Vector3 himaxposition;
//    Quaternion himaxrotation;
//    string buffer;
//    string recv ="";
//    bool isRecv = false;
//    void Awake()
//    {
//        foreach (Transform child in transform.parent)
//        {
//            if (child.name != transform.name)
//            {
//                himax = child;
//            }
//        }
//        Debug.Log(himax.name);
//        Debug.Log(transform.name);
//        Debug.Log("???");
//    }
//    public static string[] SplitString(string s)
//    {
//        string[] arr = s.Split(new char[] { '(', ')' }, StringSplitOptions.None);
//        return arr;
//    }
//    public static Vector3 StringToVector3(string sVector)
//    {
//       /* // Remove the parentheses
//        if (sVector.StartsWith("(") && sVector.EndsWith(")"))
//        {
//            sVector = sVector.Substring(1, sVector.Length - 2);
//        }*/

//        // split the items
//        string[] sArray = sVector.Split(',');

//        // store as a Vector3
//        Vector3 result = new Vector3(
//            float.Parse(sArray[0]),
//            float.Parse(sArray[1]),
//            float.Parse(sArray[2])+1);

//        return result;
//    }

//    public static Quaternion StringToQuaternion(string sVector)
//    {
//        /*// Remove the parentheses
//        if (sVector.StartsWith("(") && sVector.EndsWith(")"))
//        {
//            sVector = sVector.Substring(1, sVector.Length - 2);
//        }
//        */
//        // split the items
//        string[] sArray = sVector.Split(',');

//        Quaternion result = new Quaternion(
//                float.Parse(sArray[0]),
//                float.Parse(sArray[1]),
//                float.Parse(sArray[2]),
//                float.Parse(sArray[3])
//            );

//        return result;
//    }

//    private Material materialColored;
//#if WINDOWS_UWP
//    private Windows.Networking.Sockets.MessageWebSocket messageWebSocket;
//    DataWriter messageWriter;
//    private Windows.Networking.Sockets.StreamWebSocket streamWebSocket;
//    Uri serverUri = new Uri("ws://192.168.0.45:9091/echo");
//#endif

//    bool startPoseAccess = false;
//    bool isOpenWs = false;
//    bool sendCompleted = false;
//    bool closeWebServer = true;

//#if UNITY_UWP

//#endif

//#if UNITY_EDITOR
//    private WebSocketSharp.WebSocket ws;
//    int downRatio = 3;
//    int iterUpdate = 0;
//    int isFirst = 0;
//    private float GetFloat(string stringValue, float defaultValue)
//    {
//        float result = defaultValue;
//        float.TryParse(stringValue, out result);
//        return result;
//    }
//    private void Update()
//    {

//        var arr = SplitString(recv);

//        if (isOpenWs && !startPoseAccess)
//        {

//            startPoseAccess = true;
//            closeWebServer = false;
//            ws = new WebSocketSharp.WebSocket("ws://192.168.0.45:9091/echo");
//            ws.OnMessage += (sender, e) =>
//            {
//                string tmp = e.Data;
//                isRecv = false;
//                recv = tmp;
//                isRecv = true;

//            };

//            ws.ConnectAsync();
//            while (!ws.IsAlive)
//            {
//                //sendCompleted = false;

//            }

//            ws.SendAsync("Link start!", e => sendCompleted = true);
//        }
//        else if (isOpenWs && startPoseAccess)
//        {
//            iterUpdate++;
//            if (iterUpdate % 1 == 0)
//            {
//                ws.SendAsync("Link start!", e => sendCompleted = true);
//                iterUpdate = 0;
//            }
//            if (closeWebServer)
//            {
//                isOpenWs = false;
//                isRecv = false;
//                startPoseAccess = false;
//                ws.CloseAsync();
//                ws = null;
//            }
//        }

//        //Debug.Log(recv);

//        //yield return new WaitUntil(() => isRecv == true);

//        /*
//        if (startPoseAccess && sendCompleted)
//        {
//            sendCompleted = false;
//            string sVector = Camera.main.transform.position.ToString();
//            string rVector = Camera.main.transform.rotation.ToString();


//            sendCompleted = false;
//            ws.SendAsync((sVector+rVector),
//                        e => sendCompleted = true
//                    );


//        }*/

//        if (isRecv) {

//            //himax.rotation = StringToQuaternion(arr[3]);
//            //himax.eulerAngles = new Vector3(himax.eulerAngles.x + 90, himax.eulerAngles.y, himax.eulerAngles.z);
//            //himax.position = StringToVector3(arr[1]);
//            if (iterUpdate % 1 == 0)
//            {
//                himax.rotation = Camera.main.transform.rotation;
//                himax.position = Camera.main.transform.position;
//                string[] sArray = recv.Split(',');
//                Quaternion q = new Quaternion(
//                 float.Parse(sArray[3]),
//                 float.Parse(sArray[4]),
//                 float.Parse(sArray[5]),
//                float.Parse(sArray[6]));
//                Vector3 eangle = new Vector3(-q.eulerAngles.x, q.eulerAngles.y, -q.eulerAngles.z);
//                //Debug.Log(recv);
//                himax.transform.Rotate(eangle);

//                    /*himax.transform.Translate((float)double.Parse(sArray[0]), (float)double.Parse(sArray[1]), (float)double.Parse(sArray[2]));
//                Quaternion q = new Quaternion(
//                    (float)double.Parse(sArray[3]),
//                    (float)double.Parse(sArray[4]),
//                    (float)double.Parse(sArray[5]),
//                    (float)double.Parse(sArray[6])
//                );*/


//                himax.transform.Translate(float.Parse(sArray[0]), -float.Parse(sArray[1]), float.Parse(sArray[2]));
//                himax.transform.Rotate(90, 0, 0);
//                //Debug.Log(himax.transform.position);
//                recv = "";
//                isRecv = false;
//            }
//        }

//    }

//#elif WINDOWS_UWP
//    bool isbusy = false;
//    string sends;
//    bool fsend = false;
//    bool senddone = true;
//    void Update()
//    {
//        if (senddone)
//        {
//            senddone = false;
//            fsend = true;
//            sends = Camera.main.transform.rotation.ToString();
//        }
//        if (!isbusy && isOpenWs)    {
//            Debug.Log(putDebug);
//        }
//    }
//#endif

//    string putDebug;
//    // Called by GazeGestureManager when the user performs a Select gesture
//    void OnSelect()
//    {
//        Debug.Log("Click cube and change color.");
//        //Debug.Log(putDebug);
//        materialColored = new Material(Shader.Find("Diffuse"));
//        materialColored.color = new Color(UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value);
//        this.GetComponent<Renderer>().material = materialColored;
//#if UNITY_EDITOR
//        if (materialColored != null)
//            UnityEditor.AssetDatabase.DeleteAsset(UnityEditor.AssetDatabase.GetAssetPath(materialColored));
//        if(!isOpenWs)
//        {
//            //isOpenWs = true;
//            //ws = new WebSocketSharp.WebSocket("ws://192.168.0.45:9091/echo");
//            //ws.OnMessage += (sender, e) =>
//            //{
//            //    string tmp = e.Data;
//            //    isRecv = false;
//            //    recv = tmp;
//            //    isRecv = true;
//            //};

//            //ws.ConnectAsync();
//            //while (!ws.IsAlive)
//            //{
//            //    //sendCompleted = false;

//            //}
//            //startPoseAccess = true;
//            //while (startPoseAccess)
//            //{
//            //    ws.SendAsync("Link start!", e => sendCompleted = true);
//            //}

//            isOpenWs = true;
//        }
//        else
//        {
//            closeWebServer = true;

//        }

//#elif WINDOWS_UWP
//        if(!isbusy && !isOpenWs)
//        {
//            Debug.Log("Try to connect");
//            this.streamWebSocket = new Windows.Networking.Sockets.StreamWebSocket();

//            this.streamWebSocket.Closed += WebSocket_Closed;

//            try
//            {
//                Task connectTask = this.streamWebSocket.ConnectAsync(serverUri).AsTask();

//                connectTask.ContinueWith(_ =>
//                {
//                    Task.Run(() => this.ReceiveMessageUsingStreamWebSocket());

//                    Task.Run(() => {
//                        while (true)
//                        {
//                            if (fsend)
//                            {
//                                fsend = false;
//                                var bsend = System.Text.Encoding.ASCII.GetBytes(sends);
//                                this.SendMessageUsingStreamWebSocket(bsend);
//                                senddone = true;
//                            }
//                        }
//                    });
//                });
//            }
//            catch (Exception ex)
//            {
//                Windows.Web.WebErrorStatus webErrorStatus = Windows.Networking.Sockets.WebSocketError.GetStatus(ex.GetBaseException().HResult);
//                // Add code here to handle exceptions.
//            } 
//        }
//        else
//        {
//            isOpenWs = false;


//            messageWebSocket = null;

//        }
//#endif
//    }

//#if WINDOWS_UWP
//    private async void ReceiveMessageUsingStreamWebSocket()
//    {
//        try
//        {
//            using (var dataReader = new DataReader(this.streamWebSocket.InputStream))
//            {
//                dataReader.InputStreamOptions = InputStreamOptions.Partial;
//                await dataReader.LoadAsync(256);
//                byte[] message = new byte[dataReader.UnconsumedBufferLength];
//                dataReader.ReadBytes(message);
//                putDebug = ("Data received from StreamWebSocket: " + message.Length + " bytes");
//            }
//            this.streamWebSocket.Dispose();
//        }
//        catch (Exception ex)
//        {
//            Windows.Web.WebErrorStatus webErrorStatus = Windows.Networking.Sockets.WebSocketError.GetStatus(ex.GetBaseException().HResult);
//            // Add code here to handle exceptions.
//        }
//    }

//    private async void SendMessageUsingStreamWebSocket(byte[] message)
//    {
//        try
//        {
//            using (var dataWriter = new DataWriter(this.streamWebSocket.OutputStream))
//            {
//                dataWriter.WriteBytes(message);
//                await dataWriter.StoreAsync();
//                dataWriter.DetachStream();
//            }
//            //putDebug = ("Sending data using StreamWebSocket: " + message.Length.ToString() + " bytes");
//        }
//        catch (Exception ex)
//        {
//            Windows.Web.WebErrorStatus webErrorStatus = Windows.Networking.Sockets.WebSocketError.GetStatus(ex.GetBaseException().HResult);
//            // Add code here to handle exceptions.
//        }
//    }

//    private void WebSocket_Closed(Windows.Networking.Sockets.IWebSocket sender, Windows.Networking.Sockets.WebSocketClosedEventArgs args)
//    {
//        //Debug.WriteLine("WebSocket_Closed; Code: " + args.Code + ", Reason: \"" + args.Reason + "\"");
//        // Add additional code here to handle the WebSocket being closed.
//    }

//#endif

//}

using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using CI.HttpClient;
using System.Threading;
using System.Net.Sockets;

public class CubeCommands : MonoBehaviour
{
    private Material materialColored;
    //TransformCenter _transCenter;
    ChanTransform _transCenter;
    int index = 0;
    private bool isConnect = false;
    private bool _iskeepRun;
    string mes;
    private TcpClient _client;
    private Queue _msgEventQueue = new Queue();
    private Mutex _msgEventQueueMutex = new Mutex();
    private Thread _clientThread;
     // byte[] data = new byte[240];
    private void Start()
    {
        //isConnect = true;
        //connectToServer();       
       // _transCenter = GameObject.Find("TransformCentroller").GetComponent<TransformCenter>();
        _transCenter = GameObject.Find("TransformCentroller").GetComponent<ChanTransform>();
  
        //for (int i = 0; i < data.Length; i++)
        //{
        //    data[i] = 0x0;
        //}
    }
 
    private void Update()
    {
    
        
        //_transCenter.UpdatePoses(this, data);
        //if (mes.Length != 0)
        //{

        //    Debug.Log(mes);
        //    mes = "";
        //}
        //Debug.Log(isConnect.ToString());
        /* Consume the event queue and raise the event */
        if (_msgEventQueue.Count != 0)
        {
            _msgEventQueueMutex.WaitOne();
            while (_msgEventQueue.Count != 0)
            {
                byte[] args = (byte[])_msgEventQueue.Dequeue();
                
                _transCenter.UpdatePoses(this, args);
                
                
            }
            _msgEventQueueMutex.ReleaseMutex();
        }

    }
    private void OnDestroy()
    {
        disconnectFromServer();
    }
    // Called by GazeGestureManager when the user performs a Select gesture
    void OnSelect()
    {
        Debug.Log("Call!");
        materialColored = new Material(Shader.Find("Diffuse"));
        materialColored.color = new Color(UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value);
        this.GetComponent<Renderer>().material = materialColored;

#if UNITY_EDITOR
        if (materialColored != null)
            UnityEditor.AssetDatabase.DeleteAsset(UnityEditor.AssetDatabase.GetAssetPath(materialColored));
#endif
        if (!isConnect)
        {

            isConnect = true;
            connectToServer();
        }
        else
        {
            disconnectFromServer();
            isConnect = false;
        }
    }
    public void connectToServer()
    { 
        _clientThread = new Thread(() => RunClient());
        _clientThread.Start();
    }
    void RunClient()
    {
        _client = new TcpClient("192.168.0.129",80);
        if (_client == null|| !_client.Connected) return;
        // Get the socket stream
        NetworkStream stream = _client.GetStream();
        byte[] data = new byte[252]; // Buffer
        byte[] data1 = System.Text.Encoding.ASCII.GetBytes("G");

        stream.Write(data1, 0, data1.Length);
        Debug.Log("[TCP Client] Connected");
        _iskeepRun = true;
        bool isFirst = false;
       // _client.
        //_client
        try
        {
            while (_iskeepRun)
            {
                
                if (_client.Available > 0)
                {
                    stream.Read(data, 0, data.Length);
                    mes += ("recv\n");
                    _msgEventQueueMutex.WaitOne();
                    _msgEventQueue.Enqueue(data);
                    _msgEventQueueMutex.ReleaseMutex();
                }
                else 
                {
                    if (_client.Client.Receive(data1, SocketFlags.Peek) == 0)
                        break;
                    Thread.Sleep(20);
                    continue;
                }
            }

            stream.Close();
            _client.Close();
        }
        catch (SocketException e)
        {
            Debug.Log(e.Message);
        }
    }

    public void disconnectFromServer()
    {
        if (_client == null || !_client.Connected)
            return;

        _iskeepRun = false;
        _clientThread.Join();
        Debug.Log("[TCP Client] Closed");
    }
}