namespace Source.Pool.Interfaces
{
    public interface IPoolObject<out TEnum>
    {
        TEnum Group { get; }
        void OnPop();
        void OnPush();
        /// <summary>
        /// Return to pool
        /// </summary>
        void Push();
    }
}