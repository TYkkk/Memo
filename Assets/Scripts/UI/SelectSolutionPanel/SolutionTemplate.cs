using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BaseFramework;
using UnityEngine.UI;
using System;

public class SolutionTemplate : BaseMonoBehaviour
{
    public Text ContentText;
    public Image FlagIcon;
    public Button SelectBtn;

    public Sprite[] StateSprites;

    private delegate void ModeFunction();

    private SolutionData solutionData;
    private Dictionary<SolutionManager.SolutionPlayMode, ModeFunction> SelectBtnFunctions;

    private void Awake()
    {
        SelectBtnFunctions = new Dictionary<SolutionManager.SolutionPlayMode, ModeFunction>();
        SelectBtnFunctions.Add(SolutionManager.SolutionPlayMode.Enter, EnterElement);
        SelectBtnFunctions.Add(SolutionManager.SolutionPlayMode.Delete, DeleteElement);

        SelectBtn.onClick.AddListener(SelectBtnClicked);
    }

    private void OnDestroy()
    {
        SelectBtn.onClick.RemoveListener(SelectBtnClicked);
    }

    public void SetData(SolutionData solutionData)
    {
        this.solutionData = solutionData;
        UpdateData();
    }

    public void UpdateData()
    {
        if (solutionData == null)
        {
            return;
        }

        ContentText.text = solutionData.Content;
        FlagIcon.sprite = StateSprites[solutionData.State];
    }

    private void SelectBtnClicked()
    {
        SelectBtnFunctions[SolutionManager.Instance.Mode]?.Invoke();
    }

    private void EnterElement()
    {
        UIManager.Instance.CloseUI(UINames.SolutionSelectPanel);

        UIManager.Instance.OpenUI(UINames.ElementDisplayPanel, new Dictionary<string, object>
        {
            { "Solution",solutionData}
        });
    }

    private void DeleteElement()
    {
        Action deleteAction = () =>
        {
            DeleteAction();
        };

        UIManager.Instance.OpenUI(UINames.ConfirmPanel, new Dictionary<string, object>
        {
            { "Content","确认删除该组内容吗？"},
            { "Event", deleteAction}
        });
    }

    private void DeleteAction()
    {
        SolutionManager.Instance.DeleteSolution(solutionData);
    }
}
