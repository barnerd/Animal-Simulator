using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Localization;

public class ToolTipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public ToolTip toolTip;

    public LocalizedString localizedHeader;
    public LocalizedString localizedContent;

    private static LTDescr delayTween;

    public void OnPointerEnter(PointerEventData eventData)
    {
        StartCoroutine("ShowToolTip");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        LeanTween.cancel(delayTween.uniqueId);
        toolTip.Hide();
    }

    IEnumerator ShowToolTip()
    {
        string header = "", content = "";

        // get localized content
        var localizedContentHandler = localizedContent.GetLocalizedString();

        // get localized header
        if (localizedHeader.TableReference != null)
        {
            // load string
            var localizedHeaderHandler = localizedHeader.GetLocalizedString();

            // wait for it to load
            yield return localizedHeaderHandler;

            // ready! retrieve it.
            header = localizedHeaderHandler.Result;
        }

        // wait for it to load
        yield return localizedContentHandler;

        // ready! retrieve it.
        content = localizedContentHandler.Result;

        // load the strings and show the tooltip
        delayTween = LeanTween.delayedCall(toolTip.displayDelay.Value, () =>
        {
            toolTip.Show(content, header);
        });

    }
}
