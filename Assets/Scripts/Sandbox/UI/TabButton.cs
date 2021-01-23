using UnityEngine;

public class TabButton : MonoBehaviour
{
    public TabGroup tabGroup;

    // Start is called before the first frame update
    void Start()
    {
        tabGroup.Subscribe(this);
    }
}
