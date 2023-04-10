using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundsWrapper : MonoBehaviour
{
    [SerializeField] private ScreenWrapper screenBounds;

    private void Awake()
    {
        screenBounds.ExitTriggerFired.AddListener(TestColiderForWrapping);
    }
    public void TestColiderForWrapping(Collider2D collider)
    {
        if (collider.GetComponent<TriggerWrapper>())
        {
            Vector3 newPos = screenBounds.CalculateWrappedPosition(collider.transform.position);
            collider.gameObject.transform.position = newPos;    
        }
    }
}
