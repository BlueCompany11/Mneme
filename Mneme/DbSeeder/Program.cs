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
			for (int i = 0 ; i < 100 ; i++)
			{
				var source = new Faker<MnemeSource>()
						.RuleFor(s => s.Title, f => f.Lorem.Sentence())
						.RuleFor(s => s.CreationTime, f => f.Date.Past())
						.RuleFor(s => s.Active, f => f.Random.Bool())
						.RuleFor(s => s.Details, f => f.Lorem.Paragraph())
						.RuleFor(s => s.IntegrationId, (f, s) => MnemeSource.GenerateIntegrationId(s.Title, s.Details))
						.Generate();
				sources.Add(source);
				repository.CreateSource(source);
			}
			Random random = new Random();
			foreach (var source in sources)
			{
				int amountOfNotes = random.Next(0, 11);
				for (int i = 0 ; i < amountOfNotes ; i++)
				{
					var note = new Faker<MnemeNote>()
						.RuleFor(n => n.Title, f => f.Lorem.Sentence())
						.RuleFor(n => n.CreationTime, f => f.Date.Past())
						.RuleFor(n => n.Content, f => f.Lorem.Paragraph())
						.RuleFor(n => n.IntegrationId, f => f.Random.Guid().ToString())
						.RuleFor(n => n.Path, f => f.Lorem.Paragraph())
						.RuleFor(n => n.Source, f => source)
						.Generate();
					notes.Add(note);
					repository.CreateNote(note);
				}
			}
			foreach (var note in notes)
			{
				int amountOfTests = random.Next(0, 3);
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
						for (int j = 0 ; j < 6 ; j++)
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
