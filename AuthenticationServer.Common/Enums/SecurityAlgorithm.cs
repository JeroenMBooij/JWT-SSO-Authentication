using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationServer.Common.Enums
{
    public enum SecurityAlgorithm
    {
        // assymmetric
        RsaSha256,
        RsaSha384,
        RsaSha512,
        RsaSha256Signature,
        RsaSha384Signature,
        RsaSha512Signature,
        RsaOAEP,
        RsaPKCS1,
        RsaOaepKeyWrap,

        //ECDsa
        EcdsaSha256,
        EcdsaSha384,
        EcdsaSha512,
        EcdsaSha256Signature,
        EcdsaSha384Signature,
        EcdsaSha512Signature,

        // hash
        SHA256,
        SHA384,
        SHA512,
        Sha256Digest,
        Sha384Digest,
        Sha512Digest,

        // symmetric
        Aes128CbcHmacSha256,
        Aes192CbcHmacSha384,
        Aes256CbcHmacSha512,
        Aes128KW,
        Aes256KW,
        HmacSha256,
        HmacSha384,
        HmacSha512,
        HmacSha256Signature,
        HmacSha384Signature,
        HmacSha512Signature
    }

    public enum EncryptionType
    {
        Asymmetric,
        Hash,
        ECDsa,
        Symmetric
    }
}
