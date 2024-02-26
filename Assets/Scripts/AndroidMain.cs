using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BaseFramework;

public class AndroidMain : MonoBehaviour
{
    private void Awake()
    {
        SolutionManager.Instance.Init();
        UIManager.Instance.Init();
    }

    private void Start()
    {
        UIManager.Instance.OpenUI(UINames.BgPanel);
        UIManager.Instance.OpenUI(UINames.SolutionSelectPanel);
    }
}
