using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography.Pkcs;
using System.IO;
using Org.BouncyCastle.Cms;
using Org.BouncyCastle.X509;
using Org.BouncyCastle.X509.Store;

namespace InVisualLib.p7m
{
    public static class clsDecrypt
    {
        public static string ReadXmlSigned(string filePath, bool validateSignature = true)
        {
            try
            {
                // Most times input will be a plain (non-Base64-encoded) file.
                using (var inputStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    return m_readXmlSigned(inputStream, validateSignature);
                }
            }
            catch (CmsException)
            {
                return m_readXmlSignedBase64(filePath, validateSignature);
            }
        }

        static string  m_readXmlSignedBase64(string filePath, bool validateSignature = true)
        {
            return m_readXmlSigned(new MemoryStream(Convert.FromBase64String(File.ReadAllText(filePath))), validateSignature);
        }

        static string m_readXmlSigned(Stream stream, bool validateSignature = true)
        {

            CmsSignedData signedFile = new CmsSignedData(stream);

            if (validateSignature)
            {
                IX509Store certStore = signedFile.GetCertificates("Collection");
                ICollection certs = certStore.GetMatches(new X509CertStoreSelector());
                SignerInformationStore signerStore = signedFile.GetSignerInfos();
                ICollection signers = signerStore.GetSigners();

                foreach (object tempCertification in certs)
                {
                    Org.BouncyCastle.X509.X509Certificate certification = tempCertification as Org.BouncyCastle.X509.X509Certificate;

                    foreach (object tempSigner in signers)
                    {
                        SignerInformation signer = tempSigner as SignerInformation;
                        if (!signer.Verify(certification.GetPublicKey()))
                        {
                            //throw new FatturaElettronicaSignatureException(Resources.ErrorMessages.SignatureException);
                        }
                    }
                }
            }

            using (var memoryStream = new MemoryStream())
            {
                signedFile.SignedContent.Write(memoryStream);

                StreamReader sr = new StreamReader(memoryStream);

                byte[] b = memoryStream.ToArray();
                string s = Encoding.UTF8.GetString(b);

                return s;
            }
        }

    }
}
