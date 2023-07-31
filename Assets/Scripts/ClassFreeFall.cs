using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class ClassFreeFall : MonoBehaviour
{
    public float verticalSpeed;
    public float horizontalSpeed;
    public float gravity;
    public Stopwatch elapsed = new Stopwatch();
    public float elapsedTime;

    bool isStarted = false;

    private void FixedUpdate()
    {
        if (isStarted)
        {
            transform.Translate(Vector3.forward * horizontalSpeed * Time.fixedDeltaTime);
            transform.Translate(Vector3.down * verticalSpeed * Time.fixedDeltaTime);
            verticalSpeed += gravity * Time.fixedDeltaTime;
        }
        elapsedTime = elapsed.ElapsedMilliseconds;
    }

    [ContextMenu("Launch")]
    void Launch()
    {
        isStarted = true;
        elapsed.Start();
    }

    [ContextMenu("Restart")]
    void Restart()
    {
        transform.position = new(0, 10, 0);
        verticalSpeed = 0;
        isStarted = false;
        elapsed.Reset();
    }
}
