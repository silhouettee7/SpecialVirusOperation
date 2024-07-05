namespace VirusesGame
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();
        }
        protected override Window CreateWindow(IActivationState activationState)
        {
            var window = base.CreateWindow(activationState);

            window.Height = 1920;
            window.Width = 1080;
            window.MinimumWidth = 1245;
            window.MinimumHeight = 700;


            return window;
        }
    }
}
