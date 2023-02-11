using System;
using System.Collections.Generic;
using System.Text;

namespace HKH.CurrencyFormat
{
	public interface ICurrencyFormatter
	{
		/// <summary>
		/// Format to number from culture Currency with default digits
		/// </summary>
		/// <param name="sCurrency"></param>
		/// <returns></returns>
		double Format(string sCurrency);

		/// <summary>
		/// Format to number from culture Currency with special digits
		/// </summary>
		/// <param name="sCurrency"></param>
		/// <param name="digits"></param>
		/// <returns></returns>
		double Format(string sCurrency,int digits);
		
		/// <summary>
		/// Format from number to Chinese
		/// </summary>
		/// <param name="numRMB"></param>
		/// <returns></returns>
		string Format(double nCurrency);
	}

	public interface ICurrencyUnitProvider
	{
		/// <summary>
		/// a tag after the local Currency format
		/// </summary>
		string SuffixToken { get;}

		/// <summary>
		/// The mapping between number and local Currency number
		/// </summary>
		Dictionary<int, string> NumbersMapping { get;}

		/// <summary>
		/// Get Unit Name by 
		/// </summary>
		/// <param name="pExp">position exponential</param>
		/// <returns></returns>
		CurrencyUnit GetUnit(int pExp);

		/// <summary>
		/// Get position exponential of unit
		/// </summary>
		/// <param name="unit"></param>
		/// <returns></returns>
		CurrencyUnit GetUnit(string unit);

		/// <summary>
		/// whether a Pivotal Unit by unit
		/// </summary>
		/// <param name="unit"></param>
		/// <param name="mUnit"></param>
		/// <returns></returns>
		bool IsPivotalUnit(string unit, out CurrencyUnit mUnit);

		/// <summary>
		/// whether a Pivotal Unit by exp
		/// </summary>
		/// <param name="exp"></param>
		/// <param name="mUnit"></param>
		/// <returns></returns>
		bool IsPivotalUnit(int exp, out CurrencyUnit mUnit);

	}

	public struct CurrencyUnit
	{
		#region protected variable
		private string unit;		//local unit name
		private int exp;		//position exponential
		private bool isPivotal;
		#endregion

		#region Constructor

		public CurrencyUnit(string unit, int exp) : this(unit, exp , false)
		{
		}

		public CurrencyUnit(string unit, int exp, bool isPivotal)
		{
			this.unit = unit;
			this.exp = exp;
			this.isPivotal = isPivotal;
		}

		#endregion

		#region Property

		public string Unit
		{
			get { return unit; }
		}

		public string PrimaryUnit
		{
			get { return unit.Split('|')[0]; }
		}

		public int Exp
		{
			get { return exp; }
		}

		public bool IsPivotal
		{
			get { return isPivotal; }
		}

		#endregion

		#region Methods

		public bool IsValid(string pUnit)
		{
			return (!string.IsNullOrEmpty(pUnit)) && this.unit.IndexOf(pUnit) > -1;
		}

		public bool IsValid(int pExp)
		{
			return pExp != int.MinValue && this.exp == pExp;
		}

		#endregion
	}
}
