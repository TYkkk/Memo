using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BaseFramework;
using System.IO;
using System;
using Newtonsoft.Json;

public class ElementManager : Singleton<ElementManager>
{
    public readonly string ElementFilePath = $"{Application.persistentDataPath}/Solution_";
    public const string UpdateElementEvent = "UpdateElementEvent";

    public ElementDataManager LoadElementData(SolutionData solutionData)
    {
        ElementDataManager result = new ElementDataManager();
        result.Solution = solutionData;

        if (File.Exists(GetElementFilePath(solutionData.ID)))
        {
            try
            {
                string data = File.ReadAllText(GetElementFilePath(solutionData.ID));
                result.Elements = JsonConvert.DeserializeObject<List<ElementData>>(data);
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
            }
        }

        if (result.Elements == null)
        {
            CreateEmptyElementDataManager(result);
        }

        return result;
    }

    public string GetElementFilePath(string id)
    {
        return $"{Application.persistentDataPath}/Solution_{id}.txt";
    }

    public void SaveElementData(ElementDataManager dataManager)
    {
        if (File.Exists(GetElementFilePath(dataManager.Solution.ID)))
        {
            File.Delete(GetElementFilePath(dataManager.Solution.ID));
        }

        File.WriteAllText(GetElementFilePath(dataManager.Solution.ID), JsonConvert.SerializeObject(dataManager.Elements));
    }

    private void CreateEmptyElementDataManager(ElementDataManager dataManager)
    {
        dataManager.Elements = new List<ElementData>();
        SaveElementData(dataManager);
    }
}
