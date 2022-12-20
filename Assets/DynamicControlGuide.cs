using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class InputGuideHandler
{
    public string AxisName;
    public Image component;
    public Sprite defSprite;
    public Sprite pressSprite;
}
[System.Serializable]
public class StickInputGuideHandler
{
    public string HorizontalAxisName;
    public string VerticalAxisName;
    public Vector2 defaultPos { get; set; }
    public Transform Transform;
}
public class DynamicControlGuide : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] List<InputGuideHandler> inputGuides;
    [SerializeField] List<StickInputGuideHandler> StickGuides;
    [SerializeField] float StickGuideRange = 10.0f;
    void Start()
    {
        foreach (var item in inputGuides)
        {
            item.component.sprite = item.defSprite;
        }
        foreach (var item in StickGuides)
        {
            item.defaultPos = item.Transform.position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        foreach (var item in inputGuides)
        {
            if (Input.GetAxis(item.AxisName) >= 0.5)
            {
                if (item.component.sprite != item.pressSprite) item.component.sprite = item.pressSprite;
            }
            else
            {
                if (item.component.sprite != item.defSprite) item.component.sprite = item.defSprite;
            }
        }
        foreach (var item in StickGuides)
        {
            Vector2 vec = new Vector2(Input.GetAxis(item.HorizontalAxisName), Input.GetAxis(item.VerticalAxisName));
            item.Transform.position = item.defaultPos + vec * StickGuideRange;
        }
    }
}
