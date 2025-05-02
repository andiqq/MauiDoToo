using DoToo.ViewModels;

namespace DoToo;

public partial class App
{
	private readonly Views.MainView _view;

	public App(Views.MainView view)
	{
		InitializeComponent();
		_view = view;
	}
	
	protected override Window CreateWindow(IActivationState? activationState) => new(new NavigationPage(_view));
}

