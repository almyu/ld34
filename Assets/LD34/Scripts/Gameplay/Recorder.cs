using UnityEngine;
using System.Collections.Generic;

namespace LD34 {

    public class Recorder : MonoBehaviour {

        public AudioSource source;
        public float chunkLength = 2f;
        public int chunkIndex = 0;
        public float snap = 0.25f;
        public float pixelsPerSecond = 100f;
        public KeyCode nextHotkey, prevHotkey, resetHotkey;

        [System.Serializable]
        public struct Event {
            public KeyCode hotkey;
            public Color color;
            public string description;
        }

        public Event[] events = new[] {
            new Event { hotkey = KeyCode.LeftArrow, color = Color.red, description = "Left" },
            new Event { hotkey = KeyCode.RightArrow, color = Color.blue, description = "Right" }
        };

        public struct Beat {
            public int eventIndex;
            public float time;
        }

        public class Chunk {
            public float start, end;
            public List<Beat> beats = new List<Beat>();

            public void Reset() {
                beats.Clear();
            }

            public void AddBeat(float time, int eventIndex) {
                beats.Add(new Beat { time = time, eventIndex = eventIndex });
            }
        }

        private AudioClip track;
        private Chunk[] chunks;

        private Texture2D activeChunkTex, inactiveChunkTex;

        private void Awake() {
            track = source.clip;
            chunks = new Chunk[Mathf.FloorToInt(track.length / chunkLength)];

            for (int i = 0; i < chunks.Length; ++i) {
                chunks[i] = new Chunk {
                    start = i * chunkLength,
                    end = i * chunkLength + chunkLength
                };
            }

            activeChunkTex = new Texture2D(1, 1);
            activeChunkTex.SetPixel(0, 0, Color.white.WithA(0.2f));
            activeChunkTex.Apply(true, true);

            inactiveChunkTex = new Texture2D(1, 1);
            inactiveChunkTex.SetPixel(0, 0, Color.grey.WithA(0.2f));
            inactiveChunkTex.Apply(true, true);
        }

        private void Update() {
            if (Input.GetKeyDown(nextHotkey)) Advance(1);
            if (Input.GetKeyDown(prevHotkey)) Advance(-1);
            if (Input.GetKeyDown(resetHotkey)) chunks[chunkIndex].Reset();

            if (source.time + Time.deltaTime >= chunks[chunkIndex].end)
                source.time = chunks[chunkIndex].start;

            for (int i = 0; i < events.Length; ++i) {
                if (!Input.GetKeyDown(events[i].hotkey)) continue;

                var chunk = chunks[chunkIndex];
                var time = Snap(source.time - chunk.start);
                chunks[chunkIndex].AddBeat(time, i);
            }
        }

        private float Snap(float time) {
            return Mathf.Round(time / snap) * snap;
        }

        public void Advance(int numChunks) {
            chunkIndex = Mathf.Clamp(chunkIndex + numChunks, 0, chunks.Length);
        }

        private void DrawVerticalLine(Vector2 pos, float height, float width = 1) {
            GUI.DrawTexture(new Rect(pos - new Vector2(width * 0.5f, 0), new Vector2(width, height)), Texture2D.whiteTexture);
        }

        private void OnGUI() {
            var screen = new Vector2(Screen.width, Screen.height);
            var center = screen * 0.5f;

            var trackHeight = 50f;

            GUI.color = Color.white;
            DrawVerticalLine(new Vector2(center.x, 0), screen.y);

            var time = source.time;
            var index = 0;

            foreach (var chunk in chunks) {
                var chunkStart = chunk.start - time;
                var chunkEnd = chunk.end - time;

                var chunkStartX = chunkStart * pixelsPerSecond;
                var chunkEndX = chunkEnd * pixelsPerSecond;

                var chunkLabelRect = Rect.MinMaxRect(
                    center.x + chunkStartX, center.y,
                    center.x + chunkEndX, center.y + trackHeight * 0.5f);

                GUI.DrawTexture(chunkLabelRect, index == chunkIndex ? activeChunkTex : inactiveChunkTex);
                GUI.Label(chunkLabelRect, " " + index);

                GUI.color = Color.white;
                DrawVerticalLine(center + Vector2.right * chunkStartX, trackHeight * 1.5f);
                DrawVerticalLine(center + Vector2.right * chunkEndX, trackHeight * 1.5f);

                foreach (var beat in chunk.beats) {
                    var beatTime = chunkStart + beat.time;
                    var beatX = beatTime * pixelsPerSecond;

                    GUI.color = events[beat.eventIndex].color;
                    DrawVerticalLine(center + Vector2.right * beatX, trackHeight);
                }
                ++index;
            }
        }
    }
}
