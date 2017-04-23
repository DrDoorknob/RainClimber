using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;
using System;

public class RCButton : MonoBehaviour, ISelectHandler, IDeselectHandler, IPointerEnterHandler, IPointerExitHandler
{
    Sprite defaultSourceImage;
    public Sprite selectSourceImage;
    Image imgRender;

    // Use this for initialization
    void Start()
    {
        imgRender = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnSelect(BaseEventData eventData)
    {
        imgRender.overrideSprite = selectSourceImage;
    }

    public void OnDeselect(BaseEventData eventData)
    {
        imgRender.overrideSprite = null;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        imgRender.overrideSprite = selectSourceImage;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        imgRender.overrideSprite = null;
    }
}
