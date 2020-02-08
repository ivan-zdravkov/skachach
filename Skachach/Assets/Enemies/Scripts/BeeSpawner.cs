using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeeSpawner : MonoBehaviour
{
    [SerializeField] [Range(1, 10)] int spawnInterval = 5;
    [SerializeField] [Range(0, 50)] int spawnIntervalUnsertantyPercent = 25;
    [SerializeField] [Range(-7, 7)] int minY = -7;
    [SerializeField] [Range(-7, 7)] int maxY = 7;

    [SerializeField] GameObject bee;

    private float elapsed = 0f;
    private float shouldElapse = 0f;

    private void Start()
    {
        this.DetermineSpawnInterval();
    }

    void Update()
    {
        if (elapsed < shouldElapse)
            elapsed += Time.deltaTime;
        else
        {
            elapsed = 0;
            this.DetermineSpawnInterval();

            float speed = Random.Range(2.5f, 7.5f);
            float x = this.transform.position.x;
            float y = Random.Range(minY, maxY + 1);

            bee.GetComponent<EnemyWalk>().SetSpeed(speed);
            bee.transform.position = new Vector2(x, y);

            Instantiate(bee);
        }
    }

    private void DetermineSpawnInterval()
    {
        this.shouldElapse = Random.Range(
            min: Random.Range(spawnInterval - (spawnInterval * spawnIntervalUnsertantyPercent / 100.0f), spawnInterval),
            max: Random.Range(spawnInterval, spawnInterval + (spawnInterval * spawnIntervalUnsertantyPercent / 100.0f))
        );
    }
}
