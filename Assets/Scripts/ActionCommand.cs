using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum ActionType {
    MoveLeft,
    MoveRight,
    Jump,
    Dash
}

[System.Serializable]
public class ActionCommand {
    public ActionType actionType;
    public float startTime;
    public float stopTime;

    public ActionCommand(ActionType type, float startTime) {
        actionType = type;
        this.startTime = startTime;
        this.stopTime = -1f;
    }

    public ActionCommand(ActionType type, float startTime, float stopTime) {
        actionType = type;
        this.startTime = startTime;
        this.stopTime = stopTime;
    }

    public void SetStopTime(float time) {
        stopTime = time;
    }
}
