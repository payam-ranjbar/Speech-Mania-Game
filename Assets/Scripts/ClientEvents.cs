using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class ClientEvents : MonoBehaviour
    {
        [SerializeField] private WebsocketClient socketClient;

        private void Awake()
        {
            if (socketClient is null)
            {
                var clientFound = FindAnyObjectByType<WebsocketClient>();

                if (clientFound is not null)
                {
                    socketClient = clientFound;
                }
            }
        }


        private void OnEnable()
        {
            socketClient.OnWordReceived += OnMessage;
        }

        private void OnDisable()
        {
            socketClient.OnWordReceived -= OnMessage;
        }

        private void OnMessage(string msg)
        {
            Debug.Log("message received: " + msg);
        }
    }
}