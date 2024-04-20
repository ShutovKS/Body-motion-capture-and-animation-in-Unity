using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Example_1___Primitives.Scripts;
using UnityEngine;
using Web;

namespace Example_2___IK_Animation.Scripts
{
    public class IKSetting : MonoBehaviour
    {
        [SerializeField] private float XMultiplier = 1000f;
        [SerializeField] private float YMultiplier = 1000f;
        [SerializeField] private float ZMultiplier = 3000f;

        [Space, SerializeField] private List<LineCodeData> lineCodes;
        [Space, SerializeField] private WebClient server = new();

        [SerializeField] private WebClient webClient;
        [SerializeField] private List<Bunch> bunches;
        [SerializeField] private Transform parentTransform;

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

            parentTransform.position = new Vector3(
                (server.IntArray[24 * 3 + 0] + server.IntArray[23 * 3 + 0]) * 0.5f / XMultiplier,
                (server.IntArray[28 * 3 + 1] + server.IntArray[27 * 3 + 1]) * 0.5f / YMultiplier,
                (server.IntArray[24 * 3 + 2]) / ZMultiplier);

            foreach (var bunch in bunches)
            {
                var x = server.IntArray[bunch.pointIndex * 3 + 0] / XMultiplier;
                var y = server.IntArray[bunch.pointIndex * 3 + 1] / YMultiplier;
                var z = server.IntArray[bunch.pointIndex * 3 + 2] / ZMultiplier;
                bunch.boneTransform.position = new Vector3(x, y, z);
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

    [Serializable]
    public class Bunch
    {
        [field: SerializeField] public int pointIndex;
        [field: SerializeField] public Transform boneTransform;
    }
}