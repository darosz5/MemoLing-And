using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Ricimi;

public class CategoriesMover : MonoBehaviour {

    [Header("References")]

    public GameObject prevLevelButton;

    public GameObject nextLevelButton;

    public RectTransform CateroriesRect;

    public Text levelText;

    [Header("Variables")]

    public float MoveDistance;

    public float MoveSpeed;

    public int numOfPages;

    int m_pageIndex = 1;

    WaitForSeconds m_waitForAnim;

    bool m_canMove = true;

    public void Initialize()
    {

        m_waitForAnim = new WaitForSeconds(MoveSpeed + 0.1f);
        DisablePrevLevelButton();
        SetLevelText(m_pageIndex);
        if(m_pageIndex == numOfPages)
        {
            DisableNextLevelButton();           
        }
        else if(m_pageIndex == 1)
        {
            DisablePrevLevelButton();
        }
    }

    public void MoveRight()
    {
        if(!m_canMove || m_pageIndex >= numOfPages)
            return;
        m_canMove = false;
        StartCoroutine(MoveRightCO());
    }

    public void MoveLeft()
    {
        if (!m_canMove || m_pageIndex <= 1)
            return;
        m_canMove = false;
        StartCoroutine(MoveLeftCO());
    }

    IEnumerator MoveRightCO()
    {

        
        LeanTween.move(CateroriesRect, CateroriesRect.anchoredPosition 
            - Vector2.right * MoveDistance, MoveSpeed);
        m_pageIndex++;        
        if(m_pageIndex == numOfPages)
        {
            DisableNextLevelButton();
        }
        if(m_pageIndex == 2)
        {
            EnablePrevLevelButton();
        }
        SetLevelText(m_pageIndex);
        yield return m_waitForAnim;
        m_canMove = true;
    }

    IEnumerator MoveLeftCO()
    {
        LeanTween.move(CateroriesRect, CateroriesRect.anchoredPosition
            + Vector2.right * MoveDistance, MoveSpeed);
        m_pageIndex--;           
        if(m_pageIndex == 1)
        {
            DisablePrevLevelButton();
        }
        if(m_pageIndex == numOfPages - 1)
        {
            EnableNextLevelButton();
        }
        SetLevelText(m_pageIndex);
        yield return m_waitForAnim;
        m_canMove = true;
    }

    public void EnablePrevLevelButton()
    {
        var image = prevLevelButton.GetComponentsInChildren<Image>()[1];
        var newColor = image.color;
        newColor.a = 1.0f;
        image.color = newColor;

        var shadow = prevLevelButton.GetComponentsInChildren<Image>()[0];
        var newShadowColor = shadow.color;
        newShadowColor.a = 1.0f;
        shadow.color = newShadowColor;

        prevLevelButton.GetComponent<AnimatedButton>().interactable = true;
    }

    public void DisablePrevLevelButton()
    {
        var image = prevLevelButton.GetComponentsInChildren<Image>()[1];
        var newColor = image.color;
        newColor.a = 40 / 255.0f;
        image.color = newColor;

        var shadow = prevLevelButton.GetComponentsInChildren<Image>()[0];
        var newShadowColor = shadow.color;
        newShadowColor.a = 0.0f;
        shadow.color = newShadowColor;

        prevLevelButton.GetComponent<AnimatedButton>().interactable = false;
    }

    public void EnableNextLevelButton()
    {
        var image = nextLevelButton.GetComponentsInChildren<Image>()[1];
        var newColor = image.color;
        newColor.a = 1.0f;
        image.color = newColor;

        var shadow = nextLevelButton.GetComponentsInChildren<Image>()[0];
        var newShadowColor = shadow.color;
        newShadowColor.a = 1.0f;
        shadow.color = newShadowColor;

        nextLevelButton.GetComponent<AnimatedButton>().interactable = true;
    }

    public void DisableNextLevelButton()
    {
        var image = nextLevelButton.GetComponentsInChildren<Image>()[1];
        var newColor = image.color;
        newColor.a = 40 / 255.0f;
        image.color = newColor;

        var shadow = nextLevelButton.GetComponentsInChildren<Image>()[0];
        var newShadowColor = shadow.color;
        newShadowColor.a = 0.0f;
        shadow.color = newShadowColor;

        nextLevelButton.GetComponent<AnimatedButton>().interactable = false;
    }

    void SetLevelText(int level)
    {
        levelText.text = level.ToString() + "/" + numOfPages;
    }
}

