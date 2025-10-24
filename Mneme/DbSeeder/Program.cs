using Bogus;
using Microsoft.EntityFrameworkCore;
using Mneme.Integrations.Mneme.Contract;
using Mneme.Integrations.Mneme.Database;
using Mneme.Testing.Database;
using Mneme.Testing.TestCreation;
using System.Diagnostics;

namespace DbSeeder;

internal class Program
{
	private static void Main()
	{
		var faker = new Faker();
		var sources = new List<MnemeSource>();
		var notes = new List<MnemeNote>();
		using MnemeContext mnemeContext = new();
		using TestingContext testingContext = new();

		mnemeContext.Database.EnsureDeleted();
		mnemeContext.Database.Migrate();
		testingContext.Database.EnsureDeleted();
		testingContext.Database.Migrate();

		var globalMultiplier = 1000;
		var amountOfSources = 50 * globalMultiplier;
		var amountOfNotesPerSourceMin = 0;
		var amountOfNotesPerSourceMax = 10;
		var amountOfTestsPerNoteMin = 0;
		var amountOfTestsPerNoteMax = 2;
		var amountOfAnswersInMultipleChoiceTest = 6;

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
		sources = fakeSource.Generate(amountOfSources);
		Console.WriteLine($"{sources.Count} sources generated in {stopwatch.Elapsed}");
		stopwatch.Restart();
		mnemeContext.AddRange(sources);
		Console.WriteLine($"Sources saved in {stopwatch.Elapsed}");
		stopwatch.Restart();

		var random = new Random();
		foreach (var source in sources)
		{
			var amountOfNotes = random.Next(amountOfNotesPerSourceMin, amountOfNotesPerSourceMax + 1);
			for (var i = 0; i < amountOfNotes; i++)
			{
				var note = fakeNote.Generate();
				note.Source = source;
				notes.Add(note);
			}
		}
		mnemeContext.AddRange(notes);
		Console.WriteLine($"{notes.Count} notes generated in {stopwatch.Elapsed}");
		stopwatch.Restart();

		mnemeContext.SaveChanges();
		Console.WriteLine($"Notes saved in {stopwatch.Elapsed}");
		stopwatch.Restart();
		foreach (var note in notes)
		{
			var amountOfTests = random.Next(amountOfTestsPerNoteMin, amountOfTestsPerNoteMax + 1);
			for (var i = 0; i < amountOfTests; i++)
			{
				if (random.Next(0, 2) == 0)
				{
					var test = fakeShortTest.Generate();
					test.NoteId = note.Id;
					testingContext.Add(test);
				} else
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

					testingContext.Add(test);
				}
			}
		}
		Console.WriteLine($"Tests generated in {stopwatch.Elapsed}");
		testingContext.SaveChanges();
		Console.WriteLine($"Tests saved in {stopwatch.Elapsed}");
	}
}
