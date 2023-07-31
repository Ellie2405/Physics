using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxVolume : Volume
{
    protected override bool CheckIfVolumeIsInRange(Physical obj)
    {
        //vector from other object pointing to this
        Vector3 dPosition = transform.position - obj.transform.position;
        //get the size of the other volume
        Vector2 ox = obj.volume.xBounds;
        Vector2 oy = obj.volume.yBounds;
        Vector2 oz = obj.volume.zBounds;
        //by the pointing vector, find which corner is closest to this
        Vector3 nearestVertex = new(dPosition.x <= 0 ? ox.x : ox.y,
                                    dPosition.y <= 0 ? oy.x : oy.y,
                                    dPosition.z <= 0 ? oz.x : oz.y);
        //check collision with that corner
        return CheckIfPointIsInRange(nearestVertex);
    }

    protected override bool CheckIfVolumeIsInRange(Vector3 position, Physical obj)
    {
        //vector from other object pointing to this
        Vector3 dPosition = position - obj.transform.position;
        //get the size of the other volume
        Vector2 ox = obj.volume.xBounds;
        Vector2 oy = obj.volume.yBounds;
        Vector2 oz = obj.volume.zBounds;
        //by the pointing vector, find which corner is closest to this
        Vector3 nearestVertex = new(dPosition.x <= 0 ? ox.x : ox.y,
                                    dPosition.y <= 0 ? oy.x : oy.y,
                                    dPosition.z <= 0 ? oz.x : oz.y);
        //check collision with that corner
        return CheckIfPointIsInRange(nearestVertex);
    }

    protected override bool CheckIfPointIsInRange(Vector3 otherPos)
    {
        if (WithinBounds(otherPos.x, xBounds) &&
            WithinBounds(otherPos.y, yBounds) &&
            WithinBounds(otherPos.z, zBounds))
        {
            //Debug.Log($"collision {gameObject} {otherPos}");
            return true;
        }
        //Debug.Log($"{otherPos} not within{xBounds} {yBounds} {zBounds}");
        return false;
    }

    bool WithinBounds(float num, Vector2 bounds)
    {
        if (num >= bounds.x && num <= bounds.y)
        {
            return true;
        }
        return false;
    }
}
