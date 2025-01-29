using UnityEngine;

public class AirJump : Jump
{
    protected override void IsJumped(bool jumped)
    {
        isJumped = jumped;
        states.isAirJumped = jumped;
    }
}
