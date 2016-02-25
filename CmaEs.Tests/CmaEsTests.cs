using SteveBagnall.CmaEs_IPOP;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;

namespace CMAESReduxTest
{
    
    
    /// <summary>
    ///This is a test class for CMAESTest and is intended
    ///to contain all CMAESTest Unit Tests
    ///</summary>
	[TestClass()]
	public class CMAESTest
	{
		private const double SMALL_VALUE = 1e-3;

		private TestContext testContextInstance;

		/// <summary>
		///Gets or sets the test context which provides
		///information about and functionality for the current test run.
		///</summary>
		public TestContext TestContext
		{
			get
			{
				return testContextInstance;
			}
			set
			{
				testContextInstance = value;
			}
		}

		#region Additional test attributes
		// 
		//You can use the following additional attributes as you write your tests:
		//
		//Use ClassInitialize to run code before running the first test in the class
		//[ClassInitialize()]
		//public static void MyClassInitialize(TestContext testContext)
		//{
		//}
		//
		//Use ClassCleanup to run code after all tests in a class have run
		//[ClassCleanup()]
		//public static void MyClassCleanup()
		//{
		//}
		//
		//Use TestInitialize to run code before running each test
		//[TestInitialize()]
		//public void MyTestInitialize()
		//{
		//}
		//
		//Use TestCleanup to run code after each test has run
		//[TestCleanup()]
		//public void MyTestCleanup()
		//{
		//}
		//
		#endregion


		[TestMethod()]
		public void RastriginTest()
		{
			double[] expected = new[] { 0.0 };

			CmaEs target = new CmaEs(
				(x, o) =>
				{
					if (x.Min() < -5.12 || x.Max() > 5.12)
						return 100.0 + 10.0 * x.Length + x.Sum(xi => Math.Pow(xi, 2) - 10 * Math.Cos(2.0 * Math.PI * xi));
					else
						return 10.0 * x.Length + x.Sum(xi => Math.Pow(xi, 2) - 10 * Math.Cos(2.0 * Math.PI * xi));
				},
				initialParameters: Utilities.Randoms(expected.Length, null, null));

			double[] actual = target.Solve().BestParameters;

			for (int i = 0; i < expected.Length; i++)
				Assert.IsTrue(Math.Abs(expected[i] - actual[i]) < SMALL_VALUE);
		}

		[TestMethod()]
		public void RastriginTestMany()
		{
			double[] expected = new[] { 0.0, 0.0, 0.0, 0.0, 0.0 };

			CmaEs target = new CmaEs(
				(x, o) =>
				{
					if (x.Min() < -5.12 || x.Max() > 5.12)
						return 100.0 + 10.0 * x.Length + x.Sum(xi => Math.Pow(xi, 2) - 10 * Math.Cos(2.0 * Math.PI * xi));
					else
						return 10.0 * x.Length + x.Sum(xi => Math.Pow(xi, 2) - 10 * Math.Cos(2.0 * Math.PI * xi));
				},
				initialParameters: Utilities.Randoms(expected.Length, null, null),
				stopEvalsMultiple: 1e4);

			Solution solution = target.Solve();
			double[] actual = solution.BestParameters;

			for (int i = 0; i < expected.Length; i++)
				Assert.IsTrue(Math.Abs(expected[i] - actual[i]) < SMALL_VALUE);
		}
		
		[TestMethod()]
		public void RosenbrockTest()
		{
			double[] expected = new[] { 1.0, 1.0 };

			CmaEs target = new CmaEs(
				(x, o) =>
				{
					int dimension = x.Length;

					double res = 0;
					for (int i = 0; i < (dimension - 1); ++i)
						res += Math.Pow(1 - x[i], 2) + 100.0 * Math.Pow(x[i + 1] - Math.Pow(x[i], 2), 2);
					return res;
				},
				initialParameters: Utilities.Randoms(expected.Length, null, null));


			double[] actual = target.Solve().BestParameters;

			for (int i = 0; i < expected.Length; i++)
				Assert.IsTrue(Math.Abs(expected[i] - actual[i]) < SMALL_VALUE);
		}


		[TestMethod()]
		public void RosenbrockTwelveTest()
		{
			double[] expected = new[] { 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0 };

			CmaEs target = new CmaEs(
				(x, o) =>
				{
					int dimension = x.Length;

					double res = 0;
					for (int i = 0; i < (dimension - 1); ++i)
						res += Math.Pow(1 - x[i], 2) + 100.0 * Math.Pow(x[i + 1] - Math.Pow(x[i], 2), 2);
					return res;
				},
				initialParameters: Utilities.Randoms(expected.Length, null, null),
				stopEvalsMultiple: 1e4);

			Solution actual = target.Solve();

			for (int i = 0; i < expected.Length; i++)
				Assert.IsTrue(Math.Abs(expected[i] - actual.BestParameters[i]) < SMALL_VALUE);
		}

		[TestMethod()]
		public void RosenbrockthirtyTest()
		{
			double[] expected = new[] { 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0 };

			CmaEs target = new CmaEs(
				(x, o) =>
			{
				int dimension = x.Length;

				double res = 0;
				for (int i = 0; i < (dimension - 1); ++i)
					res += Math.Pow(1 - x[i], 2) + 100.0 * Math.Pow(x[i + 1] - Math.Pow(x[i], 2), 2);
				return res;
			},
			initialParameters: Utilities.Randoms(expected.Length, null, null),
			stopEvalsMultiple: 1e5);

			Solution actual = target.Solve();

			for (int i = 0; i < expected.Length; i++)
				Assert.IsTrue(Math.Abs(expected[i] - actual.BestParameters[i]) < SMALL_VALUE);
		}


		[TestMethod()]
		public void RosenbrockThirtyWithParamsTest()
		{
			double[] expected = new[] { 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0 };

			CmaEs target = new CmaEs((x, o) =>
			{
				int dimension = x.Length;

				double res = 0;
				for (int i = 0; i < (dimension - 1); ++i)
					res += Math.Pow(1 - x[i], 2) + 100.0 * Math.Pow(x[i + 1] - Math.Pow(x[i], 2), 2);
				return res;
			},
			initialParameters: Utilities.Randoms(expected.Length, null, null),
			stopEvalsMultiple: 1e5);

			double[] actual = target.Solve().BestParameters;

			for (int i = 0; i < expected.Length; i++)
				Assert.IsTrue(Math.Abs(expected[i] - actual[i]) < SMALL_VALUE);
		}

		[TestMethod()]
		public void SchwefelTest()
		{
			int numTests = 10;

			double[] expected = new[] { 420.9687, 420.9687, 420.9687, 420.9687, 420.9687, };

			CmaEs newCMA = new CmaEs((x, o) =>
				{
					if (x.Min() < -500.0)
						return Math.Pow(Math.Abs(x.Min()), 4);
					else if (x.Max() > 500.0)
						return Math.Pow(Math.Abs(x.Max()), 4);
					else
						return 418.9829 * x.Length + x.Sum(xi => -xi * Math.Sin(Math.Sqrt(Math.Abs(xi))));
				},
				stopEvalsMultiple: 1e5,
				stopFitnessMinimization: SMALL_VALUE,
				initialParameters: Utilities.Randoms(expected.Length, null, null));

			Stopwatch clock = new Stopwatch();

			int count = 0;
			List<TimeSpan> times = new List<TimeSpan>();


			for (int j = 0; j < numTests; j++)
			{
				clock.Restart();

				var actual = newCMA.Solve(isContinueFromLastSolution: false);

				clock.Stop();
				times.Add(clock.Elapsed);

				bool isAllOK = true;
				for (int i = 0; i < expected.Length; i++)
					if (Math.Abs(expected[i] - actual.BestParameters[i]) > 1.0)
						isAllOK = false;

				if (isAllOK)
					count++;
			}

			double percRight = count * 100.0 / numTests;
			double averageTime = times.Average(x => x.Seconds);

			Assert.IsTrue(percRight >= 90.0);
			Assert.IsTrue(averageTime < 2.0);
		}

		[TestMethod()]
		public void SchwefelContinueFromLastTest()
		{
			int numTests = 10;

			double[] expected = new[] { 420.9687, 420.9687, 420.9687, 420.9687, 420.9687, };
			
			CmaEs newCMA = new CmaEs((x, o) =>
					{
						if (x.Min() < -500.0)
							return Math.Pow(Math.Abs(x.Min()), 4);
						else if (x.Max() > 500.0)
							return Math.Pow(Math.Abs(x.Max()), 4);
						else
							return 418.9829 * x.Length + x.Sum(xi => -xi * Math.Sin(Math.Sqrt(Math.Abs(xi))));
					},
					stopEvalsMultiple:1e3,
					stopFitnessMinimization: 1e-5,
					initialParameters: Utilities.Randoms(expected.Length, null, null));

			Stopwatch clock = new Stopwatch();
		
			int count = 0;
			List<TimeSpan> times = new List<TimeSpan>();

			
			for (int j = 0; j < numTests; j++)
			{
				clock.Restart();

				var actual = newCMA.Solve(isContinueFromLastSolution:true);

				clock.Stop();
				times.Add(clock.Elapsed);

				bool isAllOK = true;
				for (int i = 0; i < expected.Length; i++)
					if (Math.Abs(expected[i] - actual.BestParameters[i]) > 1.0)
						isAllOK = false;

				if (isAllOK)
					count++;
			}

			double percRight = count * 100.0 / numTests;
			double averageTime = times.Average(x => x.Seconds);

			Assert.IsTrue(percRight >= 90.0);
			Assert.IsTrue(averageTime < 2.0);
		}

		[TestMethod()]
		public void SchwefelOneGenerationTest()
		{
			double[] expected = new[] { 420.9687, 420.9687, 420.9687, 420.9687, 420.9687, };

			CmaEs newCMA = new CmaEs((x, o) =>
				{
					if ((x.Min() < -500.0) || (x.Max() > 500.0))
						return 10000.0 * x.Length + x.Sum(xi => -xi * Math.Sin(Math.Sqrt(Math.Abs(xi))));
					else
						return 418.9829 * x.Length + x.Sum(xi => -xi * Math.Sin(Math.Sqrt(Math.Abs(xi))));
				},
				initialParameters: Utilities.Randoms(expected.Length, null, null));

			Solution lastSolution;
			int count = 0;
			while(true)
			{
				lastSolution = newCMA.Solve(isContinueFromLastSolution: true, isOneGenerationOnly: true);
				if (lastSolution.MinimizedFitness < SMALL_VALUE)
					break;

				count += lastSolution.LatestNumEvals;
			}

			Assert.IsTrue(count < 1e5);

			for (int i = 0; i < expected.Length; i++)
				Assert.IsTrue(Math.Abs(expected[i] - lastSolution.BestParameters[i]) < 1.0);
		}

	}
}
