using System.ComponentModel.DataAnnotations;

namespace DataVisualizerAPI.Models
{
    /// <summary>
    /// Use this entity in order to get data from database
    /// </summary>
    public class DatabaseDataReciever
    {
        [Required]
        public DatabaseConfig DatabaseConfig { get; set; }

        [Required]
        public string DbObjectName { get; set; }

        [Required]
        public string DbObjectType { get; set; }        
    }
}