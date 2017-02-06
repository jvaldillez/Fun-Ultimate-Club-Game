using UnityEngine;
/// <summary>
/// Beavhior that drives in a straight line toward the player.
/// </summary>
public class MoveTowardsPlayer : BehaviorTreeNode
{
    public float DistanceThreshold = 20f;

    public override bool Tick(Enemy baddie)
    {
        var playerPos = FindObjectOfType<PlayerController>().transform.position;
        if (Vector3.Distance(playerPos, baddie.transform.position) > DistanceThreshold)
            return false;
        baddie.MoveTowards(playerPos);
        return true;
    }
}
