using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BaseFramework;
using UnityEngine.UI;
using System;

public class ConfirmPanel : UIBase
{
    public Text ContentText;
    public Button OkBtn;
    public Button CloseBtn;

    public event Action OkBtnEvent;

    public override void Awake()
    {
        base.Awake();

        OkBtn.onClick.AddListener(OkBtnClicked);
        CloseBtn.onClick.AddListener(CloseBtnClicked);
    }

    public override void OnDestroy()
    {
        OkBtn.onClick.RemoveListener(OkBtnClicked);
        CloseBtn.onClick.RemoveListener(CloseBtnClicked);

        base.OnDestroy();
    }

    public override void Open()
    {
        base.Open();

        if (Param.ContainsKey("Event"))
        {
            OkBtnEvent += Param["Event"] as Action;
        }

        if (Param.ContainsKey("Content"))
        {
            ContentText.text = Param["Content"].ToString();
        }
    }

    private void OkBtnClicked()
    {
        OkBtnEvent?.Invoke();
        UIManager.Instance.CloseUI(this);
    }

    private void CloseBtnClicked()
    {
        UIManager.Instance.CloseUI(this);
    }
}
