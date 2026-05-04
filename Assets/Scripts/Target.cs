using UnityEngine;

public class Target : MonoBehaviour
{
    private float spawnTime;
    private float targetSize;
    private bool wasHit = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Destroy(gameObject, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        TargetManager.score += 10;
        TargetManager.targetsHit += 1;
        Destroy(gameObject);
    }
}
