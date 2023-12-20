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
    public event Action OnBKeyPressed = () => { };

    /// <summary>
    /// Notify the subscribers that space key was pressed.
    /// The event to say Happy New Year.
    /// </summary>
    public event Action OnNKeyPressed = () => { };

    /// <summary>
    /// Notify the subscribers that space key was pressed.
    /// The event to say Merry Christmas.
    /// </summary>
    public event Action OnMKeyPressed = () => { };

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            OnSpaceKeyPressed();
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            OnBKeyPressed();
        }

        if (Input.GetKeyDown(KeyCode.N))
        {
            OnNKeyPressed();
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            OnMKeyPressed();
        }
    }
}
