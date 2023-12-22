using UnityEngine;
using UnityEngine.UI;

public class SpeechBubbleGenerator : MonoBehaviour
{
    [SerializeField]
    private Transform canvas;

    [SerializeField]
    private GameObject HappyBirthdayBubble;

    [SerializeField]
    private GameObject HappyNewYearBubble;

    [SerializeField]
    private GameObject MerryChristmasBubble;

    [SerializeField]
    private AnimationClip SpeechBubbleAnimation;

    // The values to decide the x position of instantiated bubbles.
    // The bubbles hardly cover character face within this range.
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
            GameObject bubble = Instantiate(HappyBirthdayBubble);

            SetupBubble(bubble);
        }
        else if(_shoutType == GameManager.ShoutType.HAPPY_NEW_YEAR)
        {
            GameObject bubble = Instantiate(HappyNewYearBubble);

            SetupBubble(bubble);
        }
        else if(_shoutType == GameManager.ShoutType.MERRY_CHRISTMAS)
        {
            GameObject bubble = Instantiate(MerryChristmasBubble);

            SetupBubble(bubble);
        }
    }

    private void SetupBubble(GameObject _bubble)
    {
        _bubble.transform.SetParent(canvas);
        _bubble.transform.localScale = Vector3.one;

        // first choose left or right by Random.Range(0, 1)
        // if left chosen (<= 0.5f), choose x from the range [-600, -300].
        // if right chosen (> 0.5f), choose x from the range [300, 600].
        float instantiatePositionX = Random.Range(0f, 1f) <= 0.5f ?
            Random.Range(MIN_X_ON_LEFT_GENERATION, MAX_X_ON_LEFT_GENERATION) :
            Random.Range(MIN_X_ON_RIGHT_GENERATION, MAX_X_ON_RIGHT_GENERATION);

        Debug.LogWarning(instantiatePositionX);

        _bubble.transform.localPosition = new Vector3
        (
            x: instantiatePositionX,
            y: 0,
            z: 0
        );

        Destroy(_bubble, SpeechBubbleAnimation.length);
    }
}
