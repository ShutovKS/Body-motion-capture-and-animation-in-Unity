#region

using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Data;
using Tools;
using UnityEngine;

#endregion

namespace Web
{
    [Serializable]
    public class WebClient : IDisposable
    {
        public WebClient()
        {
            _serverInfo = JsonTool.LoadFromJson<ServerInfo>("config.json");
            _client = new TcpClient();
        }

        public WebClient(int delayProcessing) : this()
        {
            this.delayProcessing = delayProcessing;
        }

        public int[] IntArray { get; private set; }
        public bool IsConnected => _client.Connected;
        public event Action ErrorConnect;

        [SerializeField, Range(1, 9999)] private int delayProcessing;

        private readonly TcpClient _client;
        private readonly ServerInfo _serverInfo;
        
        private NetworkStream _stream;
        private CancellationTokenSource _cancellationToken;

        public async Task Connected()
        {
            try
            {
                await _client.ConnectAsync(_serverInfo.host, _serverInfo.port);
                _stream = _client.GetStream();
            }
            catch (Exception ex)
            {
                Debug.LogError($"Ошибка при подключении к серверу: {ex.Message}");
            }
        }

        public void Disconnected()
        {
            _stream?.Close();
            _client?.Close();
        }

        public async void StartProcessing()
        {
            _cancellationToken = new CancellationTokenSource();
            await Ticks(_cancellationToken.Token);
        }

        public void StopProcessing()
        {
            _cancellationToken?.Cancel();
        }

        private async Task Ticks(CancellationToken cancellationTokenToken)
        {
            while (cancellationTokenToken.IsCancellationRequested == false)
            {
                try
                {
                    if (_client.Connected == false)
                    {
                        ErrorConnect?.Invoke();
                        continue;
                    }

                    if (_stream.DataAvailable == false)
                    {
                        continue;
                    }

                    var data = new byte[1024];
                    var bytesRead = await _stream.ReadAsync(data, 0, data.Length, cancellationTokenToken);
                    var message = Encoding.ASCII.GetString(data, 0, bytesRead);
                    IntArray = ParseIntArray(message);
                }
                catch (Exception ex)
                {
                    Debug.LogError($"Ошибка при обработке TCP соединения: {ex.Message}");
                }

                await Task.Delay(delayProcessing, cancellationTokenToken);
            }
        }

        private static int[] ParseIntArray(string input)
        {
            var regex = new System.Text.RegularExpressions.Regex(@"-?\d+");
            var matches = regex.Matches(input);

            if (matches.Count % 3 != 0)
            {
                throw new ArgumentException("Входная строка содержит некорректное количество чисел.");
            }

            var vectors = new int[matches.Count];
            for (var i = 0; i < matches.Count; i += 1)
            {
                vectors[i] = int.Parse(matches[i].Value);
            }

            return vectors;
        }

        public void Dispose()
        {
            StopProcessing();
            Disconnected();

            _client?.Dispose();
            _stream?.Dispose();
        }
    }
}