using SQLite;

namespace Points;

public partial class SettingsPage : ContentPage
{
    SQLiteConnection conn = new SQLiteConnection(Constants.DatabasePath);
    public SettingsPage()
	{
		InitializeComponent();
      
	}
    private void Mode_Toggled(object sender, ToggledEventArgs e)
    {
        if (App.Current.UserAppTheme == AppTheme.Light)
        {
            App.Current.UserAppTheme = AppTheme.Dark;
            Microsoft.Maui.Controls.ViewExtensions.TranslateTo(ModeIcon, ModeIcon.TranslationX - 25, ModeIcon.TranslationY);

        }
        else
        {
            App.Current.UserAppTheme = AppTheme.Light;
            Microsoft.Maui.Controls.ViewExtensions.TranslateTo(ModeIcon, ModeIcon.TranslationX + 25, ModeIcon.TranslationY);
        }
    }
    private async void Wipe_Clicked(object sender, EventArgs e)
    {
        bool b = await DisplayAlert("Confirm", "This will empty all of your data, are you sure?", "Yes", "No");
        if(b)
        {
            conn.DropTable<User>();
            conn.DropTable<Group>();
            conn.DropTable<UserGroup>();
            conn.CreateTable<User>();
            conn.CreateTable<Group>();
            conn.CreateTable<UserGroup>();
            await DisplayAlert("Correct", "Database emptied", "Return");
        }

    }

    private async void FinDB_Clicked(object sender, EventArgs e)
    {
        await DisplayAlert("DB File Path", Constants.DatabasePath, "Return");
    }

    private void Lout_Clicked(object sender, EventArgs e)
    {
        //while(Navigation.ModalStack.Count > 0)
        //{
        //Navigation.PopAsync();
        //}
        Navigation.PushAsync(new NewPage1());
    }

    private async void GenFile_Clicked(object sender, EventArgs e)
    {
        List<Group> groups = conn.Table<Group>().ToList();
        List<string> names = (from g in groups select g.GroupName).ToList();
        string res = await DisplayActionSheet("Which group do you want to generate a score file for", "b", "c", names.ToArray());
        Group gr = (from g in groups where g.GroupName == res select g).FirstOrDefault();
        List<UserGroup> ug = conn.Table<UserGroup>().ToList();
        ug = (from a in ug where a.GroupId == gr.Id select a).ToList();
        string print = "---------"+ gr.GroupName +"----------\nUser ----------> Points\n";
        List<User> users = conn.Table<User>().ToList();
        foreach(UserGroup u in ug)
        {
            User us = (from use in  users where use.Id == u.UserId select use).FirstOrDefault();
            print += us.Username + " ---------> " + u.Points + "\n";
        }
        string fn = "Attachment.txt";
        string file = Path.Combine(FileSystem.CacheDirectory, fn);
        await DisplayAlert("Generation", "Generating file : \n " + print, "OK");
        File.WriteAllText(file, print);

        await Share.Default.RequestAsync(new ShareFileRequest
        {
            Title = "Share text file",
            File = new ShareFile(file)
        });
    }
}