/*******************************************************
 * Filename: IDbExtensions.cs
 * File description：
 * 
 * Version:	1.0
 * Created:	8/26/2013 5:54:08 PM
 * Author:	JackyLi
 * 
*****************************************************/

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling;
using HKH.Data.SqlDatabase.Metadata;

namespace HKH.Data.SqlDatabase
{
	public static class IDbExtensions
	{
		internal const int DEFAULT_MAXTHREADS = 8;

		/// <summary>
		/// Will reset the connection to the Federation Root
		/// </summary>
		/// <remarks>This is a vanity method, this simply calls another publicly available method and is only used for the explicit name if provides (this is so it is clear to a developer the intent of this method)</remarks>
		public static void UseFederationRoot(this System.Data.IDbConnection cnn)
		{
			UseFederation(cnn);
		}

		/// <summary>
		/// Will set the connection to use the federation member specified by the name and key
		/// </summary>
		/// <param name="federationName">Name of the Federation</param>
		/// <param name="distributionName">Name of the Distribution</param>
		/// <param name="key">Value of the federation key</param>
		/// <remarks>This is a vanity method, this simply calls another publicly available method and is only used for the explicit name if provides (this is so it is clear to a developer the intent of this method)</remarks>
		public static void UseFederationMember(this System.Data.IDbConnection cnn, string federationName, string distributionName, long key)
		{
			UseFederation(cnn, federationName, distributionName, key);
		}

		/// <summary>
		/// Will set the connection to use the federation member specified by the name and key
		/// </summary>
		/// <param name="federationName">Name of the Federation</param>
		/// <param name="distributionName">Name of the Distribution</param>
		/// <param name="key">Value of the federation key</param>
		/// <remarks>This is a vanity method, this simply calls another publicly available method and is only used for the explicit name if provides (this is so it is clear to a developer the intent of this method)</remarks>
		public static void UseFederationMember(this System.Data.IDbConnection cnn, string federationName, string distributionName, int key)
		{
			UseFederation(cnn, federationName, distributionName, key);
		}

		/// <summary>
		/// Will set the connection to use the federation member specified by the name and key
		/// </summary>
		/// <param name="federationName">Name of the Federation</param>
		/// <param name="distributionName">Name of the Distribution</param>
		/// <param name="key">Value of the federation key</param>
		/// <remarks>This is a vanity method, this simply calls another publicly available method and is only used for the explicit name if provides (this is so it is clear to a developer the intent of this method)</remarks>
		public static void UseFederationMember(this System.Data.IDbConnection cnn, string federationName, string distributionName, Guid key)
		{
			UseFederation(cnn, federationName, distributionName, key);
		}

		/// <summary>
		/// Will set the connection to use the federation member specified by the name and key
		/// </summary>
		/// <param name="federationName">Name of the Federation</param>
		/// <param name="distributionName">Name of the Distribution</param>
		/// <param name="key">Value of the federation key</param>
		public static void UseFederationMember(this System.Data.IDbConnection cnn, string federationName, string distributionName, byte[] key)
		{
			UseFederation(cnn, federationName, distributionName, key);
		}

		/// <summary>
		/// Will set the connection to use the federation member provided
		/// </summary>
		/// <param name="member">The instance of the federation member to connect to.</param>
		/// <remarks>This is a vanity method, this simply calls another publicly available method and is only used for the explicit name if provides (this is so it is clear to a developer the intent of this method)</remarks>
		public static void UseFederationMember(this System.Data.IDbConnection cnn, Member member)
		{
			switch (member.FedKeyType)
			{
				case FedKeyType.fedkeytypeGuid:
					UseFederation(cnn, member.Parent.Name, member.DistributionName, member.Range.GetLow<Guid>(), false);
					break;
				case FedKeyType.fedkeytypeVarbin:
					UseFederation(cnn, member.Parent.Name, member.DistributionName, member.Range.GetLow<byte[]>(), false);
					break;
				case FedKeyType.fedkeytypeInt:
					UseFederation(cnn, member.Parent.Name, member.DistributionName, member.Range.GetLow<int>(), false);
					break;
				case FedKeyType.fedkeytypeBigInt:
				default:
					UseFederation(cnn, member.Parent.Name, member.DistributionName, member.Range.GetLow<long>(), false);
					break;
			}
		}

		/// <summary>
		/// Will reset the connection to the Federation Root
		/// </summary>
		private static void UseFederation(this System.Data.IDbConnection cnn)
		{
			ExecuteCommand(cnn, "USE FEDERATION ROOT WITH RESET");
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="federationName">Name of the federation</param>
		/// <param name="key">Value of the federation key</param>
		/// <param name="filtered">NOT IMPLEMENTED</param>
		public static void UseFederation(this System.Data.IDbConnection cnn, string federationName, string distributionName, long key, bool filtered = false)
		{
			string command = string.Format("USE FEDERATION {0}({1}={2}) WITH RESET, FILTERING={3}", federationName, distributionName, key, filtered ? "ON" : "OFF");
			ExecuteCommand(cnn, command);
		}

		/// <summary>
		/// Will set the connection to use the federation member specified by the name and key
		/// </summary>
		/// <param name="federationName">Name of the Federation</param>
		/// <param name="distributionName">Name of the Distribution</param>
		/// <param name="key">Value of the federation key</param>
		/// <param name="filtered">NOT IMPLEMENTED</param>
		public static void UseFederation(this System.Data.IDbConnection cnn, string federationName, string distributionName, int key, bool filtered = false)
		{
			string command = string.Format("USE FEDERATION {0}({1}={2}) WITH RESET, FILTERING={3}", federationName, distributionName, key, filtered ? "ON" : "OFF");
			ExecuteCommand(cnn, command);
		}

		/// <summary>
		/// Will set the connection to use the federation member specified by the name and key
		/// </summary>
		/// <param name="federationName">Name of the Federation</param>
		/// <param name="distributionName">Name of the Distribution</param>
		/// <param name="key">Value of the federation key</param>
		/// <param name="filtered">NOT IMPLEMENTED</param>
		public static void UseFederation(this System.Data.IDbConnection cnn, string federationName, string distributionName, Guid key, bool filtered = false)
		{
			string command = string.Format("USE FEDERATION {0}({1}='{2}') WITH RESET, FILTERING={3}", federationName, distributionName, key, filtered ? "ON" : "OFF");
			ExecuteCommand(cnn, command);
		}

		/// <summary>
		/// Will set the connection to use the federation member specified by the name and key
		/// </summary>
		/// <param name="federationName">Name of the Federation</param>
		/// <param name="distributionName">Name of the Distribution</param>
		/// <param name="key">Value of the federation key</param>
		/// <param name="filtered">NOT IMPLEMENTED</param>
		public static void UseFederation(this System.Data.IDbConnection cnn, string federationName, string distributionName, byte[] key, bool filtered = false)
		{
			string command = string.Format("USE FEDERATION {0}({1}={2}) WITH RESET, FILTERING={3}", federationName, distributionName, key.ToHexEncodedString(), filtered ? "ON" : "OFF");
			ExecuteCommand(cnn, command);
		}

		/// <summary>
		/// Will set the connection to use the federation member specified by the name and key
		/// </summary>
		/// <param name="federationName">Name of the Federation</param>
		/// <param name="distributionName">Name of the Distribution</param>
		/// <param name="key">Value of the federation key</param>
		/// <remarks>This is a vanity method, this simply calls another publicly available method and is only used for the explicit name if provides (this is so it is clear to a developer the intent of this method)</remarks>
		public static void UseFederation(this System.Data.IDbConnection cnn, string federationName, string distributionName, FedKey key, bool filtered = false)
		{
			switch (key.Type)
			{
				case FedKeyType.fedkeytypeGuid:
					UseFederation(cnn, federationName, distributionName, key.GetValue<Guid>(), filtered);
					break;
				case FedKeyType.fedkeytypeVarbin:
					UseFederation(cnn, federationName, distributionName, key.GetValue<byte[]>(), filtered);
					break;
				case FedKeyType.fedkeytypeInt:
					UseFederation(cnn, federationName, distributionName, key.GetValue<int>(), filtered);
					break;
				case FedKeyType.fedkeytypeBigInt:
				default:
					UseFederation(cnn, federationName, distributionName, key.GetValue<long>(), filtered);
					break;
			}
		}

		/// <summary>
		/// State (open/closed) is the responsibility of the caller, we will just issue the command with the expectation
		/// that the connection is in the correct state and other responsibilities such Disposal are also handled by the
		/// caller
		/// </summary>
		private static void ExecuteCommand(System.Data.IDbConnection cnn, string commandText)
		{
			var cmd = ((ReliableSqlConnection)cnn).CreateCommand();
			cmd.CommandText = commandText;
			cmd.ExecuteNonQueryWithRetry();
		}
	}
}