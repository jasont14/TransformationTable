using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DateTransformationTable
{
    class DateTransformationRecord
    {
        private DateTime d_date;
        private DateTime t_date;
        private string t_date_name;

        public override string ToString()
        {
            return ",(CAST('" + d_date.Year.ToString() + "-" + d_date.Month.ToString() + "-" + d_date.Day.ToString() + "' AS DATE),CAST('" + t_date.Year.ToString() + "-" + t_date.Month.ToString() + "-" + t_date.Day.ToString() + "' AS DATE))";
        }

        public DateTime D_Date { get => d_date; set => d_date = value; }
        public DateTime T_Date { get => t_date; set => t_date = value; }
        public string T_Date_Name { get => t_date_name; set => t_date_name = value; }

        public DateTransformationRecord(DateTime original_Date, DateTime transform_Date, string transform_Date_Name)
        {
            D_Date = original_Date;
            T_Date = transform_Date;
            T_Date_Name = transform_Date_Name;
        }
    }
}
