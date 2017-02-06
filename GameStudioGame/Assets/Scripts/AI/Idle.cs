/// <summary>
/// Behavior that does nothing.
/// </summary>
public class Idle : BehaviorTreeNode
{
    public override bool Tick(Enemy baddie)
    {
        return false;
    }
}
