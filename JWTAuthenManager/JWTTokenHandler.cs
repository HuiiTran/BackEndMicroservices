using JWTAuthenManager.Models;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Bson;
using MongoDB.Driver;

using System;
using System.Collections;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace JWTAuthenManager
{
    public class JWTTokenHandler
    {
        public const string JWT_SECURITY_KEY = "379d884a54d81c27d9e9c23c342940516710b05cea2ef95775844e680ba6037d";
        private const int JWT_TOKEN_VALIDITY_MINS = 20;

        private readonly List<UserAccount> userAccounts;
        public JWTTokenHandler() 
        {
            userAccounts = new List<UserAccount>();

            //
            MongoClient client = new MongoClient("mongodb://localhost:27017");
            IMongoDatabase database = client.GetDatabase("AllUser");

            var collection = database.GetCollection<UserAccount>("AllUser");

            List<UserAccount> documents = collection.Find(new BsonDocument()).ToList();


            foreach (UserAccount document in documents)
            {
                userAccounts.Add(document);
            }
        }

        public AuthenticationResponse? GenerateJwtToken(AuthenticationRequest authenticationRequest)
        {
            if(string.IsNullOrWhiteSpace(authenticationRequest.UserName) || string.IsNullOrWhiteSpace(authenticationRequest.Password))
                return null;
            

            var userAccount = userAccounts.Where(x => x.UserName == authenticationRequest.UserName && x.Password == authenticationRequest.Password).FirstOrDefault();
            if (userAccount == null) return null;

            var tokenExpiryTimeSamp = DateTime.Now.AddMinutes(JWT_TOKEN_VALIDITY_MINS);
            var tokenKey = Encoding.ASCII.GetBytes(JWT_SECURITY_KEY);
            var claimsIdentity = new ClaimsIdentity(new List<Claim>
            {
//                new Claim(ClaimTypes.Sid, userAccount.Id.ToString()),
                new Claim("Id", userAccount.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Name, authenticationRequest.UserName ),
                new Claim("Role", userAccount.Role),

            });

            var signingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(tokenKey),
                SecurityAlgorithms.HmacSha256Signature);

            var securityTokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claimsIdentity,
                Expires = tokenExpiryTimeSamp,
                SigningCredentials = signingCredentials
            };


            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            var securityToken = jwtSecurityTokenHandler.CreateToken(securityTokenDescriptor);
            var token = jwtSecurityTokenHandler.WriteToken(securityToken);


            return new AuthenticationResponse
            {
                Id = userAccount.Id,
                UserName = userAccount.UserName,
                Role = userAccount.Role,
                ExpireIn = (int)tokenExpiryTimeSamp.Subtract(DateTime.Now).TotalSeconds,
                JwtToken = token,
            };
        }
    }
}
