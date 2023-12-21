using System;
using System.Collections;
using UnityEngine;

public class TransitionAnimation : MonoBehaviour
{
    /// <summary>
    /// The transition animation between TITLE -> INGAME_INITIAL, etc..
    /// </summary>
    [SerializeField]
    private Animator transitionAnimator;

    /// <summary>
    /// The time span between transition starts and ends.
    /// Used for adjusting animator on/off timing.
    /// </summary>
    private float transitionDuration;

    const string TransitionAnimationClipPath = "Animations/TransitionAnimation";

    /// <summary>
    /// The game state after a transition is made.
    /// Used for conditioning following processes.
    /// </summary>
    private GameManager.GameState nextState;

    /// <summary>
    /// The fucntion that is executed during the transition.
    /// </summary>
    private Action onBackGroundUpdate;

    public void Initialize()
    {
        AnimationClip clip = (AnimationClip)(Resources.Load(TransitionAnimationClipPath));
        transitionDuration = clip.length;

        UIManager.Instance.OnTransition += Invoke;
    }

    public void Invoke(GameManager.GameState _nextState, Action _onTransitionComplete)
    {
        nextState = _nextState;

        // The UI change after the transition is indeed executed between the transition
        onBackGroundUpdate = _onTransitionComplete;

        StartCoroutine(DoTransition());
    }

    private IEnumerator DoTransition()
    {
        // Animator ignition is done by on/off animator component
        transitionAnimator.enabled = true;

        yield return new WaitForSeconds(transitionDuration);

        // Reset the animator state so that it can run again
        transitionAnimator.Rebind();
        transitionAnimator.enabled = false;
    }

    /// <summary>
    /// The process executed during the transition.
    /// Caller is the animation event of transition animation.
    /// </summary>
    public void OnBackgroundUpdate()
    {
        onBackGroundUpdate.Invoke();
    }

    /// <summary>
    /// The process executed after the transition.
    /// Caller is the animation event of transition animation.
    /// </summary>
    public void OnTransitionComplete()
    {
        if(nextState == GameManager.GameState.INGAME_INITIAL)
        {
            GameManager.Instance.OnTransitionToInGameFinish();
        }

        if(nextState == GameManager.GameState.TITLE)
        {
            GameManager.Instance.OnTransitionToTitleFinish();
        }
    }
}
