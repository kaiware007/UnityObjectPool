using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetGenerator : ObjectPool<SampleObject> {

    public Vector3Int nums = Vector3Int.one;
    public Vector3 offset;

	// Use this for initialization
	void Start () {
        Initialize();
        Layout();
    }

    void Layout()
    {
        float offsetX = (float)(nums.x) * 0.5f - 0.5f;
        for(int x = 0; x < nums.x; x++)
        {
            for (int y = 0; y < nums.y; y++)
            {
                for (int z = 0; z < nums.z; z++)
                {
                    SampleObject obj = GetReserve();
                    if(obj != null)
                    {
                        obj.rb.interpolation = RigidbodyInterpolation.None;
                        obj.transform.position = offset + new Vector3(x - offsetX, y, z);
                        //obj.ResetVelocity();
                    }
                }
            }
        }
    }

    public void ResetLayout()
    {
        DeactiveObjectAll();
        Layout();
    }
}
