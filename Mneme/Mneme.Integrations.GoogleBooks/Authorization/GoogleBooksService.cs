using Google;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Responses;
using Google.Apis.Books.v1;
using Google.Apis.Books.v1.Data;
using Google.Apis.Services;
using Mneme.Integrations.GoogleBooks.Contract;


namespace Mneme.Integrations.GoogleBooks.Authorization
{
	public class GoogleBooksService : IDisposable
	{
		protected UserCredential? credential;
		protected readonly GoogleCredentialsProvider googleCredentialsProvider;
		protected string AppName => "Mneme";

		private readonly string fileExtensionBlackList = ".pdf";
		private readonly string mainShelfName = "My Google eBooks";
		private readonly List<GoogleBooksBook> books;
		private List<GoogleBooksAnnotation> annotations;
		protected string[] scope => [BooksService.Scope.Books];

		private BooksService? service;

		public GoogleBooksService(GoogleCredentialsProvider googleCredentialsProvider)
		{
			this.googleCredentialsProvider = googleCredentialsProvider;
			books = new List<GoogleBooksBook>();
			annotations = new List<GoogleBooksAnnotation>();
		}

		protected void CreateService()
		{
			service = new BooksService(new BaseClientService.Initializer()
			{
				HttpClientInitializer = credential,
				ApplicationName = AppName,
			});
		}

		public void Connect()
		{
			LoadCredentials();
			if (service == null)
				CreateService();
		}

		private void LoadCredentials()
		{
			if (credential != null)
				return;
			using (Stream stream = googleCredentialsProvider.GetFileStream().Invoke())
			{
				credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
						GoogleClientSecrets.FromStream(stream).Secrets,
						scope,
						Environment.UserName,
						CancellationToken.None).Result;
			}
		}

		private List<GoogleBooksAnnotation> ConvertAnnotations(Annotations annotations, Volume volume)
		{
			var ret = new List<GoogleBooksAnnotation>();
			if (annotations != null && annotations.Items != null)
			{
				foreach (var annotation in annotations.Items)
				{
					ret.Add(new GoogleBooksAnnotation(annotation, volume));
				}
			}
			return ret;
		}

		private async Task<List<Volume>> LoadVolumesAsync(CancellationToken ct)
		{
			List<Volumes> volumes = new List<Volumes>();
			try
			{
				var bookshelves = await service.Mylibrary.Bookshelves.List().ExecuteAsync(ct).ConfigureAwait(false)
					;
				volumes = await GetVolumesAsync(bookshelves, ct).ConfigureAwait(false);
				return FilterVolumes(volumes);
			}
			catch (GoogleApiException)
			{
				return FilterVolumes(volumes);
			}
			catch (TokenResponseException ex) when (ex.Error.Error == "invalid_grant")
			{
				return FilterVolumes(volumes);
			}
		}

		private List<Volume> FilterVolumes(List<Volumes> volumes)
		{
			var ret = new List<Volume>();
			foreach (var vol in volumes)
			{
				ret.AddRange(vol.Items.Where(x => !x.VolumeInfo.Title.EndsWith(fileExtensionBlackList)));
			}
			return ret;
		}

		private async Task LoadAnnotationsAsync(CancellationToken ct)
		{
			var volumes = await LoadVolumesAsync(ct).ConfigureAwait(false);
			await UpdateAnnotationsAsync(volumes, ct).ConfigureAwait(false);
		}

		public async Task<List<GoogleBooksNote>> LoadNotes(CancellationToken ct)
		{
			await LoadAnnotationsAsync(ct).ConfigureAwait(false);
			var ret = new List<GoogleBooksNote>();
			foreach (var a in annotations)
				ret.Add(Convert(a));
			return ret;
		}

		private async Task UpdateAnnotationsAsync(List<Volume> volumes, CancellationToken ct)
		{
			var ret = new List<GoogleBooksAnnotation>();
			var request = new MylibraryResource.AnnotationsResource.ListRequest(service)
			{
				MaxResults = 40,
				ContentVersion = "gbImageRange"
			};
			string pageToken = null;
			foreach (var vol in volumes)
			{
				request.VolumeId = vol.Id;
				do
				{
					request.PageToken = pageToken;
					try
					{
						var annotations = await request.ExecuteAsync(ct).ConfigureAwait(false);
						ret.AddRange(ConvertAnnotations(annotations, vol));
						pageToken = annotations.NextPageToken;
					}
					catch (Exception e) when (e is TaskCanceledException || e is GoogleApiException || e is OperationCanceledException)
					{
						break;
					}
				} while (pageToken != null);
			}
			annotations = ret;
		}

		private async Task<List<Volumes>> GetVolumesAsync(Bookshelves bookshelves, CancellationToken ct)
		{
			var ret = new List<Volumes>();
			foreach (var item in bookshelves.Items)
			{
				try
				{
					var request = service.Mylibrary.Bookshelves.Volumes.List(item.Id.ToString());
					request.MaxResults = 40;
					request.StartIndex = 0;
					Volumes result;
					if (item.VolumeCount > 0)
					{
						do
						{
							result = await request.ExecuteAsync(ct).ConfigureAwait(false);
							ret.Add(result);
							request.StartIndex += result.Items?.Count;
						} while (result.TotalItems > request.StartIndex);
					}
				}
				catch (Exception e) when (e is TaskCanceledException || e is GoogleApiException || e is OperationCanceledException)
				{
					break;
				}
			}
			return ret;
		}

		private GoogleBooksNote Convert(GoogleBooksAnnotation annotation)
		{
			return new GoogleBooksNote
			{
				Content = annotation.SelectedText,
				CreationTime = annotation.Created,
				NoteType = annotation.Type,
				Path = annotation.Path,
				Title = annotation.BookTitle,
				IntegrationId = annotation.Id,
				Source = new GoogleBooksSource { Title = annotation.BookTitle, IntegrationId = annotation.BookId, Active = true }
			};
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				service?.Dispose();
				service = null;
			}
		}
	}
}

