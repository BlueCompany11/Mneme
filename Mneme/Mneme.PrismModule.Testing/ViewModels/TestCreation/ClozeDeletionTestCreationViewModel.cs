using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using MaterialDesignThemes.Wpf;
using Mneme.Model.Interfaces;
using Mneme.Model.Notes;
using Mneme.Model.TestCreation;
using Mneme.Testing.Contracts;
using Mneme.Testing.Database;
using Mneme.Testing.TestCreation;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;

namespace Mneme.PrismModule.Testing.ViewModels.TestCreation
{

	public class ClozeDeletionTestCreationViewModel : BindableBase, INavigationAware
	{
		private string text;
		public string Text
		{
			get => text;
			set
			{
				if (!freezeText)
				{
					_ = SetProperty(ref text, value);
					AddText?.Invoke();
				}
			}
		}
		private ObservableCollection<string> clozeDeletions;
		public ObservableCollection<string> ClozeDeletions
		{
			get => clozeDeletions;
			set => SetProperty(ref clozeDeletions, value);
		}

		private string hint;

		public string Hint
		{
			get => hint;
			set => SetProperty(ref hint, value);
		}
		private List<string> importanceOptions;
		public List<string> ImportanceOptions
		{
			get => importanceOptions;
			set => SetProperty(ref importanceOptions, value);
		}
		private string selectedImportanceOption;
		public string SelectedImportanceOption
		{
			get => selectedImportanceOption;
			set => SetProperty(ref selectedImportanceOption, value);
		}

		private List<ClozeDeletionDataStructure> ClozeDeletionDataStructures { get; set; }
		private Note Note { get; set; }
		public DelegateCommand CreateTestCommand { get; set; }
		private readonly INoteTestVisitor clozeDeletionNoteTestVisitor;
		private readonly TestImportanceMapper testImportanceMapper;
		private readonly ISnackbarMessageQueue snackbarMessageQueue;
		private readonly TestingRepository repository;
		private bool freezeText;

		public ClozeDeletionTestCreationViewModel(ClozeDeletionNoteTestVisitor clozeDeletionNoteTestVisitor, TestImportanceMapper testImportanceMapper, ISnackbarMessageQueue snackbarMessageQueue, TestingRepository repository)
		{
			ClozeDeletions = [];
			this.clozeDeletionNoteTestVisitor = clozeDeletionNoteTestVisitor;
			this.testImportanceMapper = testImportanceMapper;
			this.snackbarMessageQueue = snackbarMessageQueue;
			this.repository = repository;
			ImportanceOptions = testImportanceMapper.ImportanceOptions;
			SelectedImportanceOption = ImportanceOptions[0];
			CreateTestCommand = new DelegateCommand(CreateTest);
			ClozeDeletionDataStructures = [];
		}

		private void CreateTest()
		{
			var validation = ClozeDeletions.Count > 0;
			if(!validation)
			{
				snackbarMessageQueue.Enqueue("Add cloze deletion first.");
				return;
			}
			int importance = testImportanceMapper.Map(SelectedImportanceOption);
			var testClozeDeletion = new TestClozeDeletion
			{
				Text = Text,
				Importance = importance,
				NoteId = Note.IntegrationId,
				ClozeDeletionDataStructures = ClozeDeletionDataStructures,
				Created = DateTime.Now
			};
			repository.CreateTest(testClozeDeletion);
			snackbarMessageQueue.Enqueue("Test created");
		}
		public void MarkClozeDeletion(int start, int end)
		{
			var validation = start < end && start >= 0 && end <= Text.Length;
			if (!validation)
			{
				snackbarMessageQueue.Enqueue("Marked text is invalid.");
				return;
			}
			freezeText = true;
			string text = Text[start..end];
			ClozeDeletionDataStructures.Add(new ClozeDeletionDataStructure { Start = start, End = end });
			ClozeDeletions.Add(text);
		}
		public void OnNavigatedTo(NavigationContext navigationContext)
		{
			ClearTextFromUi?.Invoke();
			Note = navigationContext.Parameters.GetValue<Note>("note");
			var data = Note.Accept(clozeDeletionNoteTestVisitor) as ClozeDeletionNoteData;
			freezeText = false;
			Text = data.Text;
			ClozeDeletions.Clear();
		}

		public bool IsNavigationTarget(NavigationContext navigationContext)
		{
			return true;
		}

		public void OnNavigatedFrom(NavigationContext navigationContext)
		{

		}
		public event Action ClearTextFromUi;
		public event Action AddText;
	}
}
