using Mneme.Model;
using Mneme.Testing.Contracts;

namespace Mneme.Testing.RepetitionAlgorithm
{
	public class SpaceRepetition
	{
		private static readonly int[] IntervalSteps = { 0, 1, 2, 4, 7, 15, 30, 60, 120, 240, 360 };
		private readonly TestingRepository repository;

		public SpaceRepetition(TestingRepository repository)
		{
			this.repository = repository;
		}
		public bool ShouldBeTested(Test userTest)
		{
			return (DateTime.Now - userTest.Updated).Days >= IntervalSteps[userTest.Interval];
		}
		public void MakeTest(Test test, bool isCorrect)
		{
			if (isCorrect)
			{
				test.Interval++;
				test.Updated = DateTime.Now;
			}
			else
			{
				test.Interval = 0;
			}
			test.Updated = DateTime.Now;
			repository.EditTest(test);
		}
	}
}
