

public abstract class FSM<T>
{
    protected T tgt;
    public FSM(T tg) { tgt = tg; }
    public abstract void OnEnter(T tgt);
    public abstract void OnEnd(T tgt);
    public abstract void Tick(T tgt);
}