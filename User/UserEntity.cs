
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using SQLite;
using System.ComponentModel.DataAnnotations;

namespace AGVSystemCommonNet6.User
{
    public class UserEntity
    {
        [PrimaryKey]
        [Key]
        public string UserName { get; set; }
        public string Password { get; set; }
        public ERole Role { get; set; }
    }
}
