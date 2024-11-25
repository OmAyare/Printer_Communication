using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Windows;
using VideoJetApp.Views;

namespace VideoJetApp.Models
{
    public class LoginServices
    {
        SqlConnection ObjSqlConnection;
        SqlCommand ObjSqlCommand;

        public LoginServices()
        {
            ObjSqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["DBConnection"].ConnectionString);
            ObjSqlCommand = new SqlCommand();
            ObjSqlCommand.Connection = ObjSqlConnection;
            ObjSqlCommand.CommandType = CommandType.StoredProcedure;
        }


        public bool CheckLogin(LoginModels loginModels)
        {
            try
            {
                ObjSqlCommand.Parameters.Clear();
                ObjSqlCommand.CommandText = "CheckLogin";
                ObjSqlCommand.Parameters.AddWithValue("@UserName", loginModels.UserName);
                ObjSqlCommand.Parameters.AddWithValue("@Password", loginModels.Password);

                ObjSqlConnection.Open();
                var role = ObjSqlCommand.ExecuteScalar()?.ToString();

                if (role == "admin")
                {
                    //AdminWindow adminWindow = new AdminWindow();
                    //adminWindow.Show();
                    BtnController btnController = new BtnController();
                    btnController.Show();
                    Application.Current.MainWindow.Close(); 
                }
                //else if (role == "Operator")
                //{
                //    OperatorWindow operatorWindow = new OperatorWindow();
                //    operatorWindow.Show();
                //    this.Close();
                //}
                else
                {
                    MessageBox.Show("Invalid username or password.");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                ObjSqlConnection.Close();
            }

            return true;
        }

    }
}

