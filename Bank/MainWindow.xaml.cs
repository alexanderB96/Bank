using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Configuration;

namespace Bank
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string connectionString;
        public MainWindow()
        {
            InitializeComponent();
            connectionString = ConfigurationManager.ConnectionStrings["ConnectBD"].ConnectionString;
        }


        private void Password_GotFocus(object sender, RoutedEventArgs e)
        {
            if (Password.Text == "Пароль")
                Password.Clear();
        }
        private void Login_GotFocus(object sender, RoutedEventArgs e) 
        {
            if (Login.Text == "Логин")
                Login.Clear();
        }
        

        private void Button_Click(object sender, RoutedEventArgs e)
        {
           string FIO = "null";
        try
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();


                using (SqlCommand getUserCommand = new SqlCommand("[dbo].[Login]", connection)
                {
                    CommandType = CommandType.StoredProcedure
                })
                {

                        getUserCommand.Parameters.Add("@ULogin", SqlDbType.VarChar).Value = Login.Text;
                        getUserCommand.Parameters.Add("@UPass", SqlDbType.VarChar).Value = Password.Text;

                       getUserCommand.Parameters.Add("@FIO", SqlDbType.VarChar, 512).Direction = ParameterDirection.Output;

                    getUserCommand.ExecuteNonQuery();

                    var  value = getUserCommand.Parameters["@FIO"].Value;
                       
                        FIO = Convert.ToString(value);
                        if (FIO != "")
                        {
                            MessageBox.Show("Вы авторизовались как:" + "\n" + FIO);
                            Home home = new Home();
                            home.Title = FIO;
                            this.Hide();
                            home.Show();

                           

                        }
                        else
                        {
                            MessageBox.Show("Пользователь не найден");
                        }
                }
            }

            
        }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

       
    }
}
