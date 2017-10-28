using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetFlag2 : MonoBehaviour {

    public Image image;

    private void Start()
    {
        image.sprite = FindObjectOfType<LangaugeFlags>().flag2.sprite;
    }
}
