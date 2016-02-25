using SteveBagnall.CmaEs_IPOP;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using Meta.Numerics.Matrices;

namespace CMAESReduxTest
{
    
    
    /// <summary>
    ///This is a test class for UtilitiesTest and is intended
    ///to contain all UtilitiesTest Unit Tests
    ///</summary>
	[TestClass()]
	public class UtilitiesTest
	{
		private const double SMALL_VALUE = 1e-2;

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


		/// <summary>
		///A test for Randoms
		///</summary>
		[TestMethod()]
		public void RandomsTest()
		{
			int dimension = 10000; 

			double min = 50.0;
			double max = 100.0;

			double?[] mins = new double?[dimension];
			double?[] maxes = new double?[dimension];

			for (int i = 0; i < dimension; i++ )
			{
				mins[i] = min;
				maxes[i] = max;
			}
			
			double[] actual = Utilities.Randoms(dimension, mins, maxes);

			foreach (double d in actual)
				Assert.IsTrue((d >= min) && (d < max));
		}

		/// <summary>
		///A test for Randoms
		///</summary>
		[TestMethod()]
		public void RandomsTestMinNull()
		{
			int dimension = 10000;

			double max = 100.0;

			double?[] mins = null;
			double?[] maxes = new double?[dimension];

			for (int i = 0; i < dimension; i++)
				maxes[i] = max;

			double[] actual = Utilities.Randoms(dimension, mins, maxes);

			foreach (double d in actual)
				Assert.IsTrue((d >= 0.0) && (d < max));
		}

		/// <summary>
		///A test for Randoms
		///</summary>
		[TestMethod()]
		public void RandomsTestMaxNull()
		{
			int dimension = 10000;
			
			double min = 50.0;

			double?[] mins = new double?[dimension];
			double?[] maxes = null;

			for (int i = 0; i < dimension; i++)
				mins[i] = min;

			double[] actual = Utilities.Randoms(dimension, mins, maxes);

			foreach (double d in actual)
				Assert.IsTrue((d >= min) && (d < (min + 1.0)));
		}

		/// <summary>
		///A test for Randoms
		///</summary>
		[TestMethod()]
		public void RandomsTestMixed()
		{
			int dimension = 10000;

			double min = -50.0;
			double max = 50.0;

			double?[] mins = new double?[dimension];
			double?[] maxes = new double?[dimension];

			for (int i = 0; i < dimension; i++)
			{
				if (i > 2000 && i < 4000)
				{
					mins[i] = null;
					maxes[i] = max;
				}
				else if (i > 4000 && i < 6000)
				{
					mins[i] = min;
					maxes[i] = null;
				}
				else if (i > 6000 && i < 8000)
				{
					mins[i] = null;
					maxes[i] = null;
				}
				else
				{
					mins[i] = min;
					maxes[i] = max;
				}
			}

			double[] actual = Utilities.Randoms(dimension, mins, maxes);

			for (int i = 0; i < actual.Length; i++)
			{
				if (i > 2000 && i < 4000)
					Assert.IsTrue((actual[i] >= 0.0) && (actual[i] < max));
				else if (i > 4000 && i < 6000)
					Assert.IsTrue((actual[i] >= min) && (actual[i] < 1.0));
				else if (i > 6000 && i < 8000)
					Assert.IsTrue((actual[i] >= 0.0) && (actual[i] < 1.0));
				else
					Assert.IsTrue((actual[i] >= min) && (actual[i] < max));
			}
		}

		[TestMethod()]
		public void IsInRangeTestFalseOutParameters()
		{
			double[] values = new double[] { 51.0, 101.0, 30.0, 20.0, -10.0};

			double?[] min = new double?[] { 50.0, 100.0, 30.0, 0.0, -5.0 };
			double?[] max = new double?[] { 55.0, 100.0, 30.0, 15.0, 20.0};

			bool expected = false;
			int[] expectedIndicesOutOfRange = new int[] { 1, 3, 4 };

			int[] actualIndicesOutOfRange;
			bool actual = Utilities.IsInRange(values, min, max, out actualIndicesOutOfRange);

			Assert.AreEqual(expected, actual);
		}

		[TestMethod()]
		public void IsInRangeTestTrue()
		{
			double[] values = new double[] { 50.0, 100.0, 30.0 };

			double?[] min = new double?[] { 50.0, 100.0, 30.0 };
			double?[] max = new double?[] { 50.0, 100.0, 30.0 };
			
			bool expected = true;

			bool actual = Utilities.IsInRange(values, min, max);

			Assert.AreEqual(expected, actual);
		}
	
		[TestMethod()]
		public void IsInRangeTestFalse()
		{
			double[] values = new double[] { 50.0, 101.0, 30.0 };

			double?[] min = new double?[] { 50.0, 100.0, 30.0 };
			double?[] max = new double?[] { 50.0, 100.0, 30.0 };
			
			bool expected = false;

			bool actual = Utilities.IsInRange(values, min, max);

			Assert.AreEqual(expected, actual);
			
		}

		[TestMethod()]
		public void IsInRangeTestMinNullTrue()
		{
			double[] values = new double[] { -100.0, 100.0, 30.0 };

			double?[] min = null;
			double?[] max = new double?[] { 50.0, 100.0, 30.0 };

			bool expected = true;

			bool actual = Utilities.IsInRange(values, min, max);

			Assert.AreEqual(expected, actual);
		}

		[TestMethod()]
		public void IsInRangeTestMinNullFalse()
		{
			double[] values = new double[] { -100.0, 100.0, 31.0 };

			double?[] min = null;
			double?[] max = new double?[] { 50.0, 100.0, 30.0 };

			bool expected = false;

			bool actual = Utilities.IsInRange(values, min, max);

			Assert.AreEqual(expected, actual);
		}


		[TestMethod()]
		public void IsInRangeTestMaxNullTrue()
		{
			double[] values = new double[] { 50.0, 100.0, 230.0 };

			double?[] min = new double?[] { 50.0, 100.0, 30.0 };
			double?[] max = null;

			bool expected = true;

			bool actual = Utilities.IsInRange(values, min, max);

			Assert.AreEqual(expected, actual);
		}

		[TestMethod()]
		public void IsInRangeTestMaxNullFalse()
		{
			double[] values = new double[] { 49.0, 100.0, 230.0 };

			double?[] min = new double?[] { 50.0, 100.0, 30.0 };
			double?[] max = null;

			bool expected = false;

			bool actual = Utilities.IsInRange(values, min, max);

			Assert.AreEqual(expected, actual);
		}

		/// <summary>
		///A test for NormalRandom
		///</summary>
		[TestMethod()]
		public void NormalRandomTest()
		{
			int num = (int)1e6;
			double[] values = new double[num];

			for (int i = 0; i < num; i++)
				values[i] = Utilities.NormalRandom();

			int numGreaterThanZero = values.Count(x => (x > 0.0));
			int numLessThanZero = values.Count(x => x < 0.0);
			int numThreeSigma = values.Count(x => (x >= -3.0) && (x <= 3.0));
			int numTwoSigma = values.Count(x => (x >= -2.0) && (x <= 2.0));
			int numOneSigma = values.Count(x => (x >= -1.0) && (x <= 1.0));

			Assert.IsTrue(Math.Abs(1.0 - ((double)numGreaterThanZero / (double)numLessThanZero)) < SMALL_VALUE);
			Assert.IsTrue(Math.Abs(0.99 - (numThreeSigma / (double)num)) < SMALL_VALUE);
			Assert.IsTrue(Math.Abs(0.95 - (numTwoSigma / (double)num)) < SMALL_VALUE);
			Assert.IsTrue(Math.Abs(0.68 - (numOneSigma / (double)num)) < SMALL_VALUE);
		}

		/// <summary>
		///A test for Multiply
		///</summary>
		[TestMethod()]
		public void MultiplyTest()
		{
			ColumnVector lhs = new ColumnVector(new [] { 0.0, 1.0, 2.0, 3.0, 4.0 });
			ColumnVector rhs = new ColumnVector(new[] { 5.0, 4.0, 3.0, 2.0, 1.0 });
			ColumnVector expected = new ColumnVector(new[] { 0.0, 4.0, 6.0, 6.0, 4.0 });
			
			ColumnVector actual = Utilities.Multiply(lhs, rhs);

			for (int i = 0; i < lhs.Dimension; i++)
				Assert.AreEqual(expected[i], actual[i]);
		}

		
		[TestMethod()]
		public void ReflectAboutBoundariesMaxNullTest()
		{
			double[] values = new double[] { -100.0, 100.0, 31.0 };

			double?[] min = new double?[] { 50.0, 100.0, 30.0 };
			double?[] max = null;

			double[] expected = new[] { 200.0, 100.0, 31.0 };

			Utilities.ReflectAboutBoundaries(ref values, min, max);

			for (int i = 0; i < expected.Length; i++)
				Assert.AreEqual(expected[i], values[i]);
		}

			
		[TestMethod()]
		public void ReflectAboutBoundariesMinNullTest()
		{
			double[] values = new double[] { -100.0, 100.0, 31.0 };

			double?[] min = null;
			double?[] max = new double?[] { 50.0, 100.0, 30.0 };

			double[] expected = new[] { -100.0, 100.0, 29.0 };

			Utilities.ReflectAboutBoundaries(ref values, min, max);

			for (int i = 0; i < expected.Length; i++)
				Assert.AreEqual(expected[i], values[i]);
		}

		[TestMethod()]
		public void ReflectAboutBoundariesTest()
		{
			double[] values = new double[] { 49.0, 101.0, 30.0 };

			double?[] min = new double?[] { 50.0, 100.0, 30.0 };
			double?[] max = new double?[] { 50.0, 100.0, 30.0 };

			double[] expected = new[] { 51.0, 99.0, 30.0 };

			try
			{
				Utilities.ReflectAboutBoundaries(ref values, min, max);
				Assert.Fail();
			}
			catch(Exception ex)
			{
				Assert.IsInstanceOfType(ex, typeof(ApplicationException));
			}
		}

		[TestMethod()]
		public void ReflectAboutBoundariesReboundTest()
		{
			double[] values = new double[] { 1.0, 101.0, 30.0 };

			double?[] min = new double?[] { 40.0, 99.0, 35.0 };
			double?[] max = new double?[] { 50.0, 102.0, 100.0 };

			double[] expected = new[] { 41.0, 101.0, 40.0 };

			Utilities.ReflectAboutBoundaries(ref values, min, max);

			for (int i = 0; i < expected.Length; i++)
				Assert.AreEqual(expected[i], values[i]);
		}
	}
}
