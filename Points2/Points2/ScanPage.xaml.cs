using SQLite;

namespace Points;

public partial class ScanPage : ContentPage
{
    string points;
    string uname;
    string modo;
    string gname;
    SQLiteConnection conn = new SQLiteConnection(Constants.DatabasePath);
    public ScanPage(string a,string b, string c, string d)
	{
		InitializeComponent();
        this.num.Text = num.Text + " (Add " + b +" points in group " + a + " to user: " + c+")";
        gname = a;
        points = b;
        uname = c;
        modo = d;
	}
    private async void cameraBarcodeReaderView_BarcodesDetected(object sender, ZXing.Net.Maui.BarcodeDetectionEventArgs e)
    {
        Device.BeginInvokeOnMainThread(async () =>
        {

            string username = e.Results[0].Value;
            if (modo == "add")
            {
                if (username == uname)
                {
                    num.Text = points + " points added " + username + " in " + gname;
                    await Task.Delay(20000);
                    Navigation.PopAsync();
                }
                else
                {
                    num.Text = "QR code not valid(" + username + ")";
                    await Task.Delay(20000);
                    Navigation.PushAsync(new TabbedPg());
                }
            }
            else
            {
                User usuario = new User
                {
                    Username = username,
                };
                conn.Insert(usuario);
                List<Group> gs = conn.Table<Group>().ToList();
                Group g = (from gr in gs where gr.GroupName == gname select gr).FirstOrDefault();
                UserGroup ug = new UserGroup
                {
                    UserId = usuario.Id,
                    GroupId = g.Id
                };
                conn.Insert(ug);
                num.Text = "User " + username + "added to the group " + gname;
                
            }


        });
        }
}