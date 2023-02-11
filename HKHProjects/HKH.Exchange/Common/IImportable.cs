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
using System.IO;
using HKH.Exchange.Configuration;

namespace HKH.Exchange.Common
{
    public interface IImportable : IDisposable
    {
        Import Setting { get; }
    }
    public interface IImportable<T, TList> : IImportable
        where T : class
        where TList : class
    {
        void Import(string sourceFile, TList successList, TList failList = null);
        void Import(Import import, string sourceFile, TList successList, TList failList = null);
    }

    public interface IExportable : IDisposable
    {
        Export Setting { get; }
        short? ExcelDateFormat { get; }
        string DateFormatString { get; }
        string NumberFormatString { get; }
        int NextRowNum();
    }
    public interface IExportable<T, TList> : IExportable
    {
        void Export(Stream stream, TList tList);
        void Export(Export export, Stream stream, TList tList);
        void Export(string targetFile, TList tList);
        void Export(Export export, string targetFile, TList tList);
    }
}