using UnityEngine;

public class UIHandler : Singleton<UIHandler>
{
    [SerializeField] Canvas uiCanvas;
    [SerializeField] CanvasGroup selectionCG;

    //canvas referecne pixels per unit
    public float GetRefPPU()
    {
        return uiCanvas.referencePixelsPerUnit;
    }

    public void SetSelectionRectPosition(Vector3 pos)
    {
        selectionCG.transform.position = pos;
    }

    public void SetSelectionRectScale(Vector2 scale)
    {
        selectionCG.transform.localScale = scale;
    }

    public void SetSelectionCGAlpha(float value)
    {
        selectionCG.alpha = value;
    }
}
