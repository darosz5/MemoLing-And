using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ExaGames.Common;
using Ricimi;
using PaperPlaneTools;

public enum GAME_STATE
{
    INIT,
    LEARN,
    SHUFFLING,
    PLAY,
    WAIT_FOR_START,
    WIN,
    BOOSTERS,
    LOSE
}

public enum GAME_GOAL
{
    MOVES,
    TIME
}

public enum POWERUP
{
    KILLER,
    SHOW_SIDE,
    TIME,
    TIME_AD,
    DEATH,
    LIVE,
    ADDITIONAL_TIME
}

public class GameManager : MonoBehaviour {

    [Header("References")]
    [Space(10)]

    public GameObject GamePlayButtonOriginal;

    public GameObject GamePlayButtonLocalized;

    public GameObject GameplayPanel;

    public TranslationText Translation;

    public GameObject QuitButton;

    public GameObject MainCanvas;

    public GameObject WinCanvas;

    public GameObject LoseCanvas;

    public GameObject ShuffleButton;

    public GameObject NextStageButton;

    public GameObject Background;

    public Text stageNUmText;

    public Text TestText;

    public GameObject GoalAnimationGO;

    [Header("AudioClips")]

    public AudioClip Correct;

    public AudioClip Incorrect;

    public AudioClip NextStageClip;

    AudioSource m_audio;

    GameObject m_iconsPanel;

    GameObject m_textspanel;

    ScoreManager m_scoreManager;

    List<Icon> m_buttonsIcon = new List<Icon>();

    List<Icon> m_buttonsText = new List<Icon>();   
     
    List<Icon> m_selectedButtons = new List<Icon>();

    Sprite[] sprites;

    DemoManager m_demoManager;

    LivesManager m_livesManager;

    BoosterManager m_boosterManager;

    GameObject m_canvas;

    GameObject m_clonedGameplayPanel;

    [Header("Variables")]

    public GAME_STATE GameState = GAME_STATE.INIT;

    [HideInInspector]
    public bool IsGameOver;

    [HideInInspector]
    public bool WasTimeAdded,   
        wasIconFirst;

    [HideInInspector]
    public LevelGoal StageGoal;

    [HideInInspector]
    public bool FirstSelection = true;

    [HideInInspector]
    public int NumOfCards;
   
    [HideInInspector]
    public bool ShowSideb,
        ShowPairb,
        MoreTimeOrMovesb;

    int m_stagenumber = 1;

    string m_keyToShow;

    int m_numOfMatches;
    
    int m_chekicgIndex; 
   
    void Awake()
    {
        GetReferences();
        SetLevelGoal();
    }

    void GetReferences()
    {
        m_boosterManager = FindObjectOfType<BoosterManager>();
        m_demoManager = FindObjectOfType<DemoManager>();
        m_canvas = GameObject.Find("Canvas");
        m_scoreManager = GameManager.FindObjectOfType<ScoreManager>();
        m_audio = GetComponent<AudioSource>();
        m_livesManager = FindObjectOfType<LivesManager>();
    }

    void SetLevelGoal()
    {
        StageGoal = new LevelGoal();
        if (!GameControl.control.IsPlayingDemo)
        {
            StageGoal.amount = LevelsManager.Instance.LevelGoals[m_stagenumber - 1].amount;
            StageGoal.goal = LevelsManager.Instance.LevelGoals[m_stagenumber - 1].goal;
        }
        else
        {
            StageGoal.amount = 1000000;
            StageGoal.goal = GAME_GOAL.TIME;
        }
    }

    void Start()
    {
        if (GameControl.control.IsPlayingDemo)
        {
            StartCoroutine(LoadDemo());
            return;
        }
        sprites = LevelsManager.Instance.bundle.LoadAllAssets<Sprite>();
        if (GameControl.control.IsChecking)
        {
            CheckCards();
            return;
        }

        GameControl.control.WasLevelWon = false;

        HandleBoostersAtStart();

        GoalAnimationGO.gameObject.SetActive(true);
        Invoke("InstantiateGrid", 4f);

    }

    void HandleBoostersAtStart()
    {
        for (int i = 0; i < GameControl.control.activeBoosters.Count; i++)
        {
            GameControl.control.inventory.Remove(GameControl.control.
                activeBoosters[i].PowerUp);

        }
        m_boosterManager.HandleBoosters();
        GameControl.control.activeBoosters.Clear();
        GameControl.control.SavePlayerData();
    }

    IEnumerator LoadDemo()
    {
        sprites = Resources.LoadAll<Sprite>("demo");
        GameControl.control.SwitchLoadingScreen(true);
        SpeachManager.Instance.SetSpeach();
        if(Application.platform == RuntimePlatform.Android)
        {
            TTSManager.Speak("Start", false, TTSManager.STREAM.Music, 0f);
            while (GameControl.control.isLanguageAvalibleForSpeech &&
                !TTSManager.IsSpeaking() &&
                Application.platform == RuntimePlatform.Android)
            {
                yield return null;
            }
        }
       
        GameControl.control.SwitchLoadingScreen(false);

        m_demoManager.InstantiateDemo();
    }

    public void AddTimeOrMovesAtStart(int amount)
    {
        m_scoreManager.AddTimeAtStart(amount);
        if (!ShowPairb && !ShowSideb && GameState != GAME_STATE.WAIT_FOR_START)
        {
            GameState = GAME_STATE.WAIT_FOR_START;
        }
    }

    public void ShowSide()
    {
        StartCoroutine(ShowSideCO());
    }

    public void ShowPair()
    {
        if (ShowSideb)
            return;
        
        m_numOfMatches++;
        
        for (int i = 0; i < m_buttonsIcon.Count; i++)
        {

            if (m_buttonsIcon[i].Key == m_keyToShow)
            {
                m_buttonsIcon[i].Reveal();
                m_buttonsIcon[i].IsMatched = true;
                m_buttonsIcon[i].Highlight(true);
            }
            if (m_buttonsText[i].Key == m_keyToShow)
            {
                m_buttonsText[i].Reveal();
                m_buttonsText[i].IsMatched = true;
                m_buttonsText[i].Highlight(true);
            }
        }
        ShowPairb = false;
        GameState = GAME_STATE.WAIT_FOR_START;        
        
    }

    IEnumerator ShowSideCO()
    {
        
        for (int i = 0; i < m_buttonsIcon.Count; i++)
        {
            m_buttonsIcon[i].RevelAtStart();
        }

        yield return new WaitForSeconds(4f);

        for (int i = 0; i < m_buttonsIcon.Count; i++)
        {
            m_buttonsIcon[i].Hide();
        }
        ShowSideb = false;
        if (!ShowPairb && GameState != GAME_STATE.WAIT_FOR_START)
        {
            GameState = GAME_STATE.WAIT_FOR_START;
        }
        else if(ShowPairb && GameState != GAME_STATE.WAIT_FOR_START)
        {
            ShowPair();
        }
        
    }

    public void StartShuffle()
    {
        m_boosterManager.DisableButtons();
        if(GameState == GAME_STATE.SHUFFLING)
            return;
        
        GameState = GAME_STATE.SHUFFLING;
        for (int i = 0; i < m_buttonsIcon.Count; i++)
        {
            m_buttonsIcon[i].HideAtStart();
            m_buttonsText[i].HideAtStart();
        }
        StartCoroutine(ShuffleCO(m_buttonsIcon, 3));
        StartCoroutine(ShuffleCO(m_buttonsText, 3));
    }

    IEnumerator ShuffleCO(List<Icon> list, int times)
    {
        
        yield return new WaitForSeconds(1f);
        ShuffleButton.gameObject.SetActive(false);
        while (times > 0)
        {
            List<RectTransform> startingRectTransforms = new List<RectTransform>();
            for (int i = 0; i < list.Count; i++)
            {
                startingRectTransforms.Add(list[i].transform.parent.
                    GetComponent<RectTransform>());
            }

            for (int j = 0; j < list.Count; j += 2)
            {
                if (startingRectTransforms.Count > 1)
                {
                    int randomIndex1 = UnityEngine.Random.Range(0, startingRectTransforms.Count);
                    RectTransform randomCard1 = startingRectTransforms[randomIndex1];
                    startingRectTransforms.Remove(randomCard1);

                    int randomIndex2 = UnityEngine.Random.Range(0, startingRectTransforms.Count);
                    RectTransform randomCard2 = startingRectTransforms[randomIndex2];
                    startingRectTransforms.Remove(randomCard2);

                    LeanTween.move(randomCard2, randomCard1.anchoredPosition, .2f);
                    LeanTween.move(randomCard1, randomCard2.anchoredPosition, .2f);
                }
            }
            times--;
            yield return new WaitForSeconds(1f);
        }

        if (list == m_buttonsText)
        {
            
            if (ShowPairb)
            {
                
                ShowPair();
                m_boosterManager.ActiveButtons.Remove(m_boosterManager.KillerBooster);
                
                
            }
            if (ShowSideb)
            {
                
                ShowSide();
                m_boosterManager.ActiveButtons.Remove(m_boosterManager.SideBooster);
               
                
            }

            if (MoreTimeOrMovesb)
            {
               
                AddTimeOrMovesAtStart(5);
                m_boosterManager.ActiveButtons.Remove(m_boosterManager.TimeBooster);
               
                MoreTimeOrMovesb = false;
            }
            
            if (GameControl.control.IsPlayingDemo)
            {
                m_demoManager.OpenPopup(m_demoManager.MatchPopup);
                GameState = GAME_STATE.WAIT_FOR_START;
            }
            else if(!MoreTimeOrMovesb && !ShowSideb && !ShowSideb)
            {
                
                GameState = GAME_STATE.WAIT_FOR_START;
            }
            

        }        
    }

    public void NextCheck()
    {
        m_chekicgIndex++;
        CleanBoard();
        CheckCards();
    }

    public void CheckCards()
    {
        m_clonedGameplayPanel = (GameObject)Instantiate
           (GameplayPanel, m_canvas.transform, false);
        m_iconsPanel = m_clonedGameplayPanel.transform.GetChild(0).gameObject;
        m_textspanel = m_clonedGameplayPanel.transform.GetChild(1).gameObject;
        List<string> m_keys = new List<string>
        (LocalizationManager.Instance.localizedText.Keys);

        Icon clonedButtonIcon = Instantiate
            (GamePlayButtonOriginal, m_iconsPanel.transform.GetChild(0), false).GetComponent<Icon>();
        clonedButtonIcon.IsIcon = true;
        Sprite sprite = FindSprite(m_keys[m_chekicgIndex]);
        clonedButtonIcon.Initialize
            (m_keys[m_chekicgIndex], LocalizationManager.Instance.GetOriginalValue(m_keys[m_chekicgIndex]), sprite);
        m_buttonsIcon.Add(clonedButtonIcon);

        Icon clonedButtonIconWithText1 = Instantiate
       (GamePlayButtonLocalized, m_textspanel.transform.GetChild(11), false).GetComponent<Icon>();
        LocalizationManager.Instance.LoadLocalizedText(LocalizationManager.Instance.category + "/english");
        clonedButtonIconWithText1.Initialize
            (m_keys[m_chekicgIndex], LocalizationManager.Instance.GetLocalizedValue(m_keys[m_chekicgIndex]));
        m_buttonsText.Add(clonedButtonIconWithText1);

        Icon clonedButtonIconWithText2 = Instantiate
      (GamePlayButtonLocalized, m_textspanel.transform.GetChild(10), false).GetComponent<Icon>();
        LocalizationManager.Instance.LoadLocalizedText(LocalizationManager.Instance.category + "/spanish");
        clonedButtonIconWithText2.Initialize
            (m_keys[m_chekicgIndex], LocalizationManager.Instance.GetLocalizedValue(m_keys[m_chekicgIndex]));
        m_buttonsText.Add(clonedButtonIconWithText2);

        Icon clonedButtonIconWithText3 = Instantiate
     (GamePlayButtonLocalized, m_textspanel.transform.GetChild(9), false).GetComponent<Icon>();
        LocalizationManager.Instance.LoadLocalizedText(LocalizationManager.Instance.category + "/french");
        clonedButtonIconWithText3.Initialize
            (m_keys[m_chekicgIndex], LocalizationManager.Instance.GetLocalizedValue(m_keys[m_chekicgIndex]));
        m_buttonsText.Add(clonedButtonIconWithText3);

        Icon clonedButtonIconWithText4 = Instantiate
     (GamePlayButtonLocalized, m_textspanel.transform.GetChild(8), false).GetComponent<Icon>();
        LocalizationManager.Instance.LoadLocalizedText(LocalizationManager.Instance.category + "/german");
        clonedButtonIconWithText4.Initialize
            (m_keys[m_chekicgIndex], LocalizationManager.Instance.GetLocalizedValue(m_keys[m_chekicgIndex]));
        m_buttonsText.Add(clonedButtonIconWithText4);

        Icon clonedButtonIconWithText5 = Instantiate
     (GamePlayButtonLocalized, m_textspanel.transform.GetChild(7), false).GetComponent<Icon>();
        LocalizationManager.Instance.LoadLocalizedText(LocalizationManager.Instance.category + "/polish");
        clonedButtonIconWithText5.Initialize
            (m_keys[m_chekicgIndex], LocalizationManager.Instance.GetLocalizedValue(m_keys[m_chekicgIndex]));
        m_buttonsText.Add(clonedButtonIconWithText5);
    }

    public void AddTimeOrMoves(int amount)
    {      
        IsGameOver = false;
        WasTimeAdded = true;       
        LoseCanvas.SetActive(false);
        Background.SetActive(true);
        GameState = GAME_STATE.WAIT_FOR_START;
        m_scoreManager.ResetNoMatchTime();
        if(StageGoal.goal == GAME_GOAL.TIME)
        {
            m_scoreManager.Timer = (float)amount;
            m_scoreManager.TimeSLider.value = m_scoreManager.Timer;
        }
        else if (StageGoal.goal == GAME_GOAL.MOVES)
        {
            StageGoal.amount += amount;
            m_scoreManager.MovesAmountText.text = StageGoal.amount.ToString();
        }
    }

    public void InstantiateGrid()
    {
        m_scoreManager.ResetTimeOrMoves(StageGoal.amount);
        m_scoreManager.SetCounterOrTimer(StageGoal.goal);
        m_boosterManager.EnableActiveButtons();

        IsGameOver = false;
        WasTimeAdded = false;
              
        List<string> m_keys = new List<string>
            (LocalizationManager.Instance.localizedText.Keys);

        float indexf = (LevelsManager.Instance.activeLevelNum - 1) * .05f * m_keys.Count;
        float countf = (.15f + LevelsManager.Instance.activeLevelNum * .05f +
            LevelsManager.Instance.difficultyLevel * .05f) * m_keys.Count;

        if (LevelsManager.Instance.activeLevelNum == 12)
        {
            countf = m_keys.Count;
        }
        int index = Mathf.RoundToInt(indexf);
        int count = Mathf.RoundToInt(countf) - index;
              
        m_keys = m_keys.GetRange(index, count);

        if(StageGoal.goal == GAME_GOAL.MOVES)
        {
            NumOfCards = 12;
        }
        else
        {
            NumOfCards = 9;
        }
        if(LevelsManager.Instance.difficultyLevel == 1)
        {
            NumOfCards -= 3;
        }
        else if(LevelsManager.Instance.difficultyLevel == 4)
        {
            NumOfCards = 12;
        }
        //if (GameControl.control.IsTesting)
        //{
        //    NumOfCards = LevelsManager.Instance.numOfCards;
        //}
        float timeOrMovesAmount = StageGoal.amount;

        NumOfCards = Mathf.Clamp(NumOfCards, 6, 12);
        StartCoroutine(InstantiateGridCO(m_keys, timeOrMovesAmount, NumOfCards));               
    }

    public IEnumerator InstantiateGridCO(List<string> keys, float timeOrMovesAmount, int num)
    {
        if (GameControl.control.IsPlayingDemo)
        {
            m_scoreManager.ResetTimeOrMoves(StageGoal.amount);
            m_scoreManager.SetCounterOrTimer(StageGoal.goal);
        }
        m_clonedGameplayPanel = (GameObject)Instantiate
            (GameplayPanel, m_canvas.transform, false);
        QuitButton.SetActive(true);
        m_iconsPanel = m_clonedGameplayPanel.transform.GetChild(0).gameObject;
        m_textspanel = m_clonedGameplayPanel.transform.GetChild(1).gameObject;
        GameState = GAME_STATE.INIT;
        

        for (int i = 0; i < num; i++)
        {           
            int randomIndex = UnityEngine.Random.Range(0, keys.Count);
            string randomKey = keys[randomIndex];


            int startIndex = 12 - num;
            Icon clonedButtonIcon = Instantiate
                (GamePlayButtonOriginal, m_iconsPanel.transform.GetChild(startIndex + i), false).GetComponent<Icon>();
            clonedButtonIcon.IsIcon = true;
            Sprite sprite = FindSprite(randomKey);
            clonedButtonIcon.Initialize
                (randomKey, LocalizationManager.Instance.GetOriginalValue(randomKey), sprite);
            m_buttonsIcon.Add(clonedButtonIcon);
            Translation.Translate(LocalizationManager.Instance.GetOriginalValue(randomKey));

            
            Icon clonedButtonIconWithText = Instantiate
                (GamePlayButtonLocalized, m_textspanel.transform.GetChild(i), false).GetComponent<Icon>();
            clonedButtonIconWithText.Initialize
                (randomKey, LocalizationManager.Instance.GetLocalizedValue(randomKey));
            m_buttonsText.Add(clonedButtonIconWithText);

            keys.RemoveAt(randomIndex);
            yield return new WaitForSeconds(1f);
        }
        m_keyToShow = m_buttonsIcon[UnityEngine.Random.Range(0, m_buttonsIcon.Count)].Key;
        GameState = GAME_STATE.LEARN;
        ShuffleButton.SetActive(true);
        ShuffleButton.transform.SetAsLastSibling();       
        EventManager.Instance.PostNotification(EVENT_TYPE.SHUFFLING, this);
        if (GameControl.control.IsPlayingDemo)
        {
            m_demoManager.OpenPopup(m_demoManager.LearnPopup);
        }
    }
    
    Sprite FindSprite(string key)
    {
        for (int i = 0; i < sprites.Length; i++)
        {
            if (sprites[i].name == key)
            {
                return sprites[i];
            }
        }
        return null;
    }

    IEnumerator HideIconsCO()
    {
        yield return new WaitForSeconds(.1f);
        for (int i = 0; i < m_selectedButtons.Count; i++)
        {
            m_selectedButtons[i].Hide();          
        }
        yield return new WaitForSeconds(1.4f);
        m_selectedButtons.Clear();
    }   
        
    public void GameOver()
    {
        m_scoreManager.MovesTextAnimator.SetBool("Low_Moves_b", false);
        GameState = GAME_STATE.LOSE;
        Background.SetActive(false);
        LoseCanvas.SetActive(true);
        LoseCanvas.transform.GetChild(LoseCanvas.transform.childCount - 1)
            .GetComponent<LosePopup>().Initialize();
        IsGameOver = true;
        if (WasTimeAdded)
        {
            m_livesManager.ConsumeLife();
        }
    }
    
    bool CheckIfMatch()
    {
        if(StageGoal.goal == GAME_GOAL.MOVES)
        {
            m_scoreManager.TakeMove();
        }
        if (m_selectedButtons[0].Key == m_selectedButtons[1].Key)
        {
            m_scoreManager.UpdateScore(NumOfCards);
            return true;           
        }
        return false;
    }

    void CheckWin()
    {
        for (int i = 0; i < m_buttonsIcon.Count; i++)
        {
            if (!m_buttonsIcon[i].IsMatched)
                return;
        }
        m_numOfMatches = 0;
        m_scoreManager.TimeSLider.value = 0;
        StartCoroutine(WinScreenCO());
    }

    public void DebugWin(int numOfStars)
    {
        switch (numOfStars)
        {
            case 1:
                m_scoreManager.Score = GameControl.control.levelStarsValues[LevelsManager.Instance.activeLevelNum - 1].Star1 + 1;
                break;
            case 2:
                m_scoreManager.Score = GameControl.control.levelStarsValues[LevelsManager.Instance.activeLevelNum - 1].Star2 + 1;
                break;
            case 3:
                m_scoreManager.Score = GameControl.control.levelStarsValues[LevelsManager.Instance.activeLevelNum - 1].Star3 + 1;
                break;
            default:
                break;
        }
        m_stagenumber = LevelsManager.Instance.numOfStages;
        StartCoroutine(WinScreenCO());
    }

    IEnumerator WinScreenCO()
    {
        m_scoreManager.MovesTextAnimator.SetBool("Low_Moves_b", false);
        //if (GameControl.control.IsTesting)
        //{
        //    TestText.gameObject.SetActive(true);
        //    TestText.text = m_scoreManager.ShowResult();
        //}     
        GameState = GAME_STATE.WIN;
        yield return new WaitForSeconds(1f);
        m_stagenumber++;

        if(m_stagenumber > LevelsManager.Instance.numOfStages &&
            !GameControl.control.IsPlayingDemo)
        {
            m_iconsPanel.SetActive(false);
            m_textspanel.SetActive(false);

            string categoryName = (LevelsManager.Instance.categoryName);
            int levelNum = LevelsManager.Instance.activeLevelNum - 1;
            CategoryState category = GameControl.control.FindCategory(categoryName);

            category.levelsState[levelNum] = 2;
        
            if(levelNum != 11)
            {
                if(category.levelsState[levelNum + 1] == 0)
                {
                    category.levelsState[levelNum + 1] = 1;
                }                           
            }

            if (m_scoreManager.Score != 0 && m_scoreManager.Score > category.scores[levelNum])
            {
                GPSManager.SetRecoredForLeaderboard(category);
                category.scores[levelNum] = m_scoreManager.Score;               
            }
            WinCanvas.SetActive(true);
            WinCanvas.transform.GetChild(WinCanvas.transform.childCount - 1)
                .GetComponent<WinPopup>().Initialize(m_scoreManager.Score);
            GameControl.control.WasLevelWon = true;
            GameControl.control.SaveCategory (LocalizationManager.Instance.secondLanguage);
            RateBox.Instance.Show();        
            QuitButton.SetActive(false);

            GPSManager.CheckForAchievments(category);
        }
        else
        {
            m_iconsPanel.SetActive(false);
            m_textspanel.SetActive(false);
            if (GameControl.control.IsPlayingDemo)
            {
                m_demoManager.OpenPopup(m_demoManager.WinPopup);
                GameControl.control.IsPlayingDemo = false;
                GameControl.control.secondLaunch = true;
                GameControl.control.SaveCategory(LocalizationManager.Instance.secondLanguage);
            }
            else
            {
                m_audio.PlayOneShot(NextStageClip);
                stageNUmText.text = m_stagenumber + "/" + LevelsManager.Instance.numOfStages;
                NextStageButton.SetActive(true);
                NextStageButton.transform.SetAsLastSibling();
                
            }           
        }        
    }

    public void StartNextStage()
    {
        StartCoroutine(StartNextStageCO());
    }

    public IEnumerator StartNextStageCO()
    {        
        StageGoal.amount = LevelsManager.Instance.LevelGoals[m_stagenumber - 1].amount;
        StageGoal.goal = LevelsManager.Instance.LevelGoals[m_stagenumber - 1].goal;
       
        //if (GameControl.control.IsTesting)
        //{
        //    TestText.gameObject.SetActive(false);
        //}
        m_iconsPanel.SetActive(true);
        m_textspanel.SetActive(true);

        CleanBoard();
        GoalAnimationGO.gameObject.SetActive(true);
        QuitButton.SetActive(false);
        Invoke("InstantiateGrid", 4f);
        yield return new WaitForSeconds(.5f);
        NextStageButton.SetActive(false);
    }

    void CleanBoard()
    {
        Destroy(m_clonedGameplayPanel.gameObject);
        m_buttonsIcon.Clear();
        m_buttonsText.Clear();      
    }

    public void SelectButton(Icon button)
    {
        if(GameState == GAME_STATE.WAIT_FOR_START)
        {
            GameState = GAME_STATE.PLAY;
        }
        if(m_selectedButtons.Count == 2)
        {
            return;
        }

        if (FirstSelection)
        {
            if (button.IsIcon)
            {
                wasIconFirst = true;
            }
            m_selectedButtons.Add(button);
            button.Reveal();
        }
        else
        {
            if(wasIconFirst)
            {
                if (button.IsIcon)
                    return;
            }
            if (!wasIconFirst)
            {
                if (!button.IsIcon)
                    return; 
            }
            if(button != m_selectedButtons[0])
            {
                wasIconFirst = false;
                m_selectedButtons.Add(button);
                button.Reveal();
                bool match = CheckIfMatch();
                if (match)
                {
                    m_audio.PlayOneShot(Correct);
                    for (int i = 0; i < m_selectedButtons.Count; i++)
                    {
                        m_selectedButtons[i].IsMatched = true;
                        m_selectedButtons[i].Highlight(true);                      
                    }
                    m_numOfMatches++;
                    if (m_numOfMatches >= NumOfCards)
                    {
                        CheckWin();
                    }
                    
                    m_selectedButtons.Clear();
                    
                }
                else
                {
                    m_audio.PlayOneShot(Incorrect);
                    StartCoroutine(HideIconsCO());
                }
            }
        }
        FirstSelection = !FirstSelection;       
    }

    void OnApplicationQuit()
    {
        if(!GameControl.control.WasLevelWon)
             m_livesManager.ConsumeLife();
    }

    void OnLevelWasLoaded(int level)
    {
        BackgroundMusic.Instance.FadeOut();
    }

    void OnDisable()
    {
        if(PlayerPrefs.GetInt("music_on") != 0)
            BackgroundMusic.Instance.GetComponent<AudioSource>().volume = BackgroundMusic.Instance.Volume;
    }
}
