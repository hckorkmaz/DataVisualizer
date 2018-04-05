using System.ComponentModel.DataAnnotations;

namespace DataVisualizerApi.Models
{
    /// <summary>
    /// Use this entity in order to pass it to DataProvider methods.
    /// </summary>
    public class DatabaseConfig
    {
        [Required]
        [RegularExpression(@"^(([a-z0-9-]+\.)+[a-z]{2,6}|((25[0-5]|2[0-4][0-9]|[01]?[0-9]?[0-9])\.){3}(25[0-5]|2[0-4][0-9]|[01]?[0-9]?[0-9]))(:(6553[0-5]|655[0-2]\\d|65[0-4]\\d{2}|6[0-4]\d{3}|[1-5]\d{4}|[1-9]\d{0,3}))?(/[\w/.]*)?$",
            ErrorMessage = "Please enter valid IP address.")]
        public string Server { get; set; }

        [Required]
        public string DatabaseName { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        public string GetConnectionString
        {
            get
            {
                return string.Format("Server={0};Database={1};User Id={2};Password = {3};", this.Server, this.DatabaseName, this.Username, this.Password);
            }
        }
    }
}