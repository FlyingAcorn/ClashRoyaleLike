using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeRadius : MonoBehaviour
{ 
    public float spawnRadius;
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position,spawnRadius);
    }
}
