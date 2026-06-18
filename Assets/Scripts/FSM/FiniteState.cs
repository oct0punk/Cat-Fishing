

public abstract class FiniteState<T>
{
    protected T tgt;
    public FiniteState(T tg) { tgt = tg; }
    public abstract void OnEnter(T tgt);
    public abstract void OnEnd(T tgt);
    public abstract void Tick(T tgt);
}