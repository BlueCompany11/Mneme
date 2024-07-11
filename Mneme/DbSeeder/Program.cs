using System;
using Bogus;
using Mneme.Integrations.Mneme.Contract;
using Mneme.Model.TestCreation;
using Mneme.Testing.Contracts;

namespace DbSeeder
{
	class Program
	{
		static void Main(string[] args)
		{
			var faker = new Faker();
			var sources = new List<MnemeSource>();
			var notes = new List<MnemeNote>();
			var repository = new MnemeIntegrationFacade();
			var testingRepository = new TestingRepository();

			int amountOfSources = 100;
			int amountOfNotesPerSourceMin = 0;
			int amountOfNotesPerSourceMax = 10;
			int amountOfTestsPerNoteMin = 0;
			int amountOfTestsPerNoteMax = 2;
			int amountOfAnswersInMultipleChoiceTest = 6;

			for (int i = 0 ; i < amountOfSources ; i++)
			{
				var source = new Faker<MnemeSource>()
						.RuleFor(s => s.Title, f => f.Random.Words())
						.RuleFor(s => s.CreationTime, f => f.Date.Past())
						.RuleFor(s => s.Active, f => f.Random.Bool())
						.RuleFor(s => s.Details, f => f.Internet.Url())
						.RuleFor(s => s.IntegrationId, (f, s) => MnemeSource.GenerateIntegrationId(s.Title, s.Details))
						.Generate();
				sources.Add(source);
				repository.CreateSource(source);
			}
			Random random = new Random();
			foreach (var source in sources)
			{
				int amountOfNotes = random.Next(amountOfNotesPerSourceMin, amountOfNotesPerSourceMax + 1);
				for (int i = 0 ; i < amountOfNotes ; i++)
				{
					var note = new Faker<MnemeNote>()
						.RuleFor(n => n.Title, f => f.Random.Words())
						.RuleFor(n => n.CreationTime, f => f.Date.Past())
						.RuleFor(n => n.Content, f => f.Random.Words())
						.RuleFor(n => n.IntegrationId, f => f.Random.Guid().ToString())
						.RuleFor(n => n.Path, f => f.Internet.Url())
						.RuleFor(n => n.Source, f => source)
						.Generate();
					notes.Add(note);
					repository.CreateNote(note);
				}
			}
			foreach (var note in notes)
			{
				int amountOfTests = random.Next(amountOfTestsPerNoteMin, amountOfTestsPerNoteMax + 1);
				for (int i = 0 ; i < amountOfTests ; i++)
				{
					if (random.Next(0, 2) == 0)
					{
						var test = new Faker<TestShortAnswer>()
							.RuleFor(t => t.Created, f => f.Date.Past())
							.RuleFor(t => t.Importance, f => f.Random.Number(0, 2))
							.RuleFor(t => t.Hint, f => f.Random.Words())
							.RuleFor(t => t.Question, f => f.Random.Words())
							.RuleFor(t => t.Answer, f => f.Random.Word())
							.RuleFor(t => t.NoteId, f => note.Id)
							.Generate();
						testingRepository.CreateTest(test);
					}
					else
					{
						var answers = new List<TestMultipleChoice>();

						//add at least one correct answer
						var corrcetAnswer = new Faker<TestMultipleChoice>()
								.RuleFor(t => t.Answer, f => f.Random.Word())
								.RuleFor(t => t.IsCorrect, f => true);
						answers.Add(corrcetAnswer);

						for (int j = 0 ; j < amountOfAnswersInMultipleChoiceTest ; j++)
						{
							var answer = new Faker<TestMultipleChoice>()
								.RuleFor(t => t.Answer, f => f.Random.Word())
								.RuleFor(t => t.IsCorrect, f => f.Random.Bool());
							answers.Add(answer);
						}

						var test = new Faker<TestMultipleChoices>()
							.RuleFor(t => t.Created, f => f.Date.Past())
							.RuleFor(t => t.Importance, f => f.Random.Number(0, 2))
							.RuleFor(t => t.Question, f => f.Random.Words())
							.RuleFor(t => t.NoteId, f => note.Id)
							.RuleFor(t => t.Answers, f => answers);
						answers.ForEach(t => t.Test = test);

						testingRepository.CreateTest(test);
					}

				}
			}
		}
	}
}
