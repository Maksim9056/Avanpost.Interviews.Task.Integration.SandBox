global using Avanpost.Interviews.Task.Integration.Data.DbCommon;
global using Avanpost.Interviews.Task.Integration.Data.Models;
global using Avanpost.Interviews.Task.Integration.Data.Models.Models;
global using Avanpost.Interviews.Task.Integration.Data.DbCommon.DbModels;
global using Microsoft.EntityFrameworkCore;

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
            throw new NotImplementedException();
        }

        public IEnumerable<UserProperty> GetUserProperties(string userLogin)
        {
            //Обработать в update
            throw new NotImplementedException();
        }

        public bool IsUserExists(string userLogin)
        {
            throw new NotImplementedException();
        }

        public void UpdateUserProperties(IEnumerable<UserProperty> properties, string userLogin)
        {


        }

        public IEnumerable<Permission> GetAllPermissions()
        {
            throw new NotImplementedException();
        }

        public void AddUserPermissions(string userLogin, IEnumerable<string> rightIds)
        {

        }

        public void RemoveUserPermissions(string userLogin, IEnumerable<string> rightIds)
        {

        }

        public IEnumerable<string> GetUserPermissions(string userLogin)
        {
            throw new NotImplementedException();
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

