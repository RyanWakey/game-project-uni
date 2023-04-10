using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider2D))]
public class ScreenWrapper : MonoBehaviour
{
    [SerializeField] private float teleportOffset = 0.2f;
    [SerializeField] private float cornerOffset = 1f;
    [SerializeField] private Camera mainCamera;

    //private PolygonCollider2D polygonCollider;
    BoxCollider2D boxCollider;
    private Transform tr;

    public UnityEvent<Collider2D> ExitTriggerFired;

    public void Awake()
    {
        mainCamera = Camera.main;
        this.mainCamera.transform.localScale = Vector3.one;
        boxCollider = GetComponent<BoxCollider2D>();
        boxCollider.isTrigger = true;
        tr = transform;
       
        
    }

    public void Start()
    {
        tr.position = Vector3.one;
        UpdateBoundsSize();
    }

    public void UpdateBoundsSize()
    {
        //Vector2 polygonColliderSize = new Vector2(mainCamera.orthographicSize * 2 * mainCamera.aspect, mainCamera.orthographicSize * 2);
        Vector2 boxColliderSize = new Vector2(mainCamera.orthographicSize * 2 * mainCamera.aspect, mainCamera.orthographicSize * 2);
        boxCollider.size = boxColliderSize;  
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        ExitTriggerFired?.Invoke(collision);
    }

    public bool isOOB(Vector3 worldPosition)
    {
        return Mathf.Abs(worldPosition.x) > Mathf.Abs(boxCollider.bounds.min.x) || Mathf.Abs(worldPosition.y) > Mathf.Abs(boxCollider.bounds.min.y);
    }

    public Vector2 CalculateWrappedPosition(Vector2 worldPosition)
    {
        bool xBoundResult = Mathf.Abs(worldPosition.x) > (Mathf.Abs(boxCollider.bounds.min.x) - cornerOffset);
        bool yBoundResult = Mathf.Abs(worldPosition.y) > (Mathf.Abs(boxCollider.bounds.min.y) - cornerOffset);
        Vector2 signWorldPosition = new Vector2(Mathf.Sign(worldPosition.x), Mathf.Sign(worldPosition.y));

        if (xBoundResult && yBoundResult)
        {
            return Vector2.Scale(worldPosition, Vector2.one * -1) +
                        Vector2.Scale(new Vector2(teleportOffset, teleportOffset), signWorldPosition);
        }
        else if (xBoundResult)
        {
            return new Vector2(worldPosition.x * -1, worldPosition.y) +
                       new Vector2(teleportOffset * signWorldPosition.x, teleportOffset);
        }
        else if (yBoundResult)
        {
            return new Vector2(worldPosition.x, worldPosition.y * -1) +
                       new Vector2(teleportOffset, teleportOffset * signWorldPosition.y);
        }
        else
        {
            return worldPosition;
        }
    }
    
    public void WrapAround(Transform tr)
    {
        Vector3 curPos = tr.position;
        if (isOOB(curPos))
        {
            tr.position = CalculateWrappedPosition(curPos);
        }
    }
}
