using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class ActionCommand {
    public enum ActionType {
        Move,
        Jump,
        Stop
    }

    public ActionType actionType;
    public float delay; 
    public Vector2 position; // Store the player's position
    public float horizontal;
    public float jumpingPower; 
    public float speed;
}
