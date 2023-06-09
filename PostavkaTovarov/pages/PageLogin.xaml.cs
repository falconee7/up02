using Newtonsoft.Json;
using PostavkaTovarov.models;
using PostavkaTovarov.nav;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
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
using System.Windows.Threading;

namespace PostavkaTovarov.pages
{
    /// <summary>
    /// Логика взаимодействия для PageLogin.xaml
    /// </summary>
    public partial class PageLogin : Page
    {
        private List<SignIn> _signIn;
        public PageLogin()
        {
            InitializeComponent();
        }

        private void BtnSignIn_Click(object sender, RoutedEventArgs e)
        {
            NavigateData.FrmMain.Navigate(new PageRegistration());
        }
        private int attempts = 0;
        private System.Timers.Timer blockTimer;

        private async void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string url = $"https://mgok6-api.easy4.ru/sign-in?email={TxbEmail.Text}&password={PsbPassword.Password}";
                HttpClient client = new HttpClient();

                var response = await client.GetAsync(url);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    attempts = 0;
                    var _signIn = JsonConvert.DeserializeObject<SignIn>(responseContent);

                    switch (_signIn.role)
                    {
                        case "admin":
                            MessageBox.Show(_signIn.role);
                            NavigateData.FrmMain.Navigate(new PageAdmin());
                            break;
                        case "Участник":
                            MessageBox.Show(_signIn.role);
                            NavigateData.FrmMain.Navigate(new PageUser());
                            break;
                        default:
                            MessageBox.Show("Неверная обработка данных");
                            break;
                    }
                } 
                else
                {
                    attempts++;
                    if (attempts == 3)
                    {
                        attempts++;
                        BtnLogin.IsEnabled = false;
                        MessageBox.Show("Вы превысили количество попыток авторизации!\nПопробуйте еще раз через 1 минуту.");
                        blockTimer = new System.Timers.Timer();
                        blockTimer.Interval = 5000; 
                        blockTimer.Elapsed += BlockTimer_Elapsed;
                        blockTimer.Start();
                        return;

                    }
                    MessageBox.Show("Неверный логин или пароль !");
                }
            }
            catch (Exception er)
            {
                er.Message.ToString();
            }
        }
        private void BlockTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            blockTimer.Stop();
            BtnLogin.Dispatcher.Invoke(() => {
                BtnLogin.IsEnabled = true;
            });
        }
    }
}
