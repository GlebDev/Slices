namespace Source.Pool.PoolObjects
{
    public class DestroyParticle : UnityPoolObject
    {
        public void OnParticleSystemStopped()
        {
            Push();
        }
    }
}
