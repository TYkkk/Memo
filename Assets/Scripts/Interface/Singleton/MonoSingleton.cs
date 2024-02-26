using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BaseFramework
{
    public class MonoSingleton<T> : BaseMonoBehaviour, IManager where T : BaseMonoBehaviour
    {
        private static bool isCreated = false;

        private static T instance;

        public static T Instance
        {
            get
            {
                if (instance == null && !isCreated)
                {
                    T[] objs = FindObjectsOfType<T>();

                    if (objs.Length > 0)
                    {
                        Debug.LogError($"MonoSingleton {typeof(T).Name} Should Never Be More Than 1 In Scene");

                        return objs[0];
                    }

                    GameObject monoSingleton = new GameObject(typeof(T).Name);
                    instance = monoSingleton.AddComponent<T>();

                    isCreated = true;
                }

                return instance;
            }
        }

        public virtual void Awake()
        {
            if (instance == null)
            {
                instance = GetComponent<T>();

                DontDestroyOnLoad(gameObject);
            }
        }

        public virtual void Init()
        {
        }

        public virtual void Release()
        {
        }

        private void OnDestroy()
        {

        }
    }

}