using UnityEngine;

namespace LD34 {

    public class EnemySpawner : MonoBehaviour {

        public Enemy[] regularPrefabs, bossPrefabs;
        public float bossMinLength = 1f;
        public float spawnExtent = 5f;
        public Transform destination;

        public TimelinePlayer spawnPlayer, destPlayer;
        public InputMatcher destMatcher;

        public float travelTime;

        private void Awake() {
            travelTime = spawnPlayer.time - destPlayer.time; // + destMatcher.maxError * 0.5f;
            spawnPlayer.onPulse.AddListener(SpawnEnemy);
        }

        protected void SpawnEnemy(float length) {
            var prefab = RollEnemyPrefab(length);
            var enemy = Instantiate(prefab);

            var dist = travelTime * enemy.speed;
            var point = new Vector2(dist, RollSpawnHeight());

            enemy.transform.position = point;
            enemy.destiantion = new Vector2(-point.x, point.y);

            destMatcher.AddPulse(Time.timeSinceLevelLoad + travelTime, length, enemy);

            //Debug.Log("Mob at " + point + " arrives in " + travelTime + " dist " + dist);
        }

        public Enemy RollEnemyPrefab(float length) {
            return length < bossMinLength
                ? regularPrefabs[Random.Range(0, regularPrefabs.Length)]
                : bossPrefabs[Random.Range(0, bossPrefabs.Length)];
        }

        public float RollSpawnHeight() {
            return Random.Range(-spawnExtent, spawnExtent);
        }
    }
}
