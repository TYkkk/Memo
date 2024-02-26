using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BaseFramework
{
    public delegate void EventHandler(IEventData data);

    public static class EventManager
    {
        private static Dictionary<string, EventHandler> handlerDict = new Dictionary<string, EventHandler>();

        public static void Register(string eventName, EventHandler eventHandler)
        {
            if (eventHandler == null)
            {
                return;
            }

            if (!handlerDict.ContainsKey(eventName))
            {
                handlerDict.Add(eventName, null);
            }

            handlerDict[eventName] += eventHandler;
        }

        public static void UnRegister(string eventName)
        {
            if (handlerDict.ContainsKey(eventName))
            {
                handlerDict.Remove(eventName);
            }
        }

        public static void UnRegister(string eventName, EventHandler eventHandler)
        {
            if (handlerDict.ContainsKey(eventName))
            {
                handlerDict[eventName] -= eventHandler;
            }
        }

        public static void Clear()
        {
            handlerDict.Clear();
        }

        public static void Fire(string eventName, IEventData data)
        {
            if (handlerDict.ContainsKey(eventName))
            {
                handlerDict[eventName]?.Invoke(data);
            }
        }

        public static void Fire(string eventName)
        {
            Fire(eventName, null);
        }
    }

    public interface IEventData
    {
        string Name { get; }
        object Sender { get; }
    }

    public class BaseEventData : IEventData
    {
        public string Name
        {
            get
            {
                return GetType().FullName;
            }
        }

        public object Sender
        {
            get; private set;
        }

        public BaseEventData()
        {

        }

        public BaseEventData(object sender)
        {
            Sender = sender;
        }

    }

    public class EventData<T> : BaseEventData
    {
        private readonly T _data;
        public T Data { get { return _data; } }

        public EventData(T data)
        {
            _data = data;
        }
    } 
}