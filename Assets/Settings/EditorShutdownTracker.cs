using UnityEditor;
using UnityEngine;

namespace Settings
{
#if UNITY_EDITOR
    [InitializeOnLoad]
    public static class EditorShutdownTracker
    {
        static EditorShutdownTracker()
        {
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
        }

        private static void OnPlayModeStateChanged(PlayModeStateChange state)
        {
            if (state == PlayModeStateChange.ExitingPlayMode)
            {
                IsExitingPlayMode = true;
            }
        }
        
        public static bool IsExitingPlayMode { get; private set; } = false;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void OnStartup()
        {
            IsExitingPlayMode = false;
        }
    }
#endif
}