using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElecticalObject : BaseElectrical {

    public GameObject lorem;
    public GameObject ipsum;

    LineRenderer l;

    private void Start()
    {
        l = gameObject.GetComponent<LineRenderer>();
    }

    private void Update()
    {
        l.SetPositions(new Vector3[] { lorem.transform.position, ipsum.transform.position });
    }

    public override void Init()
    {
        base.Init();
        Debug.Log("ElecticalObject object");
    }
}
