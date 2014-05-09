using System;
using System.Collections.Generic;
using System.Json;
using System.Linq;
using System.Threading.Tasks;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.Dialog;

namespace Xamarin.Auth.Sample.iOS
{
	[Register ("AppDelegate")]
	public partial class AppDelegate : UIApplicationDelegate
	{
		void LoginToTraditionalBasic (bool allowCancel)
		{
			var formAuth = new MyFormAuthenticator (
				createAccountLink: new Uri ("https://www.mydomain.com/createuser")
			);
			formAuth.AllowCancel = allowCancel;



			formAuth.Completed += (s, e) =>
			{
				// We presented the UI, so it's up to us to dismiss it.
				dialog.DismissViewController (true, null);

				if (!e.IsAuthenticated) {
					traditionalBasicLoginStatus.Caption = "Not authorized";
					dialog.ReloadData();
					return;
				}
			};



			UIViewController vc = formAuth.GetUI ();
			dialog.PresentViewController (vc, true, null);
		}

		public override bool FinishedLaunching (UIApplication app, NSDictionary options)
		{
			traditionalBasicLogin = new Section ("Form Authenticator");
			traditionalBasicLogin.Add (new StyledStringElement ("Log in", () => LoginToTraditionalBasic (true)));			
			traditionalBasicLogin.Add (new StyledStringElement ("Log in (no cancel)", () => LoginToTraditionalBasic (false)));
			traditionalBasicLogin.Add (traditionalBasicLoginStatus = new StringElement (String.Empty));

			dialog = new DialogViewController (new RootElement ("Xamarin.Auth Sample") {
				traditionalBasicLogin,
			});

			window = new UIWindow (UIScreen.MainScreen.Bounds);
			window.RootViewController = new UINavigationController (dialog);
			window.MakeKeyAndVisible ();
			
			return true;
		}

		private readonly TaskScheduler uiScheduler = TaskScheduler.FromCurrentSynchronizationContext();

		UIWindow window;
		DialogViewController dialog;

		Section traditionalBasicLogin;
		StringElement traditionalBasicLoginStatus;

		// This is the main entry point of the application.
		static void Main (string[] args)
		{
			UIApplication.Main (args, null, "AppDelegate");
		}
	}
}

