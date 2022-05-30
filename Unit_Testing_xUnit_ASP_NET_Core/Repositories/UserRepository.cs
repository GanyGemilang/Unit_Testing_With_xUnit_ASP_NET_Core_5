using Unit_Testing_xUnit_ASP_NET_Core.Models;
using Dapper;
using Unit_Testing_xUnit_ASP_NET_Core.Models;
using Unit_Testing_xUnit_ASP_NET_Core;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Unit_Testing_xUnit_ASP_NET_Core.Repositories
{
    public class UserRepository
    {
        private readonly SqlConnection db;
        private readonly DynamicParameters parameters = new DynamicParameters();
        private readonly CommandType mysp = CommandType.StoredProcedure;
        public UserRepository(Connectionstring connection)
        {
            db = new SqlConnection(connection.Value);
        }

        public async Task<List<modelUser>> GetUserbyusername(string username)
        {
            string SP = "SP_Getuserbyusername";
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("username", username);
            var data = await db.QueryAsync<modelUser>(SP, parameters, commandType: mysp);
            return data.ToList();
        }

        public async Task<List<modelInputUser>> GetUser()
        {
            string SP = "SP_GetUser";
            var data = await db.QueryAsync<modelInputUser>(SP, parameters, commandType: mysp);
            return data.ToList();
        }

        public async Task<int> InsertUser(modelInputUser user)
        {
            string SP = "SP_InsertUser";
            DynamicParameters parameters = new DynamicParameters();
            parameters.AddDynamicParams(user);
            var data = await db.ExecuteAsync(SP, parameters, commandType: mysp);
            return data;
        }
        
        public async Task<List<dynamic>> DeleteUser(string username)
        {
            string SP = "SP_DeleteUser";
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("username", username);
            var data = await db.QueryAsync<dynamic>(SP, parameters, commandType: mysp);
            return data.ToList();
        }
        
        public async Task<List<ResponseModel>> UpdateUser(modelUpdateUser user)
        {
            string SP = "SP_UpdateUser";
            DynamicParameters parameters = new DynamicParameters();
            parameters.AddDynamicParams(user);
            var data = await db.QueryAsync<ResponseModel>(SP, parameters, commandType: mysp);
            return data.ToList();
        }
        
        public async Task<List<dynamic>> CekUser(string username, string email)
        {
            string SP = "SP_GetuserbyusernameorEmail";
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("username", username);
            parameters.Add("email", email);
            var data = await db.QueryAsync<dynamic>(SP, parameters, commandType: mysp);
            return data.ToList();
        }

        public async Task<int> Login(InsertLogin login)
        {
            string SP = "SP_UpdateLogin";
            DynamicParameters parameters = new DynamicParameters();
            parameters.AddDynamicParams(login);
            var data = await db.ExecuteAsync(SP, parameters, commandType: mysp);
            return data;
        }
        
        public async Task<int> Logout(string username)
        {
            string SP = "SP_Logout";
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("username", username);
            var data = await db.ExecuteAsync(SP, parameters, commandType: mysp);
            return data;
        }
        
        public async Task<int> ChangePassword(string username, string password)
        {
            string SP = "SP_UpdatePassword";
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("username", username);
            parameters.Add("password", password);
            var data = await db.ExecuteAsync(SP, parameters, commandType: mysp);
            return data;
        }

        public async Task<List<dynamic>> ForgotPassword(string email, string password)
        {
            string SP = "SP_ForgotPassword";
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("email", email);
            parameters.Add("password", password);
            var data = await db.QueryAsync<dynamic>(SP, parameters, commandType: mysp);
            return data.ToList();
        }
    }
}
