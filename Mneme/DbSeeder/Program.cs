using Bogus;
using Mneme.Integrations.Mneme.Contract;
using Mneme.Testing.Contracts;
using Mneme.Testing.TestCreation;

namespace DbSeeder;

internal class Program
{
	private static void Main(string[] args)
	{
		var faker = new Faker();
		var sources = new List<MnemeSource>();
		var notes = new List<MnemeNote>();
		var repository = new MnemeIntegrationFacade();
		var testingRepository = new TestingRepository();

		var amountOfSources = 50;
		var amountOfNotesPerSourceMin = 0;
		var amountOfNotesPerSourceMax = 10;
		var amountOfTestsPerNoteMin = 0;
		var amountOfTestsPerNoteMax = 2;
		var amountOfAnswersInMultipleChoiceTest = 6;

		Faker<MnemeSource> fakeSource = new Faker<MnemeSource>()
						.RuleFor(s => s.Title, f => f.Random.Words())
						.RuleFor(s => s.CreationTime, f => f.Date.Between(DateTime.Now.AddDays(-10), DateTime.Now))
						.RuleFor(s => s.Active, f => f.Random.Bool())
						.RuleFor(s => s.Details, f => f.Internet.Url());

		Faker<MnemeNote> fakeNote = new Faker<MnemeNote>()
						.RuleFor(n => n.Title, f => f.Random.Words())
						.RuleFor(n => n.CreationTime, f => f.Date.Past())
						.RuleFor(n => n.Content, f => f.Random.Words(20))
						.RuleFor(n => n.IntegrationId, f => f.Random.Guid().ToString())
						.RuleFor(n => n.Path, f => f.Internet.Url());

		Faker<TestShortAnswer> fakeShortTest = new Faker<TestShortAnswer>()
							.RuleFor(t => t.Created, f => f.Date.Past())
							.RuleFor(t => t.Importance, f => f.Random.Number(0, 2))
							.RuleFor(t => t.Hint, f => f.Random.Words())
							.RuleFor(t => t.Question, f => f.Random.Words())
							.RuleFor(t => t.Answer, f => f.Random.Word());

		Faker<TestMultipleChoice> fakeAnswer = new Faker<TestMultipleChoice>()
							.RuleFor(t => t.Answer, f => f.Random.Word())
							.RuleFor(t => t.IsCorrect, f => f.Random.Bool());

		Faker<TestMultipleChoice> fakeCorrectAnswer = new Faker<TestMultipleChoice>()
				.RuleFor(t => t.Answer, f => f.Random.Word())
				.RuleFor(t => t.IsCorrect, f => true);

		Faker<TestMultipleChoices> fakeMultipleChoicesTest = new Faker<TestMultipleChoices>()
					.RuleFor(t => t.Created, f => f.Date.Past())
					.RuleFor(t => t.Importance, f => f.Random.Number(0, 2))
					.RuleFor(t => t.Question, f => f.Random.Words());

		//populate
		for (var i = 0; i < amountOfSources; i++)
		{
			MnemeSource source = fakeSource.Generate();
			sources.Add(source);
			_ = repository.CreateSource(source);
		}
		Console.WriteLine("Sources generated");
		var random = new Random();
		foreach (MnemeSource source in sources)
		{
			var amountOfNotes = random.Next(amountOfNotesPerSourceMin, amountOfNotesPerSourceMax + 1);
			for (var i = 0; i < amountOfNotes; i++)
			{
				MnemeNote note = fakeNote.Generate();
				note.Source = source;
				notes.Add(note);
				_ = repository.CreateNote(note);
			}
		}
		Console.WriteLine("Notes generated");
		foreach (MnemeNote note in notes)
		{
			var amountOfTests = random.Next(amountOfTestsPerNoteMin, amountOfTestsPerNoteMax + 1);
			for (var i = 0; i < amountOfTests; i++)
			{
				if (random.Next(0, 2) == 0)
				{
					TestShortAnswer test = fakeShortTest.Generate();
					test.NoteId = note.Id;
					testingRepository.CreateTest(test);
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
					TestMultipleChoices test = fakeMultipleChoicesTest.Generate();
					test.NoteId = note.Id;
					test.Answers = answers;
					answers.ForEach(t => t.Test = test);

					testingRepository.CreateTest(test);
				}
			}
		}
		Console.WriteLine("Tests generated");
	}
}
