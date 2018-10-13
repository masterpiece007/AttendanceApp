using System;
using System.Collections.Generic;
using System.Text;

namespace Attendance_App.Models
{
    public class Attendance_
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public DateTime Date { get; set; }
        public bool IsPresent { get; set; }
    }
}
