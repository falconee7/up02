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
    /// Логика взаимодействия для PageUser.xaml
    /// </summary>
    public partial class PageUser : Page
    {
        public PageUser()
        {
            InitializeComponent();
            GetProducts();
            //TxbNameUser.Text = name;
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            NavigateData.FrmMain.GoBack();
        }
        public async void GetProducts()
        {
            try
            {
                string url = $"https://mgok6-api.easy4.ru/product";
                HttpClient client = new HttpClient();

                var response = await client.GetAsync(url);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var _clients = JsonConvert.DeserializeObject<List<Product>>(responseContent);
                    GridListProducts.ItemsSource = _clients.ToList();
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
    }
}
