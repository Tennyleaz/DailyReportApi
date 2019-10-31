using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DailyReportApi.Models
{
    public class MyContext : DbContext
    {
        // 應用程式主要透過 DbContext 物件與資料庫進行連線溝通，對資料庫進行查詢、新增、修改等動作。
        // DbContext 類別名稱為資料庫名稱
        public MyContext(DbContextOptions<MyContext> options) : base(options)
        {

        }

        public bool DailyReportExist (int id)
        {
            if (DailyReportItems == null)
                return false;
            return DailyReportItems.Any(x => x.Id == id);
        }

        public bool ProjectExist(int id)
        {
            if (ProjectItems == null)
                return false;
            return ProjectItems.Any(x => x.Id == id);
        }

        public bool MantisExist(int id)
        {
            if (MantisItems == null)
                return false;
            return MantisItems.Any(x => x.Id == id);
        }

        // 每個 DbSet 會對應到特定的資料表結構，並包含該資料表的實體資料集合。
        // DbSet 屬性名稱為資料表名稱
        public DbSet<DailyReport> DailyReportItems { get; set; }
        public DbSet<Project> ProjectItems { get; set; }
        public DbSet<Mantis> MantisItems { get; set; }
    }
}
