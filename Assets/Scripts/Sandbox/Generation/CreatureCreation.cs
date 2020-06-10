using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureCreation : MonoBehaviour
{
    public int numCreatures;
    public Ground ground;

    public PlayerController playerController;
    public AIController aiController;

    public GameEvent onInventoryChange;
    public GameEvent onEquipmentChange;

    public AttributeType speed;
    public AttributeType[] attributeTypes;
    public DamageType[] damageTypes;
    public AttributeType armor;
    public AttributeType damage;
    public AttributeType[] meteredAttributeTypes;
    public EquipmentSlot[] equipmentSlots;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < numCreatures; i++)
        {
            CreateCreature(i);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    private GameObject CreateCreature(int num)
    {
        string name = "Creature " + (num + 1);
        GameObject creature = new GameObject(name);
        creature.transform.parent = this.transform;

        // add body
        GameObject body = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        body.name = "GFX";
        body.transform.parent = creature.transform;

        // add "hat brim"
        GameObject hat = GameObject.CreatePrimitive(PrimitiveType.Cube);
        hat.transform.parent = body.transform;
        hat.transform.position = new Vector3(0f, .5f, 0.4f);
        hat.transform.localScale = new Vector3(.4f, .2f, .7f);

        // set random position
        float x = Random.Range(0f, ground.size);
        float z = Random.Range(0f, ground.size);
        creature.transform.position = new Vector3(x, ground.GetHeightAtXZ(x, z) + 1.1f, z);

        /*Rigidbody creatureRB = creature.AddComponent<Rigidbody>();
        creatureRB.collisionDetectionMode = CollisionDetectionMode.Continuous;
        creatureRB.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;*/

        Creature c = creature.AddComponent<Creature>();
        c.attributes = new Attribute[attributeTypes.Length];
        for (int i = 0; i < attributeTypes.Length; i++)
        {
            c.attributes[i] = new Attribute(num + i + 5);
            c.attributes[i].type = attributeTypes[i];
        }

        c.armors = new ArmorAttribute[damageTypes.Length];
        c.damages = new DamageAttribute[damageTypes.Length];
        for (int i = 0; i < damageTypes.Length; i++)
        {
            c.armors[i] = new ArmorAttribute(0);
            c.armors[i].damageType = damageTypes[i];
            c.armors[i].type = armor;

            c.damages[i] = new DamageAttribute(0);
            c.damages[i].damageType = damageTypes[i];
            c.damages[i].type = damage;
        }

        c.meters = new MeteredAttribute[meteredAttributeTypes.Length];
        for (int i = 0; i < meteredAttributeTypes.Length; i++)
        {
            c.meters[i] = new MeteredAttribute(100);
            c.meters[i].type = meteredAttributeTypes[i];
        }

        creature.AddComponent<Interactable>();

        CreatureMotor creatureMotor = creature.AddComponent<CreatureMotor>();
        creatureMotor.speed = speed;
        InventoryManager inv = creature.AddComponent<InventoryManager>();
        EquipmentManager equipMan = creature.AddComponent<EquipmentManager>();
        equipMan.slots = equipmentSlots;

        if (num == 0)
        {
            var creatureC = creature.GetComponent<Creature>();
            creatureC.currentController = playerController;

            // set controller to be based on input
            var creatureController = (PlayerController)creatureC.currentController;
            creatureController.cam = Camera.main;
            creatureController.cam.GetComponent<CameraController>().target = creature.transform;

            // add events only to the player
            inv.onInventoryChange = onInventoryChange;
            equipMan.onEquipmentChange = onEquipmentChange;
        }
        else
        {
            creature.GetComponent<Creature>().currentController = aiController;
        }

        return creature;
    }
}
