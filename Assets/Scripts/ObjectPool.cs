using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> : IPool<T> where T : MonoBehaviour, IPoolable<T>
{
    public ObjectPool(GameObject pooledObject, int numToSpawn = 0, GameObject parent = null)
    {
        this.prefab = pooledObject;
        Spawn(numToSpawn, parent);
    }

    public ObjectPool(GameObject pooledObject, Action<T> pullObject, Action<T> pushObject, int numToSpawn = 0,
        GameObject parent = null)
    {
        this.prefab = pooledObject;
        this.pullObject = pullObject;
        this.pushObject = pushObject;
        Spawn(numToSpawn, parent);
    }

    private System.Action<T> pullObject;
    private System.Action<T> pushObject;
    private Stack<T> pooledObjects = new Stack<T>();
    private GameObject prefab;

    public int pooledCount
    {
        get { return pooledObjects.Count; }
    }

    public T Pull()
    {
        T t;
        if (pooledCount > 0)
            t = pooledObjects.Pop();
        else
            t = GameObject.Instantiate(prefab).GetComponent<T>();

        t.gameObject.SetActive(true); //ensure the object is on
        t.Initialize(Push);

        //allow default behavior and turning object back on
        pullObject?.Invoke(t);

        return t;
    }

    public T Pull(Vector3 position)
    {
        T t = Pull();
        t.transform.position = position;
        return t;
    }

    public T Pull(Vector3 position, Quaternion rotation)
    {
        T t = Pull();
        t.transform.position = position;
        t.transform.rotation = rotation;
        return t;
    }

    public GameObject PullGameObject()
    {
        return Pull().gameObject;
    }

    public GameObject PullGameObject(Vector3 position)
    {
        GameObject go = Pull().gameObject;
        go.transform.position = position;
        return go;
    }

    public GameObject PullGameObject(Vector3 position, Quaternion rotation)
    {
        GameObject go = Pull().gameObject;
        go.transform.position = position;
        go.transform.rotation = rotation;
        return go;
    }

    public void Push(T t)
    {
        pooledObjects.Push(t);

        //create default behavior to turn off objects
        pushObject?.Invoke(t);

        t.gameObject.SetActive(false);
    }

    private void Spawn(int number, GameObject parent = null)
    {
        T t;

        for (int i = 0; i < number; i++)
        {
            t = parent == null
                ? GameObject.Instantiate(prefab).GetComponent<T>()
                : GameObject.Instantiate(prefab, parent.transform).GetComponent<T>();
            pooledObjects.Push(t);
            t.gameObject.SetActive(false);
        }
    }
}