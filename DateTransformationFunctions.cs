using System;
using System.IO;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DateTransformationTable
{
    class DateTransformationFunctions
    {
        private List<DateTransformationRecord> records = new List<DateTransformationRecord>();
        public enum TranformationType { Month, Quarter, YTD }
        private string fileLocation = "";

        DateTime startDate;
        DateTime endDate;
        TranformationType tt;

        public DateTime StartDate { get => startDate; set => startDate = value; }
        public DateTime EndDate { get => endDate; set => endDate = value; }
        private string FileLocation { get => fileLocation; set => fileLocation = value; }

        public DateTransformationFunctions()
        {
            DateTime nt = DateTime.Now;
            int year = nt.Month;
            string sdt = year.ToString() + "-01-01";
            StartDate = DateTime.Parse(sdt);
            string edt = year.ToString() + "-12-31";
            EndDate = DateTime.Parse(edt);
            tt = TranformationType.Month;
        }

        public DateTransformationFunctions(int startYear, int endYear)
        {
            string sdt = startYear.ToString() + "-01-01";
            StartDate = DateTime.Parse(sdt);
            string edt = endYear.ToString() + "-12-31";
            EndDate = DateTime.Parse(edt);
            tt = TranformationType.Month;
        }

        public DateTransformationFunctions(int startYear, int endYear, TranformationType WeekMonthQuarterYTD)
        {
            string sdt = startYear.ToString() + "-01-01";
            StartDate = DateTime.Parse(sdt);
            string edt = endYear.ToString() + "-12-31";
            EndDate = DateTime.Parse(edt);
            tt = WeekMonthQuarterYTD;
        }

        public DateTransformationFunctions(int startYear, int endYear, TranformationType WeekMonthQuarterYTD, string fileLocation)
        {
            string sdt = startYear.ToString() + "-01-01";
            StartDate = DateTime.Parse(sdt);
            string edt = endYear.ToString() + "-12-31";
            EndDate = DateTime.Parse(edt);
            tt = WeekMonthQuarterYTD;
            FileLocation = fileLocation;
        }

        private DateTime GetPreviousMonthsEndDate(int month, int year)
        {
            int day = 1;
            string ss = "";

            if (month < 12)
            {
                ss = year.ToString() + "-" + (month+1).ToString() + "-" + day.ToString();
            }
            else
            {
                ss = (year+1).ToString() + "-01-" + day.ToString();
            }

            DateTime result = DateTime.Parse(ss);
            result = result.AddDays(-1);
            return result;
        }

        private void GetMonthTransformation()
        {     
            int em = 12;
            int sYear = startDate.Year;
            int eYear = endDate.Year;

            //Current Year
            while (sYear <= eYear)
            {
                int sm = 1;
                while (sm <= em)
                {
                    int loopMonth = 1;
                    while (loopMonth <= sm)
                    {
                        DateTime sdt = GetPreviousMonthsEndDate(sm, sYear);
                        DateTime tdt = GetPreviousMonthsEndDate(loopMonth, sYear);

                        records.Add(new DateTransformationRecord(sdt, tdt, tt.ToString()));
                        loopMonth++;
                    }
                    sm++;
                }
                sYear++;
            }            
        }
        private void GetQuarterTransformation()
        {
            int sYear = startDate.Year;
            int eYear = endDate.Year;

            while (sYear <= eYear)
            {

                List<DateTime> Quarters = new List<DateTime>();
                Quarters.Add(DateTime.Parse(sYear.ToString() + "-03-31"));
                Quarters.Add(DateTime.Parse(sYear.ToString() + "-06-30"));
                Quarters.Add(DateTime.Parse(sYear.ToString() + "-09-30"));
                Quarters.Add(DateTime.Parse(sYear.ToString() + "-12-31"));
                int counter = 0;

                while (counter <= 3)
                {
                    DateTime qDate = Quarters[counter];
                    DateTime sDate = DateTime.Parse(sYear.ToString() + "-01-01");
                    if (counter != 0)
                    {
                        sDate = Quarters[counter - 1];
                        sDate = sDate.AddDays(1);
                    }                   

                    while (sDate <= qDate)
                    {
                        DateTime loopDate = DateTime.Parse(sYear.ToString() + "-01-01");
                        if (counter != 0)
                        {
                            loopDate = Quarters[counter - 1];
                            loopDate = loopDate.AddDays(1);
                        }
                        while (loopDate <= Quarters[counter])
                        {
                            records.Add(new DateTransformationRecord(sDate, loopDate, "Quarter"));
                            loopDate = loopDate.AddDays(1);
                        }
                        sDate = sDate.AddDays(1);
                    }

                    counter++;
                } //counter

                sYear++;
            }  //sYear           
        }
        private void GetYTDTransformation()
        {
            int sYear = startDate.Year;
            int eYear = endDate.Year;

            while (sYear <= eYear)
            {
                DateTime qDate = DateTime.Parse(sYear.ToString() + "-12-31");
                DateTime sDate = DateTime.Parse(sYear.ToString() + "-01-01");

                while (sDate <= qDate)
                {
                    DateTime loopDate = DateTime.Parse(sYear.ToString() + "-01-01");
                    while (loopDate <= sDate)
                    {
                        records.Add(new DateTransformationRecord(sDate, loopDate, "YTD"));
                        loopDate = loopDate.AddDays(1);
                    }

                    sDate = sDate.AddDays(1);
                }

                sYear++;
            }
        }

        public int WriteTableToCSVFile()
        {
            switch (tt)
            {                
                case TranformationType.Month:
                    GetMonthTransformation();
                    break;
                case TranformationType.Quarter:
                    GetQuarterTransformation();
                    break;
                case TranformationType.YTD:
                    GetYTDTransformation();
                    break;
                default:
                    break;
            }

            using (StreamWriter writer = File.AppendText(fileLocation))
            {
                writer.Write("INSERT INTO <<TABLE NAME>> VALUES\n");
                foreach (DateTransformationRecord d in records)
                {
                    writer.WriteLine(d.ToString());
                }
            }

            return 0;
        }

    }
}
