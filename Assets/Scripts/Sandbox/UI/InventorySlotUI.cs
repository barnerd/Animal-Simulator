using UnityEngine;
using UnityEngine.UI;

public class InventorySlotUI : MonoBehaviour
{
    InventoryUI inventoryUI;

    Item item;

    public MeshFilter gfxMF;
    public MeshRenderer gfxMR;

    public Button itemButton;
    public Button removeButton;

    // Start is called before the first frame update
    void Start()
    {
        if(inventoryUI == null)
        {
            inventoryUI = transform.parent.parent.parent.GetComponent<InventoryUI>();
        }
    }

    public void AddItem(Item _item)
    {
        item = _item;
        gfxMR.material = item.material;
        gfxMF.sharedMesh = item.mesh;

        gfxMF.gameObject.GetComponent<RectTransform>().anchoredPosition = item.inventoryPosition;
        gfxMF.gameObject.GetComponent<RectTransform>().localRotation = item.inventoryRotation;
        gfxMF.gameObject.GetComponent<RectTransform>().localScale = Vector3.one * item.inventoryScale;
        gfxMF.gameObject.SetActive(true);

        itemButton.interactable = true;
        removeButton.gameObject.SetActive(true);
    }

    public void Clear()
    {
        itemButton.interactable = false;
        gfxMF.gameObject.SetActive(false);
        gfxMF.sharedMesh = null;
        gfxMR.material = null;
        item = null;
        removeButton.gameObject.SetActive(false);
    }

    public void InteractItem()
    {
        inventoryUI.InteractItem(item);
    }

    public void RemoveItem()
    {
        inventoryUI.RemoveItem(item);
    }
}
