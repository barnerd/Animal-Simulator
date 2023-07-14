using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureAttributes : MonoBehaviour
{
    [SerializeField]
    private Creature creature;

    [field: Header("Attributes")]
    [field: SerializeField] public AttributeType SpeedType { get; private set; }
    [field: SerializeField] public AttributeType StrengthType { get; private set; }

    private Dictionary<AttributeType, Attribute> attributes;

    [field: Header("Armors/Damages")]
    [field: SerializeField] public AttributeType ArmorType { get; private set; }
    [field: SerializeField] public AttributeType DamageType { get; private set; }

    [field: Space(10)]
    [field: SerializeField] public DamageType SlashDamageType { get; private set; }
    [field: SerializeField] public DamageType PierceDamageType { get; private set; }
    [field: SerializeField] public DamageType BludgeoningDamageType { get; private set; }

    private Dictionary<DamageType, ArmorAttribute> armors;
    private Dictionary<DamageType, DamageAttribute> damages;

    [field: Header("Meters")]
    [field: SerializeField] public MeteredAttributeType HealthType { get; private set; }
    [field: SerializeField] public MeteredAttributeType HungerType { get; private set; }
    [field: SerializeField] public MeteredAttributeType ThirstType { get; private set; }

    private Dictionary<MeteredAttributeType, MeteredAttribute> meters;

    [field: Space(10)]
    public GameEvent onAttributeChangeEvent;
    public GameEvent onAttribute0Event;

    [field: Space(10)]
    public MeteredAttributeUI healthBarUI;
    public MeteredAttributeUI hungerBarUI;
    public MeteredAttributeUI thirstBarUI;

    void Awake()
    {
        attributes = new Dictionary<AttributeType, Attribute>();
        armors = new Dictionary<DamageType, ArmorAttribute>();
        damages = new Dictionary<DamageType, DamageAttribute>();
        meters = new Dictionary<MeteredAttributeType, MeteredAttribute>();
    }

    void Start()
    {
        InitNewAttribute(SpeedType, creature.creatureData.speedBase);
        InitNewAttribute(StrengthType, creature.creatureData.strengthBase);

        InitNewArmorAttribute(SlashDamageType, creature.creatureData.slashArmorBase);
        InitNewArmorAttribute(PierceDamageType, creature.creatureData.pierceArmorBase);
        InitNewArmorAttribute(BludgeoningDamageType, creature.creatureData.bludgeoningArmorBase);

        InitNewDamageAttribute(SlashDamageType, creature.creatureData.slashDamageBase);
        InitNewDamageAttribute(PierceDamageType, creature.creatureData.pierceDamageBase);
        InitNewDamageAttribute(BludgeoningDamageType, creature.creatureData.bludgeoningDamageBase);

        InitNewMeteredAttribute(HealthType, creature.creatureData.healthBase);
        InitNewMeteredAttribute(HungerType, creature.creatureData.hungerBase);
        InitNewMeteredAttribute(ThirstType, creature.creatureData.thirstBase);

        healthBarUI.SetMeteredAttribute(meters[HealthType]);
        hungerBarUI.SetMeteredAttribute(meters[HungerType]);
        thirstBarUI.SetMeteredAttribute(meters[ThirstType]);

        Invoke("IncreaseHunger", 7); // 3 weeks is 72 mins * 7 * 3 or 7 * 3 * 72 * 60 = 90720
        Invoke("IncreaseThirst", 1); // 3 days is 72 mins * 3 or 3 * 72 * 60 = 12960
    }

    // Update is called once per frame
    void Update()
    {
        // for testing
        if (Input.GetKeyUp(KeyCode.T))
        {
            TakeDamage(SlashDamageType, 5);
        }
    }

    private Attribute InitNewAttribute(AttributeType _type, float _base)
    {
        Attribute _meter = new Attribute(_base);
        _meter.type = _type;

        return _meter;
    }

    private void InitNewArmorAttribute(DamageType _type, float _base)
    {
        ArmorAttribute _attribute = new ArmorAttribute(_base);
        _attribute.type = ArmorType;
        _attribute.damageType = _type;

        armors.Add(_type, _attribute);
    }

    private void InitNewDamageAttribute(DamageType _type, float _base)
    {
        DamageAttribute _attribute = new DamageAttribute(_base);
        _attribute.type = DamageType;
        _attribute.damageType = _type;

        damages.Add(_type, _attribute);
    }

    private void InitNewMeteredAttribute(MeteredAttributeType _type, float _base)
    {
        MeteredAttribute _meter = new MeteredAttribute(_base);
        _meter.type = _type;
        _meter.onMeteredAttributeChange = onAttributeChangeEvent;
        _meter.onMeteredAttribute0 = onAttribute0Event;

        meters.Add(_type, _meter);
    }

    public float GetHungerPercent()
    {
        return meters[HungerType].CurrentPercent;
    }

    public float GetHungerCurrentValue()
    {
        return meters[HungerType].currentValue;
    }

    public void ChangeHunger(float _delta)
    {
        meters[HungerType].ChangeMeter(_delta, this);
    }

    public float GetThirstPercent()
    {
        return meters[ThirstType].CurrentPercent;
    }

    public float GetThirstCurrentValue()
    {
        return meters[ThirstType].currentValue;
    }

    public void IncreaseHunger()
    {
        meters[HungerType].ChangeMeter(-1, this);

        // TODO: check thresholds for conditions/death

        Invoke("IncreaseHunger", 90.72f / 100f); // 100 units in 3 weeks is 72 mins * 7 * 3 or 7 * 3 * 72 * 60 = 90720 / 100 = 907.2f
    }

    public void IncreaseThirst()
    {
        meters[ThirstType].ChangeMeter(-1, this);

        // TODO: check thresholds for conditions/death

        Invoke("IncreaseThirst", 12.96f); // 100 units in 3 days is 72 mins * 3 or 3 * 72 * 60 = 12960 / 100 = 129.6f
    }

    public void TakeDamage(DamageType _type, float _damage)
    {
        float _delta = _damage;

        if (armors.ContainsKey(_type))
        {
            _delta -= armors[_type].currentValue;
        }

        if (_delta < 0)
            _delta = 0;

        meters[HealthType].ChangeMeter(-_delta, this);

        //Debug.Log(name + " takes " + _delta + " damage of type " + _type + ".");
    }

    // TODO: add other attributes into this, like strength and agility
    /// <summary>
    /// Sums up all damage types.
    /// </summary>
    /// <returns>The smallest total damage possible</returns>
    public float GetMinTotalDamage()
    {
        float damage = 0;

        foreach (var damageType in damages)
        {
            damage += damages[damageType.Key].GetMinDamage();
        }

        return damage;
    }

    /// <summary>
    /// Sums up all damage types.
    /// </summary>
    /// <returns>The highest total damage possible</returns>
    public float GetMaxTotalDamage()
    {
        float damage = 0;

        foreach (var damageType in damages)
        {
            damage += damages[damageType.Key].GetMaxDamage();
        }

        return damage;
    }

    /// <summary>
    /// Sums up all armor types.
    /// </summary>
    /// <returns>The total armor</returns>
    public float GetTotalArmor()
    {
        float armor = 0;

        foreach (var damageType in armors)
        {
            armor += armors[damageType.Key].currentValue;
        }

        return armor;
    }

    public void AddAttributeModifier(AttributeModifier _modifier, AttributeType _type, bool _add = true)
    {
        if (attributes.ContainsKey(_type) && _modifier != null)
        {
            if (_add)
            {
                attributes[_type].AddModifier(_modifier);
            }
            else
            {
                attributes[_type].RemoveModifier(_modifier);
            }
        }
    }

    public void AddArmorModifier(ArmorModifier _modifier, DamageType _type, bool _add = true)
    {
        if (armors.ContainsKey(_type) && _modifier != null)
        {
            if (_add)
            {
                armors[_type].AddModifier(_modifier);
            }
            else
            {
                armors[_type].RemoveModifier(_modifier);
            }
        }
    }

    public void AddDamageModifier(DamageModifier _modifier, DamageType _type, bool _add = true)
    {
        if (damages.ContainsKey(_type) && _modifier != null)
        {
            if (_add)
            {
                damages[_type].AddModifier(_modifier);
            }
            else
            {
                damages[_type].RemoveModifier(_modifier);
            }
        }
    }
}
