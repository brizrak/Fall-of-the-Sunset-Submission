using UnityEngine;

public class AirJump : Jump
{
    protected override void IsJumping(bool jumped)
    {
        isJumping = jumped;
        states.isAirJumped = jumped;
    }
}
