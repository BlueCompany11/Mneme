using System.Linq;
using AutoFixture;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Mneme.Core.Pluralsight;
using Mneme.DataAccess;
using Xunit;

namespace Mneme.Core.Tests.Pluralsight
{
	public class PluralsightConfigProviderShould
	{
		private readonly DbContextOptionsBuilder builder;
		private readonly Fixture fixture;
		public PluralsightConfigProviderShould()
		{
			fixture = new();
			builder = new DbContextOptionsBuilder();
		}
		[Fact]
		public void TriggerEventWhenPathUpdatedAndPathIsUpdated()
		{
			string path = fixture.Create<string>();
			_ = builder.UseInMemoryDatabase(nameof(TriggerEventWhenPathUpdatedAndPathIsUpdated));
			using var db = new Context(builder.Options);
			var sut = new PluralsightConfigProvider(db);
			using var monitoredSut = sut.Monitor();
			sut.UpdatePath(path);
			_ = monitoredSut.Should().Raise(nameof(sut.SourceUpdated));
			_ = db.PluralsightConfigs.Single().FilePath.Should().Be(path);
		}
	}
}
