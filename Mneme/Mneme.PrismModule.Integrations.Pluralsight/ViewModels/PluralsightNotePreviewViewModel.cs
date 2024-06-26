using Mneme.Integrations.Pluralsight.Contract;
using Mneme.Model.Notes;
using Mneme.PrismModule.Integrations.Base;
using Mneme.Views.Base;
using Prism.Regions;

namespace Mneme.PrismModule.Integrations.Pluralsight.ViewModels
{
	public class PluralsightNotePreviewViewModel : AbstractNotePreviewViewModel
	{
		private PluralsightNotePreview notePreview;
		private string title;
		public string Title
		{
			get => title;
			set => SetProperty(ref title, value);
		}
		private string sourceType;
		public string SourceType
		{
			get => sourceType;
			set => SetProperty(ref sourceType, value);
		}

		private string module;
		public string Module
		{
			get => module;
			set => SetProperty(ref module, value);
		}
		private string timeInClip;
		public string TimeInClip
		{
			get => timeInClip;
			set => SetProperty(ref timeInClip, value);
		}
		private string clip;
		public string Clip
		{
			get => clip;
			set => SetProperty(ref clip, value);
		}
		private string noteText;
		public string NoteText
		{
			get => noteText;
			set => SetProperty(ref noteText, value);
		}
		private string link;
		public string Link
		{
			get => link;
			set => SetProperty(ref link, value);
		}
		private string type;
		public string Type
		{
			get => type;
			set => SetProperty(ref type, value);
		}

		protected override Note Preelaboration { get; set; }

		public PluralsightNotePreviewViewModel(IRegionManager regionManager) : base(regionManager) {}

		private void PluralsightNotePreviewUpdate()
		{
			Title = notePreview.Title;
			SourceType = notePreview.Type;
			Module = notePreview.Module;
			TimeInClip = notePreview.TimeInClip;
			Clip = notePreview.Clip;
			NoteText = notePreview.Note;
			Link = notePreview.Link;
			Type = notePreview.Type;
		}

		protected override void LoadNote()
		{
			notePreview = PluralsightNotePreview.CreateFromNote((PluralsightNote)Preelaboration);
			PluralsightNotePreviewUpdate();
		}
	}
}
