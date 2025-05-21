using UnityEngine;
using Player.States;

namespace Player.Movement
{
    public class CheckForTouch : MonoBehaviour
    {
        [SerializeField] private LayerMask groundLayer;
        [SerializeField] private Collider2D leftBotCheck;
        [SerializeField] private Collider2D leftTopCheck;
        [SerializeField] private Collider2D rightBotCheck;
        [SerializeField] private Collider2D rightTopCheck;

        public Direction? IsTouchingWall()
        {
            if (leftBotCheck.IsTouchingLayers(groundLayer) || leftTopCheck.IsTouchingLayers(groundLayer))
            {
                return Direction.Left;
            }

            if (rightBotCheck.IsTouchingLayers(groundLayer) || rightTopCheck.IsTouchingLayers(groundLayer))
            {
                return Direction.Right;
            }

            return null;
        }
    }
}