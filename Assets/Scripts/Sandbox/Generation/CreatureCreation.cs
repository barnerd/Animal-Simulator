using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CreatureCreation : MonoBehaviour
{
    public int numCreatures;

    public Ground ground;

    public CinemachineFreeLook cinemachineCam;
    public Canvas hud;
    public InventoryUI inventoryUI;
    public EquipmentUI equipmentUI;

    public GameObject creaturePrefab;
    public CreatureData playerCreatureData;
    public CreatureData[] creatureDatas;

    public EquipmentData[] baseClothing;

    [Header("Controllers")]
    public PlayerController playerController;

    // Start is called before the first frame update
    void Start()
    {
        // set random position
        float x = Random.Range(0f, ground.size);
        float z = Random.Range(0f, ground.size);
        Vector3 position = new Vector3(x, ground.GetHeightAtXZ(x, z), z);

        Creature.Sex randomSex = (Creature.Sex)Random.Range(0, System.Enum.GetNames(typeof(Creature.Sex)).Length);

        GameObject player = Creature.Create(creaturePrefab, playerCreatureData, position, randomSex, transform);
        SetActivePlayer(player, playerController, cinemachineCam, hud, inventoryUI, equipmentUI);
        player.GetComponent<EquipmentManager>().baseClothing = baseClothing;

        // add other creatures
        for (int i = 1; i < numCreatures; i++)
        {
            // set random position
            x = Random.Range(0f, ground.size);
            z = Random.Range(0f, ground.size);
            position = new Vector3(x, ground.GetHeightAtXZ(x, z), z);

            randomSex = (Creature.Sex)Random.Range(0, System.Enum.GetNames(typeof(Creature.Sex)).Length);

            GameObject creature = Creature.Create(creaturePrefab, creatureDatas[Random.Range(0, creatureDatas.Length)], position, randomSex, transform);
            creature.name = "Creature " + i;
            if (creature.GetComponent<Creature>().creatureData == playerCreatureData)
            {
                creature.GetComponent<EquipmentManager>().baseClothing = baseClothing;
            }
        }
    }

    public static void SetActivePlayer(GameObject _activePlayer, InputController _controller, CinemachineFreeLook _cinemachineCam, Canvas _hud, InventoryUI _inventoryUI, EquipmentUI _equipmentUI)
    {
        _activePlayer.name = "Player";

        // Set Controller
        _activePlayer.GetComponent<Creature>().currentController = _controller;

        _cinemachineCam.Follow = _activePlayer.transform;
        _cinemachineCam.LookAt = _activePlayer.transform;
        // change this to the height of the creature
        _cinemachineCam.GetRig(0).GetCinemachineComponent<CinemachineComposer>().m_TrackedObjectOffset.y = 1.68f;
        _cinemachineCam.GetRig(1).GetCinemachineComponent<CinemachineComposer>().m_TrackedObjectOffset.y = 1.68f;
        _cinemachineCam.GetRig(2).GetCinemachineComponent<CinemachineComposer>().m_TrackedObjectOffset.y = 1.68f;

        // Set HUD
        MeteredAttributeUI[] meters = _hud.GetComponentsInChildren<MeteredAttributeUI>();

        CreatureAttributes creatureAttributes = _activePlayer.GetComponent<CreatureAttributes>();
        for (int i = 0; i < meters.Length; i++)
        {
            meters[i].creature = creatureAttributes;
            if (i == 0) creatureAttributes.healthBarUI = meters[i];
            if (i == 1) creatureAttributes.hungerBarUI = meters[i];
            if (i == 2) creatureAttributes.thirstBarUI = meters[i];
            //meters[i].SetPercent(creatureAttributes);
        }

        _hud.GetComponentInChildren<CompassUI>().target = _activePlayer.transform;
        _hud.GetComponentInChildren<ElevationUI>().player = _activePlayer.transform;

        // Turn off UI layer for player as it's been moved to the HUD.
        _activePlayer.GetComponentInChildren<Canvas>().gameObject.SetActive(false);

        _inventoryUI.inventory = _activePlayer.GetComponent<InventoryManager>();
        _equipmentUI.equipment = _activePlayer.GetComponent<EquipmentManager>();
    }
}
