namespace Scripts.Commands.Base
{
    public abstract class ActionBase
    {
        public abstract void Execute();
        public abstract void Undo();
    }
}