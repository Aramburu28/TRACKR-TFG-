


using System.Net.Http.Headers;
using System.Net;

using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Points;

public partial class ApiPage : ContentPage
{
	HttpClient client;
	string url = "https://cataas.com/cat";
    Uri uri = new Uri("https://cataas.com/cat");
    Uri uri2 = new Uri("https://dog.ceo/api/breeds/image/random");


    public ApiPage()
	{
		client = new HttpClient();
		InitializeComponent();
		getimage();
	}
	public class Dphoto
	{
		public string message;
		public string status;
	}
	public async void getimage()
	{
		
       
		Cat.Source = new UriImageSource
		{
			Uri = uri,
			CachingEnabled = false
		};
        //Cat2.Source = new UriImageSource
        //{
        //    Uri = uri2,
        //    CachingEnabled = false
        //};
        var resp = await client.GetAsync(uri2).Result.Content.ReadAsStringAsync();
        Dphoto content = JsonConvert.DeserializeObject<Dphoto>(resp);
        Cat2.Source = new UriImageSource
        {
            Uri = new Uri(content.message),
            CachingEnabled = false
        };





    }

    private void press_Clicked(object sender, EventArgs e)
    {
		getimage();
    }
	private async void fsonsdsa(object obj)
	{
		var resp = await  client.GetAsync(uri2).Result.Content.ReadAsStringAsync();
		Dphoto content = JsonConvert.DeserializeObject<Dphoto>(resp);
        Cat2.Source = new UriImageSource
        {
            Uri = new Uri(content.message),
            CachingEnabled = false
        };

    }
}