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
        characterObject = _characterObject;
        characterObject.SetActive(false);
        character = characterObject.GetComponent<IStageCharacter>();

        happyBirthdayStage = _happyBirthDayStage;
        happyNewYearStage = _happyNewYearStage;
        merryChristmasStage = _merryChristmasStage;

        stageLight = _stageLight;

        happyBirthdayStage.gameObject.SetActive(false);
        happyNewYearStage.gameObject.SetActive(false);
        merryChristmasStage.gameObject.SetActive(false);

        UIManager.Instance.OnTransitionProgressEvent += SetupStageBeforeAnnouncement;
        GameManager.Instance.OnAnnounceMade += SetupStageAfterAnnouncement;
        GameManager.Instance.OnShoutEnd += SetupCharacterAfterShout;
    }

    private void SetupStageBeforeAnnouncement(GameManager.GameState _nextState)
    {
        character.WearHat(false);
        character.FeelNeutral();
        happyBirthdayStage.gameObject.SetActive(false);
        happyNewYearStage.gameObject.SetActive(false);
        merryChristmasStage.gameObject.SetActive(false);
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

    private void SetupStageAfterAnnouncement()
    {
        stageLight.enabled = true;

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
            character.WearHat(true);
            merryChristmasStage.gameObject.SetActive(true);
        }
    }

    private void SetupCharacterAfterShout(bool _isWrongShout, float _)
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
