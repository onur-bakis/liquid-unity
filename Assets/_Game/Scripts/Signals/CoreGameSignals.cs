using Scripts.Keys;
using UnityEngine.Events;

namespace Scripts.Context.Signals
{
    public class OnLevelFinishedSignals
    {
        public LevelFinishParams levelFinishParams;
    }
    public class OnGameInitializeSignal
    {
        public byte levelNumber;
    }
    
    public class TapToContinueSignal {}
    public class OnStartGameSignal {}
    public class ResetGameSignal {}
    
    
}