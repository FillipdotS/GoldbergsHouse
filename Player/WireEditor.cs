using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WireEditor : MonoBehaviour {

    public AudioClip wireCreateSound;

    [Space(10)]
    public GameObject wirePrefab;

    [HideInInspector]
    public Wire draggedWire;
    [HideInInspector]
    public int indexPosOfWireBeingDragged;

    void LateUpdate()
    {
        if (draggedWire && Input.GetMouseButtonUp(0))
        {
            AudioController.Instance.PlaySound(wireCreateSound);
            draggedWire.EndDrag();
            draggedWire = null;
            // The positioning of the wire etc is handled by the ConnectionPoint script
        }
        else if (draggedWire)
        {
            Vector3 camToMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition); // easier reading
            draggedWire.DragWire(indexPosOfWireBeingDragged, new Vector3(camToMousePos.x, camToMousePos.y, 0f));
        }
    }

    public void EditExistingWire(int index, Wire wire)
    {
        draggedWire = wire;
        indexPosOfWireBeingDragged = index;

        wire.BeginDrag();
    }

    public void NewWire(ConnectionPoint connectionPoint)
    {
        AudioController.Instance.PlaySound(wireCreateSound);

        indexPosOfWireBeingDragged = 1;

        GameObject instantiatedWire = Instantiate(wirePrefab);
        draggedWire = instantiatedWire.GetComponent<Wire>();
        connectionPoint.connectedWire = draggedWire;

        draggedWire.connectionOne = connectionPoint;
        draggedWire.BeginDrag();
        draggedWire.DragWire(0, connectionPoint.transform.position);
    }
}
