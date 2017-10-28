using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour {
    
    [Header("TIME/Moves")]
    public Slider TimeSLider;
    public Slider MovesSlider;
    public Image FillImage;
    [HideInInspector]
    public float MaxTime;
    public Text MovesAmountText;
    
    [Space(10)]

    [Header("SCORE")]
    public Slider ScoreSlider;
    public int MaxScore;
    public float MatchPoints;
    public Text ScoreText;

    private Animator m_timeSliderBgAnimator;
    private Animator m_scoreTextAnim;

    public Animator TimeTextAnimator;
    public Animator GradeAnimator;
    public Animator MovesTextAnimator;
    private Text m_gradeText;
    private Text m_timeTextToAnim;
    private Text m_scoreTextToAnim;
    private AudioSource m_audio;
    private string m_result = "Result:\n";
    private GameManager m_gameManager;

    [SerializeField]
    private float m_timeSinceLastMatch;
    private float m_startingNoMatchTime;
    private float m_noMatchTime;
    [SerializeField]
    private float matchNum;
    private StarsFiller m_starFiller;
    
    

    // private int m_score;
    private float m_totalTime;
       

    public int Score;

    private float m_timer;
    private int m_moves;

 

    public float Timer
    {
        get
        {
            return m_timer;
        }
        set
        {
            m_timer =  Mathf.Clamp(value , 0, MaxTime + 1);
            if (m_timer < 5f && !m_audio.isPlaying)
            {
                m_timeSliderBgAnimator.SetTrigger("3Sec_t");
                m_audio.Play();
            }
         

            if (m_timer <= 0)
            {
                m_audio.Stop();
                if (!m_gameManager.IsGameOver)
                {
                    m_gameManager.GameOver();
                    
                }               
            }
        }
    }

    private void Awake()
    {
        m_gameManager = FindObjectOfType<GameManager>();
        m_scoreTextAnim = ScoreSlider.GetComponentInChildren<Animator>();
        m_scoreTextToAnim = ScoreSlider.GetComponentInChildren<Text>();
        m_scoreTextAnim.transform.SetAsLastSibling();
        m_starFiller = FindObjectOfType<StarsFiller>();
        m_timeTextToAnim = TimeTextAnimator.GetComponent<Text>();
        m_gradeText = GradeAnimator.GetComponent<Text>();
      

        m_timeSliderBgAnimator = TimeSLider.GetComponentInChildren<Animator>();
        m_audio = GetComponent<AudioSource>();
       
    }

    private void Start()
    {
        if (!GameControl.control.IsPlayingDemo)
        {
            MaxScore = GameControl.control.levelStarsValues
           [LevelsManager.Instance.activeLevelNum - 1].Star3;
        }
        else
        {
            MaxScore = 150;
        }
        m_startingNoMatchTime = (4 - LevelsManager.Instance.difficultyLevel) * 10 + 30f;     
        if (GameControl.control.IsPlayingDemo)
        {
            m_startingNoMatchTime = 180f;
        }
        m_noMatchTime = m_startingNoMatchTime;
        Score = 0;
        ScoreSlider.maxValue = MaxScore;
        ScoreSlider.value = Score;
        m_timeSinceLastMatch = 0;
        matchNum = 0;
        
    }

    public void ResetNoMatchTime()
    {
        m_noMatchTime = m_startingNoMatchTime;
    }

    public void SetCounterOrTimer(GAME_GOAL goal)
    {
        
        if (goal == GAME_GOAL.TIME)
        {
            TimeSLider.gameObject.SetActive(true);
            MovesAmountText.transform.parent.gameObject.SetActive(false);
        }
        else if (goal == GAME_GOAL.MOVES)
        {
            TimeSLider.gameObject.SetActive(false);
            MovesAmountText.transform.parent.gameObject.SetActive(true);
        }
        
    }


    public void AddTimeAtStart(int amount)
    {
        if (m_gameManager.StageGoal.goal == GAME_GOAL.TIME)
        {
            amount *= 2;
            MaxTime += amount;
            Timer = MaxTime;
            TimeSLider.maxValue += amount;
            TimeSLider.value += amount;
            PlayTimeAddAnim(amount);
        }
        else if (m_gameManager.StageGoal.goal == GAME_GOAL.MOVES)
        {
            m_gameManager.StageGoal.amount += amount;
            MovesAmountText.text = m_gameManager.StageGoal.amount.ToString();
            PlayTimeAddAnim(amount);
        }
       
        
    }

    public void ResetTimeOrMoves(float timeOrMovesAmount)
    {      
       if (m_gameManager.StageGoal.goal == GAME_GOAL.TIME)
        {
            MaxTime = timeOrMovesAmount;

            TimeSLider.maxValue = MaxTime;
            TimeSLider.value = MaxTime;
            Timer = MaxTime;
            FillImage.color = Color.green;
        }
        else if (m_gameManager.StageGoal.goal == GAME_GOAL.MOVES)
        {
            m_noMatchTime = m_startingNoMatchTime;
            MovesAmountText.text = timeOrMovesAmount.ToString();
            MovesSlider.maxValue = m_noMatchTime;
            MovesSlider.value = m_noMatchTime;
        }
       
        ScoreSlider.value = Score;
        ScoreText.text = Score.ToString();

        matchNum = 0;
        m_result = "Result:\n";
        m_totalTime = 0;       
        m_timeSinceLastMatch = 0;
        m_moves = 0;
    }

    public void CountersOff()
    {
        TimeSLider.gameObject.SetActive(false);
        MovesAmountText.transform.parent.gameObject.SetActive(false);
    }

    public void UpdateScore(int numOfCards)
    {      
        float pointsToAdd = 0;

        matchNum++;

        if (matchNum == numOfCards)
            return;

        pointsToAdd = 120 - m_timeSinceLastMatch * 10;
        if (m_timeSinceLastMatch < 11f)
        {
            pointsToAdd += 10f;
        }
        pointsToAdd = Mathf.Clamp(pointsToAdd, 10f, 130f);

        pointsToAdd = Mathf.RoundToInt(pointsToAdd);
      
        m_scoreTextToAnim.text = "+" + pointsToAdd.ToString();
        m_scoreTextAnim.Play("ScoreTextAnimation", -1, 0f);

        Score += (int)pointsToAdd;
        ScoreSlider.value = Score;
        ScoreText.text = Score.ToString();

        m_totalTime += m_timeSinceLastMatch;
        m_timeSinceLastMatch = 0f;
        m_noMatchTime = m_startingNoMatchTime;
        m_starFiller.FillStars(Score);

        PlayGrade((int)pointsToAdd);
                    
    }

    public void PlayTimeAddAnim(float time)
    {
        m_timeTextToAnim.text = "+" + time.ToString("0");
        TimeTextAnimator.Play("ScoreTextAnimation", -1, 0f);
    }

    public void PlayGrade(int score)
    {
        if (score <= 50)
        {
            //m_gradeText.text = "GOOD";
            m_gradeText.text = LocalizationManager.Instance.GetUIValue("_good");
            m_gradeText.color = Color.green;
        }
        else if (score <= 100)
        {
            // m_gradeText.text = "GREAT!";
            m_gradeText.text = LocalizationManager.Instance.GetUIValue("_great");
            m_gradeText.color = Color.blue;
        }
        else
        {
            // m_gradeText.text = "FANTASTIC!";
            m_gradeText.text = LocalizationManager.Instance.GetUIValue("_fantastic");
            m_gradeText.color = Color.yellow;
        }
        GradeAnimator.Play("Grade", -1, 0f);
    }


    public string ShowResult()
    {
        m_result += ("Total time: " + m_totalTime.ToString("0.0") + 
            ". Total score: " + Score.ToString("0") +
            ". Total moves: " + m_moves.ToString("0"));
        
        return m_result;
    }

    public void TakeMove()
    {
        m_moves++;
        m_gameManager.StageGoal.amount--;
        
        if (m_gameManager.StageGoal.amount <= 5)
        {
            MovesTextAnimator.SetBool("Low_Moves_b", true);
        }
        else
        {
            MovesTextAnimator.SetBool("Low_Moves_b", false);
        }
        if (!m_gameManager.IsGameOver && m_gameManager.StageGoal.amount <= 0
            && matchNum < m_gameManager.NumOfCards - 1)
        {
            m_gameManager.GameOver();
            
        }
        MovesAmountText.text = m_gameManager.StageGoal.amount.ToString();
    }

    void Update()
    {
        if( m_gameManager.GameState == GAME_STATE.PLAY)
        {
            m_timeSinceLastMatch += Time.deltaTime;
            if (m_gameManager.StageGoal.goal == GAME_GOAL.MOVES)
            {
                m_noMatchTime -= Time.deltaTime;
                MovesSlider.value = m_noMatchTime;
                if (m_noMatchTime <= 0f)
                {
                    m_gameManager.GameOver();
                                      
                }
            }
              
            else if (m_gameManager.StageGoal.goal == GAME_GOAL.TIME)
            {               
                Timer -= Time.deltaTime;
                TimeSLider.value = Timer;
                FillImage.color = Color.Lerp(Color.red, Color.green, Timer / MaxTime);
            }
        }
        

    }
}
