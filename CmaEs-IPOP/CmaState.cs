using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Meta.Numerics.Matrices;

namespace SteveBagnall.CmaEs_IPOP
{
	public class CMAState
	{
		public CMAParams p;

		public SquareMatrix C; // Covariance matrix [lower triangular matrix]
		public SquareMatrix B; // Eigen vectors (in columns)
		public double[] d; // eigen values (diagonal matrix) [valarray]
		public double[] pc; // Evolution path [valarray<double>]
		public double[] ps; // Evolution path for stepsize; [valarray<double>]

		public ColumnVector mean; // current mean to sample around [vector<double>]
		public double sigma; // global step size

		public int gen;
		public ColumnVector fitnessHistory; // [vector<double>]

		/// <summary>
		/// 
		/// </summary>
		/// <param name="params_"></param>
		/// <param name="m"></param>
		/// <param name="sigma_"></param>
		public CMAState(CMAParams params_, ColumnVector m, double sigma_)
		{
			p = params_;
			C = new SquareMatrix(p.N);
			B = new SquareMatrix(p.N);
			d = new double[p.N];
			pc = new double[p.N];
			ps = new double[p.N];
			mean = m;
			sigma = sigma_;

			gen = 0;
			fitnessHistory = new ColumnVector(3);

			double trace = p.InitialStdDevs.Sum(x => x * x);

			/* Initialize covariance structure */

			for (int i = 0; i < p.N; ++i)
			{
				B[i, i] = 1.0;
				d[i] = p.InitialStdDevs[i] * Math.Sqrt(p.N / trace);
				C[i, i] = d[i] * d[i];
				pc[i] = 0.0;
				ps[i] = 0.0;
			}

		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="v"></param>
		public double[] Sample(ColumnVector v)
		{
			int n = p.N;
			double[] array = new double[n];

			ColumnVector tmp = new ColumnVector(n); // vector<double>
			for (int i = 0; i < n; ++i)
				tmp[i] = d[i] * Utilities.NormalRandom();

			/* add mutation (sigma * B * (D*z)) */
			for (int i = 0; i < n; ++i)
			{
				double sum = 0;

				for (int j = 0; j < n; ++j)
				{
					sum += B[i, j] * tmp[j];
				}

				array[i] = mean[i] + sigma * sum;
			}

			return array;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="pop"></param>
		/// <param name="muBest"></param>
		/// <param name="muWorst"></param>
		public void ReEstimate(RectangularMatrix pop, double muBest, double muWorst)
		{
			if (pop.ColumnCount != p.Mu)
				throw new ApplicationException("");

			int n = p.N;

			fitnessHistory[gen % fitnessHistory.Count()] = muBest; // needed for divergence check

			ColumnVector oldmean = new ColumnVector(mean.Dimension);
			for (int i = 0; i < mean.Dimension; i++)
				oldmean[i] = mean [i];

			double[] BDz = new double[n]; // valarray<double>

			/* calculate xmean and rgBDz~N(0,C) */
			for (int i = 0; i < n; ++i)
			{
				mean[i] = 0.0;

				for (int j = 0; j < pop.ColumnCount; ++j)
				{
					mean[i] += p.Weights[j] * (pop.Column(j))[i];
				}

				BDz[i] = Math.Sqrt(p.MuEff) * (mean[i] - oldmean[i]) / sigma;
			}

			ColumnVector tmp = new ColumnVector(n); // [vector<double>]

			/* calculate z := D^(-1) * B^(-1) * rgBDz into rgdTmp */
			for (int i = 0; i < n; ++i)
			{
				double sum = 0.0;

				for (int j = 0; j < n; ++j)
				{
					sum += B[j, i] * BDz[j];
				}

				tmp[i] = sum / d[i];
			}

			/* cumulation for sigma (ps) using B*z */
			for (int i = 0; i < n; ++i)
			{
				double sum = 0.0;

				for (int j = 0; j < n; ++j)
					sum += B[i, j] * tmp[j];

				ps[i] = (1.0 - p.CCumSig) * ps[i] + Math.Sqrt(p.CCumSig * (2.0 - p.CCumSig)) * sum;
			}

			/* calculate norm(ps)^2 */
			double psxps = ps.Sum(x => x * x);

			double chiN = Math.Sqrt((double)p.N) * (1.0 - 1.0 / (4.0 * p.N) + 1.0 / (21.0 * p.N * p.N));

			/* cumulation for covariance matrix (pc) using B*D*z~N(0,C) */
			bool isHsig = Math.Sqrt(psxps) / Math.Sqrt(1.0 - Math.Pow(1.0 - p.CCumSig, 2.0 * gen)) / chiN < 1.5 + 1.0 / (p.N - 0.5);
			double hsig = (isHsig) ? 1.0 : 0.0;

			//pc = (1.0 - p.ccumcov) * pc + hsig * Math.Sqrt(p.ccumcov * (2.0 - p.ccumcov)) * BDz;
			pc = pc.Select(x => x * (1.0 - p.CCumCov)).Zip(BDz.Select(x => x * Math.Sqrt(p.CCumCov * (2.0 - p.CCumCov)) * hsig), (lhs, rhs) => lhs + rhs).ToArray();

			/* stop initial phase (MK, this was not reachable in the org code, deleted) */
			/* remove momentum in ps, if ps is large and fitness is getting worse */

			if (gen >= fitnessHistory.Count())
			{
				// find direction from muBest and muWorst (muBest == muWorst handled seperately
				double direction = (muBest < muWorst) ? -1.0 : 1.0;

				int now = gen % fitnessHistory.Count();
				int prev = (gen - 1) % fitnessHistory.Count();
				int prevprev = (gen - 2) % fitnessHistory.Count();

				bool fitnessWorsens = (muBest == muWorst) || // <- increase norm also when population has converged (this deviates from Hansen's scheme)
					((direction * fitnessHistory[now] < direction * fitnessHistory[prev])
					&&
					(direction * fitnessHistory[now] < direction * fitnessHistory[prevprev]));

				if (psxps / p.N > 1.5 + 10.0 * Math.Sqrt(2.0 / p.N) && fitnessWorsens)
				{
					double tfac = Math.Sqrt((1 + Math.Max(0.0, Math.Log(psxps / p.N))) * p.N / psxps);
					ps = ps.Select(x => x * tfac).ToArray();
					psxps *= tfac * tfac;
				}
			}

			/* update of C  */
			/* Adapt_C(t); not used anymore */
			if (p.CCov != 0.0)
			{
				//flgEigensysIsUptodate = 0;

				/* update covariance matrix */
				for (int i = 0; i < n; ++i)
				{
					for (int j = 0; j <= i; ++j)
					{
						C[i,j] =
							(1 - p.CCov) * C[i,j]
							+
							p.CCov * (1.0 / p.MuCov) * pc[i] * pc[j]
							+
							(1 - hsig) * p.CCumCov * (2.0 - p.CCumCov) * C[i,j];

						/*C[i][j] = (1 - p.ccov) * C[i][j]
						+ sp.ccov * (1./sp.mucov)
						* (rgpc[i] * rgpc[j]
						+ (1-hsig)*sp.ccumcov*(2.-sp.ccumcov) * C[i][j]); */
						for (int k = 0; k < p.Mu; ++k)
						{
							/* additional rank mu update */
							C[i,j] += p.CCov * (1 - 1.0 / p.MuCov) * p.Weights[k]
								* ((pop.Column(k))[i] - oldmean[i])
								* ((pop.Column(k))[j] - oldmean[j])
								/ sigma / sigma;

							// * (rgrgx[index[k]][i] - rgxold[i])
							// * (rgrgx[index[k]][j] - rgxold[j])
							// / sigma / sigma;
						}
					}
				}
			}

			/* update of sigma */
			sigma *= Math.Exp(((Math.Sqrt(psxps) / chiN) - 1.0) / p.Damp);
			/* calculate eigensystem, must be done by caller  */
			//cmaes_UpdateEigensystem(0);


			/* treat minimal standard deviations and numeric problems
			* Note that in contrast with the original code, some numerical issues are treated *before* we
			* go into the eigenvalue calculation */

			treatNumericalIssues(muBest, muWorst);

			gen++; // increase generation
		}

		private void treatNumericalIssues(double best, double worst)
		{
			/* treat stdevs */
			for (int i = 0; i < p.N; ++i)
			{
				if (sigma * Math.Sqrt(C[i, i]) < p.MinStdDevs[i])
				{
					// increase stdev
					sigma *= Math.Exp(0.05 + 1.0 / p.Damp);
					break;
				}
			}

			/* treat convergence */
			if (best == worst)
			{
				sigma *= Math.Exp(0.2 + 1.0 / p.Damp);
			}

			/* Jede Hauptachse i testen, ob x == x + 0.1 * sigma * rgD[i] * B[i] */
			/* Test if all the means are not numerically out of whack with our coordinate system*/
			for (int axis = 0; axis < p.N; ++axis)
			{
				double fac = 0.1 * sigma * d[axis];
				int coord;
				for (coord = 0; coord < p.N; ++coord)
				{
					if (mean[coord] != mean[coord] + fac * B[coord, axis])
					{
						break;
					}
				}

				if (coord == p.N)
				{
					// means are way too big (little) for numerical accuraccy. Start rocking the craddle a bit more
					sigma *= Math.Exp(0.2 + 1.0 / p.Damp);
				}
			}

			/* Testen ob eine Komponente des Objektparameters festhaengt */
			/* Correct issues with scale between objective values and covariances */
			bool theresAnIssue = false;

			for (int i = 0; i < p.N; ++i)
			{
				if (mean[i] == mean[i] + 0.2 * sigma * Math.Sqrt(C[i, i]))
				{
					C[i, i] *= (1.0 + p.CCov);
					theresAnIssue = true;
				}
			}

			if (theresAnIssue)
			{
				sigma *= Math.Exp(0.05 + 1.0 / p.Damp);
			}
		}

		static double lastGoodMinimumEigenValue = 1.0;
		public bool UpdateEigenSystem(int max_tries, int max_iters)
		{
			if (max_iters == 0)
				max_iters = 30 * p.N;

			/* Try to get a valid calculation */
			for (int tries = 0; tries < max_tries; ++tries)
			{
				int iters = Utilities.EigTriangle(C, out B, out d, max_iters, isUpper: false);
				//int iters = eig( p.n, C, d, B, max_iters);
				if (iters < max_iters)
				{
					// all is well

					/* find largest/smallest eigenvalues */
					double minEV = d.Min();
					double maxEV = d.Max();

					/* (MK Original comment was) :Limit Condition of C to dMaxSignifKond+1
					* replaced dMaxSignifKond with 1./numeric_limits<double>::epsilon()
					* */

					if (maxEV * double.Epsilon > minEV)
					{
						double tmp = maxEV * double.Epsilon - minEV;
						minEV += tmp;

						for (int i = 0; i < p.N; ++i)
						{
							C[i, i] += tmp;
							d[i] += tmp;
						}
					} /* if */

					lastGoodMinimumEigenValue = minEV;

					d = d.Select(x => Math.Sqrt(x)).ToArray();

					//flgEigensysIsUptodate = 1;
					//genOfEigensysUpdate = gen;
					//clockeigensum += clock() - clockeigenbegin;
					return true;

				} /* if cIterEig < ... */

				// numerical problems, ignore them and try again

				/* Addition des letzten minEW auf die Diagonale von C */
				/* Add the last known good eigen value to the diagonal of C */
				double summand = lastGoodMinimumEigenValue * Math.Exp((double)tries);

				for (int i = 0; i < p.N; ++i)
					C[i, i] += summand;

			} /* for iEigenCalcVers */

			return false;

		}
	}
}
