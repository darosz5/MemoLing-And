using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LangaugeFlags : MonoBehaviour {

    [Header("References")]

    public Sprite[] FlagSprites;

    public Image flag1;

    public Image flag2;

    void OnEnable()
    {
        SetFlags();
    }

    public void SetFlags()
    {
        if(!string.IsNullOrEmpty(GameControl.control.firstLanguage))
        {
            switch (GameControl.control.firstLanguage)
            {
                case "english":
                    flag1.sprite = FlagSprites[0];
                    break;
                case "german":
                    flag1.sprite = FlagSprites[1];
                    break;
                case "spanish":
                    flag1.sprite = FlagSprites[2];
                    break;
                case "french":
                    flag1.sprite = FlagSprites[3];
                    break;
                case "polish":
                    flag1.sprite = FlagSprites[4];
                    break;
                default:
                    flag1.sprite = FlagSprites[0];
                    break;
            }
        }
        else
        {
            flag1.sprite = FlagSprites[0];
        }

        if (!string.IsNullOrEmpty(GameControl.control.secondLanguage))
        {
            switch (GameControl.control.secondLanguage)
            {
                case "english":
                    flag2.sprite = FlagSprites[0];
                    break;
                case "german":
                    flag2.sprite = FlagSprites[1];
                    break;
                case "spanish":
                    flag2.sprite = FlagSprites[2];
                    break;
                case "french":
                    flag2.sprite = FlagSprites[3];
                    break;
                case "polish":
                    flag2.sprite = FlagSprites[4];
                    break;
                default:
                    flag2.sprite = FlagSprites[0];
                    break;
            }
        }
        else
        {
            flag2.sprite = FlagSprites[0];
        }
    } 
}
