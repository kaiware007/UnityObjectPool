using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class ObjectPool <T> : MonoBehaviour where T : ObjectPoolObject<T> {
	
	[SerializeField]
	protected GameObject prefab;

	/// <summary>
	/// オブジェクト数
	/// </summary>
	public int poolMax = 1;

    public LinkedList<T> actives { get { return activeList_;} }
    public int reserveCount { get { return reserveList_.Count; } }
    public int activeCount { get { return activeList_.Count; } }
    public Transform root { get { return root_; } }

	protected LinkedList<T> reserveList_ = new LinkedList<T>();
	protected LinkedList<T> activeList_ = new LinkedList<T>();

    protected Transform root_ = null;

    protected T[] poolArray_ = null;

    public void SetPrefab(GameObject pref)
    {
        prefab = pref;
    }

	/// <summary>
	/// オブジェクトプール初期化
	/// </summary>
	public virtual void Initialize()
    {
		GameObject gr = new GameObject ();
		gr.name = "Root";
		root_ = gr.transform;
		root.SetParent (this.transform);
        root.localPosition = Vector3.zero;

        reserveList_.Clear ();
		activeList_.Clear ();

        poolArray_ = new T[poolMax];

        //lock (lockObject)
        {
            for (int i = 0; i < poolMax; i++)
            {
                GameObject go = CreateChild();
                T obj = go.GetComponentInChildren<T>();
                obj.name = obj.name + "_" + i;
                poolArray_[i] = obj;
                obj.Initialize(this);

                reserveList_.AddLast(obj);

                go.SetActive(false);
            }
        }
	}

	protected GameObject CreateChild() {
		GameObject go = GameObject.Instantiate (prefab);
		go.transform.SetParent (this.root);
		return go;
	}

    /// <summary>
    /// 終了時に非アクティブ含めてすべて破棄する
    /// </summary>
    protected void OnDestroy()
    {
        //Debug.Log("ObjectPool OnDestroy " + name);

        if (poolArray_ != null)
        {
            for (int i = 0; i < poolMax; i++)
            {
                poolArray_[i].OnDestroyObject();
            }
        }
    }
    
    /// <summary>
    /// 空きオブジェクト取得
    /// </summary>
    /// <returns>The object.</returns>
    public T GetReserve() {
        T resv = null;

        //lock (lockObject)
        {
            LinkedListNode<T> node = reserveList_.First;
            
            if (node != null)
            {
                resv = node.Value;
                reserveList_.RemoveFirst();
                activeList_.AddLast(resv);
                resv.gameObject.SetActive(true);
                resv.OnActive();
            }
        }

		return resv;
	}

	/// <summary>
	/// アクティブなオブジェクトをプールに戻す
	/// </summary>
	/// <param name="obj">Object.</param>
	public virtual void DeactiveObject(T obj) {
        //lock (lockObject)
        {
            activeList_.Remove(obj);
            if (!reserveList_.Contains(obj))
            {
                reserveList_.AddLast(obj);
            }
            obj.OnDeactive();
            obj.gameObject.SetActive(false);
        }
	}

    /// <summary>
    /// すべてのアクティブなオブジェクトをプールに戻す
    /// </summary>
    public void DeactiveObjectAll()
    {
        LinkedListNode<T> node = activeList_.First;

        while (node != null)
        {
            T obj = node.Value;
            if (obj != null)
            {
                //DeactiveObject(obj);
                //reserveList_.AddLast(obj);
                obj.OnDeactive();
                obj.gameObject.SetActive(false);
            }
            node = node.Next;
        }
        activeList_.Clear();
        reserveList_.Clear();

        // 配列から追加しなおす
        for (int i = 0; i < poolMax; i++)
        {
            reserveList_.AddLast(poolArray_[i]);
        }
    }

    protected void DumpList(LinkedList<T> list, string filename)
    {
        LinkedListNode<T> node = list.First;

        StreamWriter sw;
        FileInfo fi;
        string date = System.DateTime.Now.ToString("yyyyMMddHHmmss");
        fi = new FileInfo(Application.dataPath + "/../" + filename + date + ".csv");
        sw = fi.AppendText();
        int count = 0;

        while (node != null)
        {
            T obj = node.Value;
            if (obj != null)
            {
                sw.WriteLine("" + count + "," + obj.name);
            }
            node = node.Next;
            count++;
        }

        sw.Flush();
        sw.Close();
    }

    public void Dump()
    {
        DumpList(activeList_, "ActiveList");
        DumpList(reserveList_, "ReserveList");
    }
}
