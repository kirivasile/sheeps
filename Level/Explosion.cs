using UnityEngine;
using Zenject;

namespace Sheeps.Level {
    public class Explosion : MonoBehaviour {
        SignalBus _signalBus;

        float _timeBeforeDeath;
        float _lifeDuration;

        bool _isAlive;

        [Inject]
        public void Construct(SignalBus signalBus) {
            _signalBus = signalBus;
        }

        void Reset(float lifeDuration, Vector3 position) {
            _lifeDuration = lifeDuration;
            _timeBeforeDeath = _lifeDuration;
            transform.position = position;

            _isAlive = true;
        }

        void Update() {
            if (_isAlive) {
                _timeBeforeDeath -= Time.deltaTime;

                if (_timeBeforeDeath < 0) {
                    _isAlive = false;
                    _signalBus.Fire<ExplosionDeadSignal>(
                        new ExplosionDeadSignal() { explosion = this }
                    );
                }
            }
        }

        public class Pool : MonoMemoryPool<float, Vector3, Explosion> {
            protected override void Reinitialize(float lifeDuration, Vector3 position, Explosion item) {
                item.Reset(lifeDuration, position);
            }
        }
    }
}