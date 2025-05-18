using System.Collections;
using System.Linq;
using Settings;
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

        private PlayerStates _states;
        private float _coyoteTimeCounter;
        private int _groundContactCount = 0;
        private Coroutine _coyoteTimeRoutine;
        private bool _isStopping;

        private void Awake()
        {
            _states = GetComponent<PlayerStates>();
            _isStopping = false;
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

            _states.ground = Ground.Grounded;
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
#if UNITY_EDITOR
            if (EditorShutdownTracker.IsExitingPlayMode) return;
#endif
            if (_isStopping) return;
            if (collision.gameObject.layer != groundLayer && !EvaluateCollision(collision)) return;
            _groundContactCount--;

            if (_groundContactCount > 0) return;
            _groundContactCount = 0;
            if (_states.ground is Ground.Grounded)
                _coyoteTimeRoutine = StartCoroutine(CoyoteTimeCountdown());
        }

        private IEnumerator CoyoteTimeCountdown()
        {
            _states.ground = Ground.CoyoteTime;
            yield return new WaitForSeconds(coyoteTime);
            if (_states.ground is Ground.CoyoteTime) _states.ground = Ground.Falling;
            _coyoteTimeRoutine = null;
        }

        private bool EvaluateCollision(Collision2D collision)
        {
            return collision.contacts.All(contact => Vector2.Angle(contact.normal, Vector2.up) < groundAngleLimit); // maybe Any
        }

        private void OnDisable()
        {
            if (_coyoteTimeRoutine == null) return;
            StopCoroutine(_coyoteTimeRoutine);
            _coyoteTimeRoutine = null;
        }

        private void OnApplicationQuit()
        {
            _isStopping = true;
        }
    }
}