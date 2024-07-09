﻿using Microsoft.EntityFrameworkCore;
using Mneme.DataAccess;
using Mneme.Integrations.Contracts;
using Mneme.Integrations.GoogleBooks.Database;

namespace Mneme.Integrations.GoogleBooks.Contract
{
	public class GoogleBooksIntegrationFacade : IntegrationFacadeBase<Context, GoogleBooksSource, GoogleBooksNote>
	{
		private readonly GoogleBooksNoteProvider noteProvider;

		public GoogleBooksIntegrationFacade(GoogleBooksNoteProvider noteProvider) : base()
		{
			this.noteProvider = noteProvider;
		}

		protected override Context CreateContext()
		{
			return new GoogleBooksContext();
		}
		public override async Task<IReadOnlyList<GoogleBooksNote>> GetNotes(CancellationToken ct)
		{
			return await noteProvider.GetNotesAsync(ct);
		}

		public override async Task<IReadOnlyList<GoogleBooksNote>> GetKnownNotes(bool activeOnly, CancellationToken ct)
		{
			using var context = CreateContext();
			return await context.Set<GoogleBooksNote>().ToListAsync(ct);
			//TODO activeOnly
		}
	}
}

