using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DailyReportApi.Models
{
    public class Mantis
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public DateTime Date { get; set; }
        [Required]
        public string MantisNumber { get; set; }
        public int ProjectId { get; set; }
    }

    public class DailyReport
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int ProjectId { get; set; }
        [Required]
        public string Message { get; set; }
    }

    public class Project
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string ProjectName { get; set; }
        public string Version { get; set; }
    }

    public class PeriodReport : IComparable<PeriodReport>
    {
        public DateTime Date { get; set; }
        public int ProjectID { get; set; }
        public string ProjectName { get; set; }
        public string Message { get; set; }
        public string Version { get; set; }

        int IComparable<PeriodReport>.CompareTo(PeriodReport other)
        {
            if (other == null)
                return -1;
            else
            {
                if (ProjectID == other.ProjectID)
                {
                    if (Date == other.Date)
                        return Message.CompareTo(other.Message);
                    else
                        return Date.CompareTo(other.Date);
                }
                else if (ProjectName == other.ProjectName)
                {
                    if (Version == other.Version)
                    {
                        Console.WriteLine("shouldn't go here...");
                        return 0;
                    }
                    else
                        return Version.CompareTo(other.Version);
                }
                else
                {
                    return ProjectName.CompareTo(other.ProjectName);
                }
            }
        }
    }

    /*public class Commit : BaseModel, IComparable<Commit>
    {
        public int ProjectId { get; set; }
        public long UnixTimestamp { get; set; }
        public string Message { get; set; }
        public string Author { get; set; }
        public string Hash { get; set; }

        public int CompareTo(Commit obj)
        {
            if (obj == null)
                return -1;
            return UnixTimestamp.CompareTo(obj.UnixTimestamp);
        }
    }*/
}
