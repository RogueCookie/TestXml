﻿namespace TestXml.Abstract.Models.Options
{
    /// <summary>
    /// TestXml configuration
    /// </summary>
    public class AppOptions
    {
        /// <summary>
        /// Database connection string
        /// </summary>
        public string DataBaseConnectionString { get; set; }

        /// <summary>
        /// How many milliseconds we will hold cash
        /// </summary>
        public int CachedHitLifeTime { get; set; }
    }
}