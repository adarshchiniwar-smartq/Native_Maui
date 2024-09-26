using PrinterSDK;
//#if IOS
//using Pring
//#endif

namespace Native
{
    public partial class MainPage : ContentPage
    {
        int count = 0;
        Epos2Printer Epos2Printer;

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
                Task.Run(() => PrintData()); 
#endif

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        //PrinterSDK.Epos2DeviceInfo deviceInfo;
        //Epos2FilterOption filterOption;
        private void PrintData()
        {

            try
            {
                Epos2Printer = new Epos2Printer(5, 0);
                //deviceInfo = new PrinterSDK.Epos2DeviceInfo();
                //filterOption = new Epos2FilterOption();
                //IosNative.Epos2Printer epos2Printer = new IosNative.Epos2Printer();
            }
            catch (Exception ex)
            {
            }
            //try
            //{
            //    filterOption.DeviceType = 1;
            //    int result = Epos2Discovery.Start(filterOption, new Epos2DiscoveryDelegateImplementation());
            //    if (result == 0)
            //    {

            //    }
            //}
            //catch (Exception ex) { 
            //}
           
        }
    }

    // Implement the Epos2DiscoveryDelegate interface
    //public class Epos2DiscoveryDelegateImplementation : Epos2DiscoveryDelegate
    //{
    //    public override void OnDiscovery(Epos2DeviceInfo deviceInfo)
    //    {
    //        // Handle discovery event
    //    }
    //}
}
