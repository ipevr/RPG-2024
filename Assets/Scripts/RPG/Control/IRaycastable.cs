﻿using UnityEngine;

namespace RPG.Control
{
    public interface IRaycastable
    {
        bool HandleRaycast(PlayerController playerController);
        CursorType GetCursorType();
    }
}