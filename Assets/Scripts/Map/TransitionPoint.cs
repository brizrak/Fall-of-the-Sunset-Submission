using UnityEngine;

namespace Map
{
    public class TransitionPoint : MonoBehaviour
    {
        [SerializeField] private int nextSceneIndex;
        [SerializeField] private LevelManager levelManager;
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;
            levelManager.LoadScene(nextSceneIndex);
        }
    }
}