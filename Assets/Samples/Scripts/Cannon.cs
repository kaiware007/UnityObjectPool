using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour {

    public SampleObjectPool pool;
    public float power = 10;

    public TargetGenerator generator;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetMouseButtonDown(0))
        {
            Vector3 screenPos = Input.mousePosition;
            screenPos.z = 100;
            Vector3 targetPos = Camera.main.ScreenToWorldPoint(screenPos);
            Vector3 dir = targetPos.normalized;
            Debug.DrawRay(transform.position, dir, Color.red, 1,true);
            pool.EmitSampleObject(transform.position, dir * power);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            pool.DeactiveObjectAll();
            generator.ResetLayout();
        }
	}
}
