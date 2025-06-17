using UnityEngine;

namespace Animations
{
    [CreateAssetMenu(fileName = "AnimationPreset", menuName = "Animations/AnimationPreset")]
    public class AnimationPreset : ScriptableObject
    {
        [SerializeField] private AnimationClip animation;
        public float blendTime = 0.1f;
        public string AnimationName => animation.name.Split('_')[0];
    }
}