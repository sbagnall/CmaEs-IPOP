using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Meta.Numerics.Matrices;
using Meta.Numerics;
using alglib;
using Meta.Numerics.Statistics.Distributions;

namespace SteveBagnall.CmaEs_IPOP
{
	public static class Utilities
	{
		private static NormalDistribution _normal = new NormalDistribution(0.0, 1.0);
		private static Random _random = new Random();

		public static double[] Resize(double[] array, int newSize)
		{
			double[] newArray = new double[(array.Length > newSize) ? array.Length : newSize];

			for (int i = 0; i < newSize; i++)
			{
				if (i < array.Length)
					newArray[i] = array[i];
				else
					break;
			}

			return newArray;
		}

		public static ColumnVector Resize(ColumnVector vector, int newSize)
		{
			ColumnVector newVector = new ColumnVector((vector.Dimension > newSize) ? vector.Dimension : newSize);

			for (int i = 0; i < newSize; i++)
			{
				if (i < vector.Dimension)
					newVector[i] = vector[i];
				else
					break;
			}

			return newVector;
		}

		public static bool IsTrueForAll(SquareMatrix matrix, Func<double, bool> predicate)
		{
			for (int r = 0; r < matrix.RowCount; r++)
				for (int c = 0; c < matrix.ColumnCount; c++)
					if (!predicate(matrix[r, c]))
						return false;
			return true;
		}

		public static bool IsTrueForAll(ColumnVector vector, Func<double, bool> predicate)
		{
			for (int i = 0; i < vector.Dimension; i++)
				if (!predicate(vector[i]))
					return false;
			return true;
		}

		/// <summary>
		/// element by element multiplication
		/// </summary>
		/// <param name="lhs"></param>
		/// <param name="rhs"></param>
		public static ColumnVector Multiply(ColumnVector lhs, ColumnVector rhs)
		{
			ColumnVector result = new ColumnVector(lhs.Dimension);

			for (int i = 0; i < lhs.Dimension; i++)
				result[i] = lhs[i] * rhs[i];

			return result;
		}

		public static void ReflectAboutBoundaries(ref double[] vector, double?[] min, double?[] max)
		{
			if ((min == null) && (max == null))
				return;

			bool isOK;

			do
			{
				isOK = true;

				for (int i = 0; i < vector.Length; i++)
				{
					if (max != null && min != null && min[i] == max[i])
						throw new ApplicationException("min cannot equal max");

					if ((min != null) && (min[i] != null) && (vector[i] < min[i]))
					{
						vector[i] = (double)min[i] + ((double)min[i] - vector[i]);
						isOK = false;
					}
					else if ((max != null) && (max[i] != null) && (vector[i] > max[i]))
					{
						vector[i] = (double)max[i] - (vector[i] - (double)max[i]);
						isOK = false;
					}
				}
			} while (!isOK);
		}

		public static bool IsInRange(double[] values, double?[] min, double?[] max)
		{
			int[] indicesOutOfRange;
			return IsInRange(values, min, max, out indicesOutOfRange);
		}

		public static bool IsInRange(double[] values, double?[] min, double?[] max, out int[] indicesOutOfRange)
		{
			if ((min == null) && (max == null))
			{
				indicesOutOfRange = new int[] { };
				return true;
			}

			bool isInRange = true;
			var outOf = new List<int>();
			
			for (int i = 0; i < values.Length; i++)
			{
				if (((min != null) && (min[i] != null) && (values[i] < min[i]))
					|| ((max != null) && (max[i] != null) && (values[i] > max[i])))
				{
					outOf.Add(i);
					isInRange = false;
				}
			}

			indicesOutOfRange = outOf.ToArray();
			return isInRange;
		}


		public static double NormalRandom()
		{
			return _normal.GetRandomValue(_random);
		}
			
		public static double[] Randoms(int dimension, double?[] min = null, double?[] max = null)
		{
			double[] randoms = new double[dimension];

			for (int i = 0; i < dimension; i++)
			{
				double r = _random.NextDouble();

				if ((min == null) && (max == null))
				{
					randoms[i] = r;
				}
				else
				{
					double minimum = 0.0;
					double maximum = 1.0;

					if (min == null)
					{
						maximum = max[i] ?? 1.0;
						minimum = maximum - 1.0;
					}
					else if (max == null)
					{
						minimum = min[i] ?? 0.0;
						maximum = minimum + 1.0;
					}
					else
					{
						minimum = min[i] ?? 0.0;
						maximum = max[i] ?? 1.0;
					}

					randoms[i] = minimum + (r * (maximum - minimum));
				}
			}

			return randoms;
		}

		public static bool IsSymmetrical(SquareMatrix matrix)
		{
			for (int r = 0; r < matrix.Dimension; r++)
				for (int c = 0; c < matrix.Dimension; c++)
					if (matrix[r, c] != matrix[c, r])
						return false;

			return true;
		}

		/// <summary>
		/// Uses AlgLib - seems to work with large matrices
		/// Eig(matrix, out eigenVectors, out eigenValues) produces matrices of eigenvalues and eigenvectors of matrix, 
		/// so that matrix*eigenVectors = eigenVectors*eigenValues. 
		/// Matrix eigenValues is the canonical form of matrix — a diagonal matrix with matrix's eigenvalues on the main diagonal. 
		/// Matrix eigenVectors is the modal matrix — its columns are the eigenvectors of matrix.
		/// </summary>
		/// <param name="matrix"></param>
		/// <param name="eigenVectors"></param>
		/// <param name="eigenValues"></param>
		public static int EigAlgLib(SquareMatrix matrix, out SquareMatrix eigenVectors, out SquareMatrix eigenValues, int maxIterations, bool isGetLower)
		{
			return EigTriangle(GetTriangle(matrix, isGetLower: isGetLower), out eigenVectors, out eigenValues, maxIterations, isUpper: !isGetLower);
		}

		public static int EigAlgLib(SquareMatrix matrix, out SquareMatrix eigenVectors, out double[] eigenValues, int maxIterations, bool isGetLower)
		{
			SquareMatrix eigenValuesSquare;
			int iters = EigTriangle(GetTriangle(matrix, isGetLower: isGetLower), out eigenVectors, out eigenValuesSquare, maxIterations, !isGetLower);

			eigenValues = new double[eigenValuesSquare.Dimension];
			for (int i = 0; i < eigenValuesSquare.Dimension; i++)
				eigenValues[i] = eigenValuesSquare[i, i];

			return iters;
		}

		public static int EigTriangle(SquareMatrix triangle, out SquareMatrix eigenVectors, out double[] eigenValues, int maxIterations, bool isUpper)
		{
			SquareMatrix eigenValuesSquare;
			int iters = EigTriangle(triangle, out eigenVectors, out eigenValuesSquare, maxIterations, isUpper);

			eigenValues = new double[eigenValuesSquare.Dimension];
			for (int i = 0; i < eigenValuesSquare.Dimension; i++)
				eigenValues[i] = eigenValuesSquare[i, i];

			return iters;
		}

		public static int EigTriangle(SquareMatrix triangle, out SquareMatrix eigenVectors, out SquareMatrix eigenValues, int maxIterations, bool isUpper)
		{
			int dimension = triangle.Dimension;

			eigenVectors = new SquareMatrix(dimension);
			eigenValues = new SquareMatrix(dimension);

			double[,] a = new double[dimension, dimension];
			for (int r = 0; r < dimension; r++)
				for (int c = 0; c < dimension; c++)
					a[r, c] = triangle[r, c];

			double[] eVal = new double[dimension];
			double[,] eVec = new double[dimension, dimension];

			for (int iterations = 1; iterations <= maxIterations; iterations++)
			{
				if (evd.smatrixevd(a, dimension, 1, isUpper, ref eVal, ref eVec))
				{
					for (int r = 0; r < dimension; r++)
					{
						for (int c = 0; c < dimension; c++)
							eigenVectors[r, c] = (double.IsNaN(eVec[r, c])) ? 0.0 : eVec[r, c];

						eigenValues[r, r] = (double.IsNaN(eVal[r])) ? 0.0 : eVal[r];
					}

					return iterations;
				}
			}

			return maxIterations;
		}

		/// <summary>
		/// Uses Meta.Numerics - fails with large matrices
		/// Eig(matrix, out eigenVectors, out eigenValues) produces matrices of eigenvalues and eigenvectors of matrix, 
		/// so that matrix*eigenVectors = eigenVectors*eigenValues. 
		/// Matrix eigenValues is the canonical form of matrix — a diagonal matrix with matrix's eigenvalues on the main diagonal. 
		/// Matrix eigenVectors is the modal matrix — its columns are the eigenvectors of matrix.
		/// </summary>
		/// <param name="matrix"></param>
		/// <param name="eigenVectors"></param>
		/// <param name="eigenValues"></param>
		public static void EigMetaNumerics(SquareMatrix matrix, out SquareMatrix eigenVectors, out SquareMatrix eigenValues)
		{
			if (!(Utilities.IsSymmetrical(matrix)))
				throw new ApplicationException("Matrix must be symmetrical.");

			var system = matrix.Eigensystem();

			eigenVectors = new SquareMatrix(system.Dimension);
			eigenValues = new SquareMatrix(system.Dimension);

			for (int i = 0; i < system.Dimension; i++)
			{
				int r = 0;
				foreach (Complex vector in system.Eigenvector(i))
				{
					eigenVectors[r, i] = (double.IsNaN(vector.Re)) ? 0.0 : vector.Re;
					r++;
				}

				double eValue = system.Eigenvalue(i).Re;
				eigenValues[i, i] = (double.IsNaN(eValue)) ? 0.0 : eValue;
			}
		}


		public static ColumnVector Sort(ColumnVector vector, out int[] originalIndices)
		{
			ColumnVector sorted = new ColumnVector(vector.Dimension);
			originalIndices = new int[vector.Dimension];

			Dictionary<int, double> ordered = new Dictionary<int, double>();

			for (int i = 0; i < vector.Dimension; i++)
				ordered.Add(i, vector[i]);
			
			int index = 0;
			foreach (KeyValuePair<int, double> pair in ordered.OrderBy(x => x.Value))
			{
				originalIndices[index] = pair.Key;
				sorted[index] = pair.Value;
				index++;
			}

			return sorted;
		}

		public static ColumnVector ElementByElementOperation(ColumnVector vector, Func<double, int, double> operation)
		{
			ColumnVector newVector = new ColumnVector(vector.Dimension);

			for (int i = 0; i < vector.Dimension; i++)
			{
				double value = vector[i];
				if (operation != null)
					value = operation(value, i);

				newVector[i] = (double.IsNaN(value)) ? 0.0 : value;
			}

			return newVector;
		}

		public static SquareMatrix ConvertToSquare(AnyRectangularMatrix matrix, int dimension)
		{
			if (matrix.RowCount != matrix.ColumnCount)
				return null;

			SquareMatrix square = new SquareMatrix(dimension);

			for (int r = 0; r < dimension; r++)
				for (int c = 0; c < dimension; c++)
					if ((r < matrix.RowCount) && (c < matrix.ColumnCount))
						square[r, c] = matrix[r, c];

			return square;
		}

		public static AnyRectangularMatrix GetColumns(AnyRectangularMatrix matrix, IEnumerable<int> columnIndices)
		{
			RectangularMatrix target = new RectangularMatrix(matrix.RowCount, columnIndices.Count());

			int columnIndex = 0;
			foreach (int col in columnIndices)
			{
				for (int r = 0; r < matrix.RowCount; r++)
					target[r, columnIndex] = matrix[r, col];

				columnIndex++;
			}

			return target;
		}

		public static ColumnVector Diag(SquareMatrix matrix, Func<double, double> operation)
		{
			ColumnVector diag = new ColumnVector(matrix.Dimension);

			for (int i = 0; i < matrix.Dimension; i++)
			{
				double operatedValue = 0.0;
				if ((operation != null))
					operatedValue = operation(matrix[i, i]);

				diag[i] = (operation == null) ? matrix[i, i] : operatedValue;
			}

			return diag;
		}

		public static SquareMatrix Diag(ColumnVector vector, Func<double, double> operation)
		{
			SquareMatrix diag = new SquareMatrix(vector.Dimension);

			for (int i = 0; i < vector.Dimension; i++)
			{
				double value = vector[i];
				if (operation != null)
					value = operation(value);

				diag[i, i] = (double.IsNaN(value)) ? 0.0 : value;
			}

			return diag;
		}

		public static ColumnVector Ones(int dimension)
		{
			ColumnVector ones = new ColumnVector(dimension);

			for (int i = 0; i < dimension; i++)
				ones[i] = 1.0;

			return ones;
		}

		public static SquareMatrix Eye(int dimension)
		{
			SquareMatrix eye = new SquareMatrix(dimension);

			for (int i = 0; i < dimension; i++)
				eye[i, i] = 1.0;

			return eye;
		}

		public static ColumnVector Zeros(int dimension)
		{
			return new ColumnVector(dimension);
		}

		public static AnyRectangularMatrix Repmat(AnyRectangularMatrix source, int verticalTiles, int horizontalTiles)
		{
			RectangularMatrix target = new RectangularMatrix(
				(int)Math.Floor(source.RowCount * (double)verticalTiles), 
				(int)Math.Floor(source.ColumnCount * (double)horizontalTiles));

			int rows = source.RowCount;
			int cols = source.ColumnCount;
			for (int v = 0; v < verticalTiles; v++)
				for (int h = 0; h < horizontalTiles; h++)
					for (int r = 0; r < rows; r++)
						for (int c = 0; c < cols; c++)
							target[(v * rows) + r, (h * cols) + c] = source[r, c];

			return target;
		}

		/// <summary>
		/// Copies the lower or the upper half of a square matrix to another half - thematrix diagonal remains unchanged:
		/// <example>
		/// target[i,j] = target[j,i] for i > j if lowerToUpper=false
		/// target[i,j] = target[j,i] for i &lt; j if lowerToUpper=true
		/// </example>
		/// </summary>
		/// <param name="source">Input-output ﬂoating-point square matrix</param>
		/// <param name="lowerToUper">If true, the lower half is copied to the upper half, otherwise the upper half iscopied to the lower half</param>
		public static SquareMatrix GetSymmetrical(SquareMatrix source, bool lowerToUpper = false)
		{
			return GetTriangle(source, 0, lowerToUpper) + GetTriangle(source, lowerToUpper ? -1 : 1, lowerToUpper).Transpose();
		}

		public static SquareMatrix GetTriangle(SquareMatrix source, int diagonalIndex = 0, bool isGetLower = false)
		{
			SquareMatrix triangle = new SquareMatrix(source.Dimension);

			for (int r = 0; r < source.Dimension; r++)
			{
				for (int c = 0; c < source.Dimension; c++)
				{
					if (isGetLower)
						triangle[r, c] = (r >= (c - diagonalIndex)) ? source[r, c] : 0.0;
					else
						triangle[r, c] = (r <= (c - diagonalIndex)) ? source[r, c] : 0.0;
				}
			}

			return triangle;
		}
	}
}
