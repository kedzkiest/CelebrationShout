using System;
using System.Collections;
using UnityEngine;

public class GameManager : SingletonMonoBehaviour<GameManager>
{
    public enum GameState
    {
        TITLE,              // View showing the game title
        IN_TRANSITION,      // View when a transition animation is under progress
        INGAME_INITIAL,     // View waiting for drum roll or something
        WAITING_ANNOUNCE,   // View showing the process before announcement
        AFTER_ANNOUNCE,     // View waiting for player's key input
        PLAYER_SHOUTING,    // View where player is shouting according to the input
        SHOW_RESPONSE,      // View showing the response from surroudings against the shout
        RESULT              // View showing success/failure result
    }
    private GameState currentGameState;

    public enum ShoutType
    {
        HAPPY_BIRTHDAY,
        HAPPY_NEW_YEAR,
        MERRY_CHRISTMAS
    }
    /// <summary>
    /// The expected ansser to be shouted.
    /// Randomly chosen when a game starts.
    /// </summary>
    public ShoutType correctShoutType { get; private set; }

    /// <summary>
    /// The instance that receives inputs from user and publishes them as events.
    /// </summary>
    [SerializeField]
    private UserInputHandler userInputHandler;

    /// <summary>
    /// The wait time after a transition animation finishes.
    /// </summary>
    [SerializeField]
    private float waitTimeBeforePreparation;

    /// <summary>
    /// The minimum duration of announcement prepartion (the duration of drum roll sound, for example)
    /// </summary>
    [SerializeField]
    private float minPreparationDuration;

    /// <summary>
    /// The maximum duration of announcement prepartion (the duration of drum roll sound, for example)
    /// </summary>
    [SerializeField]
    private float maxPreparationDuration;

    /// <summary>
    /// The time between response to player shout finishes and result view appears.
    /// </summary>
    [SerializeField]
    private float waitTimeBeforeResult;

    private new void Awake()
    {
        Initialize();
    }

    /// <summary>
    /// Initialization performed only once at the beginning of the game.
    /// </summary>
    private void Initialize()
    {
        // Initialie savedata
        SaveManager.Instance.Initialize();

        // Initialize sound
        SoundPlayer.Instance.Initialize();

        // Initialzie stage
        GameObject character = FindObjectOfType<Mia>().gameObject;
        HappyBirthdayStage hbStage = FindObjectOfType<HappyBirthdayStage>();
        HappyNewYearStage hnyStage = FindObjectOfType<HappyNewYearStage>();
        MerryChristmasStage mcStage = FindObjectOfType<MerryChristmasStage>();
        Light stageLight = GameObject.FindGameObjectWithTag("StageLight").GetComponent<Light>();
        StageManager.Instance.Initialize(character, hbStage, hnyStage, mcStage, stageLight);

        // Initialize UI
        TitleUI titleUI = titleUI = FindObjectOfType<TitleUI>();
        InGameUI inGameUI = FindObjectOfType<InGameUI>();
        ResultUI resultUI = FindObjectOfType<ResultUI>();
        TransitionAnimation transitionAnimation = FindObjectOfType<TransitionAnimation>();
        SpeechBubbleGenerator speechBubbleGenerator = FindObjectOfType<SpeechBubbleGenerator>();
        UIManager.Instance.Initialize(titleUI, inGameUI, resultUI, transitionAnimation, speechBubbleGenerator);

        // Subscribe user input events
        userInputHandler.OnSpaceKeyPressed += OnSpaceKeyPressed;
        userInputHandler.OnBKeyPressed += OnShoutKeyPressed;
        userInputHandler.OnNKeyPressed += OnShoutKeyPressed;
        userInputHandler.OnMKeyPressed += OnShoutKeyPressed;
        userInputHandler.OnEscapeKeyPressed += OnEscapeKeyPressed;
    }

    public event Action OnGameStart = () => { };
    public event Action OnGameRestart = () => { };
    /// <summary>
    /// Start the game from TITLE and RESULT sequence.
    /// </summary>
    private void OnSpaceKeyPressed()
    {
        if (currentGameState == GameState.IN_TRANSITION) return;

        if (currentGameState == GameState.TITLE)
        {
            StartGame();

            OnGameStart();
        }

        if (currentGameState == GameState.RESULT)
        {
            StartGame();

            OnGameRestart();
        }
    }

    private void StartGame()
    {
        currentGameState = GameState.IN_TRANSITION;
        correctShoutType = (ShoutType)UnityEngine.Random.Range(0, Enum.GetValues(typeof(ShoutType)).Length);
        Debug.Log("correctShoutType: " + correctShoutType);

        SoundPlayer.Instance.Play(SoundTable.SoundName.TRANSITION_BUZZER);
    }

    /// <summary>
    /// The method called when a transition finishes.
    /// Caller is transition animation event.
    /// </summary>
    public void OnTransitionToInGameFinish()
    {
        SoundPlayer.Instance.Stop();

        currentGameState = GameState.INGAME_INITIAL;

        StartCoroutine(WaitBeforeAnnouncePreparation(waitTimeBeforePreparation));
    }


    /// <summary>
    /// Coroutine for announcement preparation sequecne.
    /// We have this instance because we can stop it in cace of early shout penalty.
    /// </summary>
    Coroutine announcePreparationCoroutine;

    /// <summary>
    /// The sequence between transition animation finishes and announcement preparation starts.
    /// </summary>
    private IEnumerator WaitBeforeAnnouncePreparation(float _seconds)
    {
        Debug.Log("Wait Start");
        yield return new WaitForSeconds(_seconds);
        Debug.Log("Wait Finish");

        announcePreparationCoroutine = StartCoroutine(DoAnnouncePreparation());
    }


    public event Action OnAnnounceMade = () => { };
    /// <summary>
    /// The game time when a celebration announce was made.
    /// </summary>
    float announceTime;
    private IEnumerator DoAnnouncePreparation()
    {
        currentGameState = GameState.WAITING_ANNOUNCE;

        Debug.Log("Preparation Start");
        SoundPlayer.Instance.Play(SoundTable.SoundName.DRUM_ROLL);
        yield return new WaitForSeconds(UnityEngine.Random.Range(minPreparationDuration, maxPreparationDuration));
        Debug.Log("Preparation Finish");
        announceTime = Time.time;
        Debug.Log(announceTime);
        SoundPlayer.Instance.Stop();
        SoundPlayer.Instance.PlayOneShot(SoundTable.SoundName.DRUM_ROLL_FINISH);

        OnAnnounceMade();

        currentGameState = GameState.AFTER_ANNOUNCE;
        Debug.Log("Waiting for player input...");
    }

    /// <summary>
    /// How much time player shout last.
    /// Used for adjusting judge timing
    /// </summary>
    float shoutDuration;
    public event Action<ShoutType> OnShoutEnter = (_shoutType) => { };
    private void OnShoutKeyPressed(ShoutType _shoutType)
    {
        bool cannotShout =
            currentGameState == GameState.INGAME_INITIAL ||
            currentGameState == GameState.PLAYER_SHOUTING ||
            currentGameState == GameState.SHOW_RESPONSE ||
            currentGameState == GameState.RESULT;

        if (cannotShout) return;

        OnShoutEnter(_shoutType);

        // Play SE
        if (_shoutType == ShoutType.HAPPY_BIRTHDAY)
        {
            shoutDuration = SoundPlayer.Instance.PlayOneShot(SoundTable.SoundName.HAPPY_BIRTHDAY);
        }
        else if (_shoutType == ShoutType.HAPPY_NEW_YEAR)
        {
            shoutDuration = SoundPlayer.Instance.PlayOneShot(SoundTable.SoundName.HAPPY_NEW_YEAR);
        }
        else
        {
            shoutDuration = SoundPlayer.Instance.PlayOneShot(SoundTable.SoundName.MERRY_CHRISTMAS);
        }

        // Progress game sequence (penalty sequence)
        if (currentGameState == GameState.WAITING_ANNOUNCE)
        {
            StartCoroutine(WaitBeforePenalty(shoutDuration));

            StartCoroutine(ForceShout(false));
        }

        // Progress game sequence (normal sequence)
        if (currentGameState == GameState.AFTER_ANNOUNCE)
        {
            StartCoroutine(ForceShout(correctShoutType == _shoutType));
        }
    }

    /// <summary>
    /// The method to be called in case of early shout penalty.
    /// </summary>
    /// <param name="_seconds"></param>
    /// <returns></returns>
    private IEnumerator WaitBeforePenalty(float _seconds)
    {
        // Stop an announcement preparation routine so that it does not terminate over time
        StopCoroutine(announcePreparationCoroutine);

        // Wait until a shout finishes
        yield return new WaitForSeconds(_seconds);

        // After shout finishes, announce preparation sound finishes (drum roll for example)
        SoundPlayer.Instance.Stop();
    }

    /// <summary>
    /// The game time when a correct answer by player was made.
    /// </summary>
    float correctAnswerTime;
    public event Action OnBestScoreUpdated = () => { };
    public event Action<bool, float> OnShoutEnd = (_isWrongShout, _shoutTime) => { };
    private IEnumerator ForceShout(bool _isCorrectAnswer)
    {
        currentGameState = GameState.PLAYER_SHOUTING;

        correctAnswerTime = Time.time;
        Debug.Log("Player starts shouting");
        yield return new WaitForSeconds(shoutDuration);
        Debug.Log("Player finishes shouting");

        float shoutTime = 0.0f;

        if (_isCorrectAnswer)
        {
            Debug.Log("Correct");
            SoundPlayer.Instance.PlayOneShot(SoundTable.SoundName.CORRECT_SHOUT);

            // update best score
            shoutTime = correctAnswerTime - announceTime;
            if (shoutTime < SaveManager.Instance.GetQuickestShoutTime())
            {
                SaveManager.Instance.SetQuickestShoutTime(shoutTime);
                OnBestScoreUpdated();
            }
        }
        else
        {
            Debug.Log("Wrong");
            SoundPlayer.Instance.PlayOneShot(SoundTable.SoundName.WRONG_SHOUT);
        }

        OnShoutEnd(!_isCorrectAnswer, shoutTime);

        StartCoroutine(ShowResponse());
    }

    private IEnumerator ShowResponse()
    {
        currentGameState = GameState.SHOW_RESPONSE;

        Debug.Log("Response start");
        yield return new WaitForSeconds(waitTimeBeforeResult);
        Debug.Log("Response finish");

        ShowResult();
    }

    public event Action OnResultEnter = () => { };
    private void ShowResult()
    {
        OnResultEnter();

        Debug.Log("Result");
        currentGameState = GameState.RESULT;
    }

    public event Action OnBackTitle = () => { };
    public event Action OnGameReset = () => { };
    private void OnEscapeKeyPressed()
    {
        if (currentGameState == GameState.IN_TRANSITION) return;

        SoundPlayer.Instance.Play(SoundTable.SoundName.TRANSITION_BUZZER);
        OnBackTitle();

        if(currentGameState == GameState.TITLE)
        {
            SaveManager.Instance.ResetSaveData();
            OnGameReset();
        }

        currentGameState = GameState.IN_TRANSITION;

        StopAllCoroutines();
    }

    public void OnTransitionToTitleFinish()
    {
        currentGameState = GameState.TITLE;
        SoundPlayer.Instance.Stop();
    }

    /// <summary>
    /// Save some data when game terminates
    /// </summary>
    private void OnDestroy()
    {
        SaveManager.Instance.Save();
    }
}
