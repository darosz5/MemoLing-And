using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetFlag1 : MonoBehaviour {

    public Image image;

    void Start()
    {
        image.sprite = FindObjectOfType<LangaugeFlags>().flag1.sprite;
    }

}
