using System;
using UnityEngine;

namespace Example_1___Primitives
{
    [Serializable]
    public class LineCodeData
    {
        [field: SerializeField] public int BodyDotOriginNumber { get; private set; }
        [field: SerializeField] public int BodyDotDestinationNumber { get; private set; }
    }
}