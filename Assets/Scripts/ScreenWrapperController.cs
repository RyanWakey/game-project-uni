using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
// Most of the code which deals with the screen wrapping is from this tutorial - https://www.youtube.com/watch?v=1a9ag16PeFw
// which i slightly changed for my context.
[RequireComponent(typeof(BoxCollider2D))]
public class ScreenWrapperController : MonoBehaviour
{
        [SerializeField] private Camera mainCamera;
        [SerializeField] private BoxCollider2D boxCollider;

        public UnityEvent<Collider2D> ExitTriggerFired;

        [SerializeField]
        private float teleportOffset = 15f;

        [SerializeField]
        private float cornerOffset = 1;

        private void Awake()
        {
            this.mainCamera.transform.localScale = Vector3.one;
            boxCollider = GetComponent<BoxCollider2D>();
            boxCollider.isTrigger = true;
        }

        private void Start()
        {
            transform.position = Vector3.zero;
            UpdateBoundsSize();
        }

        public void UpdateBoundsSize()
        {
            Vector2 boxColliderSize = new Vector2(mainCamera.orthographicSize * 2 * mainCamera.aspect, mainCamera.orthographicSize * 2);
            boxCollider.size = boxColliderSize;
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            ExitTriggerFired?.Invoke(collision);
        }

        public bool isOOB(Vector3 worldPosition)
        {
            return
                Mathf.Abs(worldPosition.x) > Mathf.Abs(boxCollider.bounds.min.x) ||
                Mathf.Abs(worldPosition.y) > Mathf.Abs(boxCollider.bounds.min.y);
        }

        public Vector2 CalculateWrappedPosition(Vector2 worldPosition)
        {
            bool xBoundResult = Mathf.Abs(worldPosition.x) > (Mathf.Abs(boxCollider.bounds.min.x) - cornerOffset);
            bool yBoundResult = Mathf.Abs(worldPosition.y) > (Mathf.Abs(boxCollider.bounds.min.y) - cornerOffset);

            Vector2 signWorldPosition = new Vector2(Mathf.Sign(worldPosition.x), Mathf.Sign(worldPosition.y));

            if (xBoundResult && yBoundResult)
            {
                return Vector2.Scale(worldPosition, Vector2.one * -1) + Vector2.Scale(new Vector2(teleportOffset, teleportOffset), signWorldPosition);
            }
            else if (xBoundResult)
            {
                return new Vector2(worldPosition.x * -1, worldPosition.y) + new Vector2(teleportOffset * signWorldPosition.x, teleportOffset);
            }
            else if (yBoundResult)
            {
                return new Vector2(worldPosition.x, worldPosition.y * -1) + new Vector2(teleportOffset, teleportOffset * signWorldPosition.y);
            }
            else
            {
            return worldPosition;
            }
        }
        public void WrapAround(Transform tr, Rigidbody2D rb2d)
        {
            Vector3 curPos = tr.position;
            if (isOOB(curPos))
            {
               float dotProductX = Mathf.Sign(curPos.x) * rb2d.velocity.x;
               float dotProductY = Mathf.Sign(curPos.y) * rb2d.velocity.y;
               
                if(dotProductX > 0 || dotProductY > 0)
                {
                tr.position = CalculateWrappedPosition(curPos);
                } 
             }
        }
}