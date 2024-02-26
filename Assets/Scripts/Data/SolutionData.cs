using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolutionData
{
    public string ID;
    public string Content;

    /// <summary>
    /// State 0=Empty,1=UnDone,2=Done
    /// </summary>
    public int State;

    [JsonConstructor]
    public SolutionData(string id, string content, int state)
    {
        ID = id;
        Content = content;
        State = state;
    }

    public SolutionData(string content, int state = 0)
    {
        ID = System.Guid.NewGuid().ToString("N");
        Content = content;
        State = state;
    }
}
