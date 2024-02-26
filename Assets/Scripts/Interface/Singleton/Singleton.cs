using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BaseFramework
{
    public class Singleton<T> : IManager where T : new()
    {
        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new T();
                }

                return instance;
            }
        }

        private static T instance;

        public virtual void Init()
        {
        }

        public virtual void Release()
        {
        }
    } 
}
