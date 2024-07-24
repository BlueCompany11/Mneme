using Microsoft.EntityFrameworkCore;
using Mneme.Core;
using Mneme.Integrations.Contracts;
using Mneme.Integrations.Mneme.Contract;
using Mneme.Model;

namespace Mneme.Sources;

public class MnemeSourceProxy
{ 
	private readonly IIntegrationFacade<MnemeSource, MnemeNote> mnemeIntegration;

	public MnemeSourceProxy(IIntegrationFacade<MnemeSource, MnemeNote> mnemeIntegration)
	{
		this.mnemeIntegration = mnemeIntegration;
	}

	public async Task<MnemeSource?> SaveMnemeSource(string sourceTitle, string details, CancellationToken ct)
	{
		var source = new MnemeSource { Title = sourceTitle, Details = details, Active = true };
		try
		{
			await mnemeIntegration.CreateSource(source).ConfigureAwait(false);
			return await mnemeIntegration.GetSource(source.Id, ct).ConfigureAwait(false);
		}
		catch (Exception)
		{
			return null;
		}
	}

	public async Task<MnemeSource?> UpdateMnemeSource(int id, string title, string details, CancellationToken ct)
	{
		MnemeSource existingSource = await mnemeIntegration.GetSource(id, ct).ConfigureAwait(false);
		existingSource.Title = title;
		existingSource.Details = details;
		await mnemeIntegration.UpdateSource(existingSource, ct).ConfigureAwait(false);
		return await mnemeIntegration.GetSource(id, ct).ConfigureAwait(false);
	}

	public async Task<bool> DeleteSource(Source source)
	{
		try
		{
			await mnemeIntegration.DeleteSource(source.Id, default).ConfigureAwait(false);
			return true;
		}
		catch (DbUpdateException)
		{
			return false;
		}
	}
}
