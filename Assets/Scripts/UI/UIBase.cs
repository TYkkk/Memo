using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BaseFramework
{
    public class UIBase : BaseMonoBehaviour
    {
        public string UIName => uiName;

        private string guid;

        public string Guid => guid;

        private string uiName;

        public Dictionary<string, System.Object> Param = new Dictionary<string, System.Object>();

        private bool isOpened;

        public bool IsOpened => isOpened;

        public virtual void Awake()
        {
        }

        public virtual void Register()
        {

        }

        public virtual void Opening()
        {
        }

        public virtual void Open()
        {
            isOpened = true;
        }

        public virtual void Start()
        {
        }

        public virtual void UnRegister()
        {

        }

        public virtual void Close()
        {
            isOpened = false;
        }

        public virtual void OnDestroy()
        {

        }

        public void InitUI(string uiName)
        {
            guid = System.Guid.NewGuid().ToString("N");
            this.uiName = uiName;
        }
    }

}