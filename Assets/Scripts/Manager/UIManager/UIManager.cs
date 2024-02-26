using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BaseFramework
{
    public class UIManager : Singleton<UIManager>
    {
        public UIRoot UIRoot => uiRoot;

        private UIRoot uiRoot;

        private const string uiRootPath = "UI/UIRoot";

        private UIRoot uiRootTemplate;

        private readonly Dictionary<string, UIData> uiDataDict = new Dictionary<string, UIData>()
        {
            { UINames.BgPanel,new UIData(UINames.BgPanel,"UI/Background",UILayer.Bottom) },
            { UINames.SolutionSelectPanel,new UIData(UINames.SolutionSelectPanel,"UI/SolutionSelectPanel",UILayer.Layer02) },
            { UINames.CreateSolutionPanel,new UIData(UINames.CreateSolutionPanel,"UI/CreateSolutionPanel",UILayer.Pop)},
            { UINames.ElementDisplayPanel,new UIData(UINames.ElementDisplayPanel,"UI/ElementDisplayPanel",UILayer.Layer02)},
            { UINames.CreateElementPanel,new UIData(UINames.CreateElementPanel,"UI/CreateElementPanel",UILayer.Pop)},
            { UINames.ConfirmPanel,new UIData(UINames.ConfirmPanel,"UI/ConfirmPanel",UILayer.Pop)}
        };

        private static Dictionary<string, List<UIBase>> loadedUIDict;

        public override void Init()
        {
            base.Init();

            loadedUIDict = new Dictionary<string, List<UIBase>>();

            InitUIRoot();
        }

        public override void Release()
        {
            base.Release();

            loadedUIDict = null;

            ClearUIRoot();
        }

        private void InitUIRoot()
        {
            if (uiRootTemplate == null)
            {
                uiRootTemplate = Resources.Load<UIRoot>(uiRootPath);
            }

            if (uiRootTemplate == null)
            {
                Debug.LogError("uiRootTemplate Load Error");
                return;
            }

            ClearUIRoot();

            GameObject UIRootObj = Object.Instantiate(uiRootTemplate.gameObject);
            uiRoot = UIRootObj.GetComponent<UIRoot>();
        }

        private void ClearUIRoot()
        {
            if (uiRoot != null)
            {
                Object.Destroy(uiRoot.gameObject);
            }
        }

        public UIBase OpenUI(string uiName, Dictionary<string, System.Object> param = null)
        {
            UIBase uiBase = GetOrCreateUI(uiName);

            if (uiBase == null)
            {
                return null;
            }

            if (uiBase.IsOpened)
            {
                return uiBase;
            }

            if (param != null)
            {
                foreach (var child in param.Keys)
                {
                    if (!uiBase.Param.ContainsKey(child))
                    {
                        uiBase.Param.Add(child, param[child]);
                    }
                }
            }

            uiBase.Register();

            uiBase.Opening();

            uiBase.transform.SetParent(UIRoot.LayerRoot.GetChild((int)uiDataDict[uiName].Layer), false);

            uiBase.Open();

            return uiBase;
        }

        public UIBase OpenUIByGuid(string uiName, string guid)
        {
            if (loadedUIDict.ContainsKey(uiName))
            {
                foreach (var child in loadedUIDict[uiName])
                {
                    if (child.Guid == guid)
                    {
                        return child;
                    }
                }
            }

            return null;
        }

        private UIBase GetOrCreateUI(string uiName)
        {
            if (loadedUIDict.ContainsKey(uiName))
            {
                if (loadedUIDict[uiName] != null && loadedUIDict[uiName].Count > 0)
                {
                    return loadedUIDict[uiName][0];
                }
            }

            GameObject createUI = Object.Instantiate(Resources.Load<GameObject>(uiDataDict[uiName].UIPath));

            UIBase uiBase = createUI.GetComponent<UIBase>();

            if (uiBase == null)
            {
                Debug.LogError($"Load UI {uiName} Need UIBase Component");
                Object.Destroy(createUI);
                return null;
            }

            uiBase.InitUI(uiName);

            if (!loadedUIDict.ContainsKey(uiName))
            {
                loadedUIDict.Add(uiName, new List<UIBase>());
            }

            loadedUIDict[uiName].Add(uiBase);

            return uiBase;
        }

        public void CloseUI(string uiName)
        {
            if (!loadedUIDict.ContainsKey(uiName))
            {
                return;
            }

            for (int i = 0; i < loadedUIDict[uiName].Count; i++)
            {
                loadedUIDict[uiName][i].UnRegister();

                loadedUIDict[uiName][i].Close();

                Object.Destroy(loadedUIDict[uiName][i].gameObject);
            }

            loadedUIDict.Remove(uiName);
        }

        public void CloseUI(UIBase uiBase)
        {
            uiBase.UnRegister();

            uiBase.Close();

            loadedUIDict[uiBase.UIName].Remove(uiBase);

            Object.Destroy(uiBase.gameObject);
        }
    }


    public class UIData
    {
        public string UIName;
        public string UIPath;
        public UILayer Layer;
        public bool Multi;

        public UIData(string uiName, string uiPath, UILayer layer, bool multi = false)
        {
            UIName = uiName;
            UIPath = uiPath;
            Layer = layer;
            Multi = multi;
        }
    }

    public enum UILayer
    {
        Bottom,
        Layer01,
        Layer02,
        Layer03,
        Pop,
        Top,
        System,
    }
}