using Mig.Model;
using Mig.Snapshot;
using Mig.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Pool;
using UnityEngine.UI;

public class UISnapshotList : MonoBehaviour
{
    public Button addButton;
    public Button editButton;
    public SnapshotViewItem scenePrefab;
    public RectTransform contentTrans;
    public RectTransform ViewPoint;
    public RectTransform _scrollbar;
    public RectTransform scrollView;
    private List<SnapshotViewItem> m_items = new();

    private ObjectPool<SnapshotViewItem> m_SnapshotViewPool;

    private int maxShowLength = Screen.width *  2 / 3;
    private int itemLength = 180;

    protected void Awake()
    {
        SnapshotManager.Instance.OnSnapShotUpdated += OnSnapShotUpdated;

        ModelManager.Instance.OnModelLoadCompleteEvent += OnOnModelLoadEvent;

        //m_SnapshotViewPool = new ObjectPool<SnapshotViewItem>(onCreate, onGet, onRelease, onDestroy);

        addButton.onClick.AddListener(AddScene);
        addButton.gameObject.SetActive(false);
        editButton.gameObject.SetActive(true);
    }

    private void Start()
    {
        AddEventTrigger(_scrollbar.gameObject, EventTriggerType.PointerDown, OnScrollbarBeginDrag);
    }

    private void AddEventTrigger(GameObject obj, EventTriggerType type, System.Action<BaseEventData> action)
    {
        EventTrigger trigger = obj.GetComponent<EventTrigger>();
        if (trigger == null)
        {
            trigger = obj.AddComponent<EventTrigger>();
        }

        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = type;
        entry.callback = new EventTrigger.TriggerEvent();
        entry.callback.AddListener((eventData) => action(eventData));
        trigger.triggers.Add(entry);
    }

    public void OnScrollbarBeginDrag(BaseEventData data)
    {
        foreach (var item in m_items)
        {
            item.GetComponent<Image>().material = null;
        }
    }

    private void OnSnapShotUpdated()
    {
        InitSnapShotUIList();
        StartCoroutine(ArrangeViewSizeIenumerator());
    }

    /// <summary>
    /// For init snpashot, if we load a model from local or web.
    /// We will load snapshot at the same time.
    /// </summary>
    private void InitSnapShotUIList()
    {
        var currentAllSnapShot = SnapshotManager.Instance.GetCurrentAllSnapShot();

        if (currentAllSnapShot.Count > m_items.Count)
        {
            for (int i = m_items.Count;i < currentAllSnapShot.Count; i ++)
            {
                SnapshotViewItem newImageObject = Instantiate(scenePrefab, contentTrans);
                newImageObject.transform.SetAsLastSibling();
                m_items.Add(newImageObject);
            }
            addButton.transform.SetAsLastSibling();
        }
        else
        {
            for(int i = m_items.Count - 1; i > currentAllSnapShot.Count;i --)
            {
                var item = m_items[i];
                GameObject.Destroy(item.gameObject);
                m_items.RemoveAt(i);
            }
        }

        for (int i = 0; i < currentAllSnapShot.Count;i ++)
        {
            var snapShot = currentAllSnapShot[i];
            var snapShotItem = m_items[i];

            var tex = snapShot.Image;
            if (tex != null)
            {
                snapShotItem.Image.sprite = tex.ConvertToSprite();           
            }
            var index = i;
            snapShotItem.Tmp.text = index.ToString();
            snapShotItem.Button.onClick.RemoveAllListeners();
            snapShotItem.Button.onClick.AddListener(() =>
            {
                SnapshotManager.Instance.UpdateCurrentSnapShot();
                SnapshotManager.Instance.ApplyToTargetSnapshot(index);
                ShotButtonOutline(m_items[SnapshotManager.Instance.CurrentSnapshotIndex]);

            });
        }
        ShotButtonOutline(m_items[SnapshotManager.Instance.CurrentSnapshotIndex]);
    }

    private IEnumerator AdjustContentPosition()
    {
        yield return null;
        yield return null;

        contentTrans.anchoredPosition = new Vector2(0, contentTrans.anchoredPosition.y);
    }

    private IEnumerator ArrangeViewSizeIenumerator()
    {
        yield return null;
        yield return null;
        ArrangeViewSize();
    }

    private void OnOnModelLoadEvent()
    {
        addButton.gameObject.SetActive(true);
    }

    public void AddScene()
    {
        SnapshotManager.Instance.AddSnapShotStepAtEnd();
        StartCoroutine(AdjustContentPosition());
    }

    // keep until tested
    void ArrangeViewSize()
    {
        var contentLength = contentTrans.rect.width;

            // hold 
        var curSize = scrollView.sizeDelta;
        curSize.x = Mathf.Min(maxShowLength, contentLength);
        scrollView.sizeDelta = curSize;
    }

    public void ShotButtonOutline(SnapshotViewItem obj)
    {
        foreach (var item in m_items)
        {
            item.Outline.enabled = false;
        }
        obj.Outline.enabled = true;
    }
    protected void OnDestroy()
    {
        if (SnapshotManager.Instance != null)
        {
            SnapshotManager.Instance.OnSnapShotUpdated -= OnSnapShotUpdated;
        }
        if (ModelManager.Instance != null)
        {
            ModelManager.Instance.OnModelLoadCompleteEvent -= OnOnModelLoadEvent;
        }
    }
}
