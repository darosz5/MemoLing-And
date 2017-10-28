using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Ricimi;

public class CategoriesMover : MonoBehaviour {

    public GameObject prevLevelButton;
    public GameObject nextLevelButton;
    public RectTransform CateroriesRect;
    public Text levelText;
    public float MoveDistance;
    public float MoveSpeed;
    public int numOfPages;

    private int pageIndex = 1;
    private WaitForSeconds waitForAnim;
    private bool m_canMove = true;

    public void Initialize()
    {

        waitForAnim = new WaitForSeconds(MoveSpeed + 0.1f);
        DisablePrevLevelButton();
        SetLevelText(pageIndex);
        if(pageIndex == numOfPages)
        {
            DisableNextLevelButton();           
        }
        else if(pageIndex == 1)
        {
            DisablePrevLevelButton();
        }
    }

    public void MoveRight()
    {
        if(!m_canMove || pageIndex >= numOfPages)
            return;
        m_canMove = false;
        StartCoroutine(MoveRightCO());
    }

    public void MoveLeft()
    {
        if (!m_canMove || pageIndex <= 1)
            return;
        m_canMove = false;
        StartCoroutine(MoveLeftCO());
    }

    IEnumerator MoveRightCO()
    {

        
        LeanTween.move(CateroriesRect, CateroriesRect.anchoredPosition 
            - Vector2.right * MoveDistance, MoveSpeed);
        pageIndex++;        
        if(pageIndex == numOfPages)
        {
            DisableNextLevelButton();
        }
        if(pageIndex == 2)
        {
            EnablePrevLevelButton();
        }
        SetLevelText(pageIndex);
        yield return waitForAnim;
        m_canMove = true;
    }

    IEnumerator MoveLeftCO()
    {
        LeanTween.move(CateroriesRect, CateroriesRect.anchoredPosition
            + Vector2.right * MoveDistance, MoveSpeed);
        pageIndex--;           
        if(pageIndex == 1)
        {
            DisablePrevLevelButton();
        }
        if(pageIndex == numOfPages - 1)
        {
            EnableNextLevelButton();
        }
        SetLevelText(pageIndex);
        yield return waitForAnim;
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

    private void SetLevelText(int level)
    {
        levelText.text = level.ToString() + "/" + numOfPages;
    }
}

