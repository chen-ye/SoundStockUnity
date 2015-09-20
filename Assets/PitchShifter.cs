using UnityEngine;
using System.Collections;
using GAudio;
using System;
using Utility;

namespace SoundStock
{
    public class PitchShifter : MonoBehaviour
    {
        //We also need a ref. to the granular pattern to pitch shift it
        public PulsedPatternModule granularPattern;

        public float pitchMax = -15;
        public float pitchMin = -40;
        private float pitchMid;
        private float pitchDiff;
        // Clamped between 0.0 and 1.0

        [Range(-1.0f, 1.0f)]
        public float metric = 0.5F;
        private float clampedMetric;
        public float currentPitch;
        public float targetPitch;

        //IEnumerator TweenGranularPitch(float duration)
        //{
        //    print(targetPitch);

        //    float factor = 1f / duration;

        //    float val = 0f;

        //    float fromPitch = granularPattern.Samples[0].SemiTones;
        //    currentPitch = fromPitch;

        //    while (val < 1f)
        //    {
        //        val += Time.deltaTime * factor;
        //        granularPattern.Samples[0].SemiTones = Mathf.Lerp(fromPitch, targetPitch, val * val);
        //        yield return null;
        //    }
        //}

        // Use this for initialization
        void Start()
        {
            pitchDiff = pitchMax - pitchMin;
            pitchMid = pitchMin + .5f * pitchDiff;
            //And to the granular drone, in a coroutine lerp to get glissandos
            //StartCoroutine(TweenGranularPitch(6));
        }

        // Update is called once per frame
        void Update()
        {
            ModulateGranular();
        }

        private void ModulateGranular()
        {
            clampedMetric = metric.Clamp(-1f, 1f);
            targetPitch = pitchMid + clampedMetric * pitchDiff;
            granularPattern.Samples[0].SemiTones = targetPitch;
        }
    }
}