using UnityEngine;

public class ArrowPointer : MonoBehaviour
{
    Transform targetTransform;
    Transform ballTransform;
    void Start()
    {
        CellProperties[] cells = FindObjectsOfType<CellProperties>();
        foreach (CellProperties cell in cells)
        {
            if (cell.isEndPoint)
            {
                targetTransform = cell.transform;
                break;
            }
        }
        ballTransform = FindObjectOfType<DragNShoot>().transform;
    }

    void Update()
    {
        Vector2 aimingVector = targetTransform.position - ballTransform.position;
        float lookAngle = Mathf.Atan2(aimingVector.y, aimingVector.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, lookAngle);
    }
}
