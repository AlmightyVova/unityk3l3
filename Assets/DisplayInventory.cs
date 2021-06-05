using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.EventSystems;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DisplayInventory : MonoBehaviour
{
    public MouseItem MouseItem = new MouseItem();
    public GameObject inventoryPrefab;
    public InventoryObject inventory;

    public int xStart;
    public int yStart;
    public int xSpaceBetweenItems;
    public int ySpaceBetweenItems;
    public int numOfColumns;

    private Dictionary<GameObject, InventorySlot> _itemsDisplayed = new Dictionary<GameObject, InventorySlot>();

    // Start is called before the first frame update
    void Start()
    {
        CreateSlots();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateSlots();
    }

    public void CreateSlots()
    {
        _itemsDisplayed = new Dictionary<GameObject, InventorySlot>();
        for (int i = 0; i < inventory.container.items.Length; i++)
        {
            var obj = Instantiate(inventoryPrefab, Vector3.zero, Quaternion.identity, transform);
            obj.GetComponent<RectTransform>().anchoredPosition = GetPosition(i);

            AddEvent(obj, EventTriggerType.PointerEnter, delegate { OnPointerEnter(obj); });
            AddEvent(obj, EventTriggerType.PointerExit, delegate { OnPointerExit(obj); });
            AddEvent(obj, EventTriggerType.BeginDrag, delegate { OnBeginDrag(obj); });
            AddEvent(obj, EventTriggerType.EndDrag, delegate { OnEndDrag(obj); });
            AddEvent(obj, EventTriggerType.Drag, delegate { OnDrag(obj); });

            _itemsDisplayed.Add(obj, inventory.container.items[i]);
        }
    }

    public void UpdateSlots()
    {
        foreach (var slot in _itemsDisplayed)
        {
            if (slot.Value.id >= 0)
            {
                slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().sprite = inventory.database.GetItem[slot.Value.item.id].uiDisplay;
                slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 1);
                slot.Key.GetComponentInChildren<TextMeshProUGUI>().text = slot.Value.amount == 1 ? "" : slot.Value.amount.ToString("n0");
            }
            else
            {
                slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().sprite = null;
                slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 0);
                slot.Key.GetComponentInChildren<TextMeshProUGUI>().text = "";
            }
        }
    }

    private void AddEvent(GameObject obj, EventTriggerType type, UnityAction<BaseEventData> action)
    {
        EventTrigger trigger = obj.GetComponent<EventTrigger>();
        var eventTrigger = new EventTrigger.Entry();
        eventTrigger.eventID = type;
        eventTrigger.callback.AddListener(action);
        trigger.triggers.Add(eventTrigger);
    }

    public void OnPointerEnter(GameObject obj)
    {
        MouseItem.HoverObj = obj;
        if (_itemsDisplayed.ContainsKey(obj))
        {
            MouseItem.HoverItem = _itemsDisplayed[obj];
        }
    }

    public void OnPointerExit(GameObject obj)
    {
        MouseItem.HoverObj = null;
        MouseItem.HoverItem = null;
    }

    public void OnBeginDrag(GameObject obj)
    {
        var mouseObject = new GameObject();
        var rt = mouseObject.AddComponent<RectTransform>();
        rt.sizeDelta = new Vector2(50, 50);
        mouseObject.transform.SetParent(transform.parent);
        if (_itemsDisplayed[obj].id >= 0)
        {
            var image = mouseObject.AddComponent<Image>();
            image.sprite = inventory.database.GetItem[_itemsDisplayed[obj].id].uiDisplay;
            image.raycastTarget = false;
        }

        MouseItem.Obj = mouseObject;
        MouseItem.Item = _itemsDisplayed[obj];
    }

    public void OnEndDrag(GameObject obj)
    {
        if (MouseItem.HoverObj)
        {
            inventory.MoveItem(_itemsDisplayed[obj], _itemsDisplayed[MouseItem.HoverObj]);
        }
        else
        {
            inventory.RemoveItem(_itemsDisplayed[obj].item);
        }

        Destroy(MouseItem.Obj);
        MouseItem.Item = null;
    }

    public void OnDrag(GameObject obj)
    {
        if (MouseItem.Obj != null)
        {
            MouseItem.Obj.GetComponent<RectTransform>().position = Input.mousePosition;
        }
    }

    public Vector2 GetPosition(int i)
    {
        return new Vector2(xStart + xSpaceBetweenItems * (i % numOfColumns), yStart + (-ySpaceBetweenItems * (i / numOfColumns)));
    }
}