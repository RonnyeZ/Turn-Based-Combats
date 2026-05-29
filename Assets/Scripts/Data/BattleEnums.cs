public enum BattleState
{
    Start,
    PlayerTurn,
    EnemyTurn,
    EndTurn,
    Victory,
    Defeat
}

public enum EffectTiming
{
    StartRound,
    EndRound
}

public enum ActionEffect
{
    Damage,
    Heal,
    Defense,
    Buff,
    Debuff
}

public enum TargetType
{
    Self,
    Enemy
}