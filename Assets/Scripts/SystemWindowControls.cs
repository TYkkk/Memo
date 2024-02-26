using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.IO;
using System;
using System.Text;

public class SystemWindowControls : MonoBehaviour
{
    private const int windowWidth = 1024;
    private const int windowHeigth = 768;

    public Button MinBtn;
    public Button CloseBtn;

    public EmptyRaycast DragArea;
    public EmptyRaycast LeftArea;
    public EmptyRaycast RightArea;
    public EmptyRaycast TopArea;
    public EmptyRaycast BottomArea;

    private Vector2 beginOffset;
    private WindowsPosData cachePosData;
    private WindowsPosData resultPosData;

    private Texture2D LeftRightIcon;
    private Texture2D TopBottomIcon;

    private void Awake()
    {
#if !UNITY_EDITOR
        SetWindowsData();
#endif

        MinBtn.onClick.AddListener(MinBtnClicked);
        CloseBtn.onClick.AddListener(CloseBtnClicked);

        AddDragEvent();
        AddBorderDragEvent();
        AddBorderMouseIconChange();
    }

    void Update()
    {

    }

    private void OnDestroy()
    {
        MinBtn.onClick.RemoveListener(MinBtnClicked);
        CloseBtn.onClick.RemoveListener(CloseBtnClicked);
    }

    private void MinBtnClicked()
    {
        WindowsTools.SetMinWindows();
    }

    private void CloseBtnClicked()
    {
        SaveCurrentWindowsData();
        Application.Quit();
    }

    private void SetWindowsData()
    {
        if (!File.Exists("config.txt"))
        {
            SetWindowsDefaultData();
        }
        else
        {
            try
            {
                var data = JsonUtility.FromJson<WindowsPosData>(Encoding.Default.GetString(File.ReadAllBytes("config.txt")));
                SetWindowsData(data);
            }
            catch (Exception e)
            {
                SetWindowsDefaultData();
            }
        }
    }

    private void SetWindowsDefaultData()
    {
        var resolution = Screen.currentResolution;

        WindowsTools.SetNoFrameWindow(new Rect((resolution.width - windowWidth) / 2, (resolution.height - windowHeigth) / 2, windowWidth, windowHeigth));
    }

    private void SetWindowsData(WindowsPosData data)
    {
        WindowsTools.SetNoFrameWindow(new Rect(data.x, data.y, data.width, data.height));
    }

    private void SaveCurrentWindowsData()
    {
        try
        {
            WindowsTools.RECT rect = new WindowsTools.RECT();
            WindowsTools.GetWindowRect(WindowsTools.GetActiveWindow(), ref rect);

            if (File.Exists("config.txt"))
            {
                File.Delete("config.txt");
            }

            var fs = File.Create("config.txt");

            WindowsPosData windowsPosData = new WindowsPosData();
            windowsPosData.x = rect.Left;
            windowsPosData.y = rect.Top;
            windowsPosData.width = rect.Right - rect.Left;
            windowsPosData.height = rect.Bottom - rect.Top;

            byte[] data = Encoding.Default.GetBytes(JsonUtility.ToJson(windowsPosData));
            fs.Write(data, 0, data.Length);
            fs.Close();
            fs.Dispose();
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }
    }

    private void AddDragEvent()
    {
        if (DragArea.GetComponent<EventTrigger>() != null)
        {
            Destroy(DragArea.GetComponent<EventTrigger>());
        }

        EventTrigger eventTrigger = DragArea.gameObject.AddComponent<EventTrigger>();
        eventTrigger.triggers = new List<EventTrigger.Entry>();
        EventTrigger.Entry entry = new EventTrigger.Entry()
        {
            eventID = EventTriggerType.Drag,
            callback = new EventTrigger.TriggerEvent(),
        };
        entry.callback.AddListener(DragEvent);

        eventTrigger.triggers.Add(entry);
    }

    private void DragEvent(BaseEventData data)
    {
        WindowsTools.DragWindow(WindowsTools.GetActiveWindow());
    }

    private void AddBorderDragEvent()
    {
        AddBorderDragEvent(LeftArea, BeginDragEvent, LeftDragEvent);
        AddBorderDragEvent(RightArea, BeginDragEvent, RightDragEvent);
        AddBorderDragEvent(TopArea, BeginDragEvent, TopDragEvent);
        AddBorderDragEvent(BottomArea, BeginDragEvent, BottomDragEvent);
    }

    private void AddBorderDragEvent(EmptyRaycast target, UnityEngine.Events.UnityAction<BaseEventData> beginDragCallback, UnityEngine.Events.UnityAction<BaseEventData> dragCallback)
    {
        EventTrigger eventTrigger = target.gameObject.GetComponent<EventTrigger>();

        if (target.GetComponent<EventTrigger>() == null)
        {
            eventTrigger = target.gameObject.AddComponent<EventTrigger>();
        }

        if (eventTrigger.triggers == null)
        {
            eventTrigger.triggers = new List<EventTrigger.Entry>();
        }

        EventTrigger.Entry entryBeginDrag = new EventTrigger.Entry()
        {
            eventID = EventTriggerType.BeginDrag,
            callback = new EventTrigger.TriggerEvent(),
        };

        EventTrigger.Entry entryDrag = new EventTrigger.Entry()
        {
            eventID = EventTriggerType.Drag,
            callback = new EventTrigger.TriggerEvent(),
        };

        entryBeginDrag.callback.AddListener(beginDragCallback);
        entryDrag.callback.AddListener(dragCallback);
        eventTrigger.triggers.Add(entryBeginDrag);
        eventTrigger.triggers.Add(entryDrag);
    }

    private void BeginDragEvent(BaseEventData data)
    {
        var pointData = data as PointerEventData;
        beginOffset = pointData.position;

        WindowsTools.RECT rect = new WindowsTools.RECT();
        WindowsTools.GetWindowRect(WindowsTools.GetActiveWindow(), ref rect);

        cachePosData = new WindowsPosData();
        cachePosData.x = rect.Left;
        cachePosData.y = rect.Top;
        cachePosData.width = rect.Right - rect.Left;
        cachePosData.height = rect.Bottom - rect.Top;

        resultPosData = new WindowsPosData();
        resultPosData.x = cachePosData.x;
        resultPosData.y = cachePosData.y;
        resultPosData.width = cachePosData.width;
        resultPosData.height = cachePosData.height;
    }

    private void LeftDragEvent(BaseEventData data)
    {
        var pointData = data as PointerEventData;
        var offset = (int)(pointData.position.x - beginOffset.x);

        resultPosData.x += offset;
        resultPosData.width -= offset;

        WindowsTools.SetWindowPos(resultPosData);
    }

    private void RightDragEvent(BaseEventData data)
    {
        var pointData = data as PointerEventData;
        var offset = (int)(pointData.position.x - beginOffset.x);

        resultPosData.width = cachePosData.width + offset;

        WindowsTools.SetWindowPos(resultPosData);
    }

    private void TopDragEvent(BaseEventData data)
    {
        var pointData = data as PointerEventData;
        var offset = (int)(pointData.position.y - beginOffset.y);

        resultPosData.y = cachePosData.y - offset;
        resultPosData.height = cachePosData.height + offset;

        WindowsTools.SetWindowPos(resultPosData);
    }

    private void BottomDragEvent(BaseEventData data)
    {
        var pointData = data as PointerEventData;
        var offset = (int)(pointData.position.y - beginOffset.y);

        resultPosData.height -= offset;

        WindowsTools.SetWindowPos(resultPosData);
    }

    private void AddBorderMouseIconChange()
    {
        if (LeftRightIcon == null)
        {
            LeftRightIcon = Resources.Load<Texture2D>("Texture/LeftRightIcon");
        }

        if (TopBottomIcon == null)
        {
            TopBottomIcon = Resources.Load<Texture2D>("Texture/TopBottomIcon");
        }

        AddBorderMouseIconChange(LeftArea, LeftRightIcon);
        AddBorderMouseIconChange(RightArea, LeftRightIcon);
        AddBorderMouseIconChange(TopArea, TopBottomIcon);
        AddBorderMouseIconChange(BottomArea, TopBottomIcon);
    }

    private void AddBorderMouseIconChange(EmptyRaycast target, Texture2D icon)
    {
        EventTrigger eventTrigger = target.gameObject.GetComponent<EventTrigger>();

        if (target.GetComponent<EventTrigger>() == null)
        {
            eventTrigger = target.gameObject.AddComponent<EventTrigger>();
        }

        if (eventTrigger.triggers == null)
        {
            eventTrigger.triggers = new List<EventTrigger.Entry>();
        }

        EventTrigger.Entry entryEnter = new EventTrigger.Entry()
        {
            eventID = EventTriggerType.PointerEnter,
            callback = new EventTrigger.TriggerEvent(),
        };

        EventTrigger.Entry entryExit = new EventTrigger.Entry()
        {
            eventID = EventTriggerType.PointerExit,
            callback = new EventTrigger.TriggerEvent(),
        };

        entryEnter.callback.AddListener((data) =>
        {
            Cursor.SetCursor(icon, Vector2.zero, CursorMode.Auto);
        });

        entryExit.callback.AddListener((data) =>
        {
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        });

        eventTrigger.triggers.Add(entryEnter);
        eventTrigger.triggers.Add(entryExit);
    }
}

public class WindowsPosData
{
    public int x;
    public int y;
    public int width;
    public int height;
}