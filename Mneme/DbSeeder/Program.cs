using Bogus;
using Microsoft.EntityFrameworkCore;
using Mneme.Integrations.Mneme.Contract;
using Mneme.Integrations.Mneme.Database;
using Mneme.Testing.Database;
using Mneme.Testing.TestCreation;
using System.Collections.Concurrent;
using System.Diagnostics;

namespace DbSeeder;

internal class Program
{
	private static void Main()
	{
		var faker = new Faker();
		var globalMultiplier = 1000;
		var amountOfSources = 50 * globalMultiplier;
		var amountOfNotesPerSourceMin = 0;
		var amountOfNotesPerSourceMax = 10;
		var amountOfTestsPerNoteMin = 0;
		var amountOfTestsPerNoteMax = 2;
		var amountOfAnswersInMultipleChoiceTest = 6;

		// Setup faker rules outside the parallel regions
		var fakeSource = new Faker<MnemeSource>()
						.RuleFor(s => s.Title, f => f.Random.Words())
						.RuleFor(s => s.CreationTime, f => f.Date.Between(DateTime.Now.AddDays(-10), DateTime.Now))
						.RuleFor(s => s.Active, f => f.Random.Bool())
						.RuleFor(s => s.Details, f => f.Internet.Url());

		var fakeNote = new Faker<MnemeNote>()
						.RuleFor(n => n.Title, f => f.Random.Words())
						.RuleFor(n => n.CreationTime, f => f.Date.Past())
						.RuleFor(n => n.Content, f => f.Random.Words(20))
						.RuleFor(n => n.IntegrationId, f => f.Random.Guid().ToString())
						.RuleFor(n => n.Path, f => f.Internet.Url());

		var fakeShortTest = new Faker<TestShortAnswer>()
							.RuleFor(t => t.Created, f => f.Date.Past())
							.RuleFor(t => t.Importance, f => f.Random.Number(0, 2))
							.RuleFor(t => t.Hint, f => f.Random.Words())
							.RuleFor(t => t.Question, f => f.Random.Words())
							.RuleFor(t => t.Answer, f => f.Random.Word());

		var fakeAnswer = new Faker<TestMultipleChoice>()
							.RuleFor(t => t.Answer, f => f.Random.Word())
							.RuleFor(t => t.IsCorrect, f => f.Random.Bool());

		var fakeCorrectAnswer = new Faker<TestMultipleChoice>()
				.RuleFor(t => t.Answer, f => f.Random.Word())
				.RuleFor(t => t.IsCorrect, f => true);

		var fakeMultipleChoicesTest = new Faker<TestMultipleChoices>()
					.RuleFor(t => t.Created, f => f.Date.Past())
					.RuleFor(t => t.Importance, f => f.Random.Number(0, 2))
					.RuleFor(t => t.Question, f => f.Random.Words());

		var stopwatch = Stopwatch.StartNew();

		// Generate sources in parallel
		var sources = ParallelEnumerable.Range(0, amountOfSources)
			.Select(_ => fakeSource.Generate())
			.ToList();

		Console.WriteLine($"{sources.Count} sources generated in {stopwatch.Elapsed}");
		stopwatch.Restart();

		// Save sources in batches
		using (var mnemeContext = new MnemeContext())
		{
			mnemeContext.Database.EnsureDeleted();
			mnemeContext.Database.Migrate();
			
			const int batchSize = 10000;
			foreach (var sourceBatch in sources.Chunk(batchSize))
			{
				mnemeContext.AddRange(sourceBatch);
				mnemeContext.SaveChanges();
				mnemeContext.ChangeTracker.Clear();
			}
		}
		
		Console.WriteLine($"Sources saved in {stopwatch.Elapsed}");
		stopwatch.Restart();

		// Generate notes in parallel
		var notes = new ConcurrentBag<MnemeNote>();
		Parallel.ForEach(sources, source =>
		{
			var random = new Random();
			var amountOfNotes = random.Next(amountOfNotesPerSourceMin, amountOfNotesPerSourceMax + 1);
			var sourceNotes = Enumerable.Range(0, amountOfNotes)
				.Select(_ =>
				{
					var note = fakeNote.Generate();
					note.Source = source;
					return note;
				});
			foreach (var note in sourceNotes)
			{
				notes.Add(note);
			}
		});

		Console.WriteLine($"{notes.Count} notes generated in {stopwatch.Elapsed}");
		stopwatch.Restart();

		// Save notes in batches
		using (var mnemeContext = new MnemeContext())
		{
			foreach (var noteBatch in notes.Chunk(10000))
			{
				mnemeContext.AddRange(noteBatch);
				mnemeContext.SaveChanges();
				mnemeContext.ChangeTracker.Clear();
			}
		}

		Console.WriteLine($"Notes saved in {stopwatch.Elapsed}");
		stopwatch.Restart();

		// Generate and save tests in parallel batches
		var tests = new ConcurrentBag<(TestShortAnswer shortAnswer, TestMultipleChoices multipleChoice)>();
		Parallel.ForEach(notes, note =>
		{
			var random = new Random();
			var amountOfTests = random.Next(amountOfTestsPerNoteMin, amountOfTestsPerNoteMax + 1);
			for (var i = 0; i < amountOfTests; i++)
			{
				if (random.Next(0, 2) == 0)
				{
					var test = fakeShortTest.Generate();
					test.NoteId = note.Id;
					tests.Add((test, null));
				}
				else
				{
					var answers = new List<TestMultipleChoice>
					{
						fakeCorrectAnswer.Generate()
					};

					for (var j = 0; j < amountOfAnswersInMultipleChoiceTest; j++)
					{
						answers.Add(fakeAnswer.Generate());
					}

					var test = fakeMultipleChoicesTest.Generate();
					test.NoteId = note.Id;
					test.Answers = answers;
					answers.ForEach(t => t.Test = test);
					tests.Add((null, test));
				}
			}
		});

		Console.WriteLine($"Tests generated in {stopwatch.Elapsed}");
		stopwatch.Restart();

		// Save tests in batches
		using (var testingContext = new TestingContext())
		{
			testingContext.Database.Migrate();
			
			foreach (var batch in tests.Chunk(5000))
			{
				foreach (var (shortAnswer, multipleChoice) in batch)
				{
					if (shortAnswer != null)
						testingContext.Add(shortAnswer);
					if (multipleChoice != null)
						testingContext.Add(multipleChoice);
				}
				testingContext.SaveChanges();
				testingContext.ChangeTracker.Clear();
			}
		}

		Console.WriteLine($"Tests saved in {stopwatch.Elapsed}");
	}
}
