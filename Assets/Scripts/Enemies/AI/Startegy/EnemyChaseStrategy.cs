using UnityEngine;

public class EnemyChaseStrategy : IEnemyStrategy
{
    private readonly WaypointPatroller _patroller;
    private readonly TargetChaser _chaser;
    private readonly Transform _target;

    public EnemyChaseStrategy(WaypointPatroller patroller, TargetChaser chaser, Transform target)
    {
        _patroller = patroller;
        _chaser = chaser;
        _target = target;
    }

    public void Tick()
    {
        _patroller.Stop();
        _chaser.TickChase(_target);
    }
}
