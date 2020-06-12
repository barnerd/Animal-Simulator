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

    [Header("Controllers")]
    public PlayerController playerController;

    // Start is called before the first frame update
    void Start()
    {
        // set random position
        float x = Random.Range(0f, ground.size);
        float z = Random.Range(0f, ground.size);
        Vector3 position = new Vector3(x, ground.GetHeightAtXZ(x, z) + 1.1f, z);

        GameObject player = Creature.Create(creaturePrefab, position, transform);
        SetActivePlayer(player, playerController, playerCam, hud, inventoryUI);

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

    public static void SetActivePlayer(GameObject _gameObject, InputController _controller, Camera _camera, Canvas _hud, InventoryUI _inventoryUI)
    {
        _gameObject.name = "Player";

        // Set Controller
        _gameObject.GetComponent<Creature>().currentController = _controller;

        var creatureController = (PlayerController)_gameObject.GetComponent<Creature>().currentController;
        creatureController.cam = _camera;
        _camera.GetComponent<CameraController>().target = _gameObject.transform;

        // Set HUD
        MeteredAttributeUI[] meters = _hud.GetComponentsInChildren<MeteredAttributeUI>();

        for (int i = 0; i < meters.Length; i++)
        {
            meters[i].creature = _gameObject.GetComponent<CreatureAttributes>();
            meters[i].SetPercent(_gameObject.GetComponent<CreatureAttributes>());
        }

        _gameObject.GetComponentInChildren<Canvas>().gameObject.SetActive(false);

        _inventoryUI.inventory = _gameObject.GetComponent<InventoryManager>();
    }
}
