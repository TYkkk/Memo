using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BaseFramework
{
    public class BaseMonoBehaviour : MonoBehaviour
    {
        public virtual bool RegisterUpdate { get; set; }
        public virtual void DoUpdate() { }

        public virtual bool RegisterFixUpdate { get; set; }
        public virtual void DoFixUpdate() { }

        public virtual bool RegisterLateUpdate { get; set; }
        public virtual void DoLateUpdate() { }

        public virtual bool RegisterOnGUI { get; set; }
        public virtual void DoOnGUI() { }

        public virtual void OnEnable()
        {
            if (RegisterUpdate)
            {
                MonoBehaviourManager.Instance.Register(DoUpdate, UpdateMethodType.Update);
            }

            if (RegisterFixUpdate)
            {
                MonoBehaviourManager.Instance.Register(DoFixUpdate, UpdateMethodType.FixUpdate);
            }

            if (RegisterLateUpdate)
            {
                MonoBehaviourManager.Instance.Register(DoLateUpdate, UpdateMethodType.LateUpdate);
            }

            if (RegisterOnGUI)
            {
                MonoBehaviourManager.Instance.Register(DoOnGUI, UpdateMethodType.OnGUI);
            }
        }

        public virtual void OnDisable()
        {
            if (RegisterUpdate)
            {
                MonoBehaviourManager.Instance.UnRegister(DoUpdate, UpdateMethodType.Update);
            }

            if (RegisterFixUpdate)
            {
                MonoBehaviourManager.Instance.UnRegister(DoFixUpdate, UpdateMethodType.FixUpdate);
            }

            if (RegisterLateUpdate)
            {
                MonoBehaviourManager.Instance.UnRegister(DoLateUpdate, UpdateMethodType.LateUpdate);
            }

            if (RegisterOnGUI)
            {
                MonoBehaviourManager.Instance.UnRegister(DoOnGUI, UpdateMethodType.OnGUI);
            }
        }
    }
}
