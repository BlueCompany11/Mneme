﻿using Mneme.Notes;
using Mneme.PrismModule.Notes.Views;
using Prism.Ioc;
using Prism.Modularity;

namespace Mneme.PrismModule.Notes;

public class NotesModule : IModule
{
	public void OnInitialized(IContainerProvider containerProvider)
	{

	}

	public void RegisterTypes(IContainerRegistry containerRegistry)
	{
		containerRegistry.RegisterForNavigation<NotesView>();
		containerRegistry.RegisterForNavigation<EmptyNotePreviewView>();
		containerRegistry.RegisterForNavigation<NewMnemeNoteView>();
		_ = containerRegistry
			.Register<IMnemeNotesProxy, MnemeNotesProxy>()
			.Register<INotesUtility, NotesUtility>();
	}
}
