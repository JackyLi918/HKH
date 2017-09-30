/*******************************************************
 * Filename: IImport.cs
 * File description：
 * 
 * Version:	1.0
 * Created:	05/26/2015 6:29:26 PM
 * Author:	JackyLi
 * 
*****************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HKH.Exchange.Configuration;

namespace HKH.Exchange.Common
{
    public interface IImportable<T, TList> : IDisposable
        where T : class
        where TList : class
    {
        string ImportId { get; set; }
        void Import(string sourceFile, TList successList, TList failList);
    }

    public interface IExportable : IDisposable
    {
        short? ExcelDateFormat { get; }
        string DateFormatString { get; }
        string NumberFormatString { get; }
        int NextRowNum();
    }
}