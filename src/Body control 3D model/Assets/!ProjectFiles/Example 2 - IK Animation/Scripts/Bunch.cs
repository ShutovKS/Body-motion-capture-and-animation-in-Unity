#region

using System;
using UnityEngine;

#endregion

namespace Example_2___IK_Animation.Scripts
{
    [Serializable]
    public class Bunch
    {
        [field: SerializeField] public int pointIndex;
        [field: SerializeField] public Transform boneTransform;
    }
}