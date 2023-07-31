using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//basically collision and trigger
//by default, a box
public class Volume : MonoBehaviour
{
    [SerializeField] float radius;
    [SerializeField] public Vector3 size;
    [SerializeField] protected bool isTrigger;
    public Vector2 xBounds;
    public Vector2 yBounds;
    public Vector2 zBounds;

    private void Start()
    {
        ValidateBounds();
    }

    public void ValidateBounds()
    {
        xBounds = new(transform.position.x - size.x / 2, transform.position.x + size.x / 2);
        yBounds = new(transform.position.y - size.y / 2, transform.position.y + size.y / 2);
        zBounds = new(transform.position.z - size.z / 2, transform.position.z + size.z / 2);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireCube(transform.position, size);
    }

    public bool CheckPointCollision(Vector3 position)
    {
        foreach (var physical in PhysObjectManager.PhysicalColliders)
        {
            if (physical.volume == this) continue;
            if (physical.volume.CheckIfPointIsInRange(position))
                return true;
        }
        return false;
    }

    public bool CheckIfObjectsInRange()
    {
        foreach (var physical in PhysObjectManager.PhysicalColliders)
        {
            if (physical.volume == this) continue;
            if (CheckIfVolumeIsInRange(physical))
            {
                return true;
            }
        }
        return false;
    }

    public bool CheckIfObjectsInRange(Vector3 position)
    {
        foreach (var physical in PhysObjectManager.PhysicalColliders)
        {
            if (physical.volume == this)
            {
                continue;
            }
            if (CheckIfVolumeIsInRange(position, physical))
            {
                return true;
            }
        }
        return false;

    }

    public void ActionOnObjectsInRange(Action<Physical> power)
    {
        foreach (var obj in PhysObjectManager.PhysicalColliders)
        {
            if (obj.volume == this) continue;
            if (CheckIfVolumeIsInRange(obj))
            {
                power.Invoke(obj);
            }
        }
    }

    virtual protected bool CheckIfPointIsInRange(Vector3 otherPos)
    {
        Vector3 dVector = otherPos - transform.position;
        if (dVector.sqrMagnitude > radius * radius)
        {
            return false;
        }
        return true;
    }

    virtual protected bool CheckIfVolumeIsInRange(Physical obj)
    {
        Vector3 dVector = obj.transform.position - transform.position;
        float sumR = radius + obj.radius;
        if (dVector.sqrMagnitude > sumR * sumR)
        {
            return false;
        }
        return true;
    }

    virtual protected bool CheckIfVolumeIsInRange(Vector3 position, Physical obj)
    {
        Vector3 dVector = obj.transform.position - position;
        float sumR = radius + obj.radius;
        if (dVector.sqrMagnitude > sumR * sumR)
        {
            return false;
        }
        return true;
    }

}
