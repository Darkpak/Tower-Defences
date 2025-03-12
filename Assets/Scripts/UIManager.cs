using System;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager main;

    private bool _isHoveringUI;

    private void Awake()
    {
        main = this;
    }

    public void SetHoveringState(bool state)
    {
        _isHoveringUI = state;
    }

    public bool IsHoveringUI()
    {
        return _isHoveringUI;
    }
}
