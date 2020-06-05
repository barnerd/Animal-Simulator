using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureCreation : MonoBehaviour
{
    public int numCreatures;
    public Ground ground;

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

    private void CreateCreature(int num)
    {
        string name = "Creature " + (num + 1);
        GameObject creature = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        creature.name = name;

        creature.transform.parent = this.transform;

        // set random position
        float x = Random.Range(0f, ground.size);
        float z = Random.Range(0f, ground.size);
        creature.transform.position = new Vector3(x, ground.GetHeightAtXZ(x, z) + 1.1f, z);

        Rigidbody creatureRB = creature.AddComponent<Rigidbody>();
        creatureRB.collisionDetectionMode = CollisionDetectionMode.Continuous;
        creatureRB.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
    }
}
