using System;
using System.Collections.Generic;
using System.IO;
using OfficeOpenXml;

namespace LTMCompanyName.YoyoCmsTemplate.DataExporting.Excel.Epplus
{
    public abstract class EpPlusExcelImporterBase<TEntity>
    {
        public List<TEntity> ProcessExcelFile(byte[] fileBytes, Func<ExcelWorksheet, int, TEntity> processExcelRow)
        {
            var entities = new List<TEntity>();

            using (var stream = new MemoryStream(fileBytes))
            {
                using (var excelPackage = new ExcelPackage(stream))
                {
                    foreach (var worksheet in excelPackage.Workbook.Worksheets)
                    {
                        var entitiesInWorksheet = ProcessWorksheet(worksheet, processExcelRow);

                        entities.AddRange(entitiesInWorksheet);
                    }
                }
            }

            return entities;
        }

        private List<TEntity> ProcessWorksheet(ExcelWorksheet worksheet, Func<ExcelWorksheet, int, TEntity> processExcelRow)
        {
            var entities = new List<TEntity>();

            for (var i = worksheet.Dimension.Start.Row + 1; i <= worksheet.Dimension.End.Row; i++)
            {
                try
                {
                    var entity = processExcelRow(worksheet, i);

                    if (entity != null)
                    {
                        entities.Add(entity);
                    }
                }
                catch (Exception)
                {
                    //ignore
                }
            }

            return entities;
        }
    }
}
