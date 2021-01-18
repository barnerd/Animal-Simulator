using UnityEngine;
using UnityEngine.UI;

public class ElevationUI : MonoBehaviour
{
    public Text elevationText;
    public Transform player;

    [Header("Display")]
    public int meterIncrement = 1;

    // Update is called once per frame
    void Update()
    {
        // TODO: move this to a coroutine and only check every ~1 second 
        elevationText.text = ((int)(player.position.y / meterIncrement) * meterIncrement).ToString("n0") + "m";
    }
}
