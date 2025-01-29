using UnityEngine;

public class PlayerStates : MonoBehaviour
{
    public bool isCanSlide = false;
    public bool isCanDoubleJump = false;
    public bool isCanMove = true;

    [HideInInspector] public bool isGrounded = false;
    [HideInInspector] public bool isJumped = false;
    [HideInInspector] public bool isAirJumped = false;
    [HideInInspector] public bool isWallJumped = false;
    [HideInInspector] public Sides isSlide;

    //[Header("Checkers")]
    //[SerializeField] private Collider2D topCheck;

    public enum Sides
    {
        left,
        right,
        none,
    }
}
