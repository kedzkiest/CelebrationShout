using System;

public class UIManager
{
    // Make this class Singleton
    private static UIManager instance;
    public static UIManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new UIManager();
            }

            return instance;
        }
    }

    /// <summary>
    /// The instance of UI containing title elements.
    /// For example, title, instruction, best score, ...
    /// </summary>
    private TitleUI titleUI;

    /// <summary>
    /// The instance of UI containing ingame elements.
    /// For example, focus effect, speech bubble, ...
    /// </summary>
    private InGameUI inGameUI;

    /// <summary>
    /// The instance of UI containing result elements.
    /// For example, response time, flavor text, ...
    /// </summary>
    private ResultUI resultUI;

    /// <summary>
    /// The instance of TransitionAnimation.
    /// Used for registering events on transition animation happens.
    /// </summary>
    private TransitionAnimation transitionAnimation;

    /// <summary>
    /// The instance of SpeechBubbleGenerator.
    /// Only used for initializing it.
    /// </summary>
    private SpeechBubbleGenerator speechBubbleGenerator;

    /// <summary>
    /// The event for starting a transition animation.
    /// Receiver conditions following processes by _nextState, and executes _onTransitionComplete function during transition.
    /// </summary>
    public event Action<GameManager.GameState, Action> OnTransitionEnter = (_nextState, _onTransitionComplete) => { };

    public void Initialize(TitleUI _titleUI, InGameUI _inGameUI, ResultUI _resultUI, TransitionAnimation _transitionAnimation, SpeechBubbleGenerator _speechBubbleGenerator)
    {
        // Set UI instances
        titleUI = _titleUI;
        inGameUI = _inGameUI;
        resultUI = _resultUI;
        transitionAnimation = _transitionAnimation;
        speechBubbleGenerator = _speechBubbleGenerator;

        // Set initial UI state
        titleUI.SetVisible(true);
        inGameUI.SetVisible(false);
        resultUI.SetVisible(false);

        // Register events
        GameManager.Instance.OnGameStart += OnGameStart;
        GameManager.Instance.OnGameRestart += OnGameRestart;
        GameManager.Instance.OnResultEnter += OnResultEnter;
        GameManager.Instance.OnBackTitle += OnBackTitle;
        GameManager.Instance.OnBestScoreUpdated += UpdateBestScore;
        GameManager.Instance.OnGameReset += OnGameReset;

        // Run some initialization methods of compositing components
        titleUI.Initialize();
        resultUI.Initialize();
        transitionAnimation.Initialize();
        speechBubbleGenerator.Initialize();

        // Register event on transition animation happens
        transitionAnimation.OnTransitionProgressEvent += OnTransitionProgress;
    }

    /// <summary>
    /// TITLE -> INGAME_INITIAL
    /// </summary>
    private void OnGameStart()
    {
        OnTransitionEnter(GameManager.GameState.INGAME_INITIAL, SetVisibilityOnGameStart);
    }
    private void SetVisibilityOnGameStart()
    {
        titleUI.SetVisible(false);
        inGameUI.SetVisible(true);
    }

    /// <summary>
    /// RESULT -> INGAME_INITIAL
    /// </summary>
    private void OnGameRestart()
    {
        OnTransitionEnter(GameManager.GameState.INGAME_INITIAL, SetVisibilityOnGameRestart);
    }
    private void SetVisibilityOnGameRestart()
    {
        inGameUI.SetVisible(true);
        resultUI.SetVisible(false);
    }

    /// <summary>
    /// SHOW_RESPONSE -> RESULT
    /// </summary>
    private void OnResultEnter()
    {
        inGameUI.SetVisible(false);
        resultUI.SetVisible(true);
    }

    /// <summary>
    /// Any game state -> TITLE
    /// </summary>
    private void OnBackTitle()
    {
        OnTransitionEnter(GameManager.GameState.TITLE, SetVisibilityOnBackTitle);
    }
    private void SetVisibilityOnBackTitle()
    {
        titleUI.SetVisible(true);
        inGameUI.SetVisible(false);
        resultUI.SetVisible(false);
    }

    private void OnGameReset()
    {
        OnTransitionEnter(GameManager.GameState.TITLE, UpdateBestScore);
    }
    private void UpdateBestScore()
    {
        titleUI.UpdateBestScore();
    }

    public event Action<GameManager.GameState> OnTransitionProgressEvent = (_nextState) => { };
    private void OnTransitionProgress(GameManager.GameState _nextState)
    {
        OnTransitionProgressEvent(_nextState);
    }
}
