using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class InvItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    // Every inventory slot has this class

    public string uniqueName; // Each InvItem's uniqueName should be different from every other uniqueName
    public int startingAmount = 5;
    public GameObject objectPrefab;

    bool isOver; // is the mouse over this UI
    Color32 deletionColor;
    Drag dragScript;

    Image itemImage;
    InventoryController invController;
    TextMeshProUGUI amountText;

    Color ranOutColor;
    Color startingColor;

    // Update the counter while in the editor
    private void OnValidate()
    {
        amountText = transform.Find("Amount").gameObject.GetComponent<TextMeshProUGUI>();
        amountText.text = "x" + startingAmount.ToString();
    }

    void Start()
    {
        dragScript = GameObject.FindGameObjectWithTag("Player").GetComponent<Drag>();
        deletionColor = dragScript.deletionColor;

        itemImage = transform.Find("ItemImage").gameObject.GetComponent<Image>();
        invController = GameObject.Find("InventoryController").GetComponent<InventoryController>();

        startingColor = itemImage.color;
        ranOutColor = new Color(startingColor.r, startingColor.g, startingColor.b, 0.5f);

        if (startingAmount <= 0)
        {
            Debug.LogError(gameObject.name + " starting amount is below or equal to 0.");
        }
        if (objectPrefab.GetComponent<InteractiveObject>() == null)
        {
            Debug.LogError(gameObject.name + " object prefab is not an interactive object. Errors will occur.");
        }

        amountText = transform.Find("Amount").gameObject.GetComponent<TextMeshProUGUI>();
        UpdateAmountCounter();
    }

    void Update()
    {
        // If mouse is over the UI and we are releasing the mouse with an object selected, return it back
        if (isOver && dragScript.draggedObject && !dragScript.forceDraggedObject && Input.GetMouseButtonUp(0))
        {
            invController.ReturnItemToInventory(dragScript.draggedObject);
        }
    }

    // Some pointer events
    public void OnPointerEnter(PointerEventData eventData)
    {
        isOver = true;

        if (dragScript.draggedObject)
        {
            dragScript.draggedObject.GetComponent<SpriteRenderer>().material.color = deletionColor;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isOver = false;

        if (dragScript.draggedObject)
        {
            dragScript.draggedObject.GetComponent<SpriteRenderer>().material.color = dragScript.draggingColor;
        }
    }

    // Updates the actual UI text and if amount = 0 somehow show that. Better than calling it every Update()
    public void UpdateAmountCounter()
    {
        amountText.text = "x" + startingAmount.ToString();
        if (startingAmount == 0)
        {
            itemImage.color = ranOutColor;
        }
        else
        {
            itemImage.color = startingColor;
        }
    }

    // Each InvItem should be as dumb as possible, therefore most references and important things are stored
    // in the InvController, including the actual instantation of new objects
    public void SpawnNewObject()
    {
        if (startingAmount > 0)
        {
            // InvController will do some checks and only then decrease the startingAmount and update the counter
            invController.SpawnPlayerObject(objectPrefab, gameObject.GetComponent<InvItem>());
        }
    }
}
