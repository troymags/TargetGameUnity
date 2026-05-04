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

        Destroy(gameObject);
    }

    
}
