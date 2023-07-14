using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MeteredAttributeUI : MonoBehaviour
{
    public Image mask;
    public Image meter;
    public Image icon;

    private MeteredAttribute meteredAttribute;
    private MeteredAttributeType attributeType;

    public CreatureAttributes creature;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetMeteredAttribute(MeteredAttribute _attribute)
    {
        meteredAttribute = _attribute;
        attributeType = meteredAttribute.type as MeteredAttributeType;
        Sprite _icon = attributeType.icon;

        if (creature != null)
            SetPercent(creature);

        if (_icon == null)
        {
            icon.gameObject.SetActive(false);
        }
        else
        {
            icon.sprite = _icon;
        }
    }

    public void SetPercent(MonoBehaviour _actor)
    {
        if ((CreatureAttributes)_actor == creature)
        {
            // TODO: Check for outside of 0 and 1;
            float percent = meteredAttribute.CurrentPercent;

            mask.fillAmount = percent;
            meter.color = attributeType.gradientUI.Evaluate(percent);
        }
    }
}
