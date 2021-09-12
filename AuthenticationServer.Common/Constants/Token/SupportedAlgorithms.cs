using AuthenticationServer.Common.Enums;
using AuthenticationServer.Common.Models.ContractModels;
using System.Collections.Generic;
using System.Linq;

namespace AuthenticationServer.Common.Constants.Token
{

    //https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/wiki/Supported-Algorithms
    public static class SupportedAlgorithms
    {
        public static List<SecurityDescription> List
        {
            get
            {
                return new List<SecurityDescription>()
                {
                    new SecurityDescription() { Name = SecurityAlgorithm.RsaSha256, Schema = "RS256", Type = EncryptionType.Asymmetric },
                    new SecurityDescription() { Name = SecurityAlgorithm.RsaSha384, Schema = "RS384", Type = EncryptionType.Asymmetric },
                    new SecurityDescription() { Name = SecurityAlgorithm.RsaSha512, Schema = "RS512", Type = EncryptionType.Asymmetric },
                    new SecurityDescription() { Name = SecurityAlgorithm.RsaSha256Signature, Schema = "http://www.w3.org/2001/04/xmldsig-more#rsa-sha256", Type = EncryptionType.Asymmetric },
                    new SecurityDescription() { Name = SecurityAlgorithm.RsaSha384Signature, Schema = "http://www.w3.org/2001/04/xmldsig-more#rsa-sha384", Type = EncryptionType.Asymmetric },
                    new SecurityDescription() { Name = SecurityAlgorithm.RsaSha512Signature, Schema = "http://www.w3.org/2001/04/xmldsig-more#rsa-sha512", Type = EncryptionType.Asymmetric },
                    new SecurityDescription() { Name = SecurityAlgorithm.RsaOAEP, Schema = "RS-OAEP", Type = EncryptionType.Asymmetric },
                    new SecurityDescription() { Name = SecurityAlgorithm.RsaPKCS1, Schema = "RSA1_5", Type = EncryptionType.Asymmetric },
                    new SecurityDescription() { Name = SecurityAlgorithm.RsaOaepKeyWrap, Schema = "http://www.w3.org/2001/04/xmlenc#rsa-oaep", Type = EncryptionType.Asymmetric },

                    new SecurityDescription() { Name = SecurityAlgorithm.EcdsaSha256, Schema = "ES256", Type = EncryptionType.ECDsa },
                    new SecurityDescription() { Name = SecurityAlgorithm.EcdsaSha384, Schema = "ES384", Type = EncryptionType.ECDsa },
                    new SecurityDescription() { Name = SecurityAlgorithm.EcdsaSha512, Schema = "ES512", Type = EncryptionType.ECDsa },
                    new SecurityDescription() { Name = SecurityAlgorithm.EcdsaSha256Signature, Schema = "http://www.w3.org/2001/04/xmldsig-more#ecdsa-sha256", Type = EncryptionType.ECDsa },
                    new SecurityDescription() { Name = SecurityAlgorithm.EcdsaSha384Signature, Schema = "http://www.w3.org/2001/04/xmldsig-more#ecdsa-sha384", Type = EncryptionType.ECDsa },
                    new SecurityDescription() { Name = SecurityAlgorithm.EcdsaSha512Signature, Schema = "http://www.w3.org/2001/04/xmldsig-more#ecdsa-sha512", Type = EncryptionType.ECDsa },

                    new SecurityDescription() { Name = SecurityAlgorithm.EcdsaSha256, Schema = "SHA256", Type = EncryptionType.Hash },
                    new SecurityDescription() { Name = SecurityAlgorithm.EcdsaSha384, Schema = "SHA384", Type = EncryptionType.Hash },
                    new SecurityDescription() { Name = SecurityAlgorithm.EcdsaSha512, Schema = "SHA512", Type = EncryptionType.Hash },
                    new SecurityDescription() { Name = SecurityAlgorithm.EcdsaSha256Signature, Schema = "http://www.w3.org/2001/04/xmlenc#sha256", Type = EncryptionType.Hash },
                    new SecurityDescription() { Name = SecurityAlgorithm.EcdsaSha384Signature, Schema = "http://www.w3.org/2001/04/xmldsig-more#sha384", Type = EncryptionType.Hash },
                    new SecurityDescription() { Name = SecurityAlgorithm.EcdsaSha512Signature, Schema = "http://www.w3.org/2001/04/xmlenc#sha512", Type = EncryptionType.Hash },

                    new SecurityDescription() { Name = SecurityAlgorithm.Aes128CbcHmacSha256, Schema = "A128CBC-HS256", Type = EncryptionType.Symmetric },
                    new SecurityDescription() { Name = SecurityAlgorithm.Aes192CbcHmacSha384, Schema = "A192CBC-HS384", Type = EncryptionType.Symmetric },
                    new SecurityDescription() { Name = SecurityAlgorithm.Aes256CbcHmacSha512, Schema = "A256CBC-HS512", Type = EncryptionType.Symmetric },
                    new SecurityDescription() { Name = SecurityAlgorithm.Aes128KW, Schema = "A128KW", Type = EncryptionType.Symmetric },
                    new SecurityDescription() { Name = SecurityAlgorithm.Aes256KW, Schema = "A256KW", Type = EncryptionType.Symmetric },
                    new SecurityDescription() { Name = SecurityAlgorithm.HmacSha256, Schema = "HS256", Type = EncryptionType.Symmetric },
                    new SecurityDescription() { Name = SecurityAlgorithm.HmacSha384, Schema = "HS384", Type = EncryptionType.Symmetric },
                    new SecurityDescription() { Name = SecurityAlgorithm.HmacSha512, Schema = "HS512", Type = EncryptionType.Symmetric },
                    new SecurityDescription() { Name = SecurityAlgorithm.HmacSha256Signature, Schema = "http://www.w3.org/2001/04/xmldsig-more#hmac-sha256", Type = EncryptionType.Symmetric },
                    new SecurityDescription() { Name = SecurityAlgorithm.HmacSha384Signature, Schema = "http://www.w3.org/2001/04/xmldsig-more#hmac-sha384", Type = EncryptionType.Symmetric },
                    new SecurityDescription() { Name = SecurityAlgorithm.HmacSha512Signature, Schema = "http://www.w3.org/2001/04/xmldsig-more#hmac-sha512", Type = EncryptionType.Symmetric },
                };
            }
        }
    }
}
