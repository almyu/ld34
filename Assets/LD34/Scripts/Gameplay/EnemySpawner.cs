using UnityEngine;

namespace LD34 {

    public class EnemySpawner : MonoBehaviour {

        public Enemy[] regularPrefabs, bossPrefabs;
        public float bossMinLength = 1f;
        public float spawnExtent = 5f;

        public void SpawnEnemy(Pulse pulse) {
            var prefab = RollEnemyPrefab(pulse.length);
            var enemy = Instantiate(prefab);

            var dist = (pulse.actionTime - Time.realtimeSinceStartup) * enemy.speed; // shift by half error?
            enemy.transform.position = new Vector2(dist, RollSpawnHeight());

            pulse.listeners.Add(enemy);
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
