using System;
using System.Threading;
using System.Threading.Tasks;
using System.Net.Http;

namespace Xamarin.Auth.Sample.iOS
{
	public class MyFormAuthenticator : FormAuthenticator
	{
		public MyFormAuthenticator (Uri createAccountLink) : base(createAccountLink)
		{
			Fields.Add (new FormAuthenticatorField ("email", "username", FormAuthenticatorFieldType.Email, "email", ""));
			Fields.Add (new FormAuthenticatorField ("pass", "password", FormAuthenticatorFieldType.Password, "password", ""));
		}

		public override async Task<Account> SignInAsync (CancellationToken cancellationToken) {

			var httpClient = new HttpClient ();
			Account account = null;

			string username = GetFieldValue ("email");
			string password = GetFieldValue ("pass");


			//send username and password to a webservice on your server to login
			Task<string> loginTask = httpClient.GetStringAsync ("http://www.mydomain.com/");

			string result = await loginTask;

			if (result == "success") {
				account = new Account ();
			}

			//return null to signify a non-successful login

			return account;
		}
	}
}

