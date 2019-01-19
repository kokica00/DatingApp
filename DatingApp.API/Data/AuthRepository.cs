using System;
using System.Threading.Tasks;
using DatingApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Data
{
    /* This repository implements every IAuthRepo method  */
    public class AuthRepository : IAuthRepository
    {
        private readonly DataContext _context;
        public AuthRepository(DataContext context)
        {
            this._context = context;

        }

        /*  This async method writes new user in database with hmac512 safety measures
            out means that undefined variable can enter but it needs to be initialied inside*/
        public async Task<User> Register(User user, string password)
        {
            byte[] passwordHash, passwordSalt;
            
            CreatePasswordHash(password, out passwordHash, out passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return user;
        }
        /*  This async method takes care of login data 
            If username is correct depending on password verification user is logged in */
        public async Task<User> Login(string username, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Username == username);
            
            if (user == null) {
                return null;
            }

           if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt)) {
               return null;
           }

           return user;
        }  
        /*  If user exists this method returns true */
        public async Task<bool> UserExists(string username)
        {
            if (await _context.Users.AnyAsync(x => x.Username == username)) 
                return true;

            return false;
        }

        /*  This method creates passwordSalt and passwordHash which are needed for better privacy */
        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }   
        }
        /*  This method is reverse from CreatePasswordHash as it checks if entered password
            is same as password from database
            Because passwordHash is byte[] type, bytes need to be compared */
        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
             using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var ComputedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

                for (int i = 0; i < ComputedHash.Length; i++)
                {
                    if (ComputedHash[i] != passwordHash[i]) 
                        return false;
                }

                return true;
            }   
        }
    }
}