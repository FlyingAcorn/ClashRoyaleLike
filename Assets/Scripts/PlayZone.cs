using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayZone : MonoBehaviour
{
    public bool isAllyZone;
    public List<Transform> nodes;

    [SerializeField] private List<Entity> trespassers;

    public List<Entity> Trespassers
    {
        get
        {
            trespassers.RemoveAll(t => t == null);
            return trespassers;
        }
        set
        {
            trespassers.RemoveAll(t => t == null);
            trespassers = value;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log(collision.transform.name);
        if (collision.transform.TryGetComponent(out Entity entity) && entity.isAlly != isAllyZone)
        {
            Trespassers.Add(entity);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.transform.TryGetComponent(out Entity entity) && entity.isAlly != isAllyZone)
        {
            Trespassers.Remove(entity);
        }
    }
}