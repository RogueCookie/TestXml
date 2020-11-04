namespace TestXml.Abstract.Enums
{
    /// <summary>
    /// All possible statuses for user
    /// </summary>
    public enum UserStatus
    {
        /// <summary>
        /// New user
        /// </summary>
        New = 1, 

        /// <summary>
        /// User is active
        /// </summary>
        Active = 2, 

        /// <summary>
        /// User currently blocked
        /// </summary>
        Blocked = 3, 

        /// <summary>
        /// User was marked as deleted
        /// </summary>
        Deleted = 4
    }
}