using System.Text;
using PrinterSDK;
using UIKit;

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
            int result = Epos2Log.SetLogSettings(0, 1, "", 0, 1, 0);
            printer = new Epos2Printer(10, 0); ;
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
                //PrintData();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        Epos2DeviceInfo deviceInfo;
        Epos2FilterOption filterOption;

        Epos2Printer printer;
        private void PrintData()
        {

            try
            {
                //GetDevices.Epos2DeviceInfos.Clear();

                deviceInfo = new Epos2DeviceInfo();
                filterOption = new Epos2FilterOption();
                filterOption.DeviceType = 1;
                Console.WriteLine("Test Line");
                //int rea = Epos2Discovery.Start(filterOption, new Epos2DiscoveryDelegateImplementation());
                Epos2DiscoveryDelegateImplementation epos2DiscoveryDelegateImplementation = new Epos2DiscoveryDelegateImplementation();
                int a = Epos2Discovery.Start(filterOption, epos2DiscoveryDelegateImplementation);

                //result = Epos2Printer.Disconnect();



            }
            catch (Exception ex)
            {
                printer = null;
                printer = new Epos2Printer(10, 0);

            }

        }

        public class Epos2DiscoveryDelegateImplementation : Epos2DiscoveryDelegate
        {

            public Epos2DiscoveryDelegateImplementation()
            {
                GetDevices.Epos2DeviceInfos.Clear();
            }

            public override void OnDiscovery(Epos2DeviceInfo deviceInfo)
            {
                GetDevices.Epos2DeviceInfos.Add(deviceInfo);
                //base.OnDiscovery(deviceInfo);
                Device.BeginInvokeOnMainThread(() =>
                {
                    // UI updates or any operations that require the main thread
                });

            }

        }

        public class PrinterReceiverEvent : Epos2PtrReceiveDelegate
        {
            public override void Code(Epos2Printer printerObj, int code, Epos2PrinterStatusInfo status, string printJobId)
            {
                //base.Code(printerObj, code, status, printJobId);
            }
        }

        void Button_Clicked(System.Object sender, System.EventArgs e)
        {
            Epos2Discovery.Stop();
            int result = 0;
            printer = null;
            printer = new Epos2Printer(10, 0);
            PrinterReceiverEvent printerReceiverEvent = new PrinterReceiverEvent();
            printer.SetReceiveEventDelegate(printerReceiverEvent);
            //result = Epos2Printer.AddTextAlign(1);
            Task createPrint = Task.Run(() =>
            {
                CreateReceiptData();
            });
            createPrint.Wait();
            string target = GetDevices.Epos2DeviceInfos[0].Target;
            Task connectTask = Task.Run(() =>
            {
                result = printer.Connect(target, -2);
            });
            connectTask.Wait();
            Task.Run(() =>
            {
                result = printer.SendData(-2);
            });
         
            //result = Epos2Printer.Disconnect();
            //Epos2Printer = null;
        }

        public bool CreateReceiptData()
        {
            int barcodeWidth = 2;
            int barcodeHeight = 100;

            int result;
            var textData = new StringBuilder();
            //var logoData = LoadImage("store.png"); // Assuming LoadImage is a helper method to get image.

            //if (logoData == null)
            //{
            //    return false;
            //}

            // Set text alignment to center.
            result = printer.AddTextAlign((int)Epos2Align.Center);
            if (result != 0)
            {
                LogError(result, "AddTextAlign");
                return false;
            }

            /* Uncomment this if you have a method to add images in C#
            result = printer.AddImage(logoData, 0, 0,
                (int)logoData.Width,
                (int)logoData.Height,
                Epos2Printer.COLOR_1,
                Epos2Printer.MODE_MONO,
                Epos2Printer.HALFTONE_DITHER,
                Epos2Printer.PARAM_DEFAULT,
                Epos2Printer.COMPRESS_AUTO);

            if (result != 0)
            {
                printer.ClearCommandBuffer();
                LogError(result, "AddImage");
                return false;
            }*/

            // Section 1: Store information.
            result = printer.AddFeedLine(1);
            if (result != 0)
            {
                printer.ClearCommandBuffer();
                LogError(result, "AddFeedLine");
                return false;
            }

            textData.Append("THE STORE 123 (555) 555 – 5555\n");
            textData.Append("STORE DIRECTOR – John Smith\n\n");
            textData.Append("7/01/07 16:58 6153 05 0191 134\n");
            textData.Append("ST# 21 OP# 001 TE# 01 TR# 747\n");
            textData.Append("------------------------------\n");

            result = printer.AddText(textData.ToString());
            if (result != 0)
            {
                printer.ClearCommandBuffer();
                LogError(result, "AddText");
                return false;
            }
            textData.Clear();

            // Section 2: Purchased items.
            textData.Append("400 OHEIDA 3PK SPRINGF  9.99 R\n");
            textData.Append("410 3 CUP BLK TEAPOT    9.99 R\n");
            textData.Append("445 EMERIL GRIDDLE/PAN 17.99 R\n");
            textData.Append("438 CANDYMAKER ASSORT   4.99 R\n");
            textData.Append("474 TRIPOD              8.99 R\n");
            textData.Append("433 BLK LOGO PRNTED ZO  7.99 R\n");
            textData.Append("458 AQUA MICROTERRY SC  6.99 R\n");
            textData.Append("493 30L BLK FF DRESS   16.99 R\n");
            textData.Append("407 LEVITATING DESKTOP  7.99 R\n");
            textData.Append("441 **Blue Overprint P  2.99 R\n");
            textData.Append("476 REPOSE 4PCPM CHOC   5.49 R\n");
            textData.Append("461 WESTGATE BLACK 25  59.99 R\n");
            textData.Append("------------------------------\n");

            result = printer.AddText(textData.ToString());
            if (result != 0)
            {
                printer.ClearCommandBuffer();
                LogError(result, "AddText");
                return false;
            }
            textData.Clear();

            // Section 3: Payment information.
            textData.Append("SUBTOTAL                160.38\n");
            textData.Append("TAX                      14.43\n");
            result = printer.AddText(textData.ToString());
            if (result != 0)
            {
                printer.ClearCommandBuffer();
                LogError(result, "AddText");
                return false;
            }
            textData.Clear();

            result = printer.AddTextSize(2, 2);
            if (result != 0)
            {
                printer.ClearCommandBuffer();
                LogError(result, "AddTextSize");
                return false;
            }

            result = printer.AddText("TOTAL    174.81\n");
            if (result != 0)
            {
                printer.ClearCommandBuffer();
                LogError(result, "AddText");
                return false;
            }

            result = printer.AddTextSize(1, 1);
            if (result != 0)
            {
                printer.ClearCommandBuffer();
                LogError(result, "AddTextSize");
                return false;
            }

            result = printer.AddFeedLine(1);
            if (result != 0)
            {
                printer.ClearCommandBuffer();
                LogError(result, "AddFeedLine");
                return false;
            }

            textData.Append("CASH                    200.00\n");
            textData.Append("CHANGE                   25.19\n");
            textData.Append("------------------------------\n");
            result = printer.AddText(textData.ToString());
            if (result != 0)
            {
                printer.ClearCommandBuffer();
                LogError(result, "AddText");
                return false;
            }
            textData.Clear();

            // Section 4: Advertisement.
            textData.Append("Purchased item total number\n");
            textData.Append("Sign Up and Save !\n");
            textData.Append("With Preferred Saving Card\n");
            result = printer.AddText(textData.ToString());
            if (result != 0)
            {
                printer.ClearCommandBuffer();
                LogError(result, "AddText");
                return false;
            }
            textData.Clear();

            result = printer.AddFeedLine(2);
            if (result != 0)
            {
                printer.ClearCommandBuffer();
                LogError(result, "AddFeedLine");
                return false;
            }

            /* Uncomment this if you have barcode support in C#
            result = printer.AddBarcode("01209457",
                Epos2Printer.BARCODE_CODE39,
                Epos2Printer.HRI_BELOW,
                Epos2Printer.FONT_A,
                barcodeWidth,
                barcodeHeight);
            if (result != 0)
            {
                printer.ClearCommandBuffer();
                LogError(result, "AddBarcode");
                return false;
            }*/

            result = printer.AddCut(0);
            if (result != 0)
            {
                printer.ClearCommandBuffer();
                LogError(result, "AddCut");
                return false;
            }

            return true;
        }

        private void LogError(int errorCode, string method)
        {
            // Log the error to console or a file without showing popup.
            Console.WriteLine($"Error in method {method}: {errorCode}");
        }

        private UIImage LoadImage(string imageName)
        {
            // Implement this method to load the image for your application.
            return null; // Placeholder
        }
    }

    //Implement the Epos2DiscoveryDelegate interface


    public class GetDevices
    {
        public static List<Epos2DeviceInfo> Epos2DeviceInfos { get; set; } = new List<Epos2DeviceInfo>();
    }
}
