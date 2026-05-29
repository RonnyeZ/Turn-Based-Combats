public class BattleActionExecution
{
    public BattleActionData action;
    public BattleUnit user;
    public BattleUnit target;
    public bool isPlayerAction;

    public BattleActionExecution
    (
        BattleActionData action,
        BattleUnit user,
        BattleUnit target,
        bool isPlayerAction
    )
    {
        this.action = action;
        this.user = user;
        this.target = target;
        this.isPlayerAction = isPlayerAction;
    }
}