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
    Immediate,
    StartRound,
    PlayerTurn,
    EnemyTurn,
    EndRound,
    OnDamageTaken
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