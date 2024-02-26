using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementData
{
    public string ID;
    public string Content;
    /// <summary>
    /// State 0=UnDone,1=Done
    /// </summary>
    public int State;

    [JsonConstructor]
    public ElementData(string id, string content, int state)
    {
        ID = id;
        Content = content;
        State = state;
    }

    public ElementData(string content, int state = 0)
    {
        ID = System.Guid.NewGuid().ToString("N");
        Content = content;
        State = state;
    }
}
