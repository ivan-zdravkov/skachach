using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudSpawner : MonoBehaviour
{
    private CloudSettings[] cloudSettings;

    [SerializeField] GameObject[] farClouds;
    [SerializeField] GameObject[] nearClouds;

    private void Start()
    {
        this.cloudSettings = new CloudSettings[]
        {
            new CloudSettings(z: 9.75f, minX: 2f, maxX: 4f, clouds: this.farClouds),
            new CloudSettings(z: 9.25f, minX: 0f, maxX: 3f, clouds: this.farClouds),
            new CloudSettings(z: 8.75f, minX: -3f, maxX: -1f, clouds: this.nearClouds),
            new CloudSettings(z: 8.25f, minX: -8f, maxX: -4f, clouds: this.nearClouds)
        };
    }

    private class CloudSettings
    {
        public float Z { get; set; }

        public float MinX { get; set; }

        public float MaxX { get; set; }

        public GameObject[] Clouds { get; set; }

        public CloudSettings(float z, float minX, float maxX, GameObject[] clouds)
        {
            if (minX > maxX)
                throw new ArgumentException("minX cannot be largen than maxX");

            this.Z = z;
            this.MinX = minX;
            this.MaxX = maxX;
            this.Clouds = clouds;
        }
    }
}