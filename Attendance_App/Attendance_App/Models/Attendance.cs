using System;
using System.Collections.Generic;
using System.Text;

namespace Attendance_App.Models
{
    public class Attendance
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string Date { get; set; }
        public bool IsPresent { get; set; }
    }
}
