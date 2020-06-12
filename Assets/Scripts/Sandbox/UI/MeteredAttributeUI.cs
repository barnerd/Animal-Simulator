using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class MeteredAttributeUI : MonoBehaviour
{
    public Slider slider;
    public Image fill;

    public MeteredAttributeType meterType;

    public CreatureAttributes creature;

    // Start is called before the first frame update
    void Start()
    {
        if(creature != null)
            SetPercent(creature);
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
            float percent = Mathf.Clamp01((creature.GetAttributeCurrentPercent(meterType) ?? 1));
            slider.value = percent;
            fill.color = meterType.gradientUI.Evaluate(percent);
            Debug.Log("Changing UI for " + creature + " " + meterType + " bar to " + percent * 100 + "%.");
        }
    }
}
