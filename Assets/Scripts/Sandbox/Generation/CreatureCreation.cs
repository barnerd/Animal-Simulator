using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureCreation : MonoBehaviour
{
    public int numCreatures;
    public Ground ground;

    public PlayerController playerController;
    public AIController aiController;

    public GameEvent onInventoryChanged;

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

        creature.AddComponent<Creature>();
        creature.GetComponent<Creature>().currentController = playerController;

        creature.AddComponent<Interactable>();

        creature.AddComponent<CreatureMotor>();
        var inv = creature.AddComponent<Inventory>();

        if(num == 0)
        {
            var creatureC = creature.GetComponent<Creature>();
            creatureC.currentController = playerController;

            var creatureController = (PlayerController)creatureC.currentController;
            inv.onInventoryChanged = onInventoryChanged;

            creatureController.cam = Camera.main;
            creatureController.cam.GetComponent<CameraController>().target = creature.transform;
        }
        else
        {
            creature.GetComponent<Creature>().currentController = aiController;
        }

        return creature;
    }
}
