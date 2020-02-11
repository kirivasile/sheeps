using UnityEngine;
using Zenject;

namespace Sheeps.Level {
    public class ExplosionHandler {
        Explosion.Pool _explosionPool;

        public ExplosionHandler(Explosion.Pool pool) {
            _explosionPool = pool;
        }

        public void Spawn(float duration, Vector3 position) {
            _explosionPool.Spawn(duration, position);
        }

        public void Despawn(ExplosionDeadSignal signal) {
            _explosionPool.Despawn(signal.explosion);
        }

    }
}