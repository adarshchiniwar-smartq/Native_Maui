using PrinterSDK;

//#if IOS
//using Pring
//#endif

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
                //#if __IOS__
                //                Task.Run(() => PrintData()); 
                //#endif
                PrintData();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        Epos2DeviceInfo deviceInfo;
        Epos2FilterOption filterOption;
        private void PrintData()
        {

            try
            {
                Epos2Printer Epos2Printer = new Epos2Printer(10, 0);
                deviceInfo = new Epos2DeviceInfo();
                filterOption = new Epos2FilterOption();
                filterOption.DeviceType = 1;
                Console.WriteLine("Test Line");
                //int rea = Epos2Discovery.Start(filterOption, new Epos2DiscoveryDelegateImplementation());
                Epos2DiscoveryDelegateImplementation epos2DiscoveryDelegateImplementation = new Epos2DiscoveryDelegateImplementation();
                int a = Epos2Discovery.Start(filterOption, epos2DiscoveryDelegateImplementation);

            }
            catch (Exception ex)
            {
            }




        }
    }

    //Implement the Epos2DiscoveryDelegate interface
    public class Epos2DiscoveryDelegateImplementation : Epos2DiscoveryDelegate
    {

        public override void OnDiscovery(Epos2DeviceInfo deviceInfo)
        {
            base.OnDiscovery(deviceInfo);
        }

    }

}
