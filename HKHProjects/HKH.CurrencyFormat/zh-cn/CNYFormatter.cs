using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace HKH.CurrencyFormat
{
	/// <summary>
	/// Transfer CNY Format
	/// </summary>
	/// <remarks> Create By Lwt on 2006/09/23
	/// </remarks>
	public class CNYFormatter : ICurrencyFormatter
	{
		#region protected variable

		protected ICurrencyUnitProvider unitProvider;

		#endregion

		#region Constructor

		public CNYFormatter()
		{
			this.unitProvider = new CNYUnitProvider();
		}

		public CNYFormatter(ICurrencyUnitProvider unitProvider)
		{
			this.unitProvider = unitProvider;
		}

        #endregion

        #region ICurrencyFormatter Members

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sRMB"></param>
        /// <returns></returns>
        public double Format(string sRMB)
		{
			return Format(sRMB, 2);
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sRMB"></param>
        /// <param name="digits"></param>
        /// <returns></returns>
		public double Format(string sRMB, int digits)
		{

			//1.5��
			if (Regex.IsMatch(sRMB, "^\\d+[.\\d+]*\\D$"))
			{
				//ȥ����Ԫ��
				sRMB = Regex.Replace(sRMB, unitProvider.GetUnit(0).Unit, "");
				string temp = sRMB[sRMB.Length - 1].ToString();
				CurrencyUnit mUnit;
				if (unitProvider.IsPivotalUnit(temp, out mUnit))
				{
					//һ����˵��������²�����С��
					return Math.Round(Convert.ToDouble(sRMB.Substring(0, sRMB.Length - 1)) * Math.Pow(10, mUnit.Exp), digits);
				}
				else
				{
					return Convert.ToDouble(sRMB);
				}
			}
			//Ҽ����ǧ��-----��׼��ʽ
			else
			{
				return Eval(sRMB);
			}
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nRMB"></param>
        /// <returns></returns>
		public string Format(double nRMB)
		{

			if (0 == nRMB)
				return "��Ԫ��";

			StringBuilder builder = new StringBuilder();

			//��100�Ը�ʽ�����ͣ����ڴ���
			ulong uRMB = Convert.ToUInt64(nRMB * 100);

			builder.Insert(0, ToLocalFormat(Convert.ToInt32(uRMB % 100), -2));

			//ȥ��ԭ����С��λ
			uRMB = uRMB / 100;

			int iUnit = 0;

			//��ÿ4λΪһ����λ�ν��д��������±߳���10000
			while (0 != uRMB)
			{
				builder.Insert(0, ToLocalFormat(Convert.ToInt32(uRMB % 10000), iUnit));
				uRMB = uRMB / 10000;
				iUnit += 4;
			}

			builder.Replace("Ԫ����", "Ԫ��");
			builder.Replace("��Ԫ", "Ԫ");

			//��ʽ����
			return Regex.Replace(builder.ToString(), "��+", "��").Trim('��');
		}

		#endregion

		#region Private Methods

		private double Eval(string sRMB)
		{
			if (string.IsNullOrEmpty(sRMB))
				return 0;
			StringBuilder sBuilder = new StringBuilder(sRMB);
			sBuilder = Replace(sBuilder, true);

			if (0 == sBuilder.Length)
				return 0;

			#region Calculate with Exp

			int basicExp = 0;
			int currExp = 0;

			double nRMB = 0;

			for (int i = sBuilder.Length - 1; i > -1; i--)
			{
				string temp = sBuilder[i].ToString();
				CurrencyUnit mUnit;

				if (unitProvider.IsPivotalUnit(temp, out mUnit))
				{
					basicExp = mUnit.Exp;
					currExp = 0;

					continue;
				}
				else
				{
					if (Regex.IsMatch(temp, "^\\d$"))
					{
						nRMB = nRMB + Convert.ToInt32(temp) * Math.Pow(10, (basicExp + currExp));
					}
					else
					{
						currExp = mUnit.Exp;
					}
				}
			}

			#endregion

			return nRMB;
		}

		/// <summary>
		/// ������ʽ��Сд��ֵ���д�ַ�����
		/// </summary>
		/// <param name="nRMB"></param>
		/// <param name="exp"></param>
		/// <returns></returns>
		private string ToLocalFormat(int nRMB, int exp)
		{
			if (0 == nRMB)
			{
				if (-2 == exp)
				{
					return "��";
				}

				if (0 == exp)
				{
					return "Ԫ";
				}

				return "��";
			}

			StringBuilder builder = new StringBuilder();

			string sRMB = string.Empty;

			#region �Խ�/�������⴦��

			if (-2 == exp)
			{
				int jiao = nRMB / 10;
				int fen = nRMB % 10;

				if (jiao > 0)
				{
					builder.Append(jiao);
					builder.Append(unitProvider.GetUnit(-1).PrimaryUnit);

					if (fen > 0)
					{
						builder.Append(fen);
						builder.Append(unitProvider.GetUnit(-2).PrimaryUnit);
					}
				}
				else
				{
					builder.Append(fen);
					builder.Append(unitProvider.GetUnit(-2).PrimaryUnit);
				}

				return Replace(builder, false).ToString();
			}

			#endregion

			#region ����Ϊ����������������

			sRMB = nRMB.ToString("0000");

			//ǰһλ�Ƿ���0
			bool hasZero = false;

			for (int i = 0; i < sRMB.Length; i++)
			{
				//ֻ����λ�����λΪ��ǧ���������±ߵ�3-iΪ��λ����
				if ((3 - i) > 0)
				{
					if ('0' != sRMB[i])
					{
						builder.Append(sRMB[i]);
						builder.Append(unitProvider.GetUnit(3 - i).PrimaryUnit);
						hasZero = false;
					}
					else
					{
						if (!hasZero)
							builder.Append(sRMB[i]);

						hasZero = true;
					}
				}
				//���һλ���ر��ʽ����
				//�����һλ���㣬��λӦ����֮ǰ
				else
				{
					if ('0' != sRMB[i])
					{
						builder.Append(sRMB[i]);
						builder.Append(unitProvider.GetUnit(exp).PrimaryUnit);
						hasZero = false;
					}
					else
					{
						if (hasZero)
						{
							builder.Insert(builder.Length - 1, unitProvider.GetUnit(exp).PrimaryUnit);
						}
						else
						{
							builder.Append(unitProvider.GetUnit(exp).PrimaryUnit);
							builder.Append(sRMB[i]);
						}
					}
				}
			}

			//ת����д�󷵻�
			return Replace(builder,false).ToString();

			#endregion
		}

		/// <summary>
		/// Transfer between number and local RMB number
		/// </summary>
		/// <param name="sRMB"></param>
		/// <param name="toNumber">true--to number/false-- to local RMB number</param>
		/// <returns></returns>
		protected StringBuilder Replace(StringBuilder sRMB, bool toNumber)
		{
			if (toNumber)
			{
				foreach (KeyValuePair<int, string> kvp in unitProvider.NumbersMapping)
				{
					sRMB.Replace(kvp.Value, kvp.Key.ToString());
				}
			}
			else
			{
				foreach (KeyValuePair<int, string> kvp in unitProvider.NumbersMapping)
				{
					sRMB.Replace(kvp.Key.ToString(), kvp.Value);
				}
			}

			return sRMB;
		}

		#endregion
	}

	public class CNYUnitProvider : ICurrencyUnitProvider
	{
		#region private variable

		private Dictionary<int, string> numbersMapping;
		private Dictionary<int, CurrencyUnit> units;

		#endregion

		#region Constructor

		public CNYUnitProvider()
		{
			Init();
		}

		#endregion

		#region Methods

		private void Init()
		{
			numbersMapping = new Dictionary<int, string>();
			numbersMapping.Add(0, "��");
			numbersMapping.Add(1, "Ҽ");
			numbersMapping.Add(2, "��");
			numbersMapping.Add(3, "��");
			numbersMapping.Add(4, "��");
			numbersMapping.Add(5, "��");
			numbersMapping.Add(6, "½");
			numbersMapping.Add(7, "��");
			numbersMapping.Add(8, "��");
			numbersMapping.Add(9, "��");

			units = new Dictionary<int, CurrencyUnit>();
			units.Add(-2, new CurrencyUnit("��", -2));
			units.Add(-1, new CurrencyUnit("��", -1));
			units.Add(0, new CurrencyUnit("Ԫ|Բ", 0, true));
			units.Add(1, new CurrencyUnit("ʰ|ʮ", 1));
			units.Add(2, new CurrencyUnit("��|��", 2));
			units.Add(3, new CurrencyUnit("Ǫ|ǧ", 3));
			units.Add(4, new CurrencyUnit("�f|��", 4, true));
			units.Add(8, new CurrencyUnit("��", 8, true));
		}

		#endregion

		#region ICurrencyUnitProvider Members

		public string SuffixToken
		{
			get { return "��"; }
		}

		public Dictionary<int, string> NumbersMapping
		{
			get { return numbersMapping; }
		}

		public CurrencyUnit GetUnit(int pExp)
		{
			CurrencyUnit unit = new CurrencyUnit();

			units.TryGetValue(pExp, out unit);

			return unit;
		}

		public CurrencyUnit GetUnit(string unit)
		{
			foreach (KeyValuePair<int, CurrencyUnit> kvp in units)
			{
				if (kvp.Value.IsValid(unit))
					return kvp.Value;
			}

			return new CurrencyUnit();
		}

		public bool IsPivotalUnit(string unit, out CurrencyUnit mUnit)
		{
			foreach (KeyValuePair<int, CurrencyUnit> kvp in units)
			{
				if (kvp.Value.IsValid(unit))
				{
					mUnit = kvp.Value;
					return mUnit.IsPivotal;
				}
			}

			mUnit = new CurrencyUnit();

			return false;
		}

		public bool IsPivotalUnit(int exp, out CurrencyUnit mUnit)
		{
			if (units.TryGetValue(exp, out mUnit))
				return mUnit.IsPivotal;

			mUnit = new CurrencyUnit();

			return false;
		}

		#endregion
	}
}
