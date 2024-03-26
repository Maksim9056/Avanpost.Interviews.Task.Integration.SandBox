global using Avanpost.Interviews.Task.Integration.Data.DbCommon;
global using Avanpost.Interviews.Task.Integration.Data.Models;
global using Avanpost.Interviews.Task.Integration.Data.Models.Models;
global using Avanpost.Interviews.Task.Integration.Data.DbCommon.DbModels;
global using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Data;

namespace Avanpost.Interviews.Task.Integration.SandBox.Connector
{
    public class ConnectorDb : IConnector
    {
        public string connectr = "";
        public string sql = "";
        public void StartUp(string connectionString)
        {
            connectr = connectionString;
            string provider = GetProviderFromConnectionString(connectionString);

            //string input = "'PostgreSQL.9.5'";
            string output = provider.Replace("'", "");
            string output1 = output.Replace("SQL.9.5", "");
            Console.WriteLine(output); // Выведет: PostgreSQL.9.5
            if (output == "PostgreSQL.9.5") 
            {

                 var d =        output1.ToUpper();
                sql = d;
                Dbcs dbcs = new Dbcs(d);
              
            }
            else
            {

            }

        }
        public void CreateUser(UserToCreate user)
        {
            if (user.Properties != null && user.Properties.Any()) 
            {
                UserProperty userProperty = user.Properties.First(); 
                User a = new User();
                a.Login = user.Login;
                a.TelephoneNumber = "";
                a.FirstName = "";
                a.LastName = "";
                a.MiddleName = "";
                a.IsLead = Convert.ToBoolean(userProperty.Value);

                using (Dbcs dbcs = new Dbcs(connectr))
                {
                    dbcs.Users.Add(a); 
                    dbcs.SaveChanges();  
                }
            }
            else
            {
                throw new ArgumentException("Свойства пользователя отсутствуют", nameof(user));
            }
        }



        public IEnumerable<Property> GetAllProperties()
        {
            List<Property> s = new List<Property>();
            using (var dbcs = new Dbcs(connectr))
            {
                var a = dbcs.ITRoles.ToList();
                foreach (var role in a)
                {
                    Property permission = new Property(role.Id.ToString(), role.Name);
                    s.Add(permission);
                }
                return s;
            }
        }

        
        public IEnumerable<UserProperty> GetUserProperties(string userLogin)
        {
            List<UserProperty> permissions = new List<UserProperty>();
            using (var dbcs = new Dbcs(connectr))
            {
                var ts = dbcs.Users.FirstOrDefault(u => u.Login == userLogin);

                
                if (ts != null)
                {
                    UserProperty permission = new UserProperty(ts.Login, ts.TelephoneNumber);

                    permissions.Add(permission);
                }
            }

            return permissions;


        }
        public bool IsUserExists(string userLogin)
        {
            using (var dbcs = new Dbcs(connectr))
            {
                var user = dbcs.Users.FirstOrDefault(u => u.Login == userLogin);

                return user != null;
            }

        }

        public void UpdateUserProperties(IEnumerable<UserProperty> properties, string userLogin)
        {
            using (var dbcs = new Dbcs(connectr))
            {
                var user = dbcs.Users.FirstOrDefault(u => u.Login == userLogin);
                if (user != null)
                {
                    foreach (var property in properties)
                    {

                        user.TelephoneNumber = property.Value;
                    }
                    dbcs.SaveChanges();
                }
            }
        }

        public IEnumerable<Permission> GetAllPermissions()
        {
            using (var dbcs = new Dbcs(connectr))
            {
               var t = dbcs.RequestRights.ToList();
               List< Permission> permissions = new List< Permission>();
                foreach (var role in t)
                {
                    Permission permission = new Permission(role.Id.ToString(), role.Name, role.Name);
                    permissions.Add(permission);
                }
                var ts = dbcs.ITRoles.ToList();

                foreach (var role in ts)
                {
                    Permission permission = new Permission(role.Id.ToString(), role.Name, role.CorporatePhoneNumber);
                    permissions.Add(permission);
                }



                return permissions;
            }
        }
    
        //"GlavnyyNN"  "Role 1"
        public void AddUserPermissions(string userLogin, IEnumerable<string> rightIds)
        {
            string Id = "";
            foreach (string id in rightIds)
            {
                Id = id;
            }

            string output1 = Id.Replace("Role", "");
            using (var dbcs = new Dbcs(connectr))
            {
                var userITRole = new UserITRole
                {
                    UserId = userLogin,
                    RoleId =Convert.ToInt32( output1)
                };

                dbcs.UserITRoles.Add(userITRole);
                dbcs.SaveChanges();
            }
        }

        public void RemoveUserPermissions(string userLogin, IEnumerable<string> rightIds)
        {
            using (var dbcs = new Dbcs(connectr))
            {
                var user = dbcs.Users.FirstOrDefault(u => u.Login == userLogin);
                if (user != null)
                {
                    foreach (var rightId in rightIds)
                    {
                        string output1 = rightId.Replace("Request", "");

                        
                        if (int.TryParse(output1, out int parsedRightId))
                        {
                            var userRequestRight = dbcs.UserRequestRights.FirstOrDefault(ur => ur.UserId == user.Login && ur.RightId == parsedRightId);
                            if (userRequestRight != null)
                            {
                                dbcs.UserRequestRights.Remove(userRequestRight);
                            }
                        }
                        else
                        {
                        }
                    }
                    dbcs.SaveChanges();
                }
            }

        }
        public IEnumerable<string> GetUserPermissions(string userLogin)
        {

            List<string> permissions = new List<string>();

            using (var dbcs = new Dbcs(connectr))
            {
                var userPermissions = dbcs.UserRequestRights.Where(up => up.UserId == userLogin).ToList();

                foreach (var permission in userPermissions)
                {
                    permissions.Add(permission.UserId);
                }
                return permissions;
            }
        }
        public ILogger Logger { get; set; }


        public static string GetProviderFromConnectionString(string connectionString)
        {
            int providerStartIndex = connectionString.IndexOf("Provider=");

            if (providerStartIndex == -1)
            {
                return "";
            }

            int providerEndIndex = connectionString.IndexOf(";", providerStartIndex);

            if (providerEndIndex == -1)
            {
                return connectionString.Substring(providerStartIndex + "Provider=".Length);
            }
            else
            {
                return connectionString.Substring(providerStartIndex + "Provider=".Length, providerEndIndex - providerStartIndex - "Provider=".Length);
            }
        }
    }
}

