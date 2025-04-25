using System.Collections;
using UnityEngine;

namespace Player.Scripts.States
{
    [RequireComponent(typeof(PlayerStates))]
    public class GroundCheck : MonoBehaviour
    {
        [SerializeField] private LayerMask groundLayer;
        [SerializeField] private float coyoteTime;
        [SerializeField] private Collider2D bottomCheck;
        [SerializeField] private float groundAngleLimit;

        [HideInInspector] public PlayerStates states;
        private float _coyoteTimeCounter;
        private int _groundContactCount = 0;
        private Coroutine _coyoteTimeRoutine;

        private void Awake()
        {
            states = GetComponent<PlayerStates>();
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.layer != groundLayer && !EvaluateCollision(collision)) return;
            _groundContactCount++;

            if (_coyoteTimeRoutine != null)
            {
                StopCoroutine(_coyoteTimeRoutine);
                _coyoteTimeRoutine = null;
            }

            states.ground = GroundState.Grounded;
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            if (collision.gameObject.layer != groundLayer && !EvaluateCollision(collision)) return;
            _groundContactCount--;

            if (_groundContactCount > 0) return;
            _groundContactCount = 0;
            _coyoteTimeRoutine = StartCoroutine(CoyoteTimeCountdown());
        }

        private IEnumerator CoyoteTimeCountdown()
        {
            states.ground = GroundState.CoyoteTime;
            yield return new WaitForSeconds(coyoteTime);
            states.ground = GroundState.Falling;
            _coyoteTimeRoutine = null;
        }

        private bool EvaluateCollision(Collision2D collision)
        {
            return true;
            // Проверить
            // return collision.contacts.Any(contact => Vector2.Angle(contact.normal, Vector2.up) < groundAngleLimit);
        }

        private void OnDisable()
        {
            if (_coyoteTimeRoutine == null) return;
            StopCoroutine(_coyoteTimeRoutine);
            _coyoteTimeRoutine = null;
        }
    }
}