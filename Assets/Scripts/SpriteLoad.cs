using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;
using UnityEngine.UI;

public class SpriteLoad : MonoBehaviour {

    Image m_image;

    void Awake()
    {
        m_image = GetComponent<Image>();
    }

    void Start()
    {
        StartCoroutine(GetTexture());
    }

    IEnumerator GetTexture()
    {
        using (UnityWebRequest www =
            UnityWebRequestTexture.GetTexture("https://r07t.imgup.net/builder2907.jpg"))
        {
            yield return www.Send();

            if (www.isNetworkError)
            {
                Debug.Log(www.error);
            }
            else
            {
                Texture2D myTexture = DownloadHandlerTexture.GetContent(www);
                Sprite sprite = Sprite.Create(myTexture,
                    new Rect(0f, 0f,150f, 150f), new Vector2(0.5f, 0.5f));
                m_image.sprite = sprite;
            }
        }
    }
}
