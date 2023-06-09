using Newtonsoft.Json;
using PostavkaTovarov.models;
using PostavkaTovarov.nav;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PostavkaTovarov.pages
{
    /// <summary>
    /// Логика взаимодействия для PageRegistration.xaml
    /// </summary>
    public partial class PageRegistration : Page
    {
        public PageRegistration()
        {
            InitializeComponent();
           // PsbPassword.MaxLength = 6;
        }
        private const string badpass = "Плохой пароль";
        private const string normalpass = "Средний пароль";
        private const string Goodpass = "Хороший пароль";
        private const string Greatpass = "Замечательный пароль";
        private const string Urefulpass = "Отличный пароль";
        private const string error = "Пароль должен быть не менее 6 символов";
        public string ContentPass { get; set; }
        public void PasswordStrength()
        {
            int score = 0;
            if(PsbPassword.Password.Length < 4)
            {
                MessageBox.Show("Bad Password !");
                return;
            }
            var regex = new Regex(@"(.*[0 - 9].*[0 - 9].*[0 - 9])");
            if (regex.IsMatch(PsbPassword.Password))
            {
                ContentPass = normalpass;
                return;
            }
            regex = new Regex(@"(.*[!,@,#,$,%,^,&,*,?,_,~].*[!,@,#,$,%,^,&,*,?,_,~])");
            if (regex.IsMatch(PsbPassword.Password))
            {
                ContentPass = Goodpass;
                return;
            }
            regex = new Regex(@"([a - z].*[A - Z])|([A - Z].*[a - z])");
            if(regex.IsMatch(PsbPassword.Password))
            {
                ContentPass = Greatpass;
            }
            else
            {
                ContentPass = error;
                return;
            }
            regex = new Regex(@"([a - zA - Z])");
            var regex1 = new Regex(@"([0 - 9])");
            if (regex.IsMatch(PsbPassword.Password) && regex1.IsMatch(PsbPassword.Password))
            {
                ContentPass = Urefulpass;
                return;
            }
        }
        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            NavigateData.FrmMain.GoBack();
        }
        //public void CheckPassword()
        //{
        //    var _password = PsbPassword.Password;
        //    if(_password.Length < 6)
        //    {
        //        MessageBox.Show("Пароль не должен быть меньше 6 символов");
        //    }
        //   else if(_password.Length > 6)
        //}
        private async void BtnCreateUser_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string url = "https://mgok6-api.easy4.ru/create-user";
                HttpClient client = new HttpClient();
                var request = new CreateUser()
                {
                    name = TxbUserName.Text,
                    email = TxbUserEmail.Text,
                    password = PsbPassword.Password,
                    role = TxbRole.Text
                };
                var requestJson = JsonConvert.SerializeObject(request);
                StringContent sc = new StringContent(requestJson, Encoding.UTF8, "application/json");

                var response = await client.PostAsync(url, sc);
                var regex = new Regex(@"(.*[!,@,#,$,%,^,&,*,?,_,~].*[!,@,#,$,%,^,&,*,?,_,~])");
                if (PsbPassword.Password.Length < 6)
                {
                    PsbPassword.Background = new SolidColorBrush(Colors.Red);
                    MessageBox.Show(error);
                    //ContentPass = error;
                    return;
                }
                if (regex.IsMatch(PsbPassword.Password))
                {
                    MessageBox.Show("Пароль должен состоять из 3 цифр");
                    return;
                }
                if (!PsbPassword.Password.Any(Char.IsUpper))
                {
                    MessageBox.Show("Пароль должен содержать букву верхнего регистра");
                }
                //string specialCh = @"%!@#$%^&*()?/>.<,:;'\|}]{[_~`+=-" + "\"";
                //char[] specialChArray = specialCh.ToCharArray();
                //foreach (char ch in specialChArray)
                //{
                //    if (PsbPassword.Password.Contains(ch))
                //    {
                //        return;
                //    }
                //    else
                //    {
                //        MessageBox.Show("Пароль должен содержать спецсимволы");
                //        return;
                //    }
                //}

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    MessageBox.Show("Регистрация прошла успешно !");
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message.ToString());
            }
        }
    }
}
