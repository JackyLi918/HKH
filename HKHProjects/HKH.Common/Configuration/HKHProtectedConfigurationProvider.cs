/*******************************************************
 * Filename: HKHProtectedConfigurationProvider.cs
 * File description：
 * 
 * Version:	1.0
 * Created:	3/31/2014 2:04:05 PM
 * Author:	JackyLi
 * 
*****************************************************/

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using HKH.Common.Security;

namespace HKH.Configuration
{
    /// <summary>
    /// HKHProtectedConfigurationProvider
    /// </summary>
    public class HKHProtectedConfigurationProvider : ProtectedConfigurationProvider
    {
        protected IEncryption encryption = null;

        public HKHProtectedConfigurationProvider()
        {
            encryption = new AES(AesKeySize.Bits256, "$123qweCXZ!@#DSA%");
        }

        public HKHProtectedConfigurationProvider(IEncryption encryption)
        {
            this.encryption = encryption;
        }

        public override XmlNode Decrypt(XmlNode encryptedNode)
        {
            string decryptedData = encryption.Decrypt(encryptedNode.InnerText);

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.PreserveWhitespace = true;
            xmlDoc.LoadXml(decryptedData);

            return xmlDoc.DocumentElement;
        }

        public override XmlNode Encrypt(XmlNode node)
        {
            string encryptedData = encryption.Encrypt(node.OuterXml);

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.PreserveWhitespace = true;
            xmlDoc.LoadXml("<EncryptedData>" +
                encryptedData + "</EncryptedData>");

            return xmlDoc.DocumentElement;
        }
    }
}