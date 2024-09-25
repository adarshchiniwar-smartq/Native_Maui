using static System.Net.Mime.MediaTypeNames;

namespace Native
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage()
        {
            InitializeComponent();
        }

        private void OnCounterClicked(object sender, EventArgs e)
        {
            count++;

            if (count == 1)
                CounterBtn.Text = $"Clicked {count} time";
            else
                CounterBtn.Text = $"Clicked {count} times";

            SemanticScreenReader.Announce(CounterBtn.Text);
            try
            {
                // Ensure the code only runs on supported platforms
#if __IOS__ 
                //NewBindingMaciOS.DotnetNewBinding dotnetNewBinding = new NewBindingMaciOS.DotnetNewBinding();
                //string c = NewBindingMaciOS.DotnetNewBinding.GetString("Hello");
               IosNative.Epos2Printer epos2Printer = new IosNative.Epos2Printer();
           
#endif
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }

}
