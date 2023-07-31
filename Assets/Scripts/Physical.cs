using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;

//physicallity, movement
public class Physical : MonoBehaviour
{
    const int GRAVITY_FORCE = -10;
    public const string GROUND_TAG = "Ground";

    [SerializeField] Transform orientaion;
    [SerializeField] bool canBeMoved;
    [SerializeField] float minSpeed = 1;
    [SerializeField] float drag = 0.02f;

    protected bool isGrounded = false;

    [SerializeField] protected Vector2 directionH;
    [SerializeField] protected float newVSpeed;
    [SerializeField] protected float newHSpeed;
    [SerializeField] protected Vector3 movementDir;
    [SerializeField] protected float movementSpeed;

    [SerializeField] bool hasCollision;
    [SerializeField] public float radius;
    [SerializeField] public Volume volume;


    private void Awake()
    {
        //add to list of physical objects
        if (hasCollision)
        {
            PhysObjectManager.PhysicalColliders.Add(this);
        }
    }

    protected virtual void FixedUpdate()
    {
        if (canBeMoved)
        {
            NewHSpeedUpdate();
            NewDrag();
        }
        NewPositionUpdate();
    }

    void NewHSpeedUpdate()
    {
        if (!isGrounded)
        {
            newVSpeed += GRAVITY_FORCE * Time.fixedDeltaTime;
        }
    }

    public void NewDrag()
    {
        if (newHSpeed > minSpeed)
        {
            newHSpeed -= drag;
        }
        else newHSpeed = 0;

        if (movementSpeed > minSpeed)
        {
            movementSpeed -= drag;
        }
        else movementSpeed = 0;
    }

    void NewPositionUpdate()
    {
        //horizontal movement
        Vector3 outsideForce = new Vector3(directionH.x, 0, directionH.y) * newHSpeed;
        Vector3 movement = movementDir * movementSpeed;
        Vector3 toMove = (outsideForce + movement) * Time.fixedDeltaTime + transform.position;
        
        if (!hasCollision || !CheckCollision(toMove))
        {
            transform.position = toMove;
            if (hasCollision) volume.ValidateBounds();
        }

        //vertical movement
        if (hasCollision && canBeMoved)
        {
            if (newVSpeed == 0)
            {
                //check if it has something to stand on
                if (!CheckGravity(transform.position - (((volume.size.y / 2) + 0.1f) * Vector3.up)))
                {
                    Debug.Log("nothing below");
                    isGrounded = false;
                }
            }
            else
            {
                //check if it lands
                Vector3 toMoveDown = newVSpeed * Time.fixedDeltaTime * Vector3.up + transform.position;
                if (CheckGravity(toMoveDown - (volume.size.y / 2 * Vector3.up)))
                {
                    Debug.Log("landed");
                    isGrounded = true;
                    newVSpeed = 0;
                }
                else
                {
                    transform.position = toMoveDown;
                    volume.ValidateBounds();
                }
            }
        }
    }

    public void NewApplyForce(Vector3 force)
    {
        Vector3 oldVector3 = new Vector3(directionH.x * newHSpeed, newVSpeed, directionH.y * newHSpeed);
        oldVector3 += force;
        if (oldVector3.y > 0)
            isGrounded = false;
        newVSpeed = oldVector3.y;
        Vector2 horizontal = new Vector2(oldVector3.x, oldVector3.z);
        directionH = horizontal.normalized;
        newHSpeed = horizontal.magnitude;
    }

    public void NewApplyForceFrom(Vector3 source, float power)
    {
        Vector3 force = transform.position - source;
        NewApplyForce(force.normalized * power);

    }

    bool CheckCollision(Vector3 position)
    {
        return volume.CheckIfObjectsInRange(position);
    }

    bool CheckGravity(Vector3 position)
    {
        return volume.CheckPointCollision(position);


    }
}
