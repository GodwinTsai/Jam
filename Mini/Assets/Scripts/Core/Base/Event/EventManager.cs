using System;
using System.Collections.Generic;
using System.Text;

/// <summary>
    /// Adds a generic event system. The event system allows objects to register, unregister, and execute events on a particular object.
    /// </summary>
    public static class EventManager
    {
        // Internal variables
        private static Dictionary<string, List<Delegate>> _globalEventTables = new Dictionary<string, List<Delegate>>();
        private static BufferPool<Delegate> _bufferPool = new BufferPool<Delegate>();
        
        /// <summary>
        /// 当前正在执行的Owner, 为什么提权成为成员变量, 是因为如果在事件执行的过程中,
        /// 尝试注销事件, 会因为事件已经存在于当前owner中, 导致注销不成功. 所以需要在注销的时候进行删除处理.
        /// 删除的时候, 需要这个owner对象.
        /// </summary>
        private static object[] executeOwners = {new object(), new object(), new object(), new object(), new object()};

        public static void Reset()
        {
            foreach (var pair in _globalEventTables)
            {
                pair.Value.Clear();
            }
            _globalEventTables.Clear();

            _bufferPool = new BufferPool<Delegate>();
        }

        /// <summary>
        /// Register a new global event with no parameters.
        /// </summary>
        /// <param name="eventName">The name of the event.</param>
        /// <param name="handler">The function to call when the event executes.</param>
        public static void RegisterEvent(string eventName, Action handler)
        {
            RegisterEvent(eventName, (Delegate) handler);
        }
        
        /// <summary>
        /// Register a new global event with one parameter.
        /// only once
        /// </summary>
        /// <param name="eventName">The name of the event.</param>
        /// <param name="handler">The function to call when the event executes.</param>
        public static void RegisterEventOnce(string eventName, Action handler)
        {
            void Callback()
            {
                handler.Invoke();
                UnregisterEvent(eventName, Callback);
            }

            RegisterEvent(eventName, (Delegate)(Action)Callback);
        }

        /// <summary>
        /// Register a new global event with one parameter.
        /// </summary>
        /// <param name="eventName">The name of the event.</param>
        /// <param name="handler">The function to call when the event executes.</param>
        public static void RegisterEvent<T>(string eventName, Action<T> handler)
        {
            RegisterEvent(eventName, (Delegate) handler);
        }

        /// <summary>
        /// Register a new global event with one parameter.
        /// only once
        /// </summary>
        /// <param name="eventName">The name of the event.</param>
        /// <param name="handler">The function to call when the event executes.</param>
        public static void RegisterEventOnce<T>(string eventName, Action<T> handler)
        {
            void Callback(T arg)
            {
                handler.Invoke(arg);
                UnregisterEvent<T>(eventName, Callback);
            }

            RegisterEvent(eventName, (Delegate)(Action<T>)Callback);
        }
        
        /// <summary>
        /// Register a new global event with two parameters.
        /// </summary>
        /// <param name="eventName">The name of the event.</param>
        /// <param name="handler">The function to call when the event executes.</param>
        public static void RegisterEvent<T, U>(string eventName, Action<T, U> handler)
        {
            RegisterEvent(eventName, (Delegate) handler);
        }

        /// <summary>
        /// Register a new global event with three parameters.
        /// </summary>
        /// <param name="eventName">The name of the event.</param>
        /// <param name="handler">The function to call when the event executes.</param>
        public static void RegisterEvent<T, U, V>(string eventName, Action<T, U, V> handler)
        {
            RegisterEvent(eventName, (Delegate) handler);
        }

        /// <summary>
        /// Register a new global event with three parameters.
        /// </summary>
        /// <param name="eventName">The name of the event.</param>
        /// <param name="handler">The function to call when the event executes.</param>
        public static void RegisterEvent<T, U, V, W>(string eventName, Action<T, U, V, W> handler)
        {
            RegisterEvent(eventName, (Delegate) handler);
        }

        /// <summary>
        /// Executes the global event with no parameters.
        /// </summary>
        /// <param name="eventName">The name of the event.</param>
        public static void ExecuteEvent(string eventName)
        {
            object owner = GetDelegate(eventName);
            if (owner == null) return;
            executeOwners[0] = owner;
            int count = _bufferPool.Length(owner);
            for (int i = 0; i < count; ++i)
            {
                try
                {
                    if (_bufferPool.Get(i, owner) is Action handler)
                    {
                        handler();
                        _bufferPool.TryRelease(i, owner);
                    }
                }
                catch (Exception e)
                {
                    MTDebug.LogErrorFormat("[EventManager] ExecuteEvent捕捉到异常: 事件名: {0}, 错误: {1}\r\n{2}", eventName, e.Message, e.StackTrace);
                }
            }
            executeOwners[0] = null;
            _bufferPool.ReleaseBuffer(owner);
        }

        /// <summary>
        /// Executes the global event with one parameter.
        /// </summary>
        /// <param name="eventName">The name of the event.</param>
        /// <param name="arg1">The first parameter.</param>
        public static void ExecuteEvent<T>(string eventName, T arg1)
        {
            object owner = GetDelegate(eventName);
            if (owner == null) return;
            executeOwners[1] = owner;
            int count = _bufferPool.Length(owner);
            for (int i = 0; i < count; ++i)
            {
                try
                {
                    if (_bufferPool.Get(i, owner) is Action<T> handler)
                    {
                        handler(arg1);
                        _bufferPool.TryRelease(i, owner);
                    }
                }
                catch (Exception e)
                {
                    MTDebug.LogErrorFormat("[EventManager] ExecuteEvent<T>捕捉到异常: 事件名: {0}, 错误: {1}\r\n{2}", eventName, e.Message, e.StackTrace);
                }
            }
            executeOwners[1] = null;
            _bufferPool.ReleaseBuffer(owner);
        }

        /// <summary>
        /// Executes the global event with two parameters.
        /// </summary>
        /// <param name="eventName">The name of the event.</param>
        /// <param name="arg1">The first parameter.</param>
        /// <param name="arg2">The second parameter.</param>
        public static void ExecuteEvent<T, U>(string eventName, T arg1, U arg2)
        {
            object owner = GetDelegate(eventName);
            if (owner == null) return;
            executeOwners[2] = owner;

            int count = _bufferPool.Length(owner);
            for (int i = 0; i < count; ++i)
            {
                try
                {
                    if (_bufferPool.Get(i, owner) is Action<T, U> handler)
                    {
                        handler(arg1, arg2);
                        _bufferPool.TryRelease(i, owner);
                    }
                }
                catch (Exception e)
                {
                    MTDebug.LogErrorFormat("[EventManager] ExecuteEvent<T, U>捕捉到异常: 事件名: {0}, 错误: {1}\r\n{2}", eventName, e.Message, e.StackTrace);
                }
            }
            executeOwners[2] = null;
            _bufferPool.ReleaseBuffer(owner);
        }

        /// <summary>
        /// Executes the global event with three parameters.
        /// </summary>
        /// <param name="eventName">The name of the event.</param>
        /// <param name="arg1">The first parameter.</param>
        /// <param name="arg2">The second parameter.</param>
        /// <param name="arg3">The third parameter.</param>
        public static void ExecuteEvent<T, U, V>(string eventName, T arg1, U arg2, V arg3)
        {
            object owner = GetDelegate(eventName);
            if (owner == null) return;
            executeOwners[3] = owner;

            int count = _bufferPool.Length(owner);
            for (int i = 0; i < count; ++i)
            {
                try
                {
                    if (_bufferPool.Get(i, owner) is Action<T, U, V> handler)
                    {
                        handler(arg1, arg2, arg3);
                        _bufferPool.TryRelease(i, owner);
                    }
                }
                catch (Exception e)
                {
                    MTDebug.LogErrorFormat("[EventManager] ExecuteEvent<T, U, V>捕捉到异常: 事件名: {0}, 错误: {1}\r\n{2}", eventName, e.Message, e.StackTrace);
                }
            }
            executeOwners[3] = null;
            _bufferPool.ReleaseBuffer(owner);
        }

        /// <summary>
        /// Executes the global event with three parameters.
        /// </summary>
        /// <param name="eventName">The name of the event.</param>
        /// <param name="arg1">The first parameter.</param>
        /// <param name="arg2">The second parameter.</param>
        /// <param name="arg3">The third parameter.</param>
        /// <param name="arg4">The fourth parameter.</param>
        public static void ExecuteEvent<T, U, V, W>(string eventName, T arg1, U arg2, V arg3, W arg4)
        {
            object owner = GetDelegate(eventName);
            if (owner == null) return;
            executeOwners[4] = owner;

            int count = _bufferPool.Length(owner);
            for (int i = 0; i < count; ++i)
            {
                try
                {
                    if (_bufferPool.Get(i, owner) is Action<T, U, V, W> handler)
                    {
                        handler(arg1, arg2, arg3, arg4);
                        _bufferPool.TryRelease(i, owner);
                    }
                }
                catch (Exception e)
                {
                    MTDebug.LogErrorFormat("[EventManager] ExecuteEvent<T, U, V, W>捕捉到异常: 事件名: {0}, 错误: {1}\r\n{2}", eventName, e.Message, e.StackTrace);
                }
            }
            executeOwners[4] = null;
            _bufferPool.ReleaseBuffer(owner);
        }

        /// <summary>
        /// Unregisters the specified global event.
        /// </summary>
        /// <param name="eventName">The name of the event.</param>
        /// <param name="handler">The event delegate to remove.</param>
        public static void UnregisterEvent(string eventName, Action handler)
        {
            UnregisterEvent(eventName, (Delegate) handler, executeOwners[0]);
        }

        /// <summary>
        /// Unregisters the specified global event with one parameter.
        /// </summary>
        /// <param name="eventName">The name of the event.</param>
        /// <param name="handler">The event delegate to remove.</param>
        public static void UnregisterEvent<T>(string eventName, Action<T> handler)
        {
            UnregisterEvent(eventName, (Delegate) handler, executeOwners[1]);
        }

        /// <summary>
        /// Unregisters the specified global event with two parameters.
        /// </summary>
        /// <param name="eventName">The name of the event.</param>
        /// <param name="handler">The event delegate to remove.</param>
        public static void UnregisterEvent<T, U>(string eventName, Action<T, U> handler)
        {
            UnregisterEvent(eventName, (Delegate) handler, executeOwners[2]);
        }

        /// <summary>
        /// Unregisters the specified global event with three parameters.
        /// </summary>
        /// <param name="eventName">The name of the event.</param>
        /// <param name="handler">The event delegate to remove.</param>
        public static void UnregisterEvent<T, U, V>(string eventName, Action<T, U, V> handler)
        {
            UnregisterEvent(eventName, (Delegate) handler, executeOwners[3]);
        }

        /// <summary>
        /// Unregisters the specified global event with three parameters.
        /// </summary>
        /// <param name="eventName">The name of the event.</param>
        /// <param name="handler">The event delegate to remove.</param>
        public static void UnregisterEvent<T, U, V, W>(string eventName, Action<T, U, V, W> handler)
        {
            UnregisterEvent(eventName, (Delegate) handler, executeOwners[4]);
        }

        /// <summary>
        /// Registers a new global event.
        /// </summary>
        /// <param name="eventName">The name of the event.</param>
        /// <param name="handler">The function to call when the event executes.</param>
        private static void RegisterEvent(string eventName, Delegate handler)
        {
            if (_globalEventTables.TryGetValue(eventName, out List<Delegate> prevHandlers))
            {
                if (!prevHandlers.Contains(handler))
                {
                    _globalEventTables[eventName].Add(handler);
                }
            }
            else
            {
                prevHandlers = new List<Delegate> {handler};
                _globalEventTables.Add(eventName, prevHandlers);
            }
        }

        /// <summary>
        /// Returns the delegate for a particular global event.
        /// </summary>
        /// <param name="eventName">The interested event name.</param>
        /// <returns>owner of buffer pool</returns>
        private static object GetDelegate(string eventName)
        {
            if (_globalEventTables.TryGetValue(eventName, out List<Delegate> handler))
            {
                object owner = _bufferPool.AllocateBuffer(handler.Count);
                int i = 0;
                foreach (var hh in handler)
                {
                    _bufferPool.Set(i++, hh, owner);
                }
                return owner;
            }

            return null;
        }

        /// <summary>
        /// Unregisters the specified global event.
        /// </summary>
        /// <param name="eventName">The name of the event.</param>
        /// <param name="handler">The event delegate to remove.</param>
        private static void UnregisterEvent(string eventName, Delegate handler, object executeOwner)
        {
            if (_globalEventTables.TryGetValue(eventName, out List<Delegate> prevHandlers))
            {
                if (executeOwner != null)
                {
                    _bufferPool.TryRelease(executeOwner, handler);
                }
                prevHandlers.Remove(handler);
                if (prevHandlers.Count == 0)
                {
                    _globalEventTables.Remove(eventName);
                }
            }
        }
        
        public static void Print()
        {
            StringBuilder log = new StringBuilder();
            log.AppendLine("[EventManager] Registered events:");
            foreach (var eventNameToEvents in _globalEventTables)
            {
                log.AppendLine(eventNameToEvents.Key);
                foreach (var evn in eventNameToEvents.Value)
                {
                    if (evn != null)
                    {
                        log.AppendLine("\t- " + MTDebug.GetDelegateLog(evn));
                    }
                    else
                    {
                        MTDebug.LogError($"[EventManager] {eventNameToEvents.Key}");
                    }
                }
            }
            MTDebug.Log(log.ToString());
            System.IO.File.WriteAllText("/tmp/Events.txt", log.ToString());
        }
    }