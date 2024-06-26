﻿using Mneme.DataAccess;
using Mneme.Integrations.Mneme.Contract;
using Mneme.Integrations.Mneme;
using System.ComponentModel;
using Mneme.PrismModule.Integrations.Mneme.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using Mneme.Integrations.Contracts;

namespace Mneme.PrismModule.Integrations.Mneme
{
	public class MnemeModule : IModule
	{
		public void OnInitialized(IContainerProvider containerProvider)
		{

		}

		public void RegisterTypes(IContainerRegistry containerRegistry)
		{
			containerRegistry.RegisterForNavigation<MnemeNotePreviewView>();

			containerRegistry.Register<BaseSourcesProvider<MnemeSource>, MnemeSourcesProvider>();

			containerRegistry.Register<IIntegrationFacade<MnemeSource, MnemeNote>, MnemeIntegrationFacade>();
			containerRegistry.Register<IDatabase, MnemeIntegrationFacade>();
		}
	}
}