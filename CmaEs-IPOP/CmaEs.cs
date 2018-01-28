using System;
using System.Collections.Generic;
using System.Linq;
using Meta.Numerics.Matrices;

namespace SteveBagnall.CmaEs_IPOP
{
	public class CmaEs
	{
		private const int MAX_GEN = 1000;
		private const int MAX_POPULATIONSIZE = 5000;
		private const double DEFAULT_MIN_STDDEV = 0.0;
		private const double DEFAULT_INITIAL_STDDEV = 0.3;

		public Func<double[], object, double> ObjectiveFunction { get; set; }

		public double[] InitialParameters { get; set; }

		/// <summary>
		/// Data which gets passed to the object fitness method
		/// </summary>
		public object UserData { get; set; }

		/// <summary>
		/// stop if fitness &lt; stopfitness (minimization)
		/// </summary>
		public double StopFitnessMinimization { get; set; }

		/// <summary>
		/// stop after stopeval number of function evaluations
		/// </summary>
		public int StopEvals { get; set; }

		/// <summary>
		/// coordinate wise standard deviation (sigma | step size)
		/// </summary>
		public double InitialVarianceEstimate { get; set; }

		public double RecombinationRatio { get; set; }

		// restart
		public double ObjectiveFunctionTolerance { get; set; }
		public double StandardDeviationToleranceMultiple { get; set; }
		public double RestartPopulationMultiple { get; set; }
		public int MaxRestarts { get; set; }

		private CMAParams _initialCmaParams;
		private Solution _solution;

		/// <summary>
		/// http://eodev.sourceforge.net/eo/doc/html/_c_m_a_state_8cpp_source.html
		/// </summary>
		public CmaEs(
			Func<double[], object, double> fitnessMinimizationFunction,
			double[] initialParameters, 
			object userData = null,
			double[] minimumStandardDeviations = null,
			double[] initialStandardDeviations = null,
			double stopFitnessMinimization = 1e-10,
			int additionalPopulationSize = 0,
			int maxRestarts = 100,
			double stopEvalsMultiple = 50.0,
			double restartPopulationMultiple = 2.0,
			double initialVarianceEstimates = 0.5,
			double recombinationRatio = 0.5,
			double objectiveFunctionTolerance = 1e-12,
			double standardDeviationToleranceMultiple = 1e-12)
		{
			int dimensions = initialParameters.Length;

			if (recombinationRatio <= 0)
				recombinationRatio = 0.5;

			if (minimumStandardDeviations == null)
			{
				minimumStandardDeviations = new double[dimensions];
				for (int i = 0; i < dimensions; i++)
					minimumStandardDeviations[i] = DEFAULT_MIN_STDDEV;
			}

			if (initialStandardDeviations == null)
			{
				initialStandardDeviations = new double[dimensions];
				for (int i = 0; i < dimensions; i++)
					initialStandardDeviations[i] = DEFAULT_INITIAL_STDDEV;
			}

			int lambda = 4 + additionalPopulationSize + (int)(3.0 * Math.Log((double)dimensions));

			if (lambda > MAX_POPULATIONSIZE)
				lambda = MAX_POPULATIONSIZE;

			int mu = (int)Math.Floor(lambda * recombinationRatio);

			this.StopEvals = 100 + (int)(stopEvalsMultiple * Math.Pow(dimensions + 3, 2) / Math.Sqrt(lambda));
			this.ObjectiveFunction = fitnessMinimizationFunction;
			this.InitialParameters = initialParameters;
			this.UserData = userData;
			this.StopFitnessMinimization = stopFitnessMinimization;
			this.MaxRestarts = maxRestarts;
			this.InitialVarianceEstimate = initialVarianceEstimates;
			this.RecombinationRatio = recombinationRatio;
			this.RestartPopulationMultiple = restartPopulationMultiple;
			this.ObjectiveFunctionTolerance = objectiveFunctionTolerance;
			this.StandardDeviationToleranceMultiple = standardDeviationToleranceMultiple;

			_initialCmaParams = new CMAParams(
				dimensions,
				MAX_GEN,
				lambda,
				mu,
				minimumStandardDeviations,
				initialStandardDeviations,
				CMAParams.WeightType.Logarithmic);
		}

		public Solution Solve(bool isContinueFromLastSolution = false, bool isOneGenerationOnly = false)
		{
			Solution solution;
			CMAState cmaState;

			if ((!(isContinueFromLastSolution)) || (_solution.Equals(default(Solution))))
			{
				solution = new Solution() { StopReason = StopReason.NotSet };
				cmaState = new CMAState(_initialCmaParams, new ColumnVector(this.InitialParameters), this.InitialVarianceEstimate);
			}
			else
			{
				solution = _solution;
				solution.StopReason = StopReason.Restart;
				cmaState = _solution.LatestCMAState;
			}
			
			int iteration = 0;
			while ((solution.StopReason == StopReason.NotSet) || (solution.StopReason == StopReason.Restart))
			{
				Solve(iteration++, ref cmaState, ref solution, isOneGenerationOnly);
			}

			_solution = solution;

			return solution;
		}

		private void Solve(int iteration, ref CMAState cmaState, ref Solution solution, bool isOnce = false)
		{
			//ColumnVector xmean = new ColumnVector(parameters);

			double tolX = this.StandardDeviationToleranceMultiple * this.InitialVarianceEstimate;

			int stopeval = this.StopEvals;

			RectangularMatrix arx = new RectangularMatrix(cmaState.p.N, cmaState.p.Lambda);
			ColumnVector arfitness = new ColumnVector(cmaState.p.Lambda);
			
			bool isRestart = false;
			bool isGoodEnough = false;
			int counteval = 0;

			while (counteval < stopeval)
			{
				// Generate and evaluate lambda offspring
				for (int k = 0; k < cmaState.p.Lambda; k++)
				{
					double[] sample = cmaState.Sample(arx.Column(k));
					for (int r = 0; r < cmaState.p.N; r++)
						arx[r, k] = sample[r];

					arfitness[k] = this.ObjectiveFunction(arx.Column(k).ToArray(), this.UserData);

					counteval++;
				}

				// Sort by fitness and compute weighted mean into xmean
				// minimization
				int[] arindex;
				arfitness = Utilities.Sort(arfitness, out arindex); // one based index

				int mu = cmaState.p.Mu;
				cmaState.ReEstimate(
					(RectangularMatrix)Utilities.GetColumns(arx, arindex.Take(cmaState.p.Mu)), 
					arfitness.First(),
					arfitness.SkipWhile((f, i) => i < (mu - 1)).First());

				cmaState.UpdateEigenSystem(1, 0);


				if ((solution.StopReason == StopReason.NotSet) || (arfitness[0] < solution.MinimizedFitness))
				{
					solution.StopReason = StopReason.Incomplete;
					solution.MinimizedFitness = arfitness[0];
					solution.BestCMAState = cmaState;
					solution.BestNumEvals = counteval;
					solution.BestNumRestarts = iteration;
					solution.BestParameters = cmaState.mean.ToArray();
				}

				if (arfitness[0] <= this.StopFitnessMinimization)
				{
					isGoodEnough = true;
					break;
				}

				if (IsRestart(ref solution, arfitness, tolX, cmaState))
				{
					isRestart = true;
					break;
				}

				if (isOnce)
					break;
			}

			if ((isRestart) && (iteration > this.MaxRestarts))
			{
				solution.StopReason = StopReason.MaxRestarts;
			}
			else if ((isRestart) && ((cmaState.p.Lambda * this.RestartPopulationMultiple) > MAX_POPULATIONSIZE))
			{
				solution.StopReason = StopReason.MaxPopulation;
			}
			else if (isRestart)
			{
				solution.History = new Queue<double>();
				solution.StopReason = StopReason.Restart;
				int previousLambda = cmaState.p.Lambda;
				cmaState = new CMAState(_initialCmaParams, new ColumnVector(this.InitialParameters), this.InitialVarianceEstimate);
				cmaState.p.UpdateLambda((int)(previousLambda * this.RestartPopulationMultiple));
			}
			else if (isGoodEnough)
			{
				solution.StopReason = StopReason.GoodEnough;
			}
			else
			{
				solution.StopReason = StopReason.MaxIterations;
			}

			solution.LatestCMAState = cmaState;
			solution.LatestNumEvals = counteval;
			solution.LatestNumRestarts = iteration;
			solution.LatestParameters = cmaState.mean.ToArray();
		}

		private bool IsRestart(ref Solution solution, ColumnVector fitnesses, double tolX, CMAState cmaState)
		{
			bool isAllNaN = fitnesses.All(x => double.IsNaN(x) || double.IsInfinity(x));

			if (isAllNaN)
				return true;

			// Stop if the condition number of the covariance matrix
			//exceeds 1014 (conditioncov).
			bool isCConditionNumberTooHigh = false;
			if ((Utilities.Diag(cmaState.C, x => x).ToArray().Max() / Utilities.Diag(cmaState.C, x => x).ToArray().Min()) > 1e14)
				isCConditionNumberTooHigh = true;

			if (isCConditionNumberTooHigh)
				return true;

			if (solution.History == null)
				solution.History = new Queue<double>();

			//Stop if the range of the best objective function values
			//of the last 10 + [30n/lambda] generations is zero
			//(equalfunvalhist), or the range of these function
			//values and all function values of the recent generation
			//is below Tolfun= 10^-12.
			solution.History.Enqueue(fitnesses[0]);
			if (solution.History.Count > (10.0 + ((30.0 * cmaState.p.N) / cmaState.p.Lambda)))
				solution.History.Dequeue();

			bool isObjectiveFunctionRangeTooLow = (solution.History.Count == (int)(10.0 + ((30.0 * cmaState.p.N) / cmaState.p.Lambda)))
				&& ((solution.History.Max() == solution.History.Min())
				|| (((solution.History.Max() - solution.History.Min()) < ObjectiveFunctionTolerance) && ((fitnesses.Max() - fitnesses.Min()) < ObjectiveFunctionTolerance)));

			if (isObjectiveFunctionRangeTooLow)
				return true;


			// Stop if the standard deviation of the normal distribution
			//is smaller than TolX in all coordinates and [sigma]pc
			//(the evolution path from Eq. 2 in [3]) is smaller than
			//TolX in all components. We set TolX= 10^-12*[sigma]^(0).
			bool isStandardDeviationTooSmall = (Utilities.IsTrueForAll(cmaState.C, x => Math.Abs(x) < tolX) && cmaState.ps.All(x => Math.Abs(x) < tolX));

			if (isStandardDeviationTooSmall)
				return true;

			// Stop if adding a 0.1-standard deviation vector in
			//a principal axis direction of C^(g) does not change
			//<x[vector]>w^(g) (noeffectaxis)3
			int ith = (cmaState.gen % cmaState.p.N);
			ColumnVector tmpXmean = (cmaState.mean + 0.1 * cmaState.sigma * cmaState.B * new ColumnVector(cmaState.d));

			bool isNoEffectAxis = false;
			if (ith < tmpXmean.Dimension)
				isNoEffectAxis = (tmpXmean[ith] == cmaState.mean[ith]);

			if (isNoEffectAxis)
				return true;

			// Stop if adding 0.2-standard deviation in each coordinate
			//does change <x[vector]>w^(g) (noeffectcoord).
			SquareMatrix testC = 0.2 * cmaState.sigma * cmaState.C;
			SquareMatrix vectors;
			SquareMatrix values;
			Utilities.EigAlgLib(testC, out vectors, out values, 1, isGetLower: false);
			ColumnVector colValues = Utilities.Diag(values, x => Math.Sqrt(Math.Abs(x)));
			tmpXmean = (cmaState.mean + cmaState.sigma * vectors * colValues);
			bool isNoEffectCoord = true;
			for (int i = 0; i < cmaState.mean.Dimension; i++)
			{
				if (cmaState.mean[i] != tmpXmean[i])
				{
					isNoEffectCoord = false;
					break;
				}
			}

			if (isNoEffectCoord)
				return true;

			return false;
		}
	}
}
