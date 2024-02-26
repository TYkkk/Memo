using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BaseFramework;

public class CreateSolutionPanel : UIBase
{
    public InputField ContentInput;
    public Button OkBtn;
    public Button CloseBtn;

    public override void Awake()
    {
        base.Awake();
        OkBtn.onClick.AddListener(OkBtnClicked);
        CloseBtn.onClick.AddListener(CloseBtnClicked);
    }

    public override void Register()
    {
        base.Register();
    }

    public override void UnRegister()
    {
        base.UnRegister();
    }

    public override void OnDestroy()
    {
        OkBtn.onClick.RemoveListener(OkBtnClicked);
        CloseBtn.onClick.RemoveListener(CloseBtnClicked);

        base.OnDestroy();
    }

    private void OkBtnClicked()
    {
        if (string.IsNullOrEmpty(ContentInput.text))
        {
            return;
        }

        SolutionManager.Instance.CreateSolution(ContentInput.text);
        UIManager.Instance.CloseUI(this);
    }

    private void CloseBtnClicked()
    {
        UIManager.Instance.CloseUI(this);
    }
}
