using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class ActionCommand {
    public enum ActionType {
        Move,
        Jump,
        Stop,
        Dash
    }

    public ActionType actionType;
    public float delay; 
    public Vector2 position; // Store the player's position
    public float horizontal;
    public float jumpingPower; 
    public float speed;
    public float dashingPower;
    public float dashingTime;
}
