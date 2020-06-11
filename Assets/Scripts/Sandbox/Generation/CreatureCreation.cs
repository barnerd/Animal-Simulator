using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureCreation : MonoBehaviour
{
    public int numCreatures;
    public Ground ground;

    public GameObject creaturePrefab;

    public AttributeType speed;

    public PlayerController playerController;
    public AIController aiController;

    public GameEvent onInventoryChange;
    public GameEvent onEquipmentChange;

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
        // set random position
        float x = Random.Range(0f, ground.size);
        float z = Random.Range(0f, ground.size);
        Vector3 position = new Vector3(x, ground.GetHeightAtXZ(x, z) + 1.1f, z);

        GameObject creature = Instantiate(creaturePrefab, position, Quaternion.identity, transform);
        creature.name = "Creature " + (num + 1);

        CreatureAttributes ca = creature.GetComponent<CreatureAttributes>();
        for (int i = 0; i < ca.attributes.Length; i++)
        {
            AttributeType currentType = ca.attributes[i].type;
            ca.attributes[i] = new Attribute(num + i + 5);
            ca.attributes[i].type = currentType;
        }

        for (int i = 0; i < ca.meters.Length; i++)
        {
            AttributeType currentType = ca.meters[i].type;
            ca.meters[i] = new MeteredAttribute(100);
            ca.meters[i].type = currentType;
        }

        if (num == 0)
        {
            var creatureC = creature.GetComponent<Creature>();
            creatureC.currentController = playerController;

            // set controller to be based on input
            var creatureController = (PlayerController)creatureC.currentController;
            creatureController.cam = Camera.main;
            creatureController.cam.GetComponent<CameraController>().target = creatureC.transform;

            // add events only to the player
            creatureC.GetComponent<InventoryManager>().onInventoryChange = onInventoryChange;
            creatureC.GetComponent<EquipmentManager>().onEquipmentChange = onEquipmentChange;
        }
        else
        {
            creature.GetComponent<Creature>().currentController = aiController;
        }

        return creature;
    }
}
