using UnityEngine;
using UnityEngine.EventSystems;

public class ToolTipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public ToolTip toolTip;

    public string header;
    [Multiline()]
    public string content;

    private static LTDescr delayTween;

    public void OnPointerEnter(PointerEventData eventData)
    {
        delayTween = LeanTween.delayedCall(toolTip.displayDelay.Value, () =>
       {
           toolTip.Show(content, header);
       });
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        LeanTween.cancel(delayTween.uniqueId);
        toolTip.Hide();
    }
}
