using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiConnector : MonoBehaviour {

    public ConnectionPoint conPointOne;
    public ConnectionPoint conPointTwo;
    public ConnectionPoint conPointThree;

    void ApplyPowerToAll()
    {
        conPointOne.isTransferringPower = conPointTwo.isTransferringPower = conPointThree.isTransferringPower = true;
    }

    void Update()
    {
        if (conPointOne.GetWireStatus())
        {
            ApplyPowerToAll();
        }

        if (conPointTwo.GetWireStatus())
        {
            ApplyPowerToAll();
        }

        if (conPointThree.GetWireStatus())
        {
            ApplyPowerToAll();
        }
    }
}
