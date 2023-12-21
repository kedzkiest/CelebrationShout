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

    public void Initialize(TitleUI _titleUI, InGameUI _inGameUI, ResultUI _resultUI)
    {
        // Set UI instances
        titleUI = _titleUI;
        inGameUI = _inGameUI;
        resultUI = _resultUI;

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
        GameManager.Instance.OnGameReset += UpdateBestScore;

        titleUI.Initialize();
    }

    /// <summary>
    /// TITLE -> INGAME_INITIAL
    /// </summary>
    private void OnGameStart()
    {
        titleUI.SetVisible(false);
        inGameUI.SetVisible(true);
    }

    /// <summary>
    /// RESULT -> INGAME_INITIAL
    /// </summary>
    private void OnGameRestart()
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
        titleUI.SetVisible(true);
        inGameUI.SetVisible(false);
        resultUI.SetVisible(false);
    }

    private void UpdateBestScore()
    {
        titleUI.UpdateBestScore();
    }
}
