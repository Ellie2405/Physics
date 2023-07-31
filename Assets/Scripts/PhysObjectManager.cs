using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysObjectManager : MonoBehaviour
{
    static public List<Physical> PhysicalColliders = new List<Physical>();

    private void Start()
    {
        Debug.Log(PhysicalColliders.Count);
    }

}
