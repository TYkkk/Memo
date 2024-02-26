using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BaseFramework;
using System.IO;
using System;
using Newtonsoft.Json;

public class SolutionManager : Singleton<SolutionManager>
{
    public const string SolutionLoadedEvent = "SolutionLoadedEvent";

    public List<SolutionData> LoadedSolutionData
    {
        get
        {
            return loadedSolutionData;
        }
    }

    private List<SolutionData> loadedSolutionData;

    public readonly string SolutionConfigPath = $"{Application.persistentDataPath}/Solution.txt";

    public override void Init()
    {
        base.Init();

        EventManager.Register(ElementManager.UpdateElementEvent, UpdateElementEventHandler);
    }

    public enum SolutionPlayMode
    {
        Enter,
        Delete,
    }

    private SolutionPlayMode playMode = SolutionPlayMode.Enter;
    public SolutionPlayMode Mode
    {
        get
        {
            return playMode;
        }

        set
        {
            playMode = value;
        }
    }

    public void LoadSolutionData()
    {
        if (File.Exists(SolutionConfigPath))
        {
            try
            {
                string data = File.ReadAllText(SolutionConfigPath);
                loadedSolutionData = JsonConvert.DeserializeObject<List<SolutionData>>(data);
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
            }
        }

        if (loadedSolutionData == null)
        {
            CreateEmptySolutionData();
        }

        EventManager.Fire(SolutionLoadedEvent);
    }

    public void CreateSolution(string content, int state = 0)
    {
        SolutionData solutionData = new SolutionData(content, state);
        loadedSolutionData.Add(solutionData);
        SaveSolutionData();

        EventManager.Fire(SolutionLoadedEvent);
    }

    public void DeleteSolution(SolutionData data)
    {
        if (!loadedSolutionData.Contains(data))
        {
            return;
        }

        loadedSolutionData.Remove(data);

        if (File.Exists(ElementManager.Instance.GetElementFilePath(data.ID)))
        {
            File.Delete(ElementManager.Instance.GetElementFilePath(data.ID));
        }

        SaveSolutionData();

        EventManager.Fire(SolutionLoadedEvent);
    }

    private void CreateEmptySolutionData()
    {
        loadedSolutionData = new List<SolutionData>();
        SaveSolutionData();
    }

    private void SaveSolutionData()
    {
        if (File.Exists(SolutionConfigPath))
        {
            File.Delete(SolutionConfigPath);
        }

        File.WriteAllText(SolutionConfigPath, JsonConvert.SerializeObject(loadedSolutionData));
    }

    private void UpdateElementEventHandler(IEventData data)
    {
        var p = data as EventData<ElementDataManager>;

        if (CheckSolutionState(p.Data))
        {
            SaveSolutionData();
        }
    }

    private bool CheckSolutionState(ElementDataManager dataManager)
    {
        if (dataManager.Elements == null || dataManager.Elements.Count == 0)
        {
            if (dataManager.Solution.State != 0)
            {
                dataManager.Solution.State = 0;
                return true;
            }
            else
            {
                return false;
            }
        }

        int result = 2;

        foreach (var child in dataManager.Elements)
        {
            if (child.State == 0)
            {
                result = 1;
                break;
            }
        }

        if (dataManager.Solution.State == result)
        {
            return false;
        }
        else
        {
            dataManager.Solution.State = result;
            return true;
        }
    }
}
