using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using Mono.Cecil.Cil;
using UnityEngine;
using WebSocketSharp;

public class WebsocketClient : MonoBehaviour
{
    
    private WebSocket ws;
    private Thread websocketThread;
    private bool isRunning = false;  
    private static ConcurrentQueue<string> receivedWordsQueue = new ConcurrentQueue<string>(); 
    private List<string> logMessages = new List<string>(); 
   [SerializeField] private float messageDisplayTime = 20f; 
    private Dictionary<string, float> messageTimestamps = new Dictionary<string, float>(); 

    public event Action<string> OnWordReceived;

    public static WebsocketClient Instance { get; private set; }

    private void Awake()
    {
        StartWebSocket();
        Instance = this;
    }

    void OnGUI()
    {
        GUIStyle buttonStyle = new GUIStyle(GUI.skin.button);
        buttonStyle.fontSize = 24; 

        GUIStyle logStyle = new GUIStyle(GUI.skin.label);
        logStyle.fontSize = 18;
        logStyle.normal.textColor = Color.white;

        if (GUI.Button(new Rect(50, 50, 250, 80), "🔄 Connect WebSocket", buttonStyle))
        {
            if (isRunning) StopWebSocket();
            StartWebSocket();
        }

        if (GUI.Button(new Rect(50, 150, 250, 80), "❌ Disconnect", buttonStyle))
        {
            StopWebSocket();
        }

        GUI.Label(new Rect(50, 250, 500, 500), "📜 Connection Log:", logStyle);
        for (int i = 0; i < logMessages.Count; i++)
        {
            GUI.Label(new Rect(50, 280 + (i * 30), 500, 30), logMessages[i], logStyle);
        }
    }

    public void StartWebSocket()
    {
        if (isRunning) return;  
        isRunning = true;

        LogMessage("🔄 Starting WebSocket connection...");
        websocketThread = new Thread(RunWebSocket);
        websocketThread.IsBackground = true;
        websocketThread.Start();
    }

    private void RunWebSocket()
    {
        try
        {
            ws = new WebSocket("ws://localhost:8765");

            ws.OnOpen += (sender, e) => Debug.Log("✅ WebSocket Connected!");
            ws.OnClose += (sender, e) => Debug.LogError("❌ WebSocket Disconnected!");
            ws.OnError += (sender, e) => Debug.LogError("⚠️ WebSocket Error: " + e.Message);
            ws.OnMessage += (sender, e) => Enqueue(e); 

            ws.Connect();
            Debug.Log("🛜 Attempting WebSocket Connection...");

            while (isRunning)
            {
                Thread.Sleep(10);
            }

            ws.Close();
        }
        catch (Exception ex)
        {
            LogMessage("🔥 WebSocket Connection Failed: " + ex.Message);
        }
    }

    private static void Enqueue(MessageEventArgs e)
    {
        receivedWordsQueue.Enqueue(e.Data);
    }

    public void StopWebSocket()
    {
        if (!isRunning) return;  // Already stopped

        isRunning = false;
        LogMessage("🛑 Stopping WebSocket...");
        ws?.Close();
        websocketThread?.Join();
    }

    void Update()
    {
        while (receivedWordsQueue.TryDequeue(out string word))
        {
            LogMessage("📥 Received Word: " + word);
            OnWordReceived?.Invoke(word);
        }
        float currentTime = Time.time;
        logMessages.RemoveAll(msg => currentTime - messageTimestamps[msg] > messageDisplayTime);
    }

    void OnDestroy()
    {
        StopWebSocket();
    }
    void LogMessage(string message)
    {
        logMessages.Add(message);
        messageTimestamps[message] = Time.time;
        Debug.Log(message);
    }

}