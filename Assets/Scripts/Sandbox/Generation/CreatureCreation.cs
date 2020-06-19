using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureCreation : MonoBehaviour
{
    public int numCreatures;

    public Ground ground;

    public Camera playerCam;
    public Canvas hud;
    public InventoryUI inventoryUI;

    public GameObject creaturePrefab;
    public CreatureData playerCreatureData;
    public CreatureData[] creatureDatas;

    [Header("Controllers")]
    public PlayerController playerController;

    // Start is called before the first frame update
    void Start()
    {
        // set random position
        float x = Random.Range(0f, ground.size);
        float z = Random.Range(0f, ground.size);
        Vector3 position = new Vector3(x, ground.GetHeightAtXZ(x, z), z);

        GameObject player = Creature.Create(creaturePrefab, playerCreatureData, position, transform);
        SetActivePlayer(player, playerController, playerCam, hud, inventoryUI);

        // add other creatures
        for (int i = 1; i < numCreatures; i++)
        {
            // set random position
            x = Random.Range(0f, ground.size);
            z = Random.Range(0f, ground.size);
            position = new Vector3(x, ground.GetHeightAtXZ(x, z), z);

            Creature.Create(creaturePrefab, creatureDatas[Random.Range(0, creatureDatas.Length)], position, transform).name = "Creature " + i;
        }
    }

    public static void SetActivePlayer(GameObject _activePlayer, InputController _controller, Camera _camera, Canvas _hud, InventoryUI _inventoryUI)
    {
        _activePlayer.name = "Player";

        // Set Controller
        _activePlayer.GetComponent<Creature>().currentController = _controller;

        var creatureController = (PlayerController)_activePlayer.GetComponent<Creature>().currentController;
        creatureController.cam = _camera;

        CameraController creatureCameraController = _camera.GetComponent<CameraController>();
        creatureCameraController.target = _activePlayer.transform;
        creatureCameraController.offset = _activePlayer.GetComponent<Creature>().creatureData.cameraOffset;
        creatureCameraController.lookAtOffset = _activePlayer.GetComponent<CharacterController>().height;

        // Set HUD
        MeteredAttributeUI[] meters = _hud.GetComponentsInChildren<MeteredAttributeUI>();

        CreatureAttributes creatureAttributes = _activePlayer.GetComponent<CreatureAttributes>();
        for (int i = 0; i < meters.Length; i++)
        {
            meters[i].creature = creatureAttributes;
            meters[i].SetPercent(creatureAttributes);
        }

        // Turn off UI layer for player as it's been moved to the HUD.
        _activePlayer.GetComponentInChildren<Canvas>().gameObject.SetActive(false);

        _inventoryUI.inventory = _activePlayer.GetComponent<InventoryManager>();
    }
}
