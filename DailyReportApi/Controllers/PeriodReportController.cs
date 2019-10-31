using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using DailyReportApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.EntityFrameworkCore.Storage;

namespace DailyReportApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PeriodReportController : Controller
    {
        private readonly MyContext _myContext;

        public PeriodReportController(MyContext context)
        {
            _myContext = context;
        }

        // url範例：
        // http://localhost/api/PeriodReport/2019-04-24/2019-04-25
        [HttpGet("{startDate}/{endDate}")]
        public async Task<List<PeriodReport>> ReadPeriodReport(DateTime startDate, DateTime endDate)
        {
            /*
             * string query = "select * from DailyReportModel "
                + "inner join ProjectReport on DailyReportModel.ProjectId = ProjectReport.Id "
                + "where DailyReportModel.Date >= ? "
                + "order by ProjectReport.ProjectName, ProjectReport.Version, DailyReportModel.Date";
             */
            var q = (from d in _myContext.DailyReportItems
                    join p in _myContext.ProjectItems on d.ProjectId equals p.Id
                    where d.Date >= startDate && d.Date <= endDate
                    select new PeriodReport()
                    {
                        Date = d.Date,
                        ProjectID = d.ProjectId,
                        Message = d.Message,
                        ProjectName = p.ProjectName,
                        Version = p.Version
                    })
                .OrderBy(p => p.ProjectName)
                .ThenBy(p => p.Version)
                .ThenBy(d => d.Date);
            //string sql = QueryableExtensions.ToSql(q);
            List<PeriodReport> result = await q.ToListAsync();
            return result;
        }

        [HttpGet]
        public async Task<List<PeriodReport>> ReadAllReport()
        {
            var q = (from d in _myContext.DailyReportItems
                     join p in _myContext.ProjectItems on d.ProjectId equals p.Id
                     select new PeriodReport()
                     {
                         Date = d.Date,
                         ProjectID = d.ProjectId,
                         Message = d.Message,
                         ProjectName = p.ProjectName,
                         Version = p.Version
                     });
            List<PeriodReport> result = await q.ToListAsync();
            return result;
        }
    }

    /// <summary>
    /// Test sql code form IQueryable.
    /// see:
    /// https://stackoverflow.com/questions/37527783/get-sql-code-from-an-ef-core-query
    /// </summary> 
    internal static class QueryableExtensions
    {
        private static readonly TypeInfo QueryCompilerTypeInfo = typeof(QueryCompiler).GetTypeInfo();

        private static readonly FieldInfo QueryCompilerField = typeof(EntityQueryProvider).GetTypeInfo().DeclaredFields.First(x => x.Name == "_queryCompiler");
        private static readonly FieldInfo QueryModelGeneratorField = typeof(QueryCompiler).GetTypeInfo().DeclaredFields.First(x => x.Name == "_queryModelGenerator");
        private static readonly FieldInfo DataBaseField = QueryCompilerTypeInfo.DeclaredFields.Single(x => x.Name == "_database");
        private static readonly PropertyInfo DatabaseDependenciesField = typeof(Database).GetTypeInfo().DeclaredProperties.Single(x => x.Name == "Dependencies");

        public static string ToSql<TEntity>(this IQueryable<TEntity> query)
        {
            var queryCompiler = (QueryCompiler)QueryCompilerField.GetValue(query.Provider);
            var queryModelGenerator = (QueryModelGenerator)QueryModelGeneratorField.GetValue(queryCompiler);
            var queryModel = queryModelGenerator.ParseQuery(query.Expression);
            var database = DataBaseField.GetValue(queryCompiler);
            var databaseDependencies = (DatabaseDependencies)DatabaseDependenciesField.GetValue(database);
            var queryCompilationContext = databaseDependencies.QueryCompilationContextFactory.Create(false);
            var modelVisitor = (RelationalQueryModelVisitor)queryCompilationContext.CreateQueryModelVisitor();
            modelVisitor.CreateQueryExecutor<TEntity>(queryModel);
            var sql = modelVisitor.Queries.First().ToString();

            return sql;
        }
    }
}