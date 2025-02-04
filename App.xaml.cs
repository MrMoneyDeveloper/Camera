namespace Camera
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            AppDomain.CurrentDomain.UnhandledException += (sender, e) =>
            {
                var ex = e.ExceptionObject as Exception;
                // Log or display the exception details
            };
            MainPage = new AppShell();

        }
    }
}
