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
    public float horizontal; // Record horizontal input
    public bool jumpPressed; // Record if jump was pressed
    public float speed;
    public float jumpingPower; 
    public float dashingPower;
    public float dashingTime;
}
