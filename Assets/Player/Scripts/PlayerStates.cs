using UnityEngine;

public class PlayerStates : MonoBehaviour
{
    public bool isCanMove;
    public bool slideIsUnlocked;
    public bool airJumpIsUnlocked;
    public bool dashIsUnlocked;

    [HideInInspector] public bool isGrounded;
    [HideInInspector] public bool isJumped = false;
    [HideInInspector] public bool isAirJumped = false;
    [HideInInspector] public bool isWallJumped = false;
    [HideInInspector] public Sides isSlide;
    [HideInInspector] public Sides viewSide;
    [HideInInspector] public bool isDashing;
    [HideInInspector] public bool isCanAirJump;
    [HideInInspector] public bool isCanDash;

    public enum Sides
    {
        left,
        right,
        none,
    }
}
