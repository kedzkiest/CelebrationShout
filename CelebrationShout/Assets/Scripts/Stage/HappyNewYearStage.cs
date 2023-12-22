using System.Collections.Generic;
using UnityEngine;

public class HappyNewYearStage : MonoBehaviour
{
    /// <summary>
    /// The gameobjects that consists the stage for happy birthday situation.
    /// For example, kagami mochi (layered rice cake), text shows new year (2024 at this moment), etc..
    /// </summary>
    [SerializeField]
    List<GameObject> elements = new List<GameObject>();
}
