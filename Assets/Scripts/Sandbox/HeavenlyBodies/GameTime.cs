using UnityEngine;

public class GameTime : MonoBehaviour
{
    public HeavenlyBody planet;

    public const float GameSecondsPerRealSeconds = 20f * 10f; // 1 game day is 72 mins

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public GameTimeOfDay GetTimeOfDay()
    {
        return new GameTimeOfDay(GetTimeOfDayPercent());
    }

    public float GetTimeOfDayPercent()
    {
        return planet.angleRotation % (2 * Mathf.PI) / (2 * Mathf.PI);
    }

    /*
     * want to be able to get:
     * local time of day (HH:MM:SS.ssss)
     * calendar day / month / year (dd/mm/yyyy)
     */
}

public class GameTimeOfDay
{
    public const int SecondsPerMinute = 60;
    public const int MinutesPerHour = 60;
    public const int HoursPerDay = 24;
    public const int SecondsPerDay = SecondsPerMinute * MinutesPerHour * HoursPerDay;

    public int hours;
    public int minutes;
    public float seconds;
    public float totalSeconds;

    public GameTimeOfDay(float _percent)
    {
        totalSeconds = _percent * SecondsPerDay;

        hours = Mathf.FloorToInt(_percent * HoursPerDay);
        _percent = _percent * HoursPerDay - hours;
        minutes = Mathf.FloorToInt(_percent * MinutesPerHour);
        _percent = _percent * MinutesPerHour - minutes;
        seconds = _percent * SecondsPerMinute;
    }

    public string GetString(int minuteIncrement = 1, bool displaySeconds = true)
    {
        string timeToString = "";

        // add hours
        timeToString += hours.ToString("D2");

        // add minutes
        timeToString += ":";
        minuteIncrement = Mathf.Clamp(minuteIncrement, 1, MinutesPerHour);
        timeToString += (Mathf.RoundToInt(minutes / minuteIncrement) * minuteIncrement).ToString("D2");

        if (displaySeconds)
        {
            // add seconds
            timeToString += ":";
            timeToString += Mathf.FloorToInt(seconds).ToString("D2");
        }

        return timeToString;
    }
}
