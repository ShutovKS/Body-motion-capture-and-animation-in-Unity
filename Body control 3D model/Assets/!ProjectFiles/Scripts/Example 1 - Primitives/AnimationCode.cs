using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using Web;

namespace Example_1___Primitives
{
    public class AnimationCode : MonoBehaviour
    {
        [SerializeField] private int numberBodyDots;
        [SerializeField] private Material bodyDotsMaterial;
        [SerializeField] private Material linesMaterial;

        [Space, SerializeField] private List<LineCodeData> lineCodes;

        [Space, SerializeField] private WebClient server = new();

        private CancellationTokenSource _cancellationTokenReconnects;
        private GameObject[] _bodyDots;

        private void Awake()
        {
            _bodyDots = new GameObject[numberBodyDots];
            for (var i = 0; i < numberBodyDots; i++)
            {
                var bodyDot = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                bodyDot.GetComponent<MeshRenderer>().material = bodyDotsMaterial;
                bodyDot.transform.SetParent(gameObject.transform);
                bodyDot.transform.localScale = Vector3.one * 0.2f;
                _bodyDots[i] = bodyDot;
            }

            foreach (var lineCodeData in lineCodes)
            {
                var newGameObject = new GameObject();
                newGameObject.transform.SetParent(newGameObject.transform);
                var lineCode = newGameObject.AddComponent<LineCode>();
                
                var origin = _bodyDots[lineCodeData.BodyDotOriginNumber].transform;
                var destination = _bodyDots[lineCodeData.BodyDotDestinationNumber].transform;
                lineCode.SetUp(origin, destination, linesMaterial);
            }
        }

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
                _bodyDots[i].transform.localPosition = new Vector3(x, y, z);
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