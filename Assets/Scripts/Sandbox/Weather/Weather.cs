using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Weather : MonoBehaviour
{
    public enum Season
    {
        spring,
        summer,
        autumn,
        winter
    }

    public Ground ground;
    private Season currentSeason;
    public Season CurrentSeason { get { return currentSeason; } }

    // Start is called before the first frame update
    void Start()
    {
        currentSeason = Season.spring;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public Season GetWeather()
    {
        return currentSeason;
    }

    public void DropdownSelector(TMP_Dropdown dropdown)
    {
        SetSeason((Season)dropdown.value);
    }

    public void SetSeason(Season _season)
    {
        currentSeason = _season;
    }
}
