using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wire : MonoBehaviour {

    public ConnectionPoint connectionOne;
    public ConnectionPoint connectionTwo;
    public bool isTransferingPower = false;

    bool beingDragged;

    LineRenderer lineRenderer;

    private void OnValidate()
    {
        lineRenderer = gameObject.GetComponent<LineRenderer>();
        if (connectionOne && connectionTwo)
        {
            // Every frame set the wire position to the connections points
            lineRenderer.SetPositions(new Vector3[] { connectionOne.transform.position, connectionTwo.transform.position });
        }
    }

    void Awake()
    {
        lineRenderer = gameObject.GetComponent<LineRenderer>();
    }

    void Update()
    {
        // If either of the connections is transfering power, then we are transfering power
        // ternary expressions messily used to check whether connectionOne/Two even exists
        //Debug.Log(connectionOne ? connectionOne.isTransferringPower : false);
        //Debug.Log(connectionTwo ? connectionTwo.isTransferringPower : false);
        //Debug.Log((connectionOne ? connectionOne.isTransferringPower : false) || (connectionTwo ? connectionTwo.isTransferringPower : false));
        if ((connectionOne ? connectionOne.isTransferringPower : false) || (connectionTwo ? connectionTwo.isTransferringPower : false))
        {
            isTransferingPower = true;
        }
        else
        {
            isTransferingPower = false;
        }

        if (!beingDragged && connectionOne && connectionTwo)
        {
            // Every frame set the wire position to the connections points
            lineRenderer.SetPositions(new Vector3[] { connectionOne.transform.position, connectionTwo.transform.position });
        }
    }

    // Used by connection points to determine whether to create a connection or not
    public bool NotAlreadyConnectedTo(ConnectionPoint conPoint)
    {
        return (connectionOne != conPoint && connectionTwo != conPoint) ? true : false;
    }

    // Should be called just before DragWire (just once)
    public void BeginDrag()
    {
        beingDragged = true;
    }

    void CheckConnections()
    {
        if (!connectionOne || !connectionTwo)
        {
            if (connectionOne)
            {
                connectionOne.RemoveAttachedWire();
            }
            if (connectionTwo)
            {
                connectionTwo.RemoveAttachedWire();
            }
            Destroy(gameObject);
        }
    }

    // Should be called after the last DragWire call
    public void EndDrag()
    {
        beingDragged = false;

        CheckConnections();
    }

    public void DragWire(int index, Vector3 newPos)
    {
        lineRenderer.SetPosition(index, newPos);
    }

    public void DeleteNextFrame()
    {
        Invoke("CheckConnections", 0.0000001f);
    }
}
