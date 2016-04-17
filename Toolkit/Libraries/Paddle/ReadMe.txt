Include the PaddleSDK.dll file into your project. If using Visual Studio this can be done by choosing "Add Reference..." and browsing to the DLL.

To get started you will need to create an instance of the Paddle client. We recommend using the CreateSharedInstance method to create a single instance that can be statically referenced throughout your application via Paddle.SharedInstance

	public static Paddle CreateSharedInstance(string apiKey, long vendorId, string productId)

	apiKey - Your API key from the Paddle dashboard.
	vendorId - Your vendor Id from the Paddle dashboard.
	productId - The Id of the product you are integrating for.

Once you have an instance you can call StartLicensing(). This will check whether the user has activate the product or not and show them a time trial reminder if need be. It is important to check the result from StartLicensing() as this will indicate whether or not your application needs to quit. We do not force quit the application as most applications will have their own logic here to deal with exiting.

Example integration:

using PaddleSDK;

namespace PaddleSDKExample
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();

            var paddle = Paddle.CreateSharedInstance("12345678901234567890123456789012", 1234, "123456");
            if(!paddle.StartLicensing())
            {
                // StartLicensing will return false if the app needs to exit (e.g. trial has expired and 
                //  user doesn't want to pay). You may want to do some cleanup here or save the flag and
                //  exit in a more appropriate manner for your application.

                Application.Exit();
            }
        }
    }
}

If a user is offline the Paddle SDK will use the information it saved on the last successful session. If a user is running the app for the first time and is also offline the SDK will skip validation. Alternatively you can manually set information to be used using the SetFirstRunProductInformation(...) method. For example:

            var firstRunInformation = new PaddleProductInformation()
            {
                DeveloperName = "Vendor Name",
                ProductName = "Product Name",
                Currency = "USD",
                Price = 12.34M, // M for Decimal literal
                TrialLength = 10
            };
            paddle.SetFirstRunProductInformation(firstRunInformation);
            if(!paddle.StartLicensing())
            {
                // ....
	  