using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BaseFramework;

public class ElementDataManager
{
    public SolutionData Solution;
    public List<ElementData> Elements;

    public SolutionManager.SolutionPlayMode Mode = SolutionManager.SolutionPlayMode.Enter;

    public void CreateElement(string content, int state = 0)
    {
        ElementData elementData = new ElementData(content, state);
        Elements.Add(elementData);
        ElementManager.Instance.SaveElementData(this);

        EventManager.Fire(ElementManager.UpdateElementEvent, new EventData<ElementDataManager>(this));
    }

    public void ChangeElementState(ElementData elementData, int state)
    {
        if (!Elements.Contains(elementData))
        {
            return;
        }

        elementData.State = state;
        ElementManager.Instance.SaveElementData(this);

        EventManager.Fire(ElementManager.UpdateElementEvent, new EventData<ElementDataManager>(this));
    }

    public void DeleteElement(ElementData data)
    {
        if (!Elements.Contains(data))
        {
            return;
        }

        Elements.Remove(data);
        ElementManager.Instance.SaveElementData(this);
        EventManager.Fire(ElementManager.UpdateElementEvent, new EventData<ElementDataManager>(this));
    }
}
