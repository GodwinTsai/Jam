using System;
using System.Collections.Generic;

public class BufferPool<T>
{
    /// <summary>
    /// buffer数组及其长度
    /// </summary>
    private class Buffer
    {
        public T[] buff;
        public int length;

        public void Reset()
        {
            for (int i = 0; i < buff.Length; i++)
                buff[i] = default;
        }
    }

    /// <summary>
    /// buffer池
    /// </summary>
    private List<Buffer> _bufferPool = new List<Buffer>();

    /// <summary>
    /// 从buffer池中获取buffer，如没有满足条件的buffer，则创建一个新的buffer
    /// </summary>
    /// <param name="length">buffer长度</param>
    /// <returns></returns>
    private Buffer GetBufferFromPool(int length)
    {
        Buffer buf = null;
        for (int i = _bufferPool.Count - 1; i >= 0; --i)
        {
            if (_bufferPool[i].buff.Length >= length)
            {
                buf = _bufferPool[i];
                _bufferPool.FastRemoveAt(i);
                break;
            }
        }
        if (buf == null)
        {
            buf = new Buffer {buff = new T[length], length = length};
        }
        else
        {
            buf.length = length;
        }
        return buf;
    }

    /// <summary>
    /// token池的最大数量
    /// </summary>
    private const int OWNER_POOL_MAX_COUNT = 512;

    /// <summary>
    /// buffer的token池
    /// </summary>
    private List<object> _ownerPool = new List<object>();

    /// <summary>
    /// object类型
    /// </summary>
    private Type _objectType = typeof(object);

    /// <summary>
    /// 获取一个buffer的token
    /// </summary>
    /// <returns></returns>
    private object GetOwnerFromPool()
    {
        if (_ownerPool.Count > 0)
        {
            object owner = _ownerPool[_ownerPool.Count - 1];
            _ownerPool.RemoveAt(_ownerPool.Count - 1);
            return owner;
        }

        return new object();
    }

    /// <summary>
    /// 清除池子中不再使用的buffer
    /// </summary>
    public void ReleasePool()
    {
        _bufferPool.Clear();
        _ownerPool.Clear();
    }

    /// <summary>
    /// 使用中的buffer
    /// </summary>
    private Dictionary<object, Buffer> _buffers = new Dictionary<object, Buffer>();

    /// <summary>
    /// 分配buffer
    /// </summary>
    /// <param name="length">长度</param>
    /// <param name="owner">token</param>
    public object AllocateBuffer(int length, object owner = null)
    {
        if (owner == null) owner = GetOwnerFromPool();
        if (_buffers.ContainsKey(owner))
            return owner;

        Buffer buf = GetBufferFromPool(length);
        _buffers.Add(owner, buf);
        return owner;
    }

    /// <summary>
    /// 释放buffer
    /// </summary>
    /// <param name="owner">token</param>
    public void ReleaseBuffer(object owner)
    {
        if (!_buffers.TryGetValue(owner, out Buffer buffer))
            return;

        buffer.Reset();
        _buffers.Remove(owner);
        _bufferPool.Add(buffer);
        if (owner.GetType() == _objectType && _ownerPool.Count < OWNER_POOL_MAX_COUNT)
            _ownerPool.Add(owner);
    }

    /// <summary>
    /// 设置值到buffer中
    /// </summary>
    /// <param name="index">索引</param>
    /// <param name="val">值</param>
    /// <param name="owner">token</param>
    public void Set(int index, T val, object owner)
    {
        _buffers[owner].buff[index] = val;
    }

    /// <summary>
    /// 尝试设置值到buffer中
    /// </summary>
    /// <param name="index">索引</param>
    /// <param name="val">值</param>
    /// <param name="owner">token</param>
    /// <returns></returns>
    public bool TrySet(int index, T val, object owner)
    {
        if (!_buffers.TryGetValue(owner, out Buffer buffer))
            return false;
        if (index >= buffer.buff.Length)
            return false;
        buffer.buff[index] = val;
        return true;
    }

    /// <summary>
    /// 从buffer中获取值
    /// </summary>
    /// <param name="index">索引</param>
    /// <param name="owner">token</param>
    /// <returns></returns>
    public T Get(int index, object owner)
    {
        return _buffers[owner].buff[index];
    }

    public void TryRelease(int index, object owner)
    {
        if (owner == null)
        {
            return;
        }

        if (!_buffers.TryGetValue(owner, out var outBuff))
        {
            return;
        }

        if (outBuff.buff.Length > index)
        {
            outBuff.buff[index] = default(T);
        }
    }

    public void TryRelease(object owner, T action)
    {
        if (owner == null)
        {
            return;
        }

        if (!_buffers.TryGetValue(owner, out var outBuff))
        {
            return;
        }
        
        var buff = outBuff.buff;
        for (var i = 0; i < buff.Length; i++)
        {
            if (Equals(buff[i], action))
            {
                buff[i] = default(T);
                return;
            }
        }
    }

    /// <summary>
    /// 尝试从buffer中获取值
    /// </summary>
    /// <param name="index">索引</param>
    /// <param name="owner">token</param>
    /// <param name="value">输出值</param>
    /// <returns></returns>
    public bool TryGet(int index, object owner, out T value)
    {
        if (!_buffers.TryGetValue(owner, out Buffer buffer))
        {
            value = default;
            return false;
        }
        if (index >= buffer.buff.Length)
        {
            value = default;
            return false;
        }
        value = buffer.buff[index];
        return true;
    }

    /// <summary>
    /// 获取buffer的长度
    /// </summary>
    /// <param name="owner">token</param>
    /// <returns></returns>
    public int Length(object owner)
    {
        return _buffers[owner].length;
    }

    /// <summary>
    /// 尝试获取buffer长度
    /// </summary>
    /// <param name="owner">token</param>
    /// <param name="length">长度</param>
    /// <returns></returns>
    public bool TryGetLength(object owner, out int length)
    {
        bool success = _buffers.TryGetValue(owner, out Buffer buffer);
        length = success ? buffer.length : -1;
        return success;
    }

    /// <summary>
    /// 获取buffer
    /// </summary>
    /// <param name="owner">token</param>
    /// <returns></returns>
    public T[] GetBuffer(object owner)
    {
        return _buffers[owner].buff;
    }

    /// <summary>
    /// 尝试获取buffer
    /// </summary>
    /// <param name="owner">token</param>
    /// <param name="buff">缓冲</param>
    /// <returns></returns>
    public bool TryGetBuffer(object owner, out T[] buff)
    {
        bool success = _buffers.TryGetValue(owner, out Buffer buffer);
        buff = success ? buffer.buff : null;
        return success;
    }
}