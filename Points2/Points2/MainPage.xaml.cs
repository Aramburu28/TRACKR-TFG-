using Android.Widget;
using Microsoft.Maui.Animations;
using Microsoft.Maui.Platform;
using QRCoder;
using SQLite;
using System.Runtime.InteropServices;
using System.Windows.Input;


namespace Points
{
    public partial class AdminPage : ContentPage
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
        public AdminPage()
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
            string result = await DisplayPromptAsync("Name", "Insert a name for the group");
            while (result == "" || grupos.IndexOf(result) != -1)
            {
                await Task.Delay(1000);
                result = await DisplayPromptAsync("Name", "Insert a valid name");
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
                    Add2(new object(), e, arg);
                });
            Microsoft.Maui.Controls.Button bot2 = new Microsoft.Maui.Controls.Button
            {
                Text = "Add user",
                HorizontalOptions = LayoutOptions.End,
                VerticalOptions = LayoutOptions.Start,
              
                ClassId = counter.ToString(),
                Command = AddCommand,
                CommandParameter = result,

            };
            Command AdddCommand = new Command<string>(
               execute: (string arg) =>
               {
                   Add_User(new object(), e, arg);
               });
            Microsoft.Maui.Controls.Button bot4 = new Microsoft.Maui.Controls.Button
            {
                Text = "Add user(Showcase)",
                HorizontalOptions = LayoutOptions.End,
                VerticalOptions = LayoutOptions.Center,

                ClassId = counter.ToString(),
                Command = AdddCommand,
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
            grid.Add(bot4, 0, 0);
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
                VerticalOptions = LayoutOptions.Center,
               
            };
            bot.Clicked += async (sender, args) => Create_Clicked(bot, e);

            grid2.Add(bot, 0, 0);

            Base.Add(grid2, 0, count + 1);
            count++;
        }

        private async void Btn_Clicked(object sender, EventArgs e)
        {
            List<Group> Grooups = conn.Table<Group>().ToList();
            List<string> names = (from gr in Grooups select gr.GroupName).ToList();
            string result = await DisplayActionSheet("Which group would you like to manage?", "Cancel", null, names.ToArray());
            List<User> users = conn.Table<User>().ToList();
            List<string> usernms = (from use in users select use.Username).ToList();
            string user = await DisplayActionSheet("Which user should receive the points?", "Cancel", null, usernms.ToArray());
        apunto:

            string puntos = await DisplayPromptAsync("Amount", "Insert the point value to assign ", "Ok");
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
                await DisplayAlert("Warning", "Not a valid number", "Ok");
                goto apunto;
            }
            //this.puntos[index][index2] = Int32.Parse(puntos);
            await Navigation.PushAsync(new ScanPage(result, puntos, user,"add"));
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
            String username = await DisplayPromptAsync("Amount", "Insert the user username ", "Ok");
            User usuario = new User
            {
                Username = username,
            };
            conn.Insert(usuario);
            Group group = (from g in groups where g.GroupName == a select g).FirstOrDefault();
            UserGroup ug = new UserGroup
            {
                UserId = usuario.Id,
                GroupId = group.Id
            };
            
            //UserGroup userGroup = new UserGroup
            //{
            //    UserId = user.Id,
            //    GroupId = group.Id,
            //};
            conn.CreateTable<UserGroup>();
            conn.Insert(ug);
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
            string usuarios = "";
            Group gr = (from gro in grupos2 where gro.GroupName == a select gro).FirstOrDefault();
            List<UserGroup> ug = conn.Table<UserGroup>().ToList();
            ug = (from b in ug where b.GroupId == gr.Id select b).ToList();
            string print = "---------" + gr.GroupName + "----------\nUser ----------> Points\n";
            List<User> users = conn.Table<User>().ToList();
            foreach (UserGroup u in ug)
            {
                User us = (from use in users where use.Id == u.UserId select use).FirstOrDefault();
                usuarios += us.Id + " ---------> " +us.Username + " ---------> " + u.Points + "\n";
            }
            await DisplayAlert("a", usuarios, "b");
            //string groups = "";
            //foreach (Group g in grupos2)
            //{
            //    groups += g.Id + "-------" + g.GroupName + "---------" + g.AdminId + "------" + g.ImageSource + "\n";
            //}
            //await DisplayAlert("a", groups, "b");
        }
        private async void Use_points(object sender, EventArgs e, string a)
        {
            List<Group> grupos2 = conn.Table<Group>().ToList();
          
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
            List<Group> Grooups = conn.Table<Group>().ToList();
            List<Group> admingroups = (from g in Grooups where g.AdminId == adminid select g).ToList();
            List<string> names = (from gr in admingroups select gr.GroupName).ToList();
            string result = await DisplayActionSheet("Which group would you like to manage?", "Cancel", null, names.ToArray());
            if(result == null)
            {
                return;
            }
            Group groupa = (from xx in Grooups where xx.GroupName == result select xx).FirstOrDefault();
            List<UserGroup> groups = conn.Table<UserGroup>().ToList();
            List<UserGroup> userGroups = (from gra in groups where gra.GroupId == groupa.Id select gra).ToList();
           
            List<User> users = conn.Table<User>().ToList();
            List<User> usersr = new List<User>();
            foreach(UserGroup userGroup in userGroups)
            {
                User user1 = (from mo in users where mo.Id == userGroup.UserId select mo).FirstOrDefault();
                usersr.Add(user1);
            }
            List<string> usernms = (from use in usersr select use.Username).ToList();
            string user = await DisplayActionSheet("Which user should receive the points?", "Cancel", null, usernms.ToArray());
        apunto:

            string puntos = await DisplayPromptAsync("Amount", "Insert the point value to assign ", "Ok");
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
                await DisplayAlert("Warning", "Not a valid number", "Ok");
                goto apunto;
            }
            User user2 = (from asd in users where asd.Username == user select asd).FirstOrDefault();
            var sql = "UPDATE UserGroup SET points = points +  "+ puntos +" WHERE userid =" + user2.Id + " AND groupid =" + groupa.Id;
            SQLiteCommand a = new SQLiteCommand(conn);
            a.CommandText = sql;
            a.ExecuteNonQuery();

        }
            private async void Change_Image(object sender, EventArgs e, Image a)
        {
            string result = await DisplayPromptAsync("Select image", "Would you like to add an image", "Yes", "No");
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
            foreach(string res in names)
            {
            //await Task.Delay(500);
            Create_truly(sender, e, res, true);
                //await Task.Delay(1000);
            }
        }
        private async void Add2(object sender, EventArgs e, string a)
        {
            Navigation.PushAsync(new ScanPage(a, "", "", ""));
        }

        private async void Red_Clicked(object sender, EventArgs e)
        {
            List<Group> groups = conn.Table<Group>().ToList();
            List<string> admingroups = (from g in groups where g.AdminId == adminid select g.GroupName).ToList();
            string gs = "";
            string gname = await DisplayActionSheet("Which group would you like to manage?", "Cancel", null, admingroups.ToArray());
            Group ga = (from ba in groups where ba.GroupName == gname select ba).FirstOrDefault();
            List<int> sa = (from b in conn.Table<UserGroup>().ToList() where b.GroupId == ga.Id select b.UserId).ToList();
            List<string> users = (from u in conn.Table<User>().ToList() where sa.Contains(u.Id) select u.Username).ToList();
            string uname = await DisplayActionSheet("Choose a user", "Cancel", null, users.ToArray());
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
            User user =  conn.Table<User>().ToList().Where(u => u.Username == uname).FirstOrDefault();
            UserGroup ug = conn.Table<UserGroup>().ToList().Where(us => us.UserId == user.Id).FirstOrDefault();
            await DisplayAlert("b", user.Username + "---"+ user.Id + "----" + ug.Points, "a");
            var command = new SQLiteCommand(conn)
            {
                CommandText = @"UPDATE UserGroup SET Points = 10 WHERE Id =" + ug.UserId
            };
            
            var como = conn.CreateCommand("b");
            var newPoints =  command.ExecuteNonQuery();
            await DisplayAlert("b", user.Username + "---" + user.Id + "----" + ug.Points, "a");
        }
    }
}

