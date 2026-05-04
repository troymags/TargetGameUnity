// Base implementation adapted from [Making A Simple Aim Trainer Game With Unity Software In 10 Minutes | Easy Tutorial] by [Alexander Zotov]
// URL: [https://www.youtube.com/watch?v=Rs75p2ZA_SU]
// Modifications: variable target sizing, reaction time logging,
// CSV export, distance tracking — added by Leeiam Magsipoc

using UnityEngine;

public class Target : MonoBehaviour
{
    private float spawnTime;
    private float targetSize;
    private bool wasHit = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spawnTime = Time.time;
        Destroy(gameObject, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetSize(float size)
    {
        targetSize = size;
        transform.localScale = new Vector3(size, size, 1f);
    }

    private void OnMouseDown()
    {
        wasHit = true;
        float reactionTime = Time.time - spawnTime;

        TargetManager.score += 10;
        TargetManager.targetsHit += 1;
        TargetManager.LogHit(targetSize, reactionTime, transform.position);

        Destroy(gameObject);
    }

    void OnDestroy()
    {
        if (!wasHit)
        {
            TargetManager.LogMiss(targetSize, transform.position);
        }
    }

}
