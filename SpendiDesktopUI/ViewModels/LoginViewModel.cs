using Caliburn.Micro;
using SpendiDesktopUI.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpendiDesktopUI.Library.Helpers;
using SpendiDesktopUI.EventModels;

namespace SpendiDesktopUI.ViewModels
{
    public class LoginViewModel : Screen
    {
		private string _userName = "vince49341@gmail.com";
		private string _password = "Test.123";
		private IAPIHelper _apiHelper;
		private IEventAggregator _events;

		public LoginViewModel(IAPIHelper apiHelper, IEventAggregator events)
		{
			_apiHelper = apiHelper;
			_events = events;
		}
		public string UserName
		{
			get { return _userName; }
			set
			{
				_userName = value;
				NotifyOfPropertyChange(() => UserName);
			}
		}

		public string Password
		{
			get { return _password; }
			set 
			{
				_password = value;
				NotifyOfPropertyChange(() => Password);
				NotifyOfPropertyChange(() => CanLogIn);
			}
		}

		public bool CanLogIn
		{
			get
			{
				return (!String.IsNullOrEmpty(UserName) && !String.IsNullOrEmpty(Password));
			}
		}

		public async Task LogIn()
		{
			try
			{
				var result = await _apiHelper.Authenticate(UserName, Password);

				await _apiHelper.GetLoggedInUserInfo(result.Access_Token);

				_events.PublishOnUIThread(new LogInEvent());
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
		}
	}
}

