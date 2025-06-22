using System.Collections.Generic;
using States;
using UnityEngine;

namespace Player
{
    public class SafePositionRecorder : MonoBehaviour
    {
        [SerializeField] private GameObject player;
        [SerializeField] private float delay;
        [SerializeField] private float checkTime;

        private Transform _tf;
        private Queue<Vector3?> _positions;
        private Vector3 _lastStablePosition;
        private int lenght;
        private int checkLenght;
        
        private PlayerStates _states;
        
        public void SetStablePosition(Vector3 position) => _lastStablePosition = position;

        private void Awake()
        {
            _tf = player.transform;
            _positions = new Queue<Vector3?>();
            lenght = (int)(delay / Time.fixedDeltaTime);
            checkLenght = (int)(checkTime / Time.fixedDeltaTime);
            _states = player.GetComponent<PlayerStates>();
        }

        private void FixedUpdate()
        {
            _positions.Enqueue(CheckForStablePosition() ? _tf.position : null);
            
            if (_positions.Count < lenght) return;
            var position = _positions.Dequeue();
            if (position != null) _lastStablePosition = position.Value;
        }

        public Vector3 GetStablePosition()
        {
            for (int i = 0; i < checkLenght; i++)
            {
                var position = _positions.Dequeue();
                if (position != null) return position.Value;
            }
            return _lastStablePosition;
        }
        
        private bool CheckForStablePosition()
        {
            return _states.ground is Ground.Grounded/* && _states.currentAbility is not TakingDamage*/;
        }

        // private struct PositionTime
        // {
        //     public Vector3 Position;
        //     public float Time;
        //
        //     public PositionTime(Vector3 position, float time)
        //     {
        //         this.Position = position;
        //         this.Time = time;
        //     }
        // }
    }
}