using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ToolTip : MonoBehaviour
{
    public TextMeshProUGUI headerField;
    public TextMeshProUGUI contentField;

    public LayoutElement layoutElement;

    public int characterWrapLimit;

    public RectTransform rectTransform;

    public FloatReference displayDelay;

    public void SetText(string _content, string _header = "")
    {
        headerField.gameObject.SetActive(!string.IsNullOrEmpty(_header));
        headerField.text = _header;

        contentField.text = _content;

        // enable/disable layoutElement if the content is long enough
        layoutElement.enabled = headerField.text.Length > characterWrapLimit || contentField.text.Length > characterWrapLimit;
    }

    public void SetPosition()
    {
        Vector2 position = Input.mousePosition;

        float pivotX = position.x / Screen.width;
        float pivotY = position.y / Screen.height;

        rectTransform.pivot = new Vector2(pivotX, pivotY);
        transform.position = position;
    }

    public void Show(string _content, string _header = "")
    {
        SetText(_content, _header);
        SetPosition();
        this.gameObject.SetActive(true);
    }

    public void Hide()
    {
        this.gameObject.SetActive(false);
    }
}
