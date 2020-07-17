using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ConnectionPoint : MonoBehaviour {

    //[HideInInspector]
    public Wire connectedWire = null;
    //[HideInInspector]
    public bool isTransferringPower;
    //[HideInInspector]
    public int connectionID = -1;

    static ConnectionPoint clickedConnectionPoint;
    public static int largestNaturalConnectionID = 50;

    [Space(10)]
    public InteractiveObject parentInteractiveObject;
    [Space(10)]

    public bool isEditable = true;

    GameObject glowObject;
    SpriteRenderer sr;
    Sprite lidSprite;
    Sprite defaultSprite;

    WireEditor wireEditor;

    private bool isOver;

    void Start()
    {
        if (parentInteractiveObject == null)
        {
            if (transform.parent.parent.gameObject.GetComponent<InteractiveObject>() == null)
            {
                Debug.LogError("This connection point could not find it's parents InteractiveObject", gameObject);
            }
            else
            {
                parentInteractiveObject = transform.parent.parent.gameObject.GetComponent<InteractiveObject>();
            }
        }

        if (connectionID == -1)
        {
            if (!transform.parent.parent.gameObject.GetComponent<InteractiveObject>().draggable)
            {
                Debug.LogError("!!! This connection point does not have a manually set ID !!!", gameObject);
            }

            connectionID = largestNaturalConnectionID;
            largestNaturalConnectionID++;
        }

        wireEditor = GameObject.FindGameObjectWithTag("Player").GetComponent<WireEditor>();
        glowObject = transform.parent.Find("glow").gameObject;
        if (glowObject == null)
        {
            Debug.LogError("Could not find a glow object for this connection point", gameObject);
        }

        sr = gameObject.GetComponent<SpriteRenderer>();
        defaultSprite = sr.sprite;
        lidSprite = Resources.Load<Sprite>("socket_lid");
    }

    public bool GetWireStatus()
    {
        if (connectedWire != null)
        {
            return connectedWire.isTransferingPower;
        }
        return false;
    }

    void Update()
    {
        // No wire
        if (connectedWire == null)
        {
            glowObject.SetActive(true);
            sr.sprite = defaultSprite;
            sr.sortingOrder = 1;
        }
        // Wire
        else
        {
            glowObject.SetActive(false);
            sr.sprite = lidSprite;
            sr.sortingOrder = 6;
        }

        if (wireEditor.enabled && isEditable)
        {
            // If we are taking a wire from this connection point
            if (isOver && Input.GetMouseButtonDown(0))
            {
                clickedConnectionPoint = this;

                if (connectedWire)
                {
                    int indexBeingEdited = connectedWire.connectionOne == this ? 0 : 1;
                    wireEditor.EditExistingWire(indexBeingEdited, connectedWire);
                    RemoveAttachedWire();
                }
                else
                {
                    wireEditor.NewWire(this);
                }
            }
            // If we are dropping a wire on this connection point
            else if (isOver && Input.GetMouseButtonUp(0) && wireEditor.draggedWire)
            {
                // Not already connected to a wire
                if (!connectedWire)
                {
                    // If we arent dragging the wire back to ourselves, create a connection
                    if (wireEditor.draggedWire.NotAlreadyConnectedTo(this))
                    {
                        connectedWire = wireEditor.draggedWire;
                        connectedWire.DragWire(wireEditor.indexPosOfWireBeingDragged, transform.position);
                        if (wireEditor.indexPosOfWireBeingDragged == 0)
                        {
                            connectedWire.connectionOne = this;
                        }
                        else
                        {
                            connectedWire.connectionTwo = this;
                        }
                    }
                }
            }
        }
    }

    // Called in various places
    public void RemoveAttachedWire()
    {
        if (connectedWire)
        {
            if (connectedWire.connectionOne == this)
            {
                connectedWire.connectionOne = null;
            }
            else
            {
                connectedWire.connectionTwo = null;
            }
            connectedWire = null;
        }
    }

    void OnMouseEnter()
    {
        isOver = true;
    }

    void OnMouseExit()
    {
        isOver = false;
    }
}
