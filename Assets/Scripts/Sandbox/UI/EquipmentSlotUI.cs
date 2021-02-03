using UnityEngine;
using UnityEngine.UI;

public class EquipmentSlotUI : MonoBehaviour
{
    public EquipmentUI equipmentUI;
    public EquipmentSlot slotType;

    ItemData item;

    public MeshFilter gfxMF;
    public MeshRenderer gfxMR;
    public Image gfxIcon;

    public Button itemButton;
    public Button removeButton;

    public float defaultIconAlpha;

    public BoolReference showDisabledSlot;
    public bool disabled;

    // Start is called before the first frame update
    void Start()
    {
    }

    /// <summary>
    /// Display the item attached to this slot
    /// </summary>
    /// <param name="_item">the item to display</param>
    public void DisplayItem(EquipmentData _item)
    {
        if (!_item.baseClothing)
        {
            item = _item;

            //gfxMR.material = item.material;
            //gfxMF.sharedMesh = item.mesh;
            //gfxMF.gameObject.GetComponent<RectTransform>().anchoredPosition = item.inventoryPosition;
            //gfxMF.gameObject.GetComponent<RectTransform>().localRotation = item.inventoryRotation;
            //gfxMF.gameObject.GetComponent<RectTransform>().localScale = Vector3.one * item.inventoryScale;

            gfxIcon.sprite = item.icon;
            Color tmp = gfxIcon.color;
            tmp.a = 1f;
            gfxIcon.color = tmp;

            itemButton.gameObject.SetActive(true);
            itemButton.interactable = true;
            removeButton.gameObject.SetActive(true);
        }
        else
        {
            Clear();
        }
    }

    /// <summary>
    /// Reset the SlotUI and display the default image
    /// </summary>
    public void Clear()
    {
        removeButton.gameObject.SetActive(false);
        itemButton.interactable = false;

        if (showDisabledSlot.Value || !disabled)
        {
            gfxIcon.gameObject.SetActive(true);
            gfxIcon.sprite = slotType.sprite;

            Color tmp = gfxIcon.color;
            tmp.a = defaultIconAlpha;
            gfxIcon.color = tmp;

            itemButton.gameObject.SetActive(false);
        }
        else
        {
            gfxIcon.gameObject.SetActive(false);
            gfxIcon.sprite = null;

            itemButton.gameObject.SetActive(false);
        }

        gfxMF.sharedMesh = null;
        gfxMR.material = null;
        gfxMR.enabled = false;

        item = null;
    }

    /// <summary>
    /// Unequip the item from the slot and the equipment manager.
    /// </summary>
    public void UnequipItem()
    {
        equipmentUI.UnequipItem(slotType);
        //Clear();
    }
}
