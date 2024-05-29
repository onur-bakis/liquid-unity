using System.Collections.Generic;
using Scripts.Commands.Base;

namespace Scripts.Commands.Recorders
{
    public class ActionRecorder
    {
        private readonly Stack<ActionBase> _actions = new Stack<ActionBase>();

        public void Record<T>(T action) where T : ActionBase
        {
            _actions.Push(action);
            action.Execute();
        }

        public void Rewind()
        {
            if (_actions.Count <= 0) return;
            ActionBase action = _actions.Pop();
            action.Undo();
        }
    }
}