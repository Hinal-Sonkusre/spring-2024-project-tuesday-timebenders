using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class ActionCommand {
    public enum ActionType {
        MoveLeft,
        MoveRight,
        Jump,
        Dash
    }

    public ActionType actionType;
    public float duration; // Duration of the input action

    // Additional properties for movement and actions
    public float speed;          // Speed for moving left or right
    public float jumpingPower;   // Power for jumping
    public float dashingPower;   // Power for dashing
}
