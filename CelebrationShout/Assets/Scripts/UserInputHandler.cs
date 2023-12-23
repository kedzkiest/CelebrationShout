using System;
using UnityEngine;

public class UserInputHandler : MonoBehaviour
{
    /// <summary>
    /// Notify the subscribers that space key was pressed.
    /// The event to start the game.
    /// </summary>
    public event Action OnSpaceKeyPressed = () => { };

    /// <summary>
    /// Notify the subscribers that B key was pressed.
    /// The event to say Happy Birthday.
    /// </summary>
    public event Action<GameManager.ShoutType> OnBKeyPressed = (_shoutType) => { };

    /// <summary>
    /// Notify the subscribers that space key was pressed.
    /// The event to say Happy New Year.
    /// </summary>
    public event Action<GameManager.ShoutType> OnNKeyPressed = (_shoutType) => { };

    /// <summary>
    /// Notify the subscribers that space key was pressed.
    /// The event to say Merry Christmas.
    /// </summary>
    public event Action<GameManager.ShoutType> OnMKeyPressed = (_shoutType) => { };

    /// <summary>
    /// Notify the subscribers that space key was pressed.
    /// The event to reset game.
    /// </summary>
    public event Action OnEscapeKeyPressed = () => { };

    /// <summary>
    /// The cooltime to avoid sequential escape key input.
    /// </summary>
    const float ESCAPE_KEY_EVENT_COOLTIME = 3.0f;
    private float elapsedTimeFromPreviousEscapeKeyPress;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            OnSpaceKeyPressed();
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            OnBKeyPressed(GameManager.ShoutType.HAPPY_BIRTHDAY);
        }

        if (Input.GetKeyDown(KeyCode.N))
        {
            OnNKeyPressed(GameManager.ShoutType.HAPPY_NEW_YEAR);
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            OnMKeyPressed(GameManager.ShoutType.MERRY_CHRISTMAS);
        }

        elapsedTimeFromPreviousEscapeKeyPress += Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (elapsedTimeFromPreviousEscapeKeyPress > ESCAPE_KEY_EVENT_COOLTIME)
            {
                elapsedTimeFromPreviousEscapeKeyPress = 0;
                OnEscapeKeyPressed();
            }
        }
    }
}
