using BitzerIoC.Infrastructure.Utilities;
using Microsoft.Extensions;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using BitzerIoC.Infrastructure.Security;
using System.Text;
using Microsoft.AspNetCore.Identity;

namespace BitzerIoC.IntegrationTest
{

    public class MyUser
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
    public class Program
    {
        public Program()
        {

        }

        public static void Main(string[] args)
        {

            //-------------------Hash Passord--------------------------
            string salt = "9";
            string password = "GVG%55pk";
            IPasswordHasher<MyUser> _hasher = new PasswordHasher<MyUser>();
            var hashedPassword = _hasher.HashPassword(new MyUser() { Username = "khuram.mails@gmail.com" }, password + salt);
   
            PasswordVerificationResult status = _hasher.VerifyHashedPassword(new MyUser() { Username = "khuram.mails@gmail.com" }, hashedPassword, password+salt);
            if (status.Equals(PasswordVerificationResult.Success))
            {
                Console.WriteLine("Hurrrrrrrrah");
            }

            Console.ReadKey();




            //--------------------------------------------------


            //string UserName = "Khuram";
            //string callbackUrl = "http://calbackurl.com";
            //string loginUrl = "http://loginurl.com";
            //string htmlmessage = String.Format("<b>Hi {0}.</b><br/><br/>Please click<b> <a href='{1}'> here </a></b>" +
            //                      "to reset your password. <br/><br/>The link is valid for 24 hours.<br/><br/><br/><b>If you did NOT request a new password," +
            //                      "do not click on the link. </b><br/><br/>You can access the Remote Caretaking system <a href='{2}'> here. </a>",
            //                      UserName, callbackUrl, loginUrl);

            //string htmlMessage2 = String.Format("<b>Hi {0}.</b><br/><br/>You have been invited to the BitzerIoc system." +
            //                          "<br/><br/>User name = {0} <br/><br/>" +
            //                          "To activate your account and create a password please <a href='{1}' > click here. </a>" +
            //                          "<br/><br/>You can access the Remote Caretaking system <a href='{2}'> here. </a> <br/><br/>Best regards",
            //                          UserName, callbackUrl, loginUrl);
            //Console.WriteLine(htmlMessage2);

            //-----------------------------------------------------------------------------//
            //string queryString = "http://msn.com?p1=6&p2=7&p3=8";
            //if (queryString.Contains("?"))
            //{
            //    queryString = queryString.Remove(0, queryString.IndexOf('?') + 1);
            //}          
            //Dictionary<string, string> queryParameters = new Dictionary<string, string>();
            //string[] querySegments = queryString.Split('&');
            //foreach (string segment in querySegments)
            //{
            //    string[] parts = segment.Split('=');
            //    if (parts.Length > 0)
            //    {
            //        string key = parts[0].Trim(new char[] { '?', ' ' });
            //        string val = parts[1].Trim();
            //        queryParameters.Add(key, val);

            //    }
            //}

            //------------------------- Replace a specific TExt------------------
            //string returnUrl = "redirect_uri=http://loclahost:5000:/signin-oidc";
            //string redirectUri = null;
            //Dictionary<string, string> queryStringCollection = GenericHelper.ParseQueryString(returnUrl);
            //queryStringCollection.TryGetValue("redirect_uri", out redirectUri);

            //string removeString = "signin-oidc";
            //int index = redirectUri.IndexOf(removeString);
            //string cleanPath = (index < 0)
            //    ? redirectUri
            //    : redirectUri.Remove(index, removeString.Length);
            //var a = cleanPath;

            //---------------------------- Hashing --------------------------------

            //Console.Write("Enter a password: ");
            //string password = Console.ReadLine();
            //string salt = "7da61e6725aa27ca4f5a5ae0e73ea7dd";
            //string oldpassword = "";
            //var hash = HashSecurity.GetHash(password + salt);


            //if (HashSecurity.CompareHashText(password, oldpassword, salt))
            //    Console.WriteLine("Logged in");
            //else
            //    Console.WriteLine("Fail Login attempt");


            ////// generate a 128 - bit salt using a secure PRNG
            //// byte[] salt = HashSecurity.GetSalt(128 / 8);

            //// // derive a 256-bit subkey (use HMACSHA1 with 10,000 iterations)
            //// string hashed = HashSecurity.Hash(password, salt, 128 / 8, 10000, KeyDerivationPrf.HMACSHA1);
            //// Console.WriteLine("Salt:" + Convert.ToBase64String(salt));
            //// Console.WriteLine($"Hashed: {hashed}");

            //Console.WriteLine(hash);

            //------------------------------------- Static Test -----------------------------------

            //ClassA.Print();
            //Console.ReadKey();
            //ClassA.Print();
            //Console.ReadKey();
            //ClassA.Print();
            //Console.ReadKey();

            //--------------------------------------------------------------------------------------




        }


    }


    public static class ClassA
    {
        public static void Print()
        {
            int value = 1;
            Console.WriteLine(value);
            value = value + 1;
        }

    }

}
