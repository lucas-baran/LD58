using System;
using UnityEngine;

namespace LD58.Fruits
{
    [Serializable]
    public sealed class VFXGraphicsBufferProperties
    {
        [SerializeField] private string _bufferName = "PositionBuffer";
        [SerializeField] private string _eventName = "Play";
        [SerializeField] private string _countName = "Count";

        public int BufferId => Shader.PropertyToID(_bufferName);
        public int EventId => Shader.PropertyToID(_eventName);
        public int CountId => Shader.PropertyToID(_countName);
    }
}
