public class EnemyAttackStrategy : IEnemyStrategy
{
    private readonly WaypointPatroller _patroller;
    private readonly TargetChaser _chaser;

    public EnemyAttackStrategy(WaypointPatroller patroller, TargetChaser chaser)
    {
        _patroller = patroller;
        _chaser = chaser;
    }

    public void Tick()
    {
        _patroller.Stop();
        _chaser.Stop();
    }
}
