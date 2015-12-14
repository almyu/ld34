using UnityEngine;
using System.Collections.Generic;

namespace LD34 {

    public class Recorder : MonoBehaviour {

        public AudioSource source;
        public float sampleLength = 0.25f;
        public int samplesPerChunk = 16;
        public int repeatStart, repeatEnd = 16;
        public float pixelsPerSecond = 100f;
        public KeyCode nextHotkey, prevHotkey, resetHotkey;
        public KeyCode toggleRepeatHotkey = KeyCode.Return;
        public KeyCode pauseHotkey = KeyCode.Space;
        public KeyCode seekForwardHotkey = KeyCode.Equals, seekBackHotkey = KeyCode.Minus;

        [System.Serializable]
        public struct Event {
            public string hotkey;
            public Color color;
            public string description;
            public bool merge;
        }

        public Event[] events = new[] {
            new Event { hotkey = "Left", color = Color.red, description = "Left" },
            new Event { hotkey = "Right", color = Color.blue, description = "Right" },
            new Event { hotkey = "AltLeft", color = Color.red, description = "Hold Left", merge = true },
            new Event { hotkey = "AltRight", color = Color.blue, description = "Hold Right", merge = true }
        };

        private AudioClip track;
        private int[] beats;

        private float headerHeight = 20f;
        private float eventHeight = 50;
        private float trackHeight;

        private void Awake() {
            track = source.clip;
            beats = new int[Mathf.FloorToInt(track.length / sampleLength)];

            trackHeight = eventHeight * events.Length;
        }

        private void ClearBeats(int at, int count) {
            for (int i = 0; i < count; ++i)
                beats[at + i] = 0;
        }

        private int now {
            get { return Mathf.RoundToInt(source.time / sampleLength); } // HACK
            set { source.time = value * sampleLength; }
        }

        private int nowCompensated {
            get { return Mathf.RoundToInt((source.time + Time.deltaTime * 0.5f) / sampleLength); }
        }

        private bool GetEventFlag(int at, int eventIndex) {
            return (beats[at] & (1 << eventIndex)) != 0;
        }

        private void SetEventFlag(int at, int eventIndex, bool set) {
            if (set) beats[at] |= (1 << eventIndex);
            else beats[at] &= ~(1 << eventIndex);
        }

        private void ToggleEventFlag(int at, int eventIndex) {
            SetEventFlag(at, eventIndex, !GetEventFlag(at, eventIndex));
        }

        private void Update() {
            if (Input.GetKeyDown(pauseHotkey)) {
                if (source.isPlaying) {
                    source.Pause();
                    now = now;
                }
                else source.UnPause();
            }

            if (Input.GetKeyDown(seekForwardHotkey)) ++now;
            if (Input.GetKeyDown(seekBackHotkey)) --now;

            if (Input.GetKeyDown(nextHotkey)) {
                now += samplesPerChunk;
                repeatStart += samplesPerChunk;
                repeatEnd += samplesPerChunk;
            }
            if (Input.GetKeyDown(prevHotkey)) {
                now -= samplesPerChunk;
                repeatStart += samplesPerChunk;
                repeatEnd += samplesPerChunk;
            }
            if (Input.GetKeyDown(resetHotkey))
                ClearBeats(now / samplesPerChunk, samplesPerChunk);

            if (Input.GetKeyDown(toggleRepeatHotkey)) {
                if (repeatStart < repeatEnd)
                    repeatEnd = repeatStart;
                else {
                    repeatStart = nowCompensated / samplesPerChunk;
                    repeatEnd = repeatStart + samplesPerChunk;
                }
            }

            if (repeatStart < repeatEnd && source.time + Time.deltaTime >= repeatEnd * sampleLength) {
                now = repeatStart;
            }

            for (int i = 0; i < events.Length; ++i)
                if (Input.GetButtonDown(events[i].hotkey))
                    ToggleEventFlag(nowCompensated, i);
        }

        private void DrawVerticalLine(Vector2 pos, float height, float width = 1) {
            GUI.DrawTexture(new Rect(pos - new Vector2(width * 0.5f, 0), new Vector2(width, height)), Texture2D.whiteTexture);
        }

        private void OnGUI() {
            var screen = new Vector2(Screen.width, Screen.height);
            var center = screen * 0.5f;

            GUI.color = Color.white;
            DrawVerticalLine(new Vector2(center.x, 0), screen.y);

            var time = source.time;

            GUI.color = Color.white.WithA(0.2f);
            GUI.DrawTexture(new Rect(0, center.y - headerHeight, screen.x, headerHeight), Texture2D.whiteTexture);

            for (int eventIndex = 0; eventIndex < events.Length; ++eventIndex) {
                GUI.color = events[eventIndex].color.WithA(0.1f);
                GUI.DrawTexture(new Rect(0, center.y + eventIndex * eventHeight, screen.x, eventHeight), Texture2D.whiteTexture);
            }

            for (int i = 0; i < beats.Length; ++i) {
                var barTime = i * sampleLength;
                var barX = (barTime - time) * pixelsPerSecond;

                if (i % samplesPerChunk == 0) {
                    var chunkLabelRect = new Rect(
                        center + new Vector2(barX, -headerHeight),
                        new Vector2(samplesPerChunk * sampleLength * pixelsPerSecond, headerHeight));

                    var startMinute = Mathf.FloorToInt((i * sampleLength) / 60f);
                    var startSecond = Mathf.FloorToInt(Mathf.Repeat(i * sampleLength, 60f));

                    if (repeatStart <= i && i < repeatEnd) {
                        GUI.color = Color.white.WithA(0.2f);
                        GUI.DrawTexture(chunkLabelRect, Texture2D.whiteTexture);
                    }

                    GUI.color = Color.white;
                    GUI.Label(chunkLabelRect, string.Format(" {0}:{1:d2}", startMinute, startSecond));

                    DrawVerticalLine(chunkLabelRect.min, trackHeight + headerHeight);
                }

                var barH = i % 4 == 0 ? headerHeight : headerHeight * 0.5f;

                GUI.color = Color.white.WithA(0.5f);
                DrawVerticalLine(center + new Vector2(barX, -headerHeight), barH);

                for (int eventIndex = 0; eventIndex < events.Length; ++eventIndex) {
                    if (!GetEventFlag(i, eventIndex)) continue;

                    var eventY = eventIndex * eventHeight;
                    GUI.color = events[eventIndex].color;

                    if (events[eventIndex].merge && i > 0 && GetEventFlag(i - 1, eventIndex)) {
                        var eventWidth = sampleLength * pixelsPerSecond;

                        GUI.DrawTexture(new Rect(
                                center.x + barX - eventWidth, center.y + eventY,
                                eventWidth, eventHeight),
                            Texture2D.whiteTexture);
                    }
                    else DrawVerticalLine(center + new Vector2(barX, eventY), eventHeight, 2);
                }
            }
        }
    }
}
