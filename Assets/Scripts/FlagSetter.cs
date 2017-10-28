using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlagSetter : MonoBehaviour {

    public Sprite flag;

    public Image flagImage;

    public void setFlag()
    {
        flagImage.sprite = flag;
    }
}
