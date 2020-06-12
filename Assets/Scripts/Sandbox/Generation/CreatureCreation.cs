using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureCreation : MonoBehaviour
{
    public int numCreatures;

    public Ground ground;

    public Camera playerCam;

    public GameObject creaturePrefab;

    [Header("Controllers")]
    public PlayerController playerController;
    public AIController aiController;

    [Header("Events")]
    public GameEvent onInventoryChange;
    public GameEvent onEquipmentChange;

    // Start is called before the first frame update
    void Start()
    {
        // set random position
        float x = Random.Range(0f, ground.size);
        float z = Random.Range(0f, ground.size);
        Vector3 position = new Vector3(x, ground.GetHeightAtXZ(x, z) + 1.1f, z);

        GameObject player = Creature.Create(creaturePrefab, position, transform);
        player.name = "Player";
        var creatureC = player.GetComponent<Creature>();
        creatureC.currentController = playerController;

        // set controller to be based on input
        var creatureController = (PlayerController)creatureC.currentController;
        creatureController.cam = playerCam;
        creatureController.cam.GetComponent<CameraController>().target = creatureC.transform;

        // add events only to the player
        creatureC.GetComponent<InventoryManager>().onInventoryChange = onInventoryChange;
        creatureC.GetComponent<EquipmentManager>().onEquipmentChange = onEquipmentChange;

        // add other creatures
        for (int i = 1; i < numCreatures; i++)
        {
            // set random position
            x = Random.Range(0f, ground.size);
            z = Random.Range(0f, ground.size);
            position = new Vector3(x, ground.GetHeightAtXZ(x, z) + 1.1f, z);

            Creature.Create(creaturePrefab, position, transform).name = "Creature " + i;
        }
    }
}
