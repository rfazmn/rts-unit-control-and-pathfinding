using System.Collections.Generic;
using UnityEngine;

public class UnitController : MonoBehaviour
{
    [SerializeField] LayerMask groundLayer;

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = UnitSelection.Instance.cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 500f, groundLayer))
            {
                MoveUnitsToTarget(hit.point);
            }
        }
    }

    void MoveUnitsToTarget(Vector3 target)
    {
        List<Unit> selectedUnits = UnitSelection.Instance.GetSelectedUnits();
        for (int i = 0; i < selectedUnits.Count; i++)
        {
            selectedUnits[i].FollowPath(target);
        }
    }
}
