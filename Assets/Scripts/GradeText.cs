using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GradeText : MonoBehaviour {

    public Color GoodColor;
    public Color GreatColor;
    public Color FantasticColor;

    public Vector2 scoreChangeText;

    private Text m_text;
    private Animator m_anim;
    private int m_score;

    private void Awake()
    {
        m_text = GetComponent<Text>();
        m_anim = GetComponent<Animator>();
    }

    public void PlayAnim(int score)
    {
        m_score = score;
        m_anim.Play("Grade", -1, 0f);

    }

    public void SetGrade()
    {
        if (m_score <= scoreChangeText.x)
        {
            m_text.text = "GOOD";
            m_text.color = GoodColor;
        }
        else if (m_score <= scoreChangeText.y)
        {
            m_text.text = "GREAT!";
            m_text.color = GreatColor;
        }
        else
        {
            m_text.text = "FANTASTIC!";
            m_text.color = FantasticColor;
        }
    }


}
