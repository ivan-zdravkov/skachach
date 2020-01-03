using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudSpawner : MonoBehaviour
{
    private const float X_COORDINATE_TO_SPAWN_AT = 21f;
    private const float Y_COORDINATE_OFFSET = 20f;

    private CloudSettings[] cloudSettings;

    [Range(1f, 20f)] [SerializeField] float minSpawnTime = 5f;
    [Range(1f, 20f)] [SerializeField] float maxSpawnTime = 10f;
    [SerializeField] GameObject[] farClouds;
    [SerializeField] GameObject[] nearClouds;

    private void Start()
    {
        if (this.minSpawnTime > this.maxSpawnTime)
            throw new System.ArgumentException("minSpawnTime cannot be larger than maxSpawnTime.");

        this.cloudSettings = new CloudSettings[]
        {
            new CloudSettings(z: 9.75f, minY: 3f, maxY: 5f, clouds: this.farClouds),
            new CloudSettings(z: 9.25f, minY: 1f, maxY: 4f, clouds: this.farClouds),
            new CloudSettings(z: 8.75f, minY: -2f, maxY: 0f, clouds: this.nearClouds),
            new CloudSettings(z: 8.25f, minY: -3f, maxY: -1f, clouds: this.nearClouds),
            new CloudSettings(z: 8.25f, minY: -2f, maxY: 0f, clouds: this.nearClouds)
        };

        StartCoroutine(SpawnClouds());
    }

    IEnumerator SpawnClouds()
    {
        while (true)
        {
            yield return new WaitForSeconds(seconds: Random.Range(this.minSpawnTime, this.maxSpawnTime));

            CloudSettings cloudSettings = this.cloudSettings[Random.Range(0, this.cloudSettings.Length)];

            GameObject cloud = Instantiate (
                original: cloudSettings.Clouds[Random.Range(0, cloudSettings.Clouds.Length)],
                position: new Vector3(
                    x: X_COORDINATE_TO_SPAWN_AT,
                    y: Random.Range(cloudSettings.MinY, cloudSettings.MaxY) + Y_COORDINATE_OFFSET,
                    z: cloudSettings.Z),
                rotation: Quaternion.identity
            );

            cloud.transform.parent = transform;
        }
    }

    private class CloudSettings
    {
        public float Z { get; set; }

        public float MinY { get; set; }

        public float MaxY { get; set; }

        public GameObject[] Clouds { get; set; }

        public CloudSettings(float z, float minY, float maxY, GameObject[] clouds)
        {
            if (minY > maxY)
                throw new System.ArgumentException("minY cannot be largen than maxY");

            this.Z = z;
            this.MinY = minY;
            this.MaxY = maxY;
            this.Clouds = clouds;
        }
    }
}