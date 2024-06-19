using SQLite;

namespace Points;

public partial class NewPage1 : Shell
{
    SQLiteConnection conn = new SQLiteConnection(Constants.DatabasePath);
    List<string> columns = new List<string>();
    string username;
    bool dark;
    public NewPage1()
	{
		InitializeComponent();
        conn.CreateTable<User>();
        conn.CreateTable<Group>();
        conn.CreateTable<UserGroup>();
        dark = false;
    }
    private void UName_Completed(object sender, EventArgs e)
    {
        username = UName.Text;

    }

    private void Picker_Loaded(object sender, EventArgs e)
    {
        List<User> list = conn.Table<User>().ToList();
        List<string> names = (from us in list select us.Username).ToList();
        if (names.Count > 0)
        {
            UName.IsVisible = false;
            Switch.IsVisible = true;
        }
        else
        {
            pik.IsVisible = false;
        }
        pik.ItemsSource = names;
        if (names.Count != 0)
        {
            pik.SelectedItem = names[0];
        }
    }

    private void Log_Clicked(object sender, EventArgs e)
    {

        conn.CreateTable<User>();
        User us = new User();
        List<User> list = conn.Table<User>().ToList();
        List<string> names = (from user in list select us.Username).ToList();
        if ((from lo in list where lo.Username == username select lo).Count() == 0)
        {

            us.Username = username;
            conn.Insert(us);
        }
        if (username != "")
        {
            List<User> list2 = conn.Table<User>().ToList();
            User user = (from lo in list2 where lo.Username == username select lo).FirstOrDefault();
            Admin.IsVisible = true;
            Admin.IsEnabled = true;
            User.IsVisible = true;
            User.IsEnabled = true;
            LogIn.IsEnabled = false;
            LogIn.IsVisible = false;
            if(username == "Animal")
            {
                Api.IsVisible = true;
                Api.IsEnabled = true;
            }
            
            User.Title = username + " Groups";
            Admin.Title = "Admin(" + user.Id + ") Groups";
            
        }

    }

    private void pik_SelectedIndexChanged(object sender, EventArgs e)
    {
        username = pik.SelectedItem as string;
    }

    private void Switch_Clicked(object sender, EventArgs e)
    {
        if (UName.IsVisible)
        {
            UName.IsVisible = false;
            pik.IsVisible = true;
            Switch.Text = "Log in as a new user";
        }
        else
        {
            UName.IsVisible = true;
            pik.IsVisible = false;
            Switch.Text = "Log in as an existing user";
        }

    }

    private void Wipe_Clicked(object sender, EventArgs e)
    {

        conn.DropTable<User>();
        conn.DropTable<Group>();
        conn.DropTable<UserGroup>();
    }

    private void Mode_Toggled(object sender, ToggledEventArgs e)
    {
        if (App.Current.UserAppTheme == AppTheme.Dark)
        {
            App.Current.UserAppTheme = AppTheme.Light;
            LogIn.BackgroundColor = Colors.White;
            Admin.BackgroundColor = Colors.White;
            User.BackgroundColor = Colors.White;
            dark = false;
        }
        else
        {
            App.Current.UserAppTheme = AppTheme.Dark;
            DisplayAlert("a", App.Current.UserAppTheme.ToString(), "b");
            LogIn.BackgroundColor = Colors.Black; 
            Admin.BackgroundColor = Colors.Black;
            User.BackgroundColor = Colors.Black;
            dark = true;
        }
    }
}