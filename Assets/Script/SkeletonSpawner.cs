    using System.Collections;
    using UnityEngine;

    public class SkeletonSpawner : MonoBehaviour
    {
        public GameObject skeletonPrefab; // Assign this in the inspector with your skeleton prefab
        public Transform spawnPoint; // Assign this to specify where skeletons should be spawned
        
        [Tooltip("Time in seconds between each spawn.")]
        public float spawnInterval = 5f; // Default is 5 seconds, but you can change it in the Inspector

        // This method creates a skeleton instance at the spawn point
        public void SpawnSkeleton()
        {
            if (skeletonPrefab == null || spawnPoint == null)
            {
                Debug.LogError("SkeletonSpawner is missing references to the prefab or spawnPoint.");
                return;
            }

            Instantiate(skeletonPrefab, spawnPoint.position, spawnPoint.rotation);
        }

        IEnumerator SpawnSkeletonEveryInterval()
        {
            while (true) // This creates an infinite loop. Be cautious with these!
            {
                SpawnSkeleton(); // Call the spawn method
                yield return new WaitForSeconds(spawnInterval); // Wait for 5 seconds before the next iteration
            }
        }
        
        void Start()
        {
            // Start the coroutine here instead of Update
            StartCoroutine(SpawnSkeletonEveryInterval());
        }
        
    }