using UnityEngine;

public class SampleObject : ObjectPoolObject<SampleObject>
{
    public float lifeTime = 10;
    public bool isTimeDeactive = true;

    public MeshRenderer mr;
    public MeshFilter mf;
    public Collider collider;
    public Rigidbody rb;

    private float duration = 0;

    public void ResetVelocity()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.ResetInertiaTensor();
        rb.ResetCenterOfMass();
    }

    public override void OnActive()
    {
        duration = lifeTime;
        transform.rotation = Quaternion.identity;
        ResetVelocity();
    }

    public override void OnDeactive()
    {
    }

    public override void OnDestroyObject()
    {
    }

    private void Update()
    {
        if (isTimeDeactive)
        {
            duration -= Time.deltaTime;
            if (duration <= 0)
            {
                Deactive();
            }
        }
    }
}
