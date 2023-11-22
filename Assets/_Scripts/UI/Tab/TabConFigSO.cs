using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    [CreateAssetMenu(fileName = "TabConfig", menuName = "ScriptableObjects/UI/TabConfig", order = 1)]
    public class TabConFigSO : ScriptableObject
    {
        [field: SerializeField] public Color TabIdleIconColor { get; private set; }
        [field: SerializeField] public Color TabSelectedIconColor { get; private set; }
        [field: SerializeField] public Vector2 TabIdleIconSize { get; private set; }
        [field: SerializeField] public Vector2 TabSelectedIconSize { get; private set; }
        [field: SerializeField] public Vector2 TabIdleBackgroundSize { get; private set; }
        [field: SerializeField] public Vector2 TabSelectedBackgroundSize { get; private set; }
        [field: SerializeField] public Color TabIdleBackgroundColor { get; private set; }
        [field: SerializeField] public Color TabSelectedBackgroundColor { get; private set; }
    }
}

