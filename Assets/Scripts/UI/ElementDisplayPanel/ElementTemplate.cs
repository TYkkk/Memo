using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BaseFramework;
using System;

public class ElementTemplate : BaseMonoBehaviour
{
    public Image Icon;
    public Text ContentText;
    public Button FlagBtn;
    public Button ClickBtn;
    public Sprite[] IconSprites;

    private ElementData data;
    private ElementDataManager dataManager;

    private delegate void ModeFunction();

    private Dictionary<SolutionManager.SolutionPlayMode, ModeFunction> ClickBtnFunctions;


    private void Awake()
    {
        ClickBtnFunctions = new Dictionary<SolutionManager.SolutionPlayMode, ModeFunction>();
        ClickBtnFunctions.Add(SolutionManager.SolutionPlayMode.Enter, EnterElement);
        ClickBtnFunctions.Add(SolutionManager.SolutionPlayMode.Delete, DeleteElement);

        FlagBtn.onClick.AddListener(FlagBtnClicked);
        ClickBtn.onClick.AddListener(ClickBtnClicked);
    }

    public void SetData(ElementData data, ElementDataManager dataManager)
    {
        this.data = data;
        this.dataManager = dataManager;

        ContentText.text = data.Content;
        Icon.sprite = IconSprites[data.State];
    }

    private void OnDestroy()
    {
        FlagBtn.onClick.RemoveListener(FlagBtnClicked);
        ClickBtn.onClick.RemoveListener(ClickBtnClicked);
    }

    private void FlagBtnClicked()
    {
        if (data.State == 0)
        {
            dataManager.ChangeElementState(data, 1);
        }
        else if (data.State == 1)
        {
            dataManager.ChangeElementState(data, 0);
        }
    }

    private void ClickBtnClicked()
    {
        ClickBtnFunctions[dataManager.Mode]?.Invoke();
    }

    private void EnterElement()
    {

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
        dataManager.DeleteElement(data);
    }
}
