namespace DoToo;

public partial class App
{
	public App(Views.MainView view)
	{
		InitializeComponent();

		MainPage = new NavigationPage(view);
	}
}
