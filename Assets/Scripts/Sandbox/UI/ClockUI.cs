using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClockUI : MonoBehaviour
{
    public Text clockText;
    public GameTime gameTime;

    [Header("Display")]
    public int minuteIncrement = 1;
    public bool displaySeconds;

    // Start is called before the first frame update
    void Start()
    {
        clockText.text = gameTime.GetTimeOfDay().GetString(minuteIncrement, displaySeconds);
    }

    // Update is called once per frame
    void Update()
    {
        // TODO: move this to a coroutine and only check every ~1 second 
        clockText.text = gameTime.GetTimeOfDay().GetString(minuteIncrement, displaySeconds);
    }
}
