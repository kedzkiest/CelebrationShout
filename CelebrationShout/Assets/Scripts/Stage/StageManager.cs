using UnityEngine;

public class StageManager
{
    // Make this class Singleton
    private static StageManager instance;
    public static StageManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new StageManager();
            }

            return instance;
        }
    }

    private GameObject characterObject;
    private IStageCharacter character;

    const string HAPPY_BIRTHDAY_STAGE_PREFAB_NAME = "HappyBirthDayStage";
    private HappyBirthdayStage happyBirthdayStage;

    const string HAPPY_NEW_YEAR_STAGE_PREFAB_NAME = "HappyNewYearStage";
    private HappyNewYearStage happyNewYearStage;

    const string MERRY_CHRISTMAS_STAGE_PREFAB_NAME = "MerryChristmasStage";
    private MerryChristmasStage merryChristmasStage;

    [SerializeField]
    private Light stageLight;

    public void Initialize(GameObject _characterObject, HappyBirthdayStage _happyBirthDayStage,
        HappyNewYearStage _happyNewYearStage, MerryChristmasStage _merryChristmasStage, Light _stageLight)
    {
        // Initialize stage character, start with inactive state
        characterObject = _characterObject;
        characterObject.SetActive(false);
        character = characterObject.GetComponent<IStageCharacter>();

        // Initialize stage elements, start with inactive state
        happyBirthdayStage = _happyBirthDayStage;
        happyBirthdayStage.gameObject.SetActive(false);

        happyNewYearStage = _happyNewYearStage;
        happyNewYearStage.gameObject.SetActive(false);

        merryChristmasStage = _merryChristmasStage;
        merryChristmasStage.gameObject.SetActive(false);

        // Initialize stage lighting
        stageLight = _stageLight;

        UIManager.Instance.OnTransitionProgressEvent += SetupStageBeforeAnnouncement;
        GameManager.Instance.OnAnnounceMade += SetupStageAfterAnnouncement;
        GameManager.Instance.OnShoutEnd += SetupStageAfterShout;
    }

    /// <summary>
    /// Process stage setup that we want it to happen during the transition to ingame state.
    /// For example, default character/stage/light state, etc..
    /// </summary>
    /// <param name="_nextState"></param>
    private void SetupStageBeforeAnnouncement(GameManager.GameState _nextState)
    {
        // character put off santa hat, not wearing a hat is its initial state
        character.WearHat(false);
        // character reset every emotion it have, having no emotion is its initial state
        character.FeelNeutral();

        // every stages are inactive at the moment game starts
        happyBirthdayStage.gameObject.SetActive(false);
        happyNewYearStage.gameObject.SetActive(false);
        merryChristmasStage.gameObject.SetActive(false);

        // light is off at the moment game starts, and its color is white by default
        stageLight.enabled = false;
        stageLight.color = Color.white;

        // On back title transition
        if (_nextState == GameManager.GameState.TITLE)
        {
            characterObject.SetActive(false);
            return;
        }

        // On game start/restart transition
        characterObject.SetActive(true);
    }

    /// <summary>
    /// Process stage setup that we want it to happen after the announcement of certain event.
    /// For example, character/stage/light state after an announcement, etc..
    /// </summary>
    private void SetupStageAfterAnnouncement()
    {
        // light is on when an announce is made
        stageLight.enabled = true;

        // determine which stage to activate, then activate it
        GameManager.ShoutType correctShout = GameManager.Instance.correctShoutType;
        if (correctShout == GameManager.ShoutType.HAPPY_BIRTHDAY)
        {
            happyBirthdayStage.gameObject.SetActive(true);
        }
        else if (correctShout == GameManager.ShoutType.HAPPY_NEW_YEAR)
        {
            happyNewYearStage.gameObject.SetActive(true);
        }
        else if (correctShout == GameManager.ShoutType.MERRY_CHRISTMAS)
        {
            // character want to wear a santa hat on christmas
            character.WearHat(true);

            merryChristmasStage.gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// Process stage setup that we want it to happen after the announcement of certain event.
    /// For example, resulting character/stage/light state after player's shout.
    /// </summary>
    /// <param name="_isWrongShout"></param>
    /// <param name="_"></param>
    private void SetupStageAfterShout(bool _isWrongShout, float _)
    {
        // in case shout before announcement happens
        stageLight.enabled = true;

        if (_isWrongShout)
        {
            character.FeelSad();
            stageLight.color = Color.blue;
        }
        else
        {
            character.FeelHappy();
            stageLight.color = Color.yellow;
        }
    }
}
