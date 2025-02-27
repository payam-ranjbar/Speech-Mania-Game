using System;
using System.Collections.Concurrent;
using System.Threading;
using UnityEngine;
using WebSocketSharp;

public class WebsocketClient : MonoBehaviour
{
    
     private WebSocket ws;
    private Thread websocketThread;
    private bool isRunning = false;  // Ensures the thread only starts once
    private static ConcurrentQueue<string> receivedWordsQueue = new ConcurrentQueue<string>(); 

    public event Action<string> OnWordReceived;

    void OnGUI()
    {
        GUIStyle buttonStyle = new GUIStyle(GUI.skin.button);
        buttonStyle.fontSize = 24; 

        if (GUI.Button(new Rect(50, 50, 250, 80), "🔄 Connect WebSocket", buttonStyle))
        {
            if(isRunning) StopWebSocket();
            StartWebSocket();
        }

        if (GUI.Button(new Rect(50, 150, 250, 80), "❌ Disconnect", buttonStyle))
        {
            StopWebSocket();
        }
    }

    public void StartWebSocket()
    {
        if (isRunning) return;  // Prevent multiple connections
        isRunning = true;

        Debug.Log("🔄 Starting WebSocket connection...");
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
            Debug.LogError("🔥 WebSocket Connection Failed: " + ex.Message);
        }
    }

    private static void Enqueue(MessageEventArgs e)
    {
        Debug.Log($"Enqueued: {e.Data}");
        receivedWordsQueue.Enqueue(e.Data);
    }

    public void StopWebSocket()
    {
        if (!isRunning) return;  // Already stopped

        isRunning = false;
        Debug.Log("🛑 Stopping WebSocket...");
        ws?.Close();
        websocketThread?.Join();
    }

    void Update()
    {
        while (receivedWordsQueue.TryDequeue(out string word))
        {
            Debug.Log("📥 Received Word: " + word);
            OnWordReceived?.Invoke(word);
        }
    }

    void OnDestroy()
    {
        StopWebSocket();
    }
}