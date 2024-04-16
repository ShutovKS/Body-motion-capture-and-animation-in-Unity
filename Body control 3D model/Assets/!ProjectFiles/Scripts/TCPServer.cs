using System;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class TCPServer : MonoBehaviour
{
    public int[] IntArray { get; private set; }

    private TcpClient client;
    private NetworkStream stream;

    private void Update()
    {
        try
        {
            if (client is not { Connected: true })
            {
                ConnectToServer();
            }

            if (stream is { DataAvailable: true })
            {
                var data = new byte[1024];
                var bytesRead = stream.Read(data, 0, data.Length);
                var message = Encoding.ASCII.GetString(data, 0, bytesRead);

                if (!string.IsNullOrEmpty(message))
                {
                    IntArray = ParseIntArray(message);
                }
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"Ошибка при обработке TCP соединения: {ex.Message}");
            ConnectToServer();
        }
    }

    private void ConnectToServer()
    {
        try
        {
            client?.Close();

            client = new TcpClient("localhost", 50237);
            stream = client.GetStream();
        }
        catch (Exception ex)
        {
            Debug.LogError($"Ошибка при подключении к серверу: {ex.Message}");
        }
    }

    private void OnDestroy()
    {
        stream?.Close();
        client?.Close();
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
}