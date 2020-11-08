﻿using System;

namespace Support_Your_Locals.Models.ViewModels
{
    public class TimeSheetRegisterViewModel
    {
        public static string[] Days = { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public int Weekday { get; set; }

        public static bool Invalid(DateTime from, DateTime to)
        {

            return from.TimeOfDay.ToString() == to.TimeOfDay.ToString();
        }

    }
}
