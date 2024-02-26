using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BaseFramework;

public class ElementDisplayPanel : UIBase
{
    public Button AddBtn;
    public Button DeleteBtn;
    public Button ReturnBtn;

    public GameObject Template;
    public Transform TemplateRoot;

    public Sprite[] DeleteBtnIcons;

    private List<ElementTemplate> loadedElementTemplates;
    private ElementDataManager dataManager;

    public override void Awake()
    {
        base.Awake();

        loadedElementTemplates = new List<ElementTemplate>();

        AddBtn.onClick.AddListener(AddBtnClicked);
        DeleteBtn.onClick.AddListener(DeleteBtnClicked);
        ReturnBtn.onClick.AddListener(ReturnBtnClicked);
    }

    public override void Register()
    {
        base.Register();

        EventManager.Register(ElementManager.UpdateElementEvent, UpdateElementEventHandler);
    }

    public override void UnRegister()
    {
        EventManager.UnRegister(ElementManager.UpdateElementEvent, UpdateElementEventHandler);

        base.UnRegister();
    }

    public override void OnDestroy()
    {
        dataManager = null;
        loadedElementTemplates = null;

        AddBtn.onClick.RemoveListener(AddBtnClicked);
        DeleteBtn.onClick.RemoveListener(DeleteBtnClicked);
        ReturnBtn.onClick.RemoveListener(ReturnBtnClicked);

        base.OnDestroy();
    }

    public override void Opening()
    {
        base.Opening();

        if (Param.ContainsKey("Solution"))
        {
            var data = Param["Solution"] as SolutionData;
            dataManager = ElementManager.Instance.LoadElementData(data);
            UpdateElementTemplates();
        }
    }

    public override void Open()
    {
        base.Open();
    }

    public void UpdateElementTemplates()
    {
        if (dataManager == null || dataManager.Elements == null)
        {
            return;
        }

        ClearLoadedTemplates();

        List<ElementData> cacheData = new List<ElementData>();

        for (int i = 0; i < dataManager.Elements.Count; i++)
        {
            if (dataManager.Elements[i].State != 1)
            {
                CreateElementTemplate(dataManager.Elements[i]);
            }
            else
            {
                cacheData.Add(dataManager.Elements[i]);
            }
        }

        for (int i = 0; i < cacheData.Count; i++)
        {
            CreateElementTemplate(cacheData[i]);
        }
    }

    private void ClearLoadedTemplates()
    {
        if (loadedElementTemplates == null)
        {
            return;
        }

        for (int i = 0; i < loadedElementTemplates.Count; i++)
        {
            Destroy(loadedElementTemplates[i].gameObject);
        }

        loadedElementTemplates.Clear();
    }

    private void CreateElementTemplate(ElementData data)
    {
        if (data == null)
        {
            return;
        }

        GameObject createObj = Instantiate(Template);
        var template = createObj.GetComponent<ElementTemplate>();
        template.SetData(data, dataManager);
        createObj.transform.SetParent(TemplateRoot, false);
        loadedElementTemplates.Add(template);
    }

    private void UpdateElementEventHandler(IEventData data)
    {
        UpdateElementTemplates();
    }

    private void AddBtnClicked()
    {
        UIManager.Instance.OpenUI(UINames.CreateElementPanel, new Dictionary<string, object>
        {
            { "Data",dataManager}
        });
    }

    private void DeleteBtnClicked()
    {
        if (dataManager.Mode == SolutionManager.SolutionPlayMode.Enter)
        {
            dataManager.Mode = SolutionManager.SolutionPlayMode.Delete;
            DeleteBtn.GetComponent<Image>().sprite = DeleteBtnIcons[1];
        }
        else
        {
            dataManager.Mode = SolutionManager.SolutionPlayMode.Enter;
            DeleteBtn.GetComponent<Image>().sprite = DeleteBtnIcons[0];
        }
    }

    private void ReturnBtnClicked()
    {
        UIManager.Instance.CloseUI(this);

        UIManager.Instance.OpenUI(UINames.SolutionSelectPanel);
    }

}
