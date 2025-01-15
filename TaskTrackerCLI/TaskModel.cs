using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskTrackerCLI
{
    public class TaskModel(int id, string description, string status, DateTime created_at, DateTime updated_at)
    {
        public int Id { get; set; } = id;
        public string Description { get; set; } = description;
        public string Status { get; set; } = status;
        public DateTime Created_at { get; set; } = created_at;
        public DateTime Updated_at { get; set; } = updated_at;
    }
}