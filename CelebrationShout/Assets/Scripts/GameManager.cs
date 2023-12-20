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

    public GameState currentGameState { get; private set; }

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

    private new void Awake()
    {
        titleUI = FindObjectOfType<TitleUI>();
        //inGameUI = FindObjectOfType<InGameUI>();
        //resultUI = FindObjectOfType<ResultUI>();

        Initialize();

        userInputHandler.OnSpaceKeyPressed += OnSpaceKeyPressed;
        userInputHandler.OnBKeyPressed += ForceHappyBirthdayShout;
        userInputHandler.OnNKeyPressed += ForceHappyNewYearShout;
        userInputHandler.OnMKeyPressed += ForceMerryChristmasShout;
        userInputHandler.OnEscapeKeyPressed += ResetGame;
    }

    /// <summary>
    /// Set game state and UI to initial ones.
    /// </summary>
    private void Initialize()
    {
        currentGameState = GameState.TITLE;

        titleUI.SetVisible(true);
        //inGameUI.SetVisible(false);
        //resultUI.SetVisible(false);
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

        StartCoroutine(WaitBeforeAnnouncePreparation(1.0f));
    }

    private IEnumerator WaitBeforeAnnouncePreparation(float _seconds)
    {
        Debug.Log("Wait Start");
        yield return new WaitForSeconds(_seconds);
        Debug.Log("Wait Finish");

        StartCoroutine(DoAnnouncePreparation());
    }

    private IEnumerator DoAnnouncePreparation()
    {
        currentGameState = GameState.WAITING_ANNOUNCE;

        Debug.Log("Preparation Start");
        yield return new WaitForSeconds(1);
        Debug.Log("Preparation Finish");

        currentGameState = GameState.AFTER_ANNOUNCE;
        Debug.Log("Waiting for player input...");
    }

    private void ForceHappyBirthdayShout()
    {
        if (currentGameState == GameState.AFTER_ANNOUNCE)
        {
            StartCoroutine(ForceShout());
        }
    }

    private void ForceHappyNewYearShout()
    {
        if (currentGameState == GameState.AFTER_ANNOUNCE)
        {
            StartCoroutine(ForceShout());
        }
    }

    private void ForceMerryChristmasShout()
    {
        if (currentGameState == GameState.AFTER_ANNOUNCE)
        {
            StartCoroutine(ForceShout());
        }
    }

    private IEnumerator ForceShout()
    {
        currentGameState = GameState.PLAYER_SHOUTING;

        Debug.Log("Player starts shouting");
        yield return new WaitForSeconds(1);
        Debug.Log("Player finishes shouting");

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

        //inGameUI.SetVisible(false);
        //resultUI.SetVisible(true);
    }

    private void ResetGame()
    {
        Initialize();
    }
}
