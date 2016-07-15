using Divisasx.Security.Entities;
using Newtonsoft.Json;
using System;
using System.Security.Cryptography;
using System.Text;

namespace Divisasx.Security.Services
{
    public class TokenManager
    {
        private string _encryptionKey;

        public TokenManager()
        {
            _encryptionKey = "rz8LuOtFBXphj9WQfvFh"; // from app config
        }

        public string GenerateToken(int userId, string userName, string password)
        {
            // simply this with an extension method
            if (userId < 0)
            { 
                throw new ArgumentException("Invalid user id should be grater than zero", "userId"); 
            }

            if (string.IsNullOrEmpty(userName))
            {
                throw new ArgumentNullException("invalid user name");
            }

            if ( string.IsNullOrEmpty(password))
            {
                throw new ArgumentNullException("invalid password");
            }


            int tockenExpirationMinites=1;// get from app configuration file 

            var tocken=new TokenEntity
            {
                UserId=userId,
                UserName = userName,
                Password=Encrypt(password),
                IssuedOn=DateTime.Now,
                ExpiresOn=DateTime.Now.AddMinutes(tockenExpirationMinites)
            };

            var hash = JsonConvert.SerializeObject(tocken);
            return Encrypt(hash);
        }

        public TokenEntity GetTokenEntityFromHash(string tokenHash)
        {
            if (string.IsNullOrEmpty(tokenHash))
            {
                throw new ArgumentNullException("token");
            }

            string decryptedToken = Decrypt(tokenHash);

            if (decryptedToken == null)
            {
                return null;
            }

            var tockenEntity = JsonConvert.DeserializeObject<TokenEntity>(decryptedToken);
            return tockenEntity;
        }

        public bool IsValidTocken(string token)
        {
            try
            {
                var tockenEntity = GetTokenEntityFromHash(token);

                if (tockenEntity == null)
                {
                    return false;
                }

                bool expired = tockenEntity.ExpiresOn < DateTime.Now;
                if (expired)
                {
                    return false;
                }

                if (tockenEntity.UserName == "john")// get user from database
                {
                    string password = "rOjN2nhDlpovfKoUDaNi6g==";
                    // Compare the computed token with the one supplied and ensure they match.
                    return tockenEntity.Password == password;
                }
            }
            catch (CryptographicException)
            {
                return false;
            }
            return false;
        }

        public string Encrypt(string toEncrypt, bool useHashing=true)
        {
            byte[] encryptedBytes = GetTripleDESCryptoTransformBytes(toEncrypt, useHashing, true);
            return Convert.ToBase64String(encryptedBytes, 0, encryptedBytes.Length);
        }

        public string Decrypt(string cipherString, bool useHashing = true)
        {
            byte[] decryptedBytes = GetTripleDESCryptoTransformBytes(cipherString, useHashing, false);
            return UTF8Encoding.UTF8.GetString(decryptedBytes);
        }

        private byte[] GetTripleDESCryptoTransformBytes(string cipherString, bool useHashing, bool encryption)
        {
            byte[] keyArray;
            byte[] result;
            byte[] cryptArray = encryption ? UTF8Encoding.UTF8.GetBytes(cipherString) : Convert.FromBase64String(cipherString);

            if (useHashing)
            {

                using (MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider())
                {
                    keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(_encryptionKey));
                }
            }
            else
            {
                keyArray = UTF8Encoding.UTF8.GetBytes(_encryptionKey);
            }

            using (TripleDESCryptoServiceProvider tripleDESCryp = new TripleDESCryptoServiceProvider())
            {
                tripleDESCryp.Key = keyArray;
                tripleDESCryp.Mode = CipherMode.ECB;
                tripleDESCryp.Padding = PaddingMode.PKCS7;
                ICryptoTransform cTransform = encryption ? tripleDESCryp.CreateEncryptor() : tripleDESCryp.CreateDecryptor();
                result = cTransform.TransformFinalBlock(cryptArray, 0, cryptArray.Length);
            }

            return result;
        }

    }
}
