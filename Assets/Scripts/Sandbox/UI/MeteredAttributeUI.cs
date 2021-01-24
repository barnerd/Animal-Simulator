using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MeteredAttributeUI : MonoBehaviour
{
    public Image mask;
    public Image meter;
    public Image icon;

    public ToolTipTrigger toolTip;

    public MeteredAttributeType meterType;

    public CreatureAttributes creature;

    // Start is called before the first frame update
    void Start()
    {
        if(creature != null)
            SetPercent(creature);

        if(meterType.icon == null)
        {
            icon.gameObject.SetActive(false);
        }
        else
        {
            icon.sprite = meterType.icon;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetPercent(MonoBehaviour _actor)
    {
        if ((CreatureAttributes)_actor == creature)
        {
            // TODO: Check for outside of 0 and 1;
            float percent = Mathf.Clamp01(creature.GetAttributeCurrentPercent(meterType) ?? 1);
            mask.fillAmount = percent;
            meter.color = meterType.gradientUI.Evaluate(percent);

            // TODO: Bug fix: tooltip content does update if tooltip is active when the text updates
            toolTip.content = meterType.name + ": " + (percent * 100) + "%";
        }
    }
}
