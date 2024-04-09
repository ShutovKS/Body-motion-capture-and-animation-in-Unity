using UnityEngine;

public class AnimationCode : MonoBehaviour
{
    public GameObject[] Body;

    [SerializeField] private TCPServer server;

    private void Update()
    {
        if (server == null || server.IntArray.Length == 0)
        {
            return;
        }

        for (var i = 0; i <= 32; i++)
        {
            var x = (float)(server.IntArray[0 + (i * 3)]) / 100;
            var y = (float)(server.IntArray[1 + (i * 3)]) / 100;
            var z = (float)(server.IntArray[2 + (i * 3)]) / 300;
            Body[i].transform.localPosition = new Vector3(x, y, z);
        }
    }
}