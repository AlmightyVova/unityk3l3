using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.EventSystems;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class UserInterface : MonoBehaviour
{
    public PlayerInventoryScript playerInventoryScript;

    public InventoryObject inventory;

    protected Dictionary<GameObject, InventorySlot> ItemsDisplayed = new Dictionary<GameObject, InventorySlot>();

    // Start is called before the first frame update
    void Start()
    {
        for (var index = 0; index < inventory.container.items.Length; index++)
        {
            var item = inventory.container.items[index];
            item.parent = this;
        }

        CreateSlots();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateSlots();
    }

    public abstract void CreateSlots();

    public void UpdateSlots()
    {
        foreach (var slot in ItemsDisplayed)
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

    protected void AddEvent(GameObject obj, EventTriggerType type, UnityAction<BaseEventData> action)
    {
        EventTrigger trigger = obj.GetComponent<EventTrigger>();
        var eventTrigger = new EventTrigger.Entry();
        eventTrigger.eventID = type;
        eventTrigger.callback.AddListener(action);
        trigger.triggers.Add(eventTrigger);
    }

    public void OnPointerEnter(GameObject obj)
    {
        playerInventoryScript.MouseItem.HoverObj = obj;
        if (ItemsDisplayed.ContainsKey(obj))
        {
            playerInventoryScript.MouseItem.HoverItem = ItemsDisplayed[obj];
        }
    }

    public void OnPointerExit(GameObject obj)
    {
        playerInventoryScript.MouseItem.HoverObj = null;
        playerInventoryScript.MouseItem.HoverItem = null;
    }

    public void OnBeginDrag(GameObject obj)
    {
        var mouseObject = new GameObject();
        var rt = mouseObject.AddComponent<RectTransform>();
        rt.sizeDelta = new Vector2(50, 50);
        mouseObject.transform.SetParent(transform.parent);
        if (ItemsDisplayed[obj].id >= 0)
        {
            var image = mouseObject.AddComponent<Image>();
            image.sprite = inventory.database.GetItem[ItemsDisplayed[obj].id].uiDisplay;
            image.raycastTarget = false;
        }

        playerInventoryScript.MouseItem.Obj = mouseObject;
        playerInventoryScript.MouseItem.Item = ItemsDisplayed[obj];
    }

    public void OnEndDrag(GameObject obj)
    {
        var mouseItem = playerInventoryScript.MouseItem;
        var mouseHoverItem = mouseItem.HoverItem;
        var mouseHoverObj = mouseItem.HoverObj;
        var getItemObject = inventory.database.GetItem;

        if (mouseHoverObj)
        {
            if (mouseHoverItem.CanPlaceInSlot(getItemObject[ItemsDisplayed[obj].id]))
                inventory.MoveItem(ItemsDisplayed[obj], mouseHoverItem.parent.ItemsDisplayed[mouseHoverObj] /*ItemsDisplayed[playerInventoryScript.MouseItem.HoverObj]*/);
        }
        else
        {
            // inventory.RemoveItem(ItemsDisplayed[obj].item);
        }

        Destroy(mouseItem.Obj);
        mouseItem.Item = null;
    }

    public void OnDrag(GameObject obj)
    {
        if (playerInventoryScript.MouseItem.Obj != null)
        {
            playerInventoryScript.MouseItem.Obj.GetComponent<RectTransform>().position = Input.mousePosition;
        }
    }
}

public class MouseItem
{
    public GameObject Obj;
    public InventorySlot Item;
    public InventorySlot HoverItem;
    public GameObject HoverObj;
}