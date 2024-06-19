namespace Points;

public partial class DisplayCodePage : ContentPage
{	
	public DisplayCodePage(ImageSource a)
	{
		InitializeComponent();
		QrCodeImage.Source = a;
	}
}