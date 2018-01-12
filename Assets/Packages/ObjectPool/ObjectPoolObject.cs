using UnityEngine;
using System.Collections;

public abstract class ObjectPoolObject <T> : MonoBehaviour where T : ObjectPoolObject<T>
{
    [HideInInspector]
    protected ObjectPool<T> parentPool;

    public virtual void Initialize(ObjectPool<T> parent = null)
    {
        parentPool = parent;
    }

    public abstract void OnActive();        // 発生時イベント
    public abstract void OnDeactive();      // 非表示時イベント

    public abstract void OnDestroyObject(); // 終了時

    public virtual void Deactive()
    {
        parentPool.DeactiveObject((T)this);
    }

}
