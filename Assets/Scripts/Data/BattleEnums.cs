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
    None,
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

public enum ActionCategory
{
    Attack,
    Skill,
    Item,
    Defense,
    Escape,
    Utility
}

public enum ResourceType
{
    None,
    PP,
    Uses
}