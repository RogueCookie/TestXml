namespace UserManagerService.Integration.Interfaces
{
    /// <summary>
    /// Common windows service
    /// </summary>
    public interface IWindowsService
    {
        /// <summary>
        /// On start service
        /// </summary>
        void Start();

        /// <summary>
        /// On stop service
        /// </summary>
        void Stop();
    }
}