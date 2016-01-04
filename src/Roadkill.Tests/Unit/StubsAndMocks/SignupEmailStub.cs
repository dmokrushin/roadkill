﻿using Roadkill.Core.Configuration;
using Roadkill.Core.Database;
using Roadkill.Core.Database.Repositories;
using Roadkill.Core.Email;
using Roadkill.Core.Mvc.ViewModels;

namespace Roadkill.Tests.Unit.StubsAndMocks
{
	public class SignupEmailStub : SignupEmail
	{
		public bool IsSent { get; set; }
		public UserViewModel ViewModel { get; set; }

		public SignupEmailStub(NonConfigurableSettings settings, ISettingsRepository settingsRepository, IEmailClient emailClient)
			: base(settings, settingsRepository, emailClient)
		{
		}
		
		public override void Send(UserViewModel model)
		{
			ReplaceTokens(model, "{EMAIL}");
			IsSent = true;
			ViewModel = model;
		}
	}
}
