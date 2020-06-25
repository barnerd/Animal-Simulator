using UnityEngine;
using UnityEngine.UI;

public class CompassUI : MonoBehaviour
{
    public Transform target;
    public RawImage compassImage;
    public RawImage compassOrdinalImage;
    public RawImage compassIntercardinalImage;

    // Update is called once per frame
    void Update()
    {
        float lookAngle = target.localEulerAngles.y;

        SetUVRect(compassImage, lookAngle);
        SetUVRect(compassOrdinalImage, lookAngle);
        SetUVRect(compassIntercardinalImage, lookAngle);
    }

    void SetUVRect(RawImage _image, float _angle)
    {
        _image.uvRect = new Rect(_angle / 360f + _image.uvRect.width / 2, _image.uvRect.y, _image.uvRect.width, _image.uvRect.height);
    }
}
