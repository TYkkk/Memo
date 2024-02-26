using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BaseFramework;
using System.IO;
using UnityEngine.UI;

public class SelectSolutionPanel : UIBase
{
    public GameObject Template;
    public Transform TemplateRoot;

    public Button AddBtn;
    public Button DeleteBtn;

    public Sprite[] DeleteBtnIcons;

    private List<SolutionTemplate> loadedTemplates;

    public override void Awake()
    {
        base.Awake();

        loadedTemplates = new List<SolutionTemplate>();

        AddBtn.onClick.AddListener(AddBtnClicked);
        DeleteBtn.onClick.AddListener(DeleteBtnClicked);
    }

    public override void Register()
    {
        base.Register();

        EventManager.Register(SolutionManager.SolutionLoadedEvent, SolutionLoadedEventHandler);
    }

    public override void UnRegister()
    {
        EventManager.UnRegister(SolutionManager.SolutionLoadedEvent, SolutionLoadedEventHandler);

        base.UnRegister();
    }

    public override void Opening()
    {
        base.Opening();
        SolutionManager.Instance.LoadSolutionData();
    }

    public override void OnDestroy()
    {
        loadedTemplates = null;

        AddBtn.onClick.RemoveListener(AddBtnClicked);
        DeleteBtn.onClick.RemoveListener(DeleteBtnClicked);

        base.OnDestroy();
    }

    private void SolutionLoadedEventHandler(IEventData data)
    {
        ClearLoadedTemplates();

        List<SolutionData> cacheData = new List<SolutionData>();

        for (int i = 0; i < SolutionManager.Instance.LoadedSolutionData.Count; i++)
        {
            if (SolutionManager.Instance.LoadedSolutionData[i].State != 2)
            {
                CreateTemplate(SolutionManager.Instance.LoadedSolutionData[i]);
            }
            else
            {
                cacheData.Add(SolutionManager.Instance.LoadedSolutionData[i]);
            }
        }

        for (int i = 0; i < cacheData.Count; i++)
        {
            CreateTemplate(cacheData[i]);
        }
    }

    private void CreateTemplate(SolutionData solutionData)
    {
        if (solutionData == null)
        {
            return;
        }

        GameObject createObj = Instantiate(Template);
        var template = createObj.GetComponent<SolutionTemplate>();
        template.SetData(solutionData);
        createObj.transform.SetParent(TemplateRoot, false);
        loadedTemplates.Add(template);
    }

    private void ClearLoadedTemplates()
    {
        if (loadedTemplates == null || loadedTemplates.Count == 0)
        {
            return;
        }

        for (int i = 0; i < loadedTemplates.Count; i++)
        {
            Destroy(loadedTemplates[i].gameObject);
        }

        loadedTemplates.Clear();
    }

    private void AddBtnClicked()
    {
        UIManager.Instance.OpenUI(UINames.CreateSolutionPanel);
    }

    private void DeleteBtnClicked()
    {
        if (SolutionManager.Instance.Mode == SolutionManager.SolutionPlayMode.Enter)
        {
            SolutionManager.Instance.Mode = SolutionManager.SolutionPlayMode.Delete;
            DeleteBtn.GetComponent<Image>().sprite = DeleteBtnIcons[1];
        }
        else
        {
            SolutionManager.Instance.Mode = SolutionManager.SolutionPlayMode.Enter;
            DeleteBtn.GetComponent<Image>().sprite = DeleteBtnIcons[0];
        }
    }
}
