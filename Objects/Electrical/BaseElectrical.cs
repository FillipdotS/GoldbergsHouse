using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseElectrical : MonoBehaviour {

    public bool isPowerSource = false;
    public bool isBeingPowered = false;

    ConnectionPoint connectionPoint;

	void Awake()
    {
        Init();
    }

    void Update()
    {
        if (isPowerSource)
        {
            connectionPoint.isTransferringPower = true;
        }

        if(IsSwitchedOn())
        {
            OnAction();
        }
        else
        {
            OffAction();
        }
    }

    public virtual void Init()
    {
        connectionPoint = transform.Find("ConnectionPoint").GetComponent<ConnectionPoint>();
        if (connectionPoint == null)
        {
            Debug.LogError("Could not get connection point for this object", gameObject);
        }
    }

    public virtual bool IsSwitchedOn() {
        if (connectionPoint.connectedWire != null)
        {
            return connectionPoint.connectedWire.isTransferingPower;
        }
        else
        {
            return false;
        }
    }

    public virtual void OnAction() { }
    public virtual void OffAction() { }
}
