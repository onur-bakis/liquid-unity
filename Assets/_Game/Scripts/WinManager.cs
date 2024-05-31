using System.Collections;
using System.Collections.Generic;
using Scripts.Context.Signals;
using Scripts.Keys;
using UnityEngine;
using UnityEngine.Events;
using Zenject;
using Scripts.Context.Signals;

namespace Scripts.Managers
{
    public class WinManager : MonoBehaviour
    {
        [Inject] public SignalBus _signalBus;
        public ParticleSystem[] particleSystem;
        private LevelFinishParams _levelFinishParams;
        private void OnEnable()
        {
            _signalBus.Subscribe<OnLevelFinishedSignals>(InvokeParticles);
        }

        public void InvokeParticles(OnLevelFinishedSignals onLevelFinishedSignals)
        {
            _levelFinishParams = onLevelFinishedSignals.levelFinishParams;
            Invoke(nameof(PlayParticles),1f);
        }

        private void PlayParticles()
        {
            if (!_levelFinishParams.win)
            {
                return;
            }
            particleSystem[0].Play();
            particleSystem[1].Play();

            if (_levelFinishParams.highScore)
            {
                particleSystem[2].Play();
                particleSystem[3].Play();
            }
            else
            {
                particleSystem[4].Play();
            }
        }


    }
}