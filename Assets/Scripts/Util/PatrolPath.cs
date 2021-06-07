using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolPath : MonoBehaviour
{
    void OnDrawGizmos()
    {

        Gizmos.color = Color.red;
        for (int i = 0; i < transform.childCount; i++)
        {
            int j = GetNextIndex(i);
            Gizmos.DrawSphere(GetWaypoint(i), 0.3f);
            Gizmos.DrawLine(GetWaypoint(i), GetWaypoint(j));

        }
    }

    public int GetNextIndex(int i)
    {
        if (i == transform.childCount - 1)
        {
            return i = 0;
        }
        return i + 1;

    }

    public Vector3 GetWaypoint(int i)
    {
        return transform.GetChild(i).position;
    }
    public int GetChildCount()
    {
        return transform.childCount;
    }
}