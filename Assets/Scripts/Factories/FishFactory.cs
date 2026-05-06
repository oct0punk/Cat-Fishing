using UnityEngine;

public class FishFactory : Factory<Fish>
{
    public float spawnRadius = 10.0f;

    public override void OnCreate(Fish fish)
    {
        Vector2 spawnpos = Random.onUnitCircle * spawnRadius;
        Vector2 norm = spawnpos.normalized;
        fish.transform.SetPositionAndRotation(
            Boot.player.transform.position + new Vector3(spawnpos.x, 0.0f, spawnpos.y),
            Quaternion.identity);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(Boot.player.transform.position, spawnRadius);
    }
}
