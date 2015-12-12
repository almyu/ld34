using UnityEngine;

namespace LD34 {

    public class EnemySpawner : MonoBehaviour {

        public Enemy[] regularPrefabs, bossPrefabs;
        public float bossMinLength = 1f;
        public Rect spawnArea;
        public Transform destination;

        public TimelinePlayer spawnPlayer, destPlayer;
        public InputMatcher destMatcher;

        private float travelTime;

        private void Awake() {
            travelTime = spawnPlayer.time - destPlayer.time;
            spawnPlayer.onPulse.AddListener(SpawnEnemy);
        }

        protected void SpawnEnemy(float length) {
            var prefab = RollEnemyPrefab(length);
            var enemy = Instantiate(prefab);

            var dist = travelTime * enemy.speed;
            var point = RollSpawnPoint() - (Vector2) destination.position;
            point = point.normalized * dist;
            point *= Mathf.Abs(destination.position.x / point.x);
            point = point.normalized * (1f + dist);

            enemy.transform.position = point;
            enemy.destiantion = destination.position;

            destMatcher.AddPulse(Time.timeSinceLevelLoad + travelTime, length, enemy);

            //Debug.Log("Mob at " + point + " arrives in " + travelTime + " dist " + dist);
        }

        public Enemy RollEnemyPrefab(float length) {
            return length < bossMinLength
                ? regularPrefabs[Random.Range(0, regularPrefabs.Length)]
                : bossPrefabs[Random.Range(0, bossPrefabs.Length)];
        }

        public Vector2 RollSpawnPoint() {
            return spawnArea.min + Vector2.Scale(spawnArea.size, new Vector2(Random.value, Random.value));
        }
    }
}
