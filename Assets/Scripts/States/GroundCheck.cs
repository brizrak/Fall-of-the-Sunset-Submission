using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Settings;

namespace States
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
        private HashSet<Collider2D> _colliders;

        private void Awake()
        {
            _states = GetComponent<PlayerStates>();
            _isStopping = false;
            _colliders = new HashSet<Collider2D>();
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (!LayerComparison(groundLayer, collision.gameObject.layer) || !EvaluateCollision(collision)) return;
            if (!_colliders.Add(collision.collider)) return;
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
            if (!LayerComparison(groundLayer, collision.gameObject.layer)) return;
            if (!_colliders.Remove(collision.collider)) return;
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
            return collision.contacts.Any(contact => Vector2.Angle(contact.normal, Vector2.up) < groundAngleLimit);
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

        private static bool LayerComparison(LayerMask mask, int layer)
        {
            return (mask.value & (1 << layer)) != 0;
        }
    }
}