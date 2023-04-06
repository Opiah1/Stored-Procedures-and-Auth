using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using SPandAuth.Data;
using SPandAuth.Dbervices;
using SPandAuth.Models;
using SPandAuth.RequestObj;
using SPandAuth.ResponseObj;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Security.Cryptography;

namespace SPandAuth.Services
{
    public class dbOperations : IdbOperations
    {
        public static User user = new User();
        private Random random = new Random();
        private readonly IConfiguration _config;
        private readonly DataContext _context; 
        public dbOperations(IConfiguration config, DataContext context)
        {
            _config = config;
            _context = context;
        }

        public string RandomNumberic(int length)
        {
            const string chars = "2345678";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            };
        }
        public CreateAcctData RegisterAccount(UserRegisterRequest request)
        {
            CreateAcctData crtact = new CreateAcctData();

            try
            {
                string AccountNumber = RandomNumberic(10);
                CreatePasswordHash(request.password, out byte[] passwordHash, out byte[] passwordSalt);
                var prfl = new User
                {
                    ID = Guid.NewGuid(),
                    Username = request.UserName,
                    Email = request.Email,
                    FullName = request.FullName,
                    AccountNUmber = AccountNumber,
                    Balance = request.Balance,
                    PasswordHash = passwordHash,
                    PasswordSalt = passwordSalt,
                };
                _context.Users.Add(prfl);
                _context.SaveChanges();

                crtact.issuccessful = true;
                crtact.message = "success";
                crtact.AccountNumber = AccountNumber;
                crtact.UserName = request.UserName;
            }
            catch (Exception ex)
            {
                crtact.issuccessful = false;
                crtact.message = ex.Message;
            }
            return crtact;
        }

        public BalanceResponse2 BalanceEnquiry(string accountnumber)
        {
            bool Status = false;
            string Message = "";
            if(accountnumber == null)
            {
                return null;
            }
            try
            {
                SqlParameter retval = new SqlParameter("@retval", SqlDbType.VarChar, 4);
                retval.Direction = System.Data.ParameterDirection.Output;

                SqlParameter retmsg = new SqlParameter("@retmsg", SqlDbType.VarChar, 200);
                retmsg.Direction = System.Data.ParameterDirection.Output;

                var data =_context.BalanceResponses.FromSqlRaw<BalanceResponse>("BalanceEnquiry @accountnumber,@retval out, @retmsg OUT",
                new SqlParameter("@accountnumber", accountnumber),
                retval, retmsg).AsEnumerable().FirstOrDefault();



                var xRetval = retval.Value.ToString();
                string xRetMsg = retmsg.Value.ToString();
                Message = xRetMsg;
                Status = true;

                if (data != null)
                {
                    return new BalanceResponse2 { retval = xRetval, retmessage = Message, Enquiry=data};
                }
                else
                {
                    return new BalanceResponse2 { retval = "1", retmessage = "Account Has No Valid Customer Record" };
                }

            }

            catch 
            {
                throw;
            }
        }
    }

}
