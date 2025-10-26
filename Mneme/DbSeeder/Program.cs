using Bogus;
using Microsoft.EntityFrameworkCore;
using Mneme.Integrations.Mneme.Contract;
using Mneme.Integrations.Mneme.Database;
using Mneme.Model;
using Mneme.Testing.Database;
using Mneme.Testing.TestCreation;
using System.Collections.Concurrent;
using System.Diagnostics;

namespace DbSeeder;

internal class Program
{
	private static void Main()
	{
		var globalMultiplier = 100;
		var amountOfSources = 50 * globalMultiplier;
		var amountOfNotesPerSourceMin = 0;
		var amountOfNotesPerSourceMax = 10;
		var amountOfTestsPerNoteMin = 0;
		var amountOfTestsPerNoteMax = 2;
		var amountOfAnswersInMultipleChoiceTest = 3;

		var faker = new Faker();
		var sources = new List<MnemeSource>(amountOfSources);
		var notes = new ConcurrentBag<MnemeNote>();
		var tests = new List<Test>();
		using MnemeContext mnemeContext = new();
		using TestingContext testingContext = new();

		_ = mnemeContext.Database.EnsureDeleted();
		_ = testingContext.Database.EnsureDeleted();
		mnemeContext.Database.Migrate();
		testingContext.Database.Migrate();

		var fakeSource = SourceFaker();
		var fakeNote = NoteFaker();
		var fakeShortTest = ShortTestFaker();
		var fakeAnswer = TestMultipleChoiceFaker();
		var fakeCorrectAnswer = TestMultipleChoiceAlwaysCorrectAnswerFaker();
		var fakeMultipleChoicesTest = TestMultipleChoicesFaker();

		var stopwatch = Stopwatch.StartNew();
		{//sources generation
			sources = fakeSource.Generate(amountOfSources);
			Console.WriteLine($"{sources.Count} sources generated in {stopwatch.Elapsed}");
			stopwatch.Restart();
			mnemeContext.AddRange(sources);
			Console.WriteLine($"Sources added to dbContext in {stopwatch.Elapsed}");
			stopwatch.Restart();
		}

		var random = new Random();
		var seed = random.Next();
		{//notes generation
			var chunkSize = (int)Math.Floor(sources.Count / (double)Environment.ProcessorCount);
			var sourceChunks = sources.Chunk(chunkSize).ToList();

			_ = Parallel.ForEach(sourceChunks, sourceChunk =>
			{
				var rand = new Random(Interlocked.Increment(ref seed));

				foreach (var source in sourceChunk)
				{
					var sourceNotes = Enumerable.Range(0, rand.Next(amountOfNotesPerSourceMin, amountOfNotesPerSourceMax + 1))
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
				}
			});

			mnemeContext.AddRange(notes);
			Console.WriteLine($"{notes.Count} notes generated in {stopwatch.Elapsed}");
			stopwatch.Restart();

			_ = mnemeContext.SaveChanges();
			Console.WriteLine($"Sources and notes saved in {stopwatch.Elapsed}");
			stopwatch.Restart();
		}
		{//tests generation
			foreach (var note in notes)
			{
				var amountOfTests = random.Next(amountOfTestsPerNoteMin, amountOfTestsPerNoteMax + 1);
				for (var i = 0; i < amountOfTests; i++)
				{
					if (random.Next(0, 2) == 0)
					{
						var test = fakeShortTest.Generate();
						test.NoteId = note.Id;
						tests.Add(test);
					}
					else
					{
						var answers = new List<TestMultipleChoice>
					{
            //add at least one correct answer
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
						tests.Add(test);
					}
				}
			}
			Console.WriteLine($"{tests.Count} tests generated in {stopwatch.Elapsed}");
			stopwatch.Restart();
			testingContext.AddRange(tests);
			_ = testingContext.SaveChanges();
			Console.WriteLine($"Tests saved in {stopwatch.Elapsed}");

		}
	}

	private static Faker<TestMultipleChoices> TestMultipleChoicesFaker() =>
			new Faker<TestMultipleChoices>()
						.RuleFor(t => t.Created, f => f.Date.Past())
						.RuleFor(t => t.Importance, f => f.Random.Number(0, 2))
						.RuleFor(t => t.Question, f => f.Random.Words());
	private static Faker<TestMultipleChoice> TestMultipleChoiceAlwaysCorrectAnswerFaker() =>
			new Faker<TestMultipleChoice>()
					.RuleFor(t => t.Answer, f => f.Random.Word())
					.RuleFor(t => t.IsCorrect, f => true);
	private static Faker<TestMultipleChoice> TestMultipleChoiceFaker() =>
			new Faker<TestMultipleChoice>()
								.RuleFor(t => t.Answer, f => f.Random.Word())
								.RuleFor(t => t.IsCorrect, f => f.Random.Bool());
	private static Faker<TestShortAnswer> ShortTestFaker() =>
			new Faker<TestShortAnswer>()
								.RuleFor(t => t.Created, f => f.Date.Past())
								.RuleFor(t => t.Importance, f => f.Random.Number(0, 2))
								.RuleFor(t => t.Hint, f => f.Random.Words())
								.RuleFor(t => t.Question, f => f.Random.Words())
								.RuleFor(t => t.Answer, f => f.Random.Word());
	private static Faker<MnemeNote> NoteFaker() =>
			new Faker<MnemeNote>()
							.RuleFor(n => n.Title, f => f.Random.Words())
							.RuleFor(n => n.CreationTime, f => f.Date.Past())
							.RuleFor(n => n.Content, f => f.Random.Words(20))
							.RuleFor(n => n.IntegrationId, f => f.Random.Guid().ToString())
							.RuleFor(n => n.Path, f => f.Internet.Url());
	private static Faker<MnemeSource> SourceFaker() =>
			new Faker<MnemeSource>()
							.RuleFor(s => s.Title, f => f.Random.Words())
							.RuleFor(s => s.CreationTime, f => f.Date.Between(DateTime.Now.AddDays(-10), DateTime.Now))
							.RuleFor(s => s.Active, f => f.Random.Bool())
							.RuleFor(s => s.Details, f => f.Internet.Url());
}
