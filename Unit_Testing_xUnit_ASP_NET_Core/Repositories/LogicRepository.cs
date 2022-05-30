using Unit_Testing_xUnit_ASP_NET_Core.Models;
using Unit_Testing_xUnit_ASP_NET_Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Unit_Testing_xUnit_ASP_NET_Core.Repositories
{
    public class LogicRepository
    {
        private readonly UserRepository userRepository;
        private readonly jwtToken jwttoken;
        public LogicRepository(UserRepository userRepository, jwtToken jwttoken)
        {
            this.userRepository = userRepository;
            this.jwttoken = jwttoken;
        }

        public async Task<dynamic> LogicChangePassword(string username, modelChangePassword model)
        {
            //Get Data By Username in tbl_user
            var getuser = await userRepository.GetUserbyusername(username);
            var user = getuser.FirstOrDefault();
            if (getuser.Count < 1)
            {
                return new { Code = "404", Hasil = "Username Not Found" };
            }

            //Cek Password in tbl_user
            if (BCrypt.Net.BCrypt.Verify(model.password, user.password) == false)
            {
                return new { Code = "400", Hasil = "Password is wrong" };
            }

            //Cek newpass harus sama dengan confirmpass
            if (model.newpassword != model.confirmpassword)
            {
                return new { Code = "400", Hasil = "newpassword is not the same as cofirmpassword" };
            }

            //Encrypt Password
            string passwordhash = BCrypt.Net.BCrypt.HashPassword(model.newpassword);
            return new { Code = "200", Hasil = passwordhash };

        }
        
        public async Task<dynamic> LogicRegister(modelInputUser user)
        {
            //Cek Username and Email
            var getuser = await userRepository.CekUser(user.username, user.email);

            if (getuser.Count > 0)
            {
                return new { Code = "404", Hasil = getuser.FirstOrDefault().message };
            }

            //Encrypt Password
            string passwordhash = BCrypt.Net.BCrypt.HashPassword(user.password);
            return new { Code = "200", Hasil = passwordhash };

        }
        
        public async Task<dynamic> LogicLogin(modelLogin login)
        {
            //Get data By Username
            var getuser = await userRepository.GetUserbyusername(login.username);
            var user = getuser.FirstOrDefault();
            if (getuser.Count < 1)
            {
                return new { Code = "404", Hasil = "Username Not Found" };
            }
            //Cek Password
            if (BCrypt.Net.BCrypt.Verify(login.password, user.password) == false)
            {
                return new { Code = "400", Hasil = "Password is wrong" };
            }
            else if (user.online == true)
            {
                if (user.expiredToken >= Utilities.Time.timezone(DateTime.Now))
                {
                    return new { Code = "404", Hasil = "user is online" };
                }

            }
            //Token JWT
            var jwt = jwttoken.jwt(login.username);
            //Update tbl_user
            InsertLogin data = new InsertLogin();
            data.username = user.username;
            data.token = jwt.Token;
            data.expiredToken = jwt.timejwt;
            data.online = true;
            return new { Code = "200", Hasil = data, Token = jwt.Token };
        }
    }
}
