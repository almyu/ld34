using UnityEngine;
using System.Collections.Generic;

namespace LD34 {

    public class Recorder : MonoBehaviour {

        public AudioSource source;
        public float chunkLength = 2f;
        public int chunkIndex = 0;
        public float snap = 0.25f;
        public bool snapAntiLag = true;
        public float pixelsPerSecond = 100f;
        public KeyCode nextHotkey, prevHotkey, resetHotkey;

        [System.Serializable]
        public struct Event {
            public string hotkey;
            public Color color;
            public string description;
        }

        public Event[] events = new[] {
            new Event { hotkey = "Left", color = Color.red, description = "Left" },
            new Event { hotkey = "Right", color = Color.blue, description = "Right" }
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
                if (!Input.GetButtonDown(events[i].hotkey)) continue;

                var chunk = chunks[chunkIndex];
                var time = Snap(source.time - chunk.start);
                chunks[chunkIndex].AddBeat(time, i);
            }
        }

        private float Snap(float time) {
            if (snapAntiLag) time -= Time.deltaTime * 0.5f;
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

            var trackHeight = 100f;
            var headerHeight = 20f;
            var snapHeight = 15f;
            var eventHeight = trackHeight / events.Length;

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
                    center.x + chunkStartX, center.y - headerHeight,
                    center.x + chunkEndX, center.y);

                var startMinute = Mathf.FloorToInt(chunk.start / 60f);
                var startSecond = Mathf.FloorToInt(Mathf.Repeat(chunk.start, 60f));

                GUI.color = Color.white;
                GUI.DrawTexture(chunkLabelRect, index == chunkIndex ? activeChunkTex : inactiveChunkTex);
                GUI.Label(chunkLabelRect, string.Format(" {0}:{1:d2}", startMinute, startSecond));

                DrawVerticalLine(center + new Vector2(chunkStartX, -headerHeight), trackHeight + headerHeight);
                DrawVerticalLine(center + new Vector2(chunkEndX, -headerHeight), trackHeight + headerHeight);

                GUI.color = Color.white.WithA(0.5f);
                for (int i = 0, n = Mathf.FloorToInt(chunkLength / snap); i < n; ++i) {
                    var barTime = chunkStart + i * snap;
                    var barX = barTime * pixelsPerSecond;

                    DrawVerticalLine(center + new Vector2(barX, -headerHeight), snapHeight);
                }

                foreach (var beat in chunk.beats) {
                    var beatTime = chunkStart + beat.time;
                    var beatX = beatTime * pixelsPerSecond;
                    var eventY = beat.eventIndex * eventHeight;

                    GUI.color = events[beat.eventIndex].color;
                    DrawVerticalLine(center + new Vector2(beatX, eventY), eventHeight, 2);
                }
                ++index;
            }
        }
    }
}
