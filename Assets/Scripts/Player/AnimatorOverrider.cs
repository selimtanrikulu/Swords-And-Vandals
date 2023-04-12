using System.Collections.Generic;
using UnityEngine;

public class AnimatorOverrider : MonoBehaviour
{

    private AnimatorOverrideController animatorOverrideController;

    private void Start()
    {
        animatorOverrideController = GetComponent<Animator>().runtimeAnimatorController as AnimatorOverrideController;
    }

    public void SetAnimation(string stateName,AnimationClip animationClip)
    {
        List<KeyValuePair<AnimationClip, AnimationClip>> overrides = new List<KeyValuePair<AnimationClip, AnimationClip>>();
        animatorOverrideController.GetOverrides(overrides);
        var keyValuePair = overrides.Find(x => x.Key.name == stateName);
        overrides.Add(new KeyValuePair<AnimationClip, AnimationClip>(keyValuePair.Key,animationClip));
        overrides.Remove(keyValuePair);
        animatorOverrideController.ApplyOverrides(overrides);
    }
}
