using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleObjectPool : ObjectPool<SampleObject> {

    private void Start()
    {
        Initialize();
    }

    public SampleObject EmitSampleObject(Vector3 pos, Vector3 velocity)
    {
        SampleObject obj = GetReserve();
        if(obj != null)
        {
            obj.transform.position = pos;
            //obj.ResetVelocity();
            obj.rb.AddForce(velocity, ForceMode.Impulse);
            obj.rb.AddTorque(Random.onUnitSphere * Mathf.PI * Random.Range(10, 20), ForceMode.Impulse);
        }
        return obj;
    }
}
