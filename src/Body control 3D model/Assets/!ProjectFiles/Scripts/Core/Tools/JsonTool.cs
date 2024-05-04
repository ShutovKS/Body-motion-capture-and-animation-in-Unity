#region

using System.IO;
using UnityEngine;

#endregion

namespace Tools
{
    public static class JsonTool
    {
        public static T LoadFromJson<T>(string fileName) where T : class
        {
            var filePath = Path.Combine(Application.streamingAssetsPath, fileName);

            if (File.Exists(filePath))
            {
                var json = File.ReadAllText(filePath);
                return JsonUtility.FromJson<T>(json);
            }

            Debug.LogError("Файл не найден: " + filePath);
            return null;
        }
    }
}