﻿namespace PulsePoint.Models
{
    public class Workplace
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public ICollection<User> Users { get; set; } = new List<User>(); // Navigation property
    }
}
