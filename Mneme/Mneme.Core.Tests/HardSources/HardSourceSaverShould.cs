using System;
using AutoFixture;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Mneme.Core.HardSources;
using Mneme.DataAccess;
using Mneme.Integrations.Mneme;
using Xunit;

namespace Mneme.Core.Tests.HardSources
{
	public class HardSourceSaverShould
	{
		private readonly Fixture fixture;
		private readonly DbContextOptionsBuilder builder;
		public HardSourceSaverShould()
		{
			fixture = new Fixture();
			builder = new DbContextOptionsBuilder();
		}
		[Fact]
		public void AddMnemeSourceToDb()
		{
			string dbName = nameof(AddMnemeSourceToDb);
			_ = builder.UseInMemoryDatabase(dbName);
			using var db = new Context(builder.Options);
			var sut = new MnemeSourceSaver(db);
			var source = fixture.Create<MnemeSource>();
			sut.Save(source);
			db.MnemeSources.Find(source.Id).Should().NotBeNull();
		}
	}
}
