using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Meta.Numerics.Matrices;

namespace SteveBagnall.CmaEs_IPOP
{
	public struct CMAParams
	{
		public enum WeightType
		{
			NotSet = 0,
			Logarithmic,
			Equal,
			Linear,
		}

		private WeightType _weightType;

		public int N; 
		public int MaxGen;
		public int Lambda;
		public int Mu;
		public double[] Weights;
		public double MuEff;
		public double MuCov;
		public double Damp;
		public double CCumSig;
		public double CCumCov;
		public double CCov;
		public double[] MinStdDevs;
		public double[] InitialStdDevs;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="dimensionality">Dimensionality (N) of the problem</param>
		/// <param name="maxgen">Maximum number of generations that the system will run (needed for damping)</param>
		/// <param name="lambda">Number of offspring</param>
		/// <param name="mu">PopulationSize</param>
		/// <param name="weightType">Weighting scheme (for 'selection'): logarithmic, equal, linear</param>
		/// <param name="minSDs">minimum stdevs, last one will apply for all remaining axes</param>
		/// <param name="initialSDs">initial stdevs, last one will apply for all remaining axes</param>
		public CMAParams(
			int dimensionality,
			int maxgen,
			int lambda,
			int mu,
			double[] minSDs,
			double[] initialSDs,
			WeightType weightType = WeightType.Logarithmic)
		{
			N = dimensionality;

			MaxGen = maxgen;

			if (lambda < 2)
				this.Lambda = 4 + (int)(3.0 * Math.Log((double)N));
			else
				this.Lambda = (int)lambda;

			_weightType = weightType;

			if ((mu >= Lambda) || (mu < 1))
				Mu = (int)Math.Floor(Lambda / 2.0);
			else
				Mu = (int)mu;

			Weights = new double[Mu];

			switch (weightType)
			{
				case WeightType.Equal:
					for (int i = 0; i < Weights.Length; ++i)
						Weights[i] = Mu - i;

					break;

				case WeightType.Linear:
					Weights = Weights.Select(x => 1.0).ToArray();

					break;

				case WeightType.Logarithmic:
				case WeightType.NotSet:
				default:

					for (int i = 0; i < Weights.Length; ++i)
						Weights[i] = Math.Log(Mu + 1.0) - Math.Log(i + 1.0);

					break;
			}


			/* Normalize weights and set mu_eff */
			double sumw = Weights.Sum();
			MuEff = sumw * sumw / Weights.Sum(x => x * x);
			Weights = Weights.Select(x => x / sumw).ToArray();

			/* set the others using Nikolaus logic. If you want to tweak, you can parameterize over these defaults */
			MuCov = MuEff;
			CCumSig = (MuEff + 2.0) / (N + MuEff + 3.0);
			CCumCov = 4.0 / (N + 4);

			double t1 = 2.0 / ((N + 1.4142) * (N + 1.4142));
			double t2 = (2.0 * MuCov - 1.0) / ((N + 2.0) * (N + 2.0) + MuCov);
			t2 = (t2 > 1) ? 1 : t2;
			t2 = (1.0 / MuCov) * t1 + (1.0 - 1.0 / MuCov) * t2;

			CCov = t2;

			Damp = 1 + Math.Max(0.3, (1.0 - (double)N / (double)maxgen))
				* (1.0 + 2.0 * Math.Max(0.0, Math.Sqrt((MuEff - 1.0) / (N + 1.0)) - 1.0)) /* limit sigma increase */
				/ CCumSig;

			if (minSDs == null)
			{
				minSDs = new double[N];
				for (int i = 0; i < N; i++)
					minSDs[i] = 0.0;
			}

			if (initialSDs == null)
			{
				initialSDs = new double[N];
				for (int i = 0; i < N; i++)
					initialSDs[i] = 0.3;
			}

			MinStdDevs = minSDs;
			InitialStdDevs = initialSDs;
		}

		public void UpdateLambda(int value)
		{
			Lambda = value;
			Mu = (int)Math.Floor(Lambda / 2.0);

			Weights = new double[Mu];

			switch (_weightType)
			{
				case WeightType.Equal:
					for (int i = 0; i < Weights.Length; ++i)
						Weights[i] = Mu - i;

					break;

				case WeightType.Linear:
					Weights = Weights.Select(x => 1.0).ToArray();

					break;

				case WeightType.Logarithmic:
				case WeightType.NotSet:
				default:

					for (int i = 0; i < Weights.Length; ++i)
						Weights[i] = Math.Log(Mu + 1.0) - Math.Log(i + 1.0);

					break;
			}


			/* Normalize weights and set mu_eff */
			double sumw = Weights.Sum();
			MuEff = sumw * sumw / Weights.Sum(x => x * x);
			Weights = Weights.Select(x => x / sumw).ToArray();

			/* set the others using Nikolaus logic. If you want to tweak, you can parameterize over these defaults */
			MuCov = MuEff;
			CCumSig = (MuEff + 2.0) / (N + MuEff + 3.0);
			CCumCov = 4.0 / (N + 4);

			double t1 = 2.0 / ((N + 1.4142) * (N + 1.4142));
			double t2 = (2.0 * MuCov - 1.0) / ((N + 2.0) * (N + 2.0) + MuCov);
			t2 = (t2 > 1) ? 1 : t2;
			t2 = (1.0 / MuCov) * t1 + (1.0 - 1.0 / MuCov) * t2;

			CCov = t2;

			Damp = 1 + Math.Max(0.3, (1.0 - (double)N / (double)MaxGen))
				* (1.0 + 2.0 * Math.Max(0.0, Math.Sqrt((MuEff - 1.0) / (N + 1.0)) - 1.0)) /* limit sigma increase */
				/ CCumSig;
		}
	}
}
