using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace LD34 {

    public class CurveBuilder {

        public List<Keyframe> keys = new List<Keyframe>(2);
        
        public CurveBuilder Add(float time, float value) {
            keys.Add(new Keyframe(time, value));
            return this;
        }

        public CurveBuilder Add(float time, float value, float inTangent, float outTangent) {
            keys.Add(new Keyframe(time, value, inTangent, outTangent));
            return this;
        }

        public AnimationCurve ToCurve() {
            return new AnimationCurve(keys.ToArray());
        }
    }


    public static class CanvasRendererTintExt {

        public const float defaultTime = 0.5f;
        public static readonly AnimationCurve defaultEasing = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

        public static AnimationCurve MakeSpikeEasing(float spikeTime, float spikeValue, float fallValue = 0.5f) {
            var inTangent = spikeValue / spikeTime * 2f;
            var outTangent = (spikeValue - fallValue) * 4f / (spikeTime - 1f);

            return new CurveBuilder()
                .Add(0f, 0f)
                .Add(spikeTime, spikeValue, inTangent, outTangent)
                .Add(1f, 1f)
                .ToCurve();
        }

        public static Coroutine Tint(this CanvasRenderer ren, Color color, float time = defaultTime) {
            return ren.Tint(color, time, defaultEasing);
        }

        public static Coroutine Tint(this CanvasRenderer ren, Color color, float time, AnimationCurve easing) {
            return CoroutineRunner.Start(DoTint(ren, color, time, easing));
        }

        public static IEnumerator DoTint(this CanvasRenderer ren, Color color, float time, AnimationCurve easing) {
            var startColor = ren.GetColor();

            for (var t = 0f; t <= time; t += Time.deltaTime) {
                ren.SetColor(Color.Lerp(startColor, color, easing.Evaluate(t / time)));
                yield return null;
            }
            ren.SetColor(color);
        }
    }
}
