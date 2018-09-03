using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Cryptography;
using System.Text;
using System.IO;

namespace ezapp
{
    /// <summary>
    /// 加解密函數
    /// </summary>
    public class Cryptography
    {
        #region 建構子
        /// <summary>
        /// 加解密函數建構子
        /// </summary>
        public Cryptography()
        {
            Key = "ABCDWXYZ";
            IV = "10558167";
        }
        /// <summary>
        /// 加解密函數建構子
        /// </summary>
        /// <param name="sKey">基底金鑰</param>
        /// <param name="sIV">金鑰參數</param>
        public Cryptography(string sKey , string sIV)
        {
            Key = sKey;
            IV = sIV;
        }
        #endregion
        #region 屬性
        private string _Key;
        private string _IV;

        /// <summary>
        /// 基底金鑰
        /// </summary>
        public string Key
        {
            get { return (string.IsNullOrEmpty(_Key) ? "JOHNSONS" : _Key); }
            set { _Key = value; }
        }
        /// <summary>
        /// 金鑰參數
        /// </summary>
        public string IV
        {
            get { return (string.IsNullOrEmpty(_IV) ? "10558167" : _IV); }
            set { _IV = value; }
        }
        #endregion
        #region 不可逆加密法 MD5
        /// <summary>
        /// MD5 加密法
        /// </summary>
        /// <param name="source">加密文字字串</param>
        /// <returns>
        /// 值:abcdefg
        /// 加密:esZsDxSN6VGbi9JkMSxNZA==
        /// </returns>
        public string MD5Encode(string source)
        {
            //建立一個MD5
            MD5 md5 = MD5.Create();
            //將字串轉為Byte[]
            byte[] bsource = Encoding.Default.GetBytes(source);
            //進行MD5加密
            byte[] crypto = md5.ComputeHash(bsource);
            //把加密後的字串從Byte[]轉為字串
            return Convert.ToBase64String(crypto);
        }
        #endregion
        #region 不可逆加密法 SHA
        /// <summary>
        /// SHA1 加密法
        /// </summary>
        /// <param name="source">加密文字字串</param>
        /// <returns>
        /// 值:abcdefg
        /// 加密:L7XhNBn8iSRoZeejJPR27GJOh0A=
        /// </returns>
        public string SHA1Encode(string source)
        {
            //建立一個SHA1
            SHA1 sha1 = new SHA1CryptoServiceProvider();
            //將字串轉為Byte[]
            byte[] bsource = Encoding.Default.GetBytes(source);
            //進行SHA1加密
            byte[] crypto = sha1.ComputeHash(bsource);
            //把加密後的字串從Byte[]轉為字串
            return Convert.ToBase64String(crypto);
        }
        /// <summary>
        /// SHA256 加密法
        /// </summary>
        /// <param name="source">加密文字字串</param>
        /// <returns>
        /// 值:abcdefg
        /// 加密:fRpUEnsiJQL1t5tfsIAwYRUqRPkrN+I8ZSe69mXU2po=
        /// </returns>
        public string SHA256Encode(string source)
        {
            //建立一個SHA256
            SHA256 sha256 = new SHA256CryptoServiceProvider();
            //將字串轉為Byte[]
            byte[] bsource = Encoding.Default.GetBytes(source);
            //進行SHA256加密
            byte[] crypto = sha256.ComputeHash(bsource);
            //把加密後的字串從Byte[]轉為字串
            return Convert.ToBase64String(crypto);
        }
        /// <summary>
        /// SHA384 加密法
        /// </summary>
        /// <param name="source">加密文字字串</param>
        /// <returns>
        /// 值:abcdefg
        /// 加密:nxH8ExEj+ETBIm9Cm2oKavBSXZ9A8FbH/BbN8bBr2gjjAlVEF6Wfp9z2JHQhlZ0i
        /// </returns>
        public string SHA384Encode(string source)
        {
            //建立一個SHA384
            SHA384 sha384 = new SHA384CryptoServiceProvider();
            //將字串轉為Byte[]
            byte[] bsource = Encoding.Default.GetBytes(source);
            //進行SHA384加密
            byte[] crypto = sha384.ComputeHash(bsource);
            //把加密後的字串從Byte[]轉為字串
            return Convert.ToBase64String(crypto);
        }
        /// <summary>
        /// SHA512 加密法
        /// </summary>
        /// <param name="source">加密文字字串</param>
        /// <returns>
        /// 值:abcdefg
        /// 加密:1xakGIVptoqxtt+sF45XARTN8Oo6HMDjFIbD5BJBvGp2Qk6MN6sm8Jb8he+YhsjLY0GH9P3f9kX7CZ8f9UxrjA==
        /// </returns>
        public string SHA512Encode(string source)
        {
            //建立一個SHA512
            SHA512 sha512 = new SHA512CryptoServiceProvider();
            //將字串轉為Byte[]
            byte[] bsource = Encoding.Default.GetBytes(source);
            //進行SHA512加密
            byte[] crypto = sha512.ComputeHash(bsource);
            //把加密後的字串從Byte[]轉為字串
            return Convert.ToBase64String(crypto);
        }
        #endregion
        #region 可逆加密法 DES
        /// <summary>
        /// DES 加密,使用預設的基底金鑰及參數
        /// </summary>
        /// <param name="source">加密字串</param>
        /// <returns></returns>
        public string DESEncode(string source)
        {
            return DESEncode(source, Key, IV);
        }
        /// <summary>
        /// DES 加密
        /// </summary>
        /// <param name="source">加密字串</param>
        /// <param name="key">基底金鑰(8Bytes)</param>
        /// <param name="iv">參數(8Bytes)</param>
        /// <returns></returns>
        public string DESEncode(string source, string key, string iv)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();

            // Rfc2898DeriveBytes 類別可以用來從基底金鑰與其他參數中產生衍生金鑰。
            // 使用 MD5 來 Hash 出 Rfc2898 需要的 Salt。
            Rfc2898DeriveBytes rfc2898 = new Rfc2898DeriveBytes(key, this.HashByMD5(key));

            // 8 bits = 1 byte，將 KeySize 及 BlockSize 個別除以 8，取得 Rfc2898 產生衍生金鑰的長度。
            des.Key = rfc2898.GetBytes(des.KeySize / 8);
            des.IV = rfc2898.GetBytes(des.BlockSize / 8);

            var dateByteArray = Encoding.UTF8.GetBytes(source);

            // 加密
            using (MemoryStream ms = new MemoryStream())
            using (CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write))
            {
                cs.Write(dateByteArray, 0, dateByteArray.Length);
                cs.FlushFinalBlock();

                return Convert.ToBase64String(ms.ToArray());
            }
        }
        /// <summary>
        /// DES 解密,使用預設的基底金鑰及參數
        /// </summary>
        /// <param name="source">解密字串</param>
        /// <returns></returns>
        public string DESDecode(string source)
        {
            return DESDecode(source, Key, IV);
        }
        /// <summary>
        /// DES 解密
        /// </summary>
        /// <param name="source">解密字串</param>
        /// <param name="key">基底金鑰(8Bytes)</param>
        /// <param name="iv">參數(8Bytes)</param>
        /// <returns></returns>
        public string DESDecode(string source, string key, string iv)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();

            // Rfc2898DeriveBytes 類別可以用來從基底金鑰與其他參數中產生衍生金鑰。
            // 使用 MD5 來 Hash 出 Rfc2898 需要的 Salt。
            Rfc2898DeriveBytes rfc2898 = new Rfc2898DeriveBytes(key, this.HashByMD5(key));

            // 8 bits = 1 byte，將 KeySize 及 BlockSize 個別除以 8，取得 Rfc2898 產生衍生金鑰的長度。
            des.Key = rfc2898.GetBytes(des.KeySize / 8);
            des.IV = rfc2898.GetBytes(des.BlockSize / 8);

            var dateByteArray = Convert.FromBase64String(source);

            // 解密
            using (MemoryStream ms = new MemoryStream())
            using (CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write))
            {
                cs.Write(dateByteArray, 0, dateByteArray.Length);
                cs.FlushFinalBlock();

                return Encoding.UTF8.GetString(ms.ToArray());
            }
        }
        private byte[] HashByMD5(string source)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();

            return md5.ComputeHash(Encoding.UTF8.GetBytes(source));
        }
        #endregion
        #region 可逆加密法 AES
        /// <summary>
        /// AES 加密,使用預設的基底金鑰及參數
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public string AESEncode(string source)
        {
            return AESEncode(source, Key, IV);
        }
        /// <summary>
        /// AES 加密
        /// </summary>
        /// <param name="source">加密字串</param>
        /// <param name="key">基底金鑰(8Bytes)</param>
        /// <param name="iv">參數(8Bytes)</param>
        /// <returns></returns>
        public string AESEncode(string source, string key, string iv)
        {
            RijndaelManaged AES = new RijndaelManaged();
            MD5CryptoServiceProvider MD5 = new MD5CryptoServiceProvider();
            byte[] SourceData = Encoding.Unicode.GetBytes(source);
            byte[] keyData = MD5.ComputeHash(Encoding.Unicode.GetBytes(key));
            byte[] IVData = MD5.ComputeHash(Encoding.Unicode.GetBytes(iv));
            ICryptoTransform transform = AES.CreateEncryptor(keyData, IVData);
            byte[] outputData = transform.TransformFinalBlock(SourceData, 0, SourceData.Length);
            return Convert.ToBase64String(outputData);
        }
        /// <summary>
        /// AES 解密,使用預設的基底金鑰及參數
        /// </summary>
        /// <param name="source">解密字串</param>
        /// <returns></returns>
        public string AESDecode(string source)
        {
            return AESDecode(source, Key, IV);
        }
        /// <summary>
        /// AES 解密
        /// </summary>
        /// <param name="source">解密字串</param>
        /// <param name="key">基底金鑰(8Bytes)</param>
        /// <param name="iv">參數(8Bytes)</param>
        /// <returns></returns>
        public string AESDecode(string source, string key, string iv)
        {
            return decrypt(Convert.FromBase64String(source), key, iv);
        }
        private string decrypt(byte[] source, string key, string iv)
        {
            RijndaelManaged AES = new RijndaelManaged();
            MD5CryptoServiceProvider MD5 = new MD5CryptoServiceProvider();
            byte[] keyData = MD5.ComputeHash(Encoding.Unicode.GetBytes(key));
            byte[] IVData = MD5.ComputeHash(Encoding.Unicode.GetBytes(iv));
            ICryptoTransform transform = AES.CreateDecryptor(keyData, IVData);
            byte[] outputData = transform.TransformFinalBlock(source, 0, source.Length);
            return Encoding.Unicode.GetString(outputData);
        }
        #endregion
    }
}
