using System;
using System.Collections;
using UnityEngine;

public class GameManager : SingletonMonoBehaviour<GameManager>
{
    public enum GameState
    {
        TITLE,              // View showing the game title
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
    private ShoutType correctShoutType;

    /// <summary>
    /// The instance of UI containing title elements.
    /// For example, title, instruction, best score, ...
    /// </summary>
    [SerializeField]
    private ISceneUI titleUI;

    /// <summary>
    /// The instance of UI containing ingame elements.
    /// For example, focus effect, speech bubble, ...
    /// </summary>
    [SerializeField]
    private ISceneUI inGameUI;

    /// <summary>
    /// The instance of UI containing result elements.
    /// For example, response time, flavor text, ...
    /// </summary>
    [SerializeField]
    private ISceneUI resultUI;

    /// <summary>
    /// The instance that receives inputs from user and publishes them as events.
    /// </summary>
    [SerializeField]
    private UserInputHandler userInputHandler;

    /// <summary>
    /// Minimun time from the start of counting until the answer model appears.
    /// </summary>
    [SerializeField]
    private float minPreparationTime;

    /// <summary>
    /// Maximun time from the start of counting until the answer model appears.
    /// </summary>
    [SerializeField]
    private float maxPreparationTime;

    private new void Awake()
    {
        InitializeOnce();
        Initialize();
    }

    /// <summary>
    /// Initialization performed only once at the beginning of the game.
    /// </summary>
    private void InitializeOnce()
    {
        // Initialie savedata
        SaveManager.Instance.Initialize();

        // Initialize Sound
        SoundPlayer.Instance.Initialize();

        // Set UI instances
        titleUI = FindObjectOfType<TitleUI>();
        titleUI.Initialize();

        inGameUI = FindObjectOfType<InGameUI>();
        resultUI = FindObjectOfType<ResultUI>();

        // Subscribe user input events
        userInputHandler.OnSpaceKeyPressed += OnSpaceKeyPressed;
        userInputHandler.OnBKeyPressed += OnShoutKeyPressed;
        userInputHandler.OnNKeyPressed += OnShoutKeyPressed;
        userInputHandler.OnMKeyPressed += OnShoutKeyPressed;
        userInputHandler.OnEscapeKeyPressed += ResetGame;    
    }

    /// <summary>
    /// Set game state and UI to initial ones.
    /// May be executed multiple times.
    /// </summary>
    private void Initialize()
    {
        currentGameState = GameState.TITLE;

        titleUI.SetVisible(true);
        inGameUI.SetVisible(false);
        resultUI.SetVisible(false);
    }

    /// <summary>
    /// Start the game from TITLE and RESULT sequence.
    /// </summary>
    private void OnSpaceKeyPressed()
    {
        if(currentGameState == GameState.TITLE)
        {
            inGameUI.SetVisible(true);
            titleUI.SetVisible(false);

            StartGame();
        }

        if(currentGameState == GameState.RESULT)
        {
            inGameUI.SetVisible(true);
            resultUI.SetVisible(false);

            StartGame();
        }
    }

    private void StartGame()
    {
        currentGameState = GameState.INGAME_INITIAL;
        correctShoutType = (ShoutType)UnityEngine.Random.Range(0, Enum.GetValues(typeof(ShoutType)).Length);
        Debug.Log("correctShoutType: " + correctShoutType);

        StartCoroutine(WaitBeforeAnnouncePreparation(1.0f));
    }

    Coroutine announcePreparationCoroutine;
    private IEnumerator WaitBeforeAnnouncePreparation(float _seconds)
    {
        Debug.Log("Wait Start");
        yield return new WaitForSeconds(_seconds);
        Debug.Log("Wait Finish");

        announcePreparationCoroutine = StartCoroutine(DoAnnouncePreparation());
    }

    /// <summary>
    /// The game time when a celebration announce was made.
    /// </summary>
    float announceTime;
    private IEnumerator DoAnnouncePreparation()
    {
        currentGameState = GameState.WAITING_ANNOUNCE;

        Debug.Log("Preparation Start");
        SoundPlayer.Instance.Play(SoundTable.SoundName.DRUM_ROLL);
        yield return new WaitForSeconds(UnityEngine.Random.Range(minPreparationTime, maxPreparationTime));
        Debug.Log("Preparation Finish");
        announceTime = Time.time;
        Debug.Log(announceTime);
        SoundPlayer.Instance.Stop();
        SoundPlayer.Instance.PlayOneShot(SoundTable.SoundName.DRUM_ROLL_FINISH);

        currentGameState = GameState.AFTER_ANNOUNCE;
        Debug.Log("Waiting for player input...");
    }

    /// <summary>
    /// How much time player shout last.
    /// Used for adjusting judge timing
    /// </summary>
    float shoutDuration;
    private void OnShoutKeyPressed(ShoutType _shoutType)
    {
        bool cannotShout =
            currentGameState == GameState.INGAME_INITIAL ||
            currentGameState == GameState.PLAYER_SHOUTING ||
            currentGameState == GameState.SHOW_RESPONSE ||
            currentGameState == GameState.RESULT;

        if (cannotShout) return;

        // Play SE
        if(_shoutType == ShoutType.HAPPY_BIRTHDAY)
        {
            shoutDuration = SoundPlayer.Instance.PlayOneShot(SoundTable.SoundName.HAPPY_BIRTHDAY);
        }
        else if(_shoutType == ShoutType.HAPPY_NEW_YEAR)
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

    private IEnumerator WaitBeforePenalty(float _seconds)
    {
        StopCoroutine(announcePreparationCoroutine);

        yield return new WaitForSeconds(_seconds);

        SoundPlayer.Instance.Stop();
    }

    /// <summary>
    /// The game time when a correct answer by player was made.
    /// </summary>
    float correctAnswerTime;
    public event Action OnBestScoreUpdated = () => { };
    private IEnumerator ForceShout(bool _isCorrectAnswer)
    {
        currentGameState = GameState.PLAYER_SHOUTING;

        correctAnswerTime = Time.time;
        Debug.Log("Player starts shouting");
        yield return new WaitForSeconds(shoutDuration);
        Debug.Log("Player finishes shouting");

        if(_isCorrectAnswer)
        {
            Debug.Log("Correct");
            SoundPlayer.Instance.PlayOneShot(SoundTable.SoundName.CORRECT_SHOUT);

            // update best score
            float shoutTime = correctAnswerTime - announceTime;
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

        StartCoroutine(ShowResponse());
    }

    private IEnumerator ShowResponse()
    {
        currentGameState = GameState.SHOW_RESPONSE;

        Debug.Log("Response start");
        yield return new WaitForSeconds(1);
        Debug.Log("Response finish");

        ShowResult();
    }

    private void ShowResult()
    {
        Debug.Log("Result");
        currentGameState = GameState.RESULT;

        inGameUI.SetVisible(false);
        resultUI.SetVisible(true);
    }

    public event Action OnGameReset = () => { };
    private void ResetGame()
    {
        if (currentGameState == GameState.TITLE)
        {
            SaveManager.Instance.ResetSaveData();
            OnGameReset();
        }

        SoundPlayer.Instance.Stop();
        StopAllCoroutines();
        Initialize();
    }

    /// <summary>
    /// Save some data when game terminates
    /// </summary>
    private void OnDestroy()
    {
        SaveManager.Instance.Save();
    }
}
