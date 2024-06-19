using Android.Widget;
using Microsoft.Maui.Platform;
using QRCoder;
using SQLite;
using System.Diagnostics.Metrics;
using System.Windows.Input;


namespace Points
{
    public partial class UserPage : ContentPage
    {
        List<string> grupos = new List<string>();
        int count = 1;
        SQLiteConnection conn = new SQLiteConnection(Constants.DatabasePath);

        public UserPage()
        {
            InitializeComponent();
        }

        private void OnCounterClicked(object sender, EventArgs e)
        {

        }

        private async void Create_Clicked(object sender, EventArgs e)
        {
            string result = await DisplayPromptAsync("Nombre", "Introduzca un nombre para el grupo :");
            while(result == "")
            { 
               result = await DisplayPromptAsync("Nombre", "Introduzca un nombre no nulo :");
            }
            
           Create_truly(sender,e, result);
        }

        private void Btn_Clicked(object sender, EventArgs e)
        {
            string username = this.Title.Substring(0, this.Title.IndexOf(' '));
            DisplayAlert("a", "Generando codigo QR para usuario " + username, "Ok");
                QRCodeGenerator qrGenerator = new QRCodeGenerator();
                QRCodeData qrCodeData = qrGenerator.CreateQrCode(username + "us", QRCodeGenerator.ECCLevel.H);
                PngByteQRCode qRCode = new PngByteQRCode(qrCodeData);
                byte[] qrCodeBytes = qRCode.GetGraphic(20);
                ImageSource b = ImageSource.FromStream(() => new MemoryStream(qrCodeBytes));
                Navigation.PushAsync(new DisplayCodePage(b));
        }
        private async void Create_truly(object sender, EventArgs e, string result, bool skip = false)
        {
            Grid grid = new Grid
            {
                BackgroundColor = Colors.Grey,
               
            };
            Microsoft.Maui.Controls.Button bot2 = new Microsoft.Maui.Controls.Button
            {
                Text = "Add user",
                HorizontalOptions = LayoutOptions.End,
                VerticalOptions = LayoutOptions.Start,
                CommandParameter = result,

            };

            Image img = new Image
            {
                Source = "temp",
                HorizontalOptions = LayoutOptions.Start,
                VerticalOptions = LayoutOptions.Center,
                WidthRequest = 100,

            };
            Microsoft.Maui.Controls.Button bot3 = new Microsoft.Maui.Controls.Button
            {
                Text = "Check Users",
                HorizontalOptions = LayoutOptions.End,
                VerticalOptions = LayoutOptions.End,
               
                CommandParameter = result,
            };

            grid.Add(new Label
            {
                Text = result,
                FontSize = 14,
                TextColor = Colors.Black,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
            });
            grid.Add(bot2, 0, 0);
            grid.Add(bot3, 0, 0);

            Basee.Add(grid, 0, count);

           
            conn.CreateTable<Group>();
            if (!skip)
            {

                Group g = new Group
                {
                    GroupName = result,
                };

                conn.Insert(g);

            }
            Grid grid2 = new Grid
            {
                BackgroundColor
            = Colors.White,
            };
            Microsoft.Maui.Controls.Button bot = new Microsoft.Maui.Controls.Button
            {
                Text = "Create",
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
               
            };

            bot.Clicked += async (sender, args) => Create_Clicked(bot, e);

            grid2.Add(bot, 0, 0);

            Basee.Add(grid2, 0, count + 1);
            await DisplayAlert("A", "DONE","b");
            count++;


        }
    }

}
