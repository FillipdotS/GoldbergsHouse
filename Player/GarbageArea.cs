using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GarbageArea : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    bool isOver = false;

    Drag dragScript;
    InventoryController invController;
    Color32 deletionColor;

    void Start()
    {
        dragScript = GameObject.FindGameObjectWithTag("Player").GetComponent<Drag>();
        deletionColor = dragScript.deletionColor;
        invController = GameObject.FindGameObjectWithTag("InventoryController").GetComponent<InventoryController>();
    }

    void Update()
    {
        if (isOver && dragScript.draggedObject && Input.GetMouseButtonUp(0))
        {
            invController.ReturnItemToInventory(dragScript.draggedObject);
        }
    }

    // The two events which change isOver

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
}
