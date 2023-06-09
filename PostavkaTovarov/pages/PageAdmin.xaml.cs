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

namespace PostavkaTovarov.pages
{
    /// <summary>
    /// Логика взаимодействия для PageAdmin.xaml
    /// </summary>
    public partial class PageAdmin : Page
    {
        public PageAdmin()
        {
            InitializeComponent();
            GetUsers();
            //TxbLoginUser.Text = name;
        }
        public async void GetUsers()
        {
            try
            {
                string url = $"https://mgok6-api.easy4.ru/users";
                HttpClient client = new HttpClient();

                var response = await client.GetAsync(url);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var _clients = JsonConvert.DeserializeObject<List<Users>>(responseContent);
                    GridListUsers.ItemsSource = _clients.ToList();
                }
                else
                {
                    MessageBox.Show("Данные обработать нельзя!");
                }

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message.ToString());
            }
        }

        private async void BtnUserDel1_Click(object sender, RoutedEventArgs e)
        {
            {
                {
                    try
                    {
                        for (int i = 0; i < GridListUsers.SelectedItems.Count; i++)
                        {
                            Users _user = GridListUsers.SelectedItems[i] as Users;
                            string url = $"http://mgok6-api.easy4.ru/delete-user/{_user.id}";



                            HttpClient httpClient = new HttpClient();
                            var requestJson = JsonConvert.SerializeObject(_user);
                            StringContent sc = new StringContent(requestJson, Encoding.UTF8, "application/json");
                            var responce = await httpClient.PostAsync(url, sc);



                            if (responce.StatusCode == System.Net.HttpStatusCode.OK)
                            {
                                MessageBox.Show("Пользователь удалён !");
                                GetUsers();
                            }
                            else
                            {
                                MessageBox.Show("Данные обработать нельзя!");
                            }
                        }



                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message.ToString());
                    }
                }
            }
        }

        private void BtnGoInSite_Click(object sender, RoutedEventArgs e)
        {
            NavigateData.FrmMain.Navigate(new PageUser());
        }
    }
}
