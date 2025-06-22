using UnityEngine;
using UnityEngine.SceneManagement;

namespace Map
{
    public class LevelManager : MonoBehaviour
    {
        public void LoadScene(int level)
        {
            SceneManager.LoadScene(level);
        }
        
    }
}