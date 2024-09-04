/*******************************************************
 * Filename: Enums.cs
 * File description：
 * 
 * Version:	1.0
 * Created:	04/10/2015 11:40:50 AM
 * Author:	JackyLi
 * 
*****************************************************/

namespace HKH.Exchange.Configuration
{

    public enum InheritDirection
    {
        Left,
        Up
    }

    public enum PropertyType
    {
        Normal,
        Expression,
        Picture
    }

    public enum XlsFormat
    {
        Auto,
        Xls,
        Xlsx
    }

    public enum ExportMode
    {
        Export,
        Fill
    }

    public enum FillRowMode
    {
        New,
        Copy,
        Fill
    }
    public enum ColumnMapType
    {
        ExcelHeader,
        DataHeader
    }
}