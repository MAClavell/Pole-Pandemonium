using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizontalTextScroll : MonoBehaviour
{
    [SerializeField]
    private RectTransform text;
    [SerializeField]
    private float scrollSpeed;

    private RectTransform text2;
    private Vector3 textPos;
    private Vector3 text2Pos;
    private float textWidth;
    private float rectWidth;

    // Start is called before the first frame update
    void Start()
    {
        rectWidth = GetComponent<RectTransform>().rect.width;
     
        text.localPosition = new Vector3(rectWidth / 2, text.localPosition.y, text.localPosition.z);
        textPos = text.localPosition;
        textWidth = text.rect.width;
     
        text2 = Instantiate(text.gameObject, text.parent).GetComponent<RectTransform>();
        text2.localPosition = new Vector3(textPos.x + textWidth, textPos.y, textPos.z);
        text2Pos = text2.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        //Scroll the text objects and wrap them when offscreen

        textPos.x += -scrollSpeed * Time.deltaTime;
        if (textPos.x + textWidth < -rectWidth / 2)
        {
            textPos.x = text2.localPosition.x + textWidth;
        }
        text.localPosition = textPos;

        text2Pos.x += -scrollSpeed * Time.deltaTime;
        if (text2Pos.x + textWidth < -rectWidth / 2)
        {
            text2Pos.x = text.localPosition.x + textWidth;
        }
        text2.localPosition = text2Pos;
    }
}
