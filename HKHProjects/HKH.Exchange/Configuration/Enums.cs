/*******************************************************
 * Filename: Enums.cs
 * File description：
 * 
 * Version:	1.0
 * Created:	04/10/2015 11:40:50 AM
 * Author:	JackyLi
 * 
*****************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
		Expression
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
}