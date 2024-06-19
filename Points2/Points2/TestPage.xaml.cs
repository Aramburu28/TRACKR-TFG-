using SQLite;

namespace Points;

public partial class TestPage : ContentPage
{
    int cont = 0;
    SQLiteConnection conn = new SQLiteConnection(Constants.DatabasePath);
    public TestPage()
	{
		InitializeComponent();
	}

    private void btnad_Clicked(object sender, EventArgs e)
    {
        
        if(cont == 0)
        {
            cont++;
            conn.CreateTable<User>();
        }
        User Juan = new User();
        Juan.Username = btnad.Text;
        conn.Insert(Juan);
        Result.Text = "Done";
    }

    private void btnsee_Clicked(object sender, EventArgs e)
    {
        List.Text = "";
        List<User> list = conn.Table<User>().ToList();
        foreach (User item in list)
        {
            
            List.Text += item.Id + " ---> " + item.Username +"\n";
        }
    }
}