using System.Collections.Generic;
using UnityEngine;

public class UnitSelection : Singleton<UnitSelection>
{
    public Camera cam;
    [SerializeField] LayerMask unitLayer;
    [SerializeField] KeyCode multiSelectKey;
    [SerializeField] float selectionThreshold = 20f;
    List<Unit> units = new List<Unit>();
    List<Unit> selectedUnits = new List<Unit>();
    Vector3 startPos;
    Vector3 endPos;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            startPos = Input.mousePosition;
            endPos = Input.mousePosition;
            UIHandler.Instance.SetSelectionRectPosition(startPos);
            UIHandler.Instance.SetSelectionCGAlpha(1f);
        }

        if (Input.GetMouseButton(0))
        {
            endPos = Input.mousePosition;
            Vector3 diff = endPos - startPos;
            diff /= UIHandler.Instance.GetRefPPU();
            UIHandler.Instance.SetSelectionRectScale(new Vector2(diff.x, -diff.y));
        }

        if (Input.GetMouseButtonUp(0))
        {
            endPos = Input.mousePosition;
            UIHandler.Instance.SetSelectionCGAlpha(0f);
            SelectUnit(Input.GetKey(multiSelectKey));
        }
    }

    public void AddUnitToList(Unit unit)
    {
        units.Add(unit);
    }

    void SelectUnit(bool multiSelectKeyPressed)
    {
        //unit selection with ui rect
        if (startPos != endPos && (endPos - startPos).magnitude > selectionThreshold)
        {
            if (!multiSelectKeyPressed)
                ClearSelectedUnitList();

            Rect rect = CalculateSelectionRect(startPos, endPos);
            for (int i = 0; i < units.Count; i++)
            {
                if (rect.Contains(cam.WorldToScreenPoint(units[i].transform.position)))
                {
                    SelectUnitWithRect(units[i]);
                }
            }
        }
        else
        {
            //raycasting
            Ray ray = cam.ScreenPointToRay(endPos);
            Physics.Raycast(ray, out RaycastHit hit, 500f, unitLayer);

            if (multiSelectKeyPressed)
            {
                //Multiple select/deselect with key
                if (hit.transform == null)
                    return;

                CheckUnitSelected(hit.transform.GetComponent<Unit>());
            }
            else
            {
                //single unit selection
                ClearSelectedUnitList();

                if (hit.transform != null)
                {
                    SelectUnit(hit.transform.GetComponent<Unit>());
                }
            }
        }
    }


    void CheckUnitSelected(Unit unit)
    {
        if (!selectedUnits.Contains(unit))
            SelectUnit(unit);
        else
            DeselectUnit(unit);
    }

    void SelectUnit(Unit unit)
    {
        unit.SetSelectionCanvas(true);
        AddUnit(unit);
    }

    void SelectUnitWithRect(Unit unit)
    {
        if (selectedUnits.Contains(unit))
            return;

        unit.SetSelectionCanvas(true);
        AddUnit(unit);
    }

    void DeselectUnit(Unit unit)
    {
        unit.SetSelectionCanvas(false);
        RemoveUnit(unit);
    }

    void AddUnit(Unit unit)
    {
        selectedUnits.Add(unit);
    }

    void RemoveUnit(Unit unit)
    {
        selectedUnits.Remove(unit);
    }

    void ClearSelectedUnitList()
    {
        for (int i = 0; i < selectedUnits.Count; i++)
        {
            selectedUnits[i].SetSelectionCanvas(false);
        }

        selectedUnits.Clear();
    }

    #region Util
    Rect CalculateSelectionRect(Vector3 startPos, Vector3 endPos)
    {
        Rect rect = new Rect();

        rect.xMin = Mathf.Min(startPos.x, endPos.x);
        rect.xMax = Mathf.Max(startPos.x, endPos.x);

        rect.yMin = Mathf.Min(startPos.y, endPos.y);
        rect.yMax = Mathf.Max(startPos.y, endPos.y);

        return rect;
    }
    #endregion

    public List<Unit> GetSelectedUnits()
    {
        return selectedUnits;
    }
}
