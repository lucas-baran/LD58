using System;
using Unity.Collections;
using UnityEngine;
using UnityEngine.LowLevel;
using UnityEngine.PlayerLoop;
using UnityEngine.VFX;

namespace LD58.Fruits
{
    public sealed class VFXGraphicsBuffer<T> : IDisposable
        where T : unmanaged
    {
        private const int DEFAULT_MAX_CAPACITY = 64;

        private readonly VisualEffect _vfx;
        private readonly VFXEventAttribute _eventAttribute;
        private readonly int _stride;
        private readonly int _bufferId;
        private readonly int _eventId;
        private readonly int _countId;
        private readonly bool _autoResize;
        private readonly Type _subsystemType;

        private GraphicsBuffer _graphicsBuffer;
        private NativeList<T> _data;
        private bool _disposed = false;

        public int MaxCapacity { get; set; } = DEFAULT_MAX_CAPACITY;

        public VFXGraphicsBuffer(VisualEffect vfx, int capacity, int stride, VFXGraphicsBufferProperties properties, bool auto_resize, Type subsystem_type)
            : this(vfx, capacity, stride, properties.BufferId, properties.EventId, properties.CountId, auto_resize, subsystem_type)
        {
        }

        public VFXGraphicsBuffer(VisualEffect vfx, int capacity, int stride, string buffer_id, string event_id, string count_id, bool auto_resize, Type subsystem_type)
            : this(vfx, capacity, stride, Shader.PropertyToID(buffer_id), Shader.PropertyToID(event_id), Shader.PropertyToID(count_id), auto_resize, subsystem_type)
        {
        }

        public VFXGraphicsBuffer(VisualEffect vfx, int capacity, int stride, int buffer_id, int event_id, int count_id, bool auto_resize, Type subsystem_type)
        {
            _vfx = vfx;
            _eventAttribute = _vfx.CreateVFXEventAttribute();
            _stride = stride;
            _bufferId = buffer_id;
            _eventId = event_id;
            _countId = count_id;
            _autoResize = auto_resize;
            _subsystemType = subsystem_type;
            _data = new NativeList<T>(capacity, Allocator.Persistent);
            EnsureCapacity(capacity);

            PlayerLoopSystem system = new()
            {
                type = subsystem_type,
                updateDelegate = SendEvent,
                subSystemList = null,
            };

            PlayerLoopUtilities.InsertSubSystem<PreLateUpdate>(ref system, index: 0);
        }

        public void AddData(T data)
        {
            if (_data.Length < MaxCapacity)
            {
                _data.Add(data);
            }
        }

        private void SendEvent()
        {
            if (_data.Length == 0)
            {
                return;
            }

            int count;

            if (_autoResize)
            {
                count = Mathf.Min(_data.Length, MaxCapacity);
                EnsureCapacity(count);
                _graphicsBuffer.SetData(_data.AsArray());
            }
            else
            {
                count = Mathf.Min(Mathf.Min(_data.Length, MaxCapacity), _graphicsBuffer.count);
                _graphicsBuffer.SetData(_data.AsArray(), 0, 0, count);
            }

            _vfx.SetGraphicsBuffer(_bufferId, _graphicsBuffer);
            _eventAttribute.SetInt(_countId, count);
            _vfx.SendEvent(_eventId, _eventAttribute);
            _data.Clear();
        }

        public void EnsureCapacity(int capacity)
        {
            if (_graphicsBuffer == null || _graphicsBuffer.count < capacity)
            {
                _graphicsBuffer?.Dispose();
                _graphicsBuffer = new GraphicsBuffer(GraphicsBuffer.Target.Structured, capacity, _stride);
            }
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                _eventAttribute.Dispose();
                _graphicsBuffer.Dispose();
                _data.Dispose();
                PlayerLoopUtilities.RemoveSystem(_subsystemType);
                _disposed = true;
            }
        }
    }
}
