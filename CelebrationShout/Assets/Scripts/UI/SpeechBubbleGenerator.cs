using UnityEngine;

public class SpeechBubbleGenerator : MonoBehaviour
{
    /// <summary>
    /// The UI canvas to draw speech bubbles.
    /// </summary>
    [SerializeField]
    private Transform canvas;

    /// <summary>
    /// The prefab of happy birthday speech bubble.
    /// </summary>
    [SerializeField]
    private GameObject happyBirthdayBubble;

    /// <summary>
    /// The prefab of happy new year speech bubble.
    /// </summary>
    [SerializeField]
    private GameObject happyNewYearBubble;

    /// <summary>
    /// The prefab of merry christmas speech bubble.
    /// </summary>
    [SerializeField]
    private GameObject merryChristmasBubble;

    /// <summary>
    /// The animation clip used for speech bubble generation.
    /// Used for adjusting the timing to destroy generated speech bubbles.
    /// </summary>
    [SerializeField]
    private AnimationClip speechBubbleAnimation;

    // The values to decide the x position of instantiated bubbles.
    // The bubbles hardly cover the stage center (where character face exists for example) within this range.
    const float MIN_X_ON_LEFT_GENERATION = -600f;
    const float MAX_X_ON_LEFT_GENERATION = -300f;
    const float MIN_X_ON_RIGHT_GENERATION = 300f;
    const float MAX_X_ON_RIGHT_GENERATION = 600f;

    public void Initialize()
    {
        GameManager.Instance.OnShoutEnter += Generate;
    }

    private void Generate(GameManager.ShoutType _shoutType)
    {
        if(_shoutType == GameManager.ShoutType.HAPPY_BIRTHDAY)
        {
            GameObject bubble = Instantiate(happyBirthdayBubble);

            SetupBubble(bubble);
        }
        else if(_shoutType == GameManager.ShoutType.HAPPY_NEW_YEAR)
        {
            GameObject bubble = Instantiate(happyNewYearBubble);

            SetupBubble(bubble);
        }
        else if(_shoutType == GameManager.ShoutType.MERRY_CHRISTMAS)
        {
            GameObject bubble = Instantiate(merryChristmasBubble);

            SetupBubble(bubble);
        }
    }

    private void SetupBubble(GameObject _bubble)
    {
        // Set bubble's parent to canvas for drawing as UI
        _bubble.transform.SetParent(canvas);

        // first choose left or right by Random.Range(0, 1)
        // if left chosen (<= 0.5f), choose x from the range [-600, -300].
        // if right chosen (> 0.5f), choose x from the range [300, 600].
        float instantiatePositionX = Random.Range(0f, 1f) <= 0.5f ?
            Random.Range(MIN_X_ON_LEFT_GENERATION, MAX_X_ON_LEFT_GENERATION) :
            Random.Range(MIN_X_ON_RIGHT_GENERATION, MAX_X_ON_RIGHT_GENERATION);

        // Set bubble's spawn position
        _bubble.transform.localPosition = new Vector3
        (
            x: instantiatePositionX,
            y: 0,
            z: 0
        );

        // Destroy generated bubbles after its animation finishes
        Destroy(_bubble, speechBubbleAnimation.length);
    }
}
