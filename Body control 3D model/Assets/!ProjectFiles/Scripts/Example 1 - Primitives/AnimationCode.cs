using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using Web;

namespace Example_1___Primitives
{
    public class AnimationCode : MonoBehaviour
    {
        [SerializeField] private GameObject[] body;
        [SerializeField] private WebClient server = new();
        private CancellationTokenSource _cancellationTokenReconnects;

        private void OnEnable()
        {
            Connect();
        }

        private void OnDisable()
        {
            Disconnected();
        }

        private void Update()
        {
            if (!server.IsConnected || server.IntArray.Length == 0)
            {
                return;
            }

            for (var i = 0; i <= 32; i++)
            {
                var x = (float)(server.IntArray[0 + (i * 3)]) / 100;
                var y = (float)(server.IntArray[1 + (i * 3)]) / 100;
                var z = (float)(server.IntArray[2 + (i * 3)]) / 300;
                body[i].transform.localPosition = new Vector3(x, y, z);
            }
        }

        private async void Connect()
        {
            _cancellationTokenReconnects = new CancellationTokenSource();

            while (_cancellationTokenReconnects.Token.IsCancellationRequested == false)
            {
                try
                {
                    await server.Connected();

                    if (server.IsConnected)
                    {
                        server.StartProcessing();
                        break;
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError("Ошибка при попытке подключения: " + e.Message);
                }

                await Task.Yield();
            }
        }

        private void Disconnected()
        {
            _cancellationTokenReconnects.Cancel();
            server.StopProcessing();
            server.Disconnected();
        }
    }
}