using SQLite;

namespace Points;

public partial class UsersPage : ContentPage
{

    List<string> grupos = new List<string>();
    int counter = 0;
    List<List<string>> usuarios2 = new List<List<string>>();
    List<List<int>> puntos = new List<List<int>>();
    SQLiteConnection conn = new SQLiteConnection(Constants.DatabasePath);
    int count = 1;
    string usname;
    Stream currentimageStream;
    int adminid;
    public UsersPage()
    {
        InitializeComponent();
        conn.CreateTable<Group>();
        conn.CreateTable<UserGroup>();




    }

    private void OnCounterClicked(object sender, EventArgs e)
    {

    }

    private async void Create_Clicked(object sender, EventArgs e)
    {
        string result = await DisplayPromptAsync("Nombre", "Introduzca un nombre para el grupo :");
        while (result == "" || grupos.IndexOf(result) != -1)
        {
            await Task.Delay(1000);
            result = await DisplayPromptAsync("Nombre", "Introduzca un nombre valido :");
        }
        grupos.Add(result);
        Create_truly(sender, e, result);

    }

    private async void Create_truly(object sender, EventArgs e, string result, bool skip = false)
    {
        Grid grid = new Grid
        {
            BackgroundColor = Colors.Grey,
            ClassId = counter.ToString(),
        };
        Command AddCommand = new Command<string>(
            execute: (string arg) =>
            {
                Add_User(new object(), e, arg);
            });
        Microsoft.Maui.Controls.Button bot2 = new Microsoft.Maui.Controls.Button
        {
            Text = "Add user",
            HorizontalOptions = LayoutOptions.End,
            VerticalOptions = LayoutOptions.Start,
            BackgroundColor = Colors.White,
            ClassId = counter.ToString(),
            Command = AddCommand,
            CommandParameter = result,

        };

        Image img = new Image
        {
            Source = "temp",
            HorizontalOptions = LayoutOptions.Start,
            VerticalOptions = LayoutOptions.Center,
            WidthRequest = 100,

        };
        if (!skip)
        {
            img.Loaded += (sender, args) => { Change_Image(img, e, img); };
        }
        grid.Add(img, 0, 0);
        Command GetCommand = new Command<string>(
            execute: (string arg) =>
            {
                Get_Users(new object(), e, arg);
            });
        Microsoft.Maui.Controls.Button bot3 = new Microsoft.Maui.Controls.Button
        {
            Text = "Check Users",
            HorizontalOptions = LayoutOptions.End,
            VerticalOptions = LayoutOptions.End,
            BackgroundColor = Colors.White,
            Command = GetCommand,
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

        Base.Add(grid, 0, count);

        adminid = Int32.Parse(this.Title.Substring(this.Title.IndexOf("(") + 1, this.Title.IndexOf(")") - 6));
        conn.CreateTable<Group>();
        if (!skip)
        {

            Group g = new Group
            {
                GroupName = result,
                AdminId = adminid,
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
            VerticalOptions = LayoutOptions.Start,
            BackgroundColor = Colors.White,
        };

        bot.Clicked += async (sender, args) => Create_Clicked(bot, e);

        grid2.Add(bot, 0, 0);

        Base.Add(grid2, 0, count + 1);
        count++;


    }

    private async void Btn_Clicked(object sender, EventArgs e)
    {

        string result = await DisplayActionSheet("En que grupo esta el usuario?", "Cancel", null, grupos.ToArray());
        int index = grupos.IndexOf(result);
        string user = await DisplayActionSheet("A que usuario se dara?", "Cancel", null, usuarios2[index].ToArray());
        int index2 = usuarios2[index].IndexOf(user);
    apunto:

        string puntos = await DisplayPromptAsync("Cantidad", "Introduzca la cantidad de puntos a otorgar: ", "Ok");
        if (puntos == null)
        {
            return;
        }
        try
        {
            Int32.Parse(puntos);
        }
        catch (Exception ex)
        {
            await DisplayAlert("Aviso", "No es un numero valido", "Ok");
            goto apunto;
        }
        this.puntos[index][index2] = Int32.Parse(puntos);


        Navigation.PushAsync(new ScanPage(result, puntos, user,"add"));


    }

    private async void Add_User(object sender, EventArgs e, string a)
    {
        List<Group> groups = conn.Table<Group>().ToList();
        List<Group> admingroups = (from g in groups where g.AdminId == adminid select g).ToList();
        string gs = "";
        foreach (Group g in admingroups)
        {
            gs += g.Id + "-------" + g.GroupName + "---------" + g.AdminId + "------" + g.ImageSource + "\n";
        }
        await DisplayAlert("a", gs, "b");
        //entry:
        //    bool taken = false;
        //    int num = Int32.Parse(a);
        //    string username = await DisplayPromptAsync("Introduci usuario", "Introduzca el nombre de usuario: ", "Ok");
        //    if (username == null) { return; }
        //    if (usuarios2[num].IndexOf(username) != -1)
        //    {
        //        DisplayAlert("", "Usuario ya existente en el grupo", "Ok");
        //        await Task.Delay(1000);
        //        goto entry;
        //    }
        //    usuarios2.ElementAt(num).Add(username);
        //    puntos.ElementAt(num).Add(0);
        List<User> users = conn.Table<User>().ToList();
        List<string> names = (from use in users select use.Username).ToList();
        string name = await DisplayActionSheet("Selecciona el usuario a añadir", "Cancelar", null, names.ToArray());
        User user = (from u in users where u.Username == name select u).FirstOrDefault();
        Group group = (from g in groups where g.GroupName == a select g).FirstOrDefault();
        UserGroup userGroup = new UserGroup
        {
            UserId = user.Id,
            GroupId = group.Id,
        };
        conn.CreateTable<UserGroup>();
        conn.Insert(userGroup);

    }
    private async void Get_Users(object sender, EventArgs e, string a)
    {
        List<Group> grupos2 = conn.Table<Group>().ToList();
        //int index = Int32.Parse(a);
        //string result = "Usuarios en grupo "+ grupos[index] + " : \n";
        //foreach(string b in usuarios2[index])
        //{
        //    result += b + " --> " + puntos[index][usuarios2[index].IndexOf(b)] + " puntos\n";
        //}
        //DisplayAlert("a", result, "a");
        Group grouppp = (from gr in grupos2 where gr.GroupName == a select gr).FirstOrDefault();
        List<UserGroup> userGroups = conn.Table<UserGroup>().ToList();
        List<User> useres = conn.Table<User>().ToList();
        List<User> users = (from us in useres join ug in userGroups on us.Id equals ug.UserId where ug.GroupId == grouppp.Id select us).ToList();
        string usuarios = "";
        foreach (User u in users)
        {
            usuarios += u.Id + "-------" + u.Username + "\n";
        }
        await DisplayAlert("a", usuarios, "b");
        string groups = "";
        foreach (Group g in grupos2)
        {
            groups += g.Id + "-------" + g.GroupName + "---------" + g.AdminId + "------" + g.ImageSource + "\n";
        }
        await DisplayAlert("a", groups, "b");

    }

    private async void Add_Clicked(object sender, EventArgs e)
    {
        string result = await DisplayActionSheet("En que grupo esta el usuario?", "Cancel", null, grupos.ToArray());
        int index = grupos.IndexOf(result);
        string user = await DisplayActionSheet("A que usuario se dara?", "Cancel", null, usuarios2[index].ToArray());
        int index2 = usuarios2[index].IndexOf(user);
    apunto:

        string puntos = await DisplayPromptAsync("Cantidad", "Introduzca la cantidad de puntos a otorgar: ", "Ok");
        if (puntos == null)
        {
            return;
        }
        try
        {
            Int32.Parse(puntos);
        }
        catch (Exception ex)
        {
            await DisplayAlert("Aviso", "No es un numero valido", "Ok");
            goto apunto;
        }
        this.puntos[index][index2] = Int32.Parse(puntos);

    }
    private async void Change_Image(object sender, EventArgs e, Image a)
    {
        string result = await DisplayPromptAsync("Seleccion de imagen", "Desea darle una imagen al grupo?", "Si", "No");
        if (result == null || result == "No") { return; }
        FileResult photo = await MediaPicker.Default.PickPhotoAsync(new MediaPickerOptions
        {
            Title = "Select your photo"
        });
        if (photo != null)
        {
            var stream = await photo.OpenReadAsync();
            currentimageStream = stream;
            a.Source = ImageSource.FromStream(() => stream);
        }
    }
    private byte[] GetImageBytes()
    {
        byte[] imageBytes;
        using (var MemoryStream = new System.IO.MemoryStream())
        {
            currentimageStream.CopyTo(MemoryStream);
            imageBytes = MemoryStream.ToArray();
        }
        return imageBytes;
    }

    private void ScrollView_Loaded(object sender, EventArgs e)
    {
        List<Group> groups = conn.Table<Group>().ToList();
        List<Group> admingroups = (from g in groups where g.AdminId == adminid select g).ToList();
        foreach (var group in admingroups)
        {
            string result = group.GroupName;
            Create_truly(sender, e, result);
        }
    }

    private async void Pre_Clicked(object sender, EventArgs e)
    {
        adminid = Int32.Parse(this.Title.Substring(this.Title.IndexOf("(") + 1, this.Title.IndexOf(")") - 6));
        List<Group> Grooups = conn.Table<Group>().ToList();
        List<Group> Grroups = (from g in Grooups where g.AdminId == adminid select g).ToList();
        List<string> names = (from gr in Grroups select gr.GroupName).ToList();
        //string name = await DisplayActionSheet("Selecciona el usuario a añadir", "Cancelar", null, names.ToArray());
        //if(name == null) { return; }
        //Group group = (from g in Grroups where g.GroupName == name select g).FirstOrDefault();
        //string res = group.GroupName;
        foreach (string res in names)
        {
            //await Task.Delay(500);
            Create_truly(sender, e, res, true);
            //await Task.Delay(1000);
        }
    }
}