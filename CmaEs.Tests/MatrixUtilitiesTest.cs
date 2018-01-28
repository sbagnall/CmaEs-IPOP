using SteveBagnall.CmaEs_IPOP;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Meta.Numerics.Matrices;
using System.Collections.Generic;

namespace CMAESReduxTest
{
    
    
    /// <summary>
    ///This is a test class for MatrixUtilitiesTest and is intended
    ///to contain all MatrixUtilitiesTest Unit Tests
    ///</summary>
	[TestClass()]
	public class MatrixUtilitiesTest
	{
		private const double VERY_SMALL_VALUE = 1e-10;

		#region Large Matrices

		private SquareMatrix LargeTenByTenSquare
		{
			get
			{
				SquareMatrix square = new SquareMatrix(10);
				square[0, 0] = 0.99214806217944562;
				square[1, 0] = 0.00078459551623940541;
				square[2, 0] = 0.0137193907870768;
				square[3, 0] = 0.0015114793098809749;
				square[4, 0] = -0.013265035351143239;
				square[5, 0] = -0.011282240110563148;
				square[6, 0] = -0.003612444365710493;
				square[7, 0] = -0.0028851732639102816;
				square[8, 0] = -0.0017400375909807067;
				square[9, 0] = 0.0041730209987321382;
				square[0, 1] = 0.000784595516239406;
				square[1, 1] = 0.98345664469725658;
				square[2, 1] = -0.00056206424353653494;
				square[3, 1] = 0.022384149199749178;
				square[4, 1] = -0.022388178384753016;
				square[5, 1] = -0.00968161260316717;
				square[6, 1] = 0.0096360306871534872;
				square[7, 1] = -0.0070531661382691657;
				square[8, 1] = 0.010009310728467896;
				square[9, 1] = -0.02350913857345456;
				square[0, 2] = 0.0137193907870768;
				square[1, 2] = -0.00056206424353653494;
				square[2, 2] = 0.98181677458421868;
				square[3, 2] = -0.0011610825531500634;
				square[4, 2] = -0.0069877610778769778;
				square[5, 2] = -0.0068669951187971725;
				square[6, 2] = -0.0034556259390485303;
				square[7, 2] = -0.0013043479854180969;
				square[8, 2] = -0.0021942449371264066;
				square[9, 2] = 0.0052134122403081091;
				square[0, 3] = 0.0015114793098809751;
				square[1, 3] = 0.022384149199749175;
				square[2, 3] = -0.0011610825531500643;
				square[3, 3] = 1.0169555947690962;
				square[4, 3] = -0.044681678909170026;
				square[5, 3] = -0.019306760216304952;
				square[6, 3] = 0.019257401958021506;
				square[7, 3] = -0.014080126287533438;
				square[8, 3] = 0.019997761855960191;
				square[9, 3] = -0.046969524755300779;
				square[0, 4] = -0.013265035351143243;
				square[1, 4] = -0.022388178384753016;
				square[2, 4] = -0.0069877610778769778;
				square[3, 4] = -0.044681678909170026;
				square[4, 4] = 1.0238671449281569;
				square[5, 4] = 0.025591072332671445;
				square[6, 4] = -0.0167217422527691;
				square[7, 4] = 0.015499050134860002;
				square[8, 4] = -0.018555750794009895;
				square[9, 4] = 0.043531610729381637;
				square[0, 5] = -0.011282240110563148;
				square[1, 5] = -0.00968161260316717;
				square[2, 5] = -0.0068669951187971725;
				square[3, 5] = -0.019306760216304949;
				square[4, 5] = 0.025591072332671438;
				square[5, 5] = 0.98627940486962706;
				square[6, 5] = -0.0060330975469938793;
				square[7, 5] = 0.0073707898636681934;
				square[8, 5] = -0.0073421845476972347;
				square[9, 5] = 0.017198750782931257;
				square[0, 6] = -0.003612444365710493;
				square[1, 6] = 0.0096360306871534872;
				square[2, 6] = -0.0034556259390485311;
				square[3, 6] = 0.019257401958021506;
				square[4, 6] = -0.0167217422527691;
				square[5, 6] = -0.0060330975469938776;
				square[6, 6] = 0.98146006490962789;
				square[7, 6] = -0.0055475288628857762;
				square[8, 6] = 0.0091330749757561024;
				square[9, 6] = -0.021469710939622744;
				square[0, 7] = -0.0028851732639102825;
				square[1, 7] = -0.0070531661382691666;
				square[2, 7] = -0.0013043479854180971;
				square[3, 7] = -0.014080126287533442;
				square[4, 7] = 0.015499050134860002;
				square[5, 7] = 0.0073707898636681951;
				square[6, 7] = -0.0055475288628857771;
				square[7, 7] = 0.97697596319199853;
				square[8, 7] = -0.00600493668631261;
				square[9, 7] = 0.014093575390274785;
				square[0, 8] = -0.0017400375909807067;
				square[1, 8] = 0.010009310728467896;
				square[2, 8] = -0.0021942449371264074;
				square[3, 8] = 0.019997761855960191;
				square[4, 8] = -0.018555750794009892;
				square[5, 8] = -0.0073421845476972347;
				square[6, 8] = 0.0091330749757561024;
				square[7, 8] = -0.006004936686312609;
				square[8, 8] = 0.98148842756322263;
				square[9, 8] = -0.021711331416396865;
				square[0, 9] = 0.0041730209987321382;
				square[1, 9] = -0.023509138573454564;
				square[2, 9] = 0.0052134122403081108;
				square[3, 9] = -0.046969524755300772;
				square[4, 9] = 0.043531610729381637;
				square[5, 9] = 0.017198750782931257;
				square[6, 9] = -0.021469710939622737;
				square[7, 9] = 0.014093575390274784;
				square[8, 9] = -0.021711331416396865;
				square[9, 9] = 1.0232683186175922;

				return square;
			}
		}

		private SquareMatrix LargeTenByTenSquareUpperTriangleInclusive
		{
			get
			{
				SquareMatrix square = new SquareMatrix(10);
				square[0, 0] = 0.99214806217944562;
				square[1, 0] = 0.0;
				square[2, 0] = 0.0;
				square[3, 0] = 0.0;
				square[4, 0] = 0.0;
				square[5, 0] = 0.0;
				square[6, 0] = 0.0;
				square[7, 0] = 0.0;
				square[8, 0] = 0.0;
				square[9, 0] = 0.0;
				square[0, 1] = 0.000784595516239406;
				square[1, 1] = 0.98345664469725658;
				square[2, 1] = 0.0;
				square[3, 1] = 0.0;
				square[4, 1] = 0.0;
				square[5, 1] = 0.0;
				square[6, 1] = 0.0;
				square[7, 1] = 0.0;
				square[8, 1] = 0.0;
				square[9, 1] = 0.0;
				square[0, 2] = 0.0137193907870768;
				square[1, 2] = -0.00056206424353653494;
				square[2, 2] = 0.98181677458421868;
				square[3, 2] = 0.0;
				square[4, 2] = 0.0;
				square[5, 2] = 0.0;
				square[6, 2] = 0.0;
				square[7, 2] = 0.0;
				square[8, 2] = 0.0;
				square[9, 2] = 0.0;
				square[0, 3] = 0.0015114793098809751;
				square[1, 3] = 0.022384149199749175;
				square[2, 3] = -0.0011610825531500643;
				square[3, 3] = 1.0169555947690962;
				square[4, 3] = 0.0;
				square[5, 3] = 0.0;
				square[6, 3] = 0.0;
				square[7, 3] = 0.0;
				square[8, 3] = 0.0;
				square[9, 3] = 0.0;
				square[0, 4] = -0.013265035351143243;
				square[1, 4] = -0.022388178384753016;
				square[2, 4] = -0.0069877610778769778;
				square[3, 4] = -0.044681678909170026;
				square[4, 4] = 1.0238671449281569;
				square[5, 4] = 0.0;
				square[6, 4] = 0.0;
				square[7, 4] = 0.0;
				square[8, 4] = 0.0;
				square[9, 4] = 0.0;
				square[0, 5] = -0.011282240110563148;
				square[1, 5] = -0.00968161260316717;
				square[2, 5] = -0.0068669951187971725;
				square[3, 5] = -0.019306760216304949;
				square[4, 5] = 0.025591072332671438;
				square[5, 5] = 0.98627940486962706;
				square[6, 5] = 0.0;
				square[7, 5] = 0.0;
				square[8, 5] = 0.0;
				square[9, 5] = 0.0;
				square[0, 6] = -0.003612444365710493;
				square[1, 6] = 0.0096360306871534872;
				square[2, 6] = -0.0034556259390485311;
				square[3, 6] = 0.019257401958021506;
				square[4, 6] = -0.0167217422527691;
				square[5, 6] = -0.0060330975469938776;
				square[6, 6] = 0.98146006490962789;
				square[7, 6] = 0.0;
				square[8, 6] = 0.0;
				square[9, 6] = 0.0;
				square[0, 7] = -0.0028851732639102825;
				square[1, 7] = -0.0070531661382691666;
				square[2, 7] = -0.0013043479854180971;
				square[3, 7] = -0.014080126287533442;
				square[4, 7] = 0.015499050134860002;
				square[5, 7] = 0.0073707898636681951;
				square[6, 7] = -0.0055475288628857771;
				square[7, 7] = 0.97697596319199853;
				square[8, 7] = 0.0;
				square[9, 7] = 0.0;
				square[0, 8] = -0.0017400375909807067;
				square[1, 8] = 0.010009310728467896;
				square[2, 8] = -0.0021942449371264074;
				square[3, 8] = 0.019997761855960191;
				square[4, 8] = -0.018555750794009892;
				square[5, 8] = -0.0073421845476972347;
				square[6, 8] = 0.0091330749757561024;
				square[7, 8] = -0.006004936686312609;
				square[8, 8] = 0.98148842756322263;
				square[9, 8] = 0.0;
				square[0, 9] = 0.0041730209987321382;
				square[1, 9] = -0.023509138573454564;
				square[2, 9] = 0.0052134122403081108;
				square[3, 9] = -0.046969524755300772;
				square[4, 9] = 0.043531610729381637;
				square[5, 9] = 0.017198750782931257;
				square[6, 9] = -0.021469710939622737;
				square[7, 9] = 0.014093575390274784;
				square[8, 9] = -0.021711331416396865;
				square[9, 9] = 1.0232683186175922;

				return square;
			}
		}

		private SquareMatrix LargeTenByTenSquareUpperTriangleExclusive
		{
			get
			{
				SquareMatrix square = new SquareMatrix(10);
				square[0, 0] = 0.0;
				square[1, 0] = 0.0;
				square[2, 0] = 0.0;
				square[3, 0] = 0.0;
				square[4, 0] = 0.0;
				square[5, 0] = 0.0;
				square[6, 0] = 0.0;
				square[7, 0] = 0.0;
				square[8, 0] = 0.0;
				square[9, 0] = 0.0;
				square[0, 1] = 0.000784595516239406;
				square[1, 1] = 0.0;
				square[2, 1] = 0.0;
				square[3, 1] = 0.0;
				square[4, 1] = 0.0;
				square[5, 1] = 0.0;
				square[6, 1] = 0.0;
				square[7, 1] = 0.0;
				square[8, 1] = 0.0;
				square[9, 1] = 0.0;
				square[0, 2] = 0.0137193907870768;
				square[1, 2] = -0.00056206424353653494;
				square[2, 2] = 0.0;
				square[3, 2] = 0.0;
				square[4, 2] = 0.0;
				square[5, 2] = 0.0;
				square[6, 2] = 0.0;
				square[7, 2] = 0.0;
				square[8, 2] = 0.0;
				square[9, 2] = 0.0;
				square[0, 3] = 0.0015114793098809751;
				square[1, 3] = 0.022384149199749175;
				square[2, 3] = -0.0011610825531500643;
				square[3, 3] = 0.0;
				square[4, 3] = 0.0;
				square[5, 3] = 0.0;
				square[6, 3] = 0.0;
				square[7, 3] = 0.0;
				square[8, 3] = 0.0;
				square[9, 3] = 0.0;
				square[0, 4] = -0.013265035351143243;
				square[1, 4] = -0.022388178384753016;
				square[2, 4] = -0.0069877610778769778;
				square[3, 4] = -0.044681678909170026;
				square[4, 4] = 0.0;
				square[5, 4] = 0.0;
				square[6, 4] = 0.0;
				square[7, 4] = 0.0;
				square[8, 4] = 0.0;
				square[9, 4] = 0.0;
				square[0, 5] = -0.011282240110563148;
				square[1, 5] = -0.00968161260316717;
				square[2, 5] = -0.0068669951187971725;
				square[3, 5] = -0.019306760216304949;
				square[4, 5] = 0.025591072332671438;
				square[5, 5] = 0.0;
				square[6, 5] = 0.0;
				square[7, 5] = 0.0;
				square[8, 5] = 0.0;
				square[9, 5] = 0.0;
				square[0, 6] = -0.003612444365710493;
				square[1, 6] = 0.0096360306871534872;
				square[2, 6] = -0.0034556259390485311;
				square[3, 6] = 0.019257401958021506;
				square[4, 6] = -0.0167217422527691;
				square[5, 6] = -0.0060330975469938776;
				square[6, 6] = 0.0;
				square[7, 6] = 0.0;
				square[8, 6] = 0.0;
				square[9, 6] = 0.0;
				square[0, 7] = -0.0028851732639102825;
				square[1, 7] = -0.0070531661382691666;
				square[2, 7] = -0.0013043479854180971;
				square[3, 7] = -0.014080126287533442;
				square[4, 7] = 0.015499050134860002;
				square[5, 7] = 0.0073707898636681951;
				square[6, 7] = -0.0055475288628857771;
				square[7, 7] = 0.0;
				square[8, 7] = 0.0;
				square[9, 7] = 0.0;
				square[0, 8] = -0.0017400375909807067;
				square[1, 8] = 0.010009310728467896;
				square[2, 8] = -0.0021942449371264074;
				square[3, 8] = 0.019997761855960191;
				square[4, 8] = -0.018555750794009892;
				square[5, 8] = -0.0073421845476972347;
				square[6, 8] = 0.0091330749757561024;
				square[7, 8] = -0.006004936686312609;
				square[8, 8] = 0.0;
				square[9, 8] = 0.0;
				square[0, 9] = 0.0041730209987321382;
				square[1, 9] = -0.023509138573454564;
				square[2, 9] = 0.0052134122403081108;
				square[3, 9] = -0.046969524755300772;
				square[4, 9] = 0.043531610729381637;
				square[5, 9] = 0.017198750782931257;
				square[6, 9] = -0.021469710939622737;
				square[7, 9] = 0.014093575390274784;
				square[8, 9] = -0.021711331416396865;
				square[9, 9] = 0.0;

				return square;
			}
		}

		private SquareMatrix LargeTenByTenSymmetrical
		{
			get
			{
				SquareMatrix square = new SquareMatrix(10);
				square[0, 0] = 0.99214806217944562;
				square[0, 1] = 0.000784595516239406;
				square[1, 1] = 0.98345664469725658;
				square[0, 2] = 0.0137193907870768;
				square[1, 2] = -0.00056206424353653494;
				square[2, 2] = 0.98181677458421868;
				square[0, 3] = 0.0015114793098809751;
				square[1, 3] = 0.022384149199749175;
				square[2, 3] = -0.0011610825531500643;
				square[3, 3] = 1.0169555947690962;
				square[0, 4] = -0.013265035351143243;
				square[1, 4] = -0.022388178384753016;
				square[2, 4] = -0.0069877610778769778;
				square[3, 4] = -0.044681678909170026;
				square[4, 4] = 1.0238671449281569;
				square[0, 5] = -0.011282240110563148;
				square[1, 5] = -0.00968161260316717;
				square[2, 5] = -0.0068669951187971725;
				square[3, 5] = -0.019306760216304949;
				square[4, 5] = 0.025591072332671438;
				square[5, 5] = 0.98627940486962706;
				square[0, 6] = -0.003612444365710493;
				square[1, 6] = 0.0096360306871534872;
				square[2, 6] = -0.0034556259390485311;
				square[3, 6] = 0.019257401958021506;
				square[4, 6] = -0.0167217422527691;
				square[5, 6] = -0.0060330975469938776;
				square[6, 6] = 0.98146006490962789;
				square[0, 7] = -0.0028851732639102825;
				square[1, 7] = -0.0070531661382691666;
				square[2, 7] = -0.0013043479854180971;
				square[3, 7] = -0.014080126287533442;
				square[4, 7] = 0.015499050134860002;
				square[5, 7] = 0.0073707898636681951;
				square[6, 7] = -0.0055475288628857771;
				square[7, 7] = 0.97697596319199853;
				square[0, 8] = -0.0017400375909807067;
				square[1, 8] = 0.010009310728467896;
				square[2, 8] = -0.0021942449371264074;
				square[3, 8] = 0.019997761855960191;
				square[4, 8] = -0.018555750794009892;
				square[5, 8] = -0.0073421845476972347;
				square[6, 8] = 0.0091330749757561024;
				square[7, 8] = -0.006004936686312609;
				square[8, 8] = 0.98148842756322263;
				square[0, 9] = 0.0041730209987321382;
				square[1, 9] = -0.023509138573454564;
				square[2, 9] = 0.0052134122403081108;
				square[3, 9] = -0.046969524755300772;
				square[4, 9] = 0.043531610729381637;
				square[5, 9] = 0.017198750782931257;
				square[6, 9] = -0.021469710939622737;
				square[7, 9] = 0.014093575390274784;
				square[8, 9] = -0.021711331416396865;
				square[9, 9] = 1.0232683186175922;

				square[1, 0] = 0.000784595516239406;
				square[2, 0] = 0.0137193907870768;
				square[2, 1] = -0.00056206424353653494;
				square[3, 0] = 0.0015114793098809751;
				square[3, 1] = 0.022384149199749175;
				square[3, 2] = -0.0011610825531500643;
				square[4, 0] = -0.013265035351143243;
				square[4, 1] = -0.022388178384753016;
				square[4, 2] = -0.0069877610778769778;
				square[4, 3] = -0.044681678909170026;
				square[5, 0] = -0.011282240110563148;
				square[5, 1] = -0.00968161260316717;
				square[5, 2] = -0.0068669951187971725;
				square[5, 3] = -0.019306760216304949;
				square[5, 4] = 0.025591072332671438;
				square[6, 0] = -0.003612444365710493;
				square[6, 1] = 0.0096360306871534872;
				square[6, 2] = -0.0034556259390485311;
				square[6, 3] = 0.019257401958021506;
				square[6, 4] = -0.0167217422527691;
				square[6, 5] = -0.0060330975469938776;
				square[7, 0] = -0.0028851732639102825;
				square[7, 1] = -0.0070531661382691666;
				square[7, 2] = -0.0013043479854180971;
				square[7, 3] = -0.014080126287533442;
				square[7, 4] = 0.015499050134860002;
				square[7, 5] = 0.0073707898636681951;
				square[7, 6] = -0.0055475288628857771;
				square[8, 0] = -0.0017400375909807067;
				square[8, 1] = 0.010009310728467896;
				square[8, 2] = -0.0021942449371264074;
				square[8, 3] = 0.019997761855960191;
				square[8, 4] = -0.018555750794009892;
				square[8, 5] = -0.0073421845476972347;
				square[8, 6] = 0.0091330749757561024;
				square[8, 7] = -0.006004936686312609;
				square[9, 0] = 0.0041730209987321382;
				square[9, 1] = -0.023509138573454564;
				square[9, 2] = 0.0052134122403081108;
				square[9, 3] = -0.046969524755300772;
				square[9, 4] = 0.043531610729381637;
				square[9, 5] = 0.017198750782931257;
				square[9, 6] = -0.021469710939622737;
				square[9, 7] = 0.014093575390274784;
				square[9, 8] = -0.021711331416396865;

				return square;
			}
		}


		#endregion

		private SquareMatrix ThreeByThreeSquare
		{
			get
			{
				SquareMatrix square = new SquareMatrix(3);
				square[0, 0] = 0.0; square[0, 1] = 1.0; square[0, 2] = 2.0;
				square[1, 0] = 3.0; square[1, 1] = 4.0; square[1, 2] = 5.0;
				square[2, 0] = 0.0; square[2, 1] = 0.0; square[2, 2] = 0.0;

				return square;
			}
		}

		private ColumnVector ThreeByThreeDiagOverTwo
		{
			get
			{
				ColumnVector column = new ColumnVector(3);
				column[0] = 0.0;
				column[1] = 2.0;
				column[2] = 0.0;

				return column;
			}
		}

		private ColumnVector UnsortedColumn
		{
			get
			{
				ColumnVector column = new ColumnVector(10);
				column[0] = 5.0;
				column[1] = 6.0;
				column[2] = 3.0;
				column[3] = 2.0;
				column[4] = 7.0;
				column[5] = 8.0;
				column[6] = 4.0;
				column[7] = 1.0;
				column[8] = 9.0;
				column[9] = 0.0;

				return column;
			}
		}

		private ColumnVector ThreeRowColumn
		{
			get
			{
				ColumnVector column = new ColumnVector(3);
				column[0] = 1.0;
				column[1] = 2.0;
				column[2] = 3.0;

				return column;
			}
		}

		private ColumnVector ThreeRowColumnCubed
		{
			get
			{
				ColumnVector column = new ColumnVector(3);
				column[0] = Math.Pow(1.0, 3);
				column[1] = Math.Pow(2.0, 3);
				column[2] = Math.Pow(3.0, 3);

				return column;
			}
		}


		private SquareMatrix ThreeRowColumnSquareOverTwo
		{
			get
			{
				SquareMatrix square = new SquareMatrix(3);
				square[0, 0] = 0.5; square[0, 1] = 0.0; square[0, 2] = 0.0; 
				square[1, 0] = 0.0; square[1, 1] = 1.0; square[1, 2] = 0.0;
				square[2, 0] = 0.0; square[2, 1] = 0.0; square[2, 2] = 1.5;

				return square;
			}
		}

		private SquareMatrix ThreeByThreeEye
		{
			get
			{
				SquareMatrix square = new SquareMatrix(3);
				square[0, 0] = 1.0; square[0, 1] = 0.0; square[0, 2] = 0.0;
				square[1, 0] = 0.0; square[1, 1] = 1.0; square[1, 2] = 0.0;
				square[2, 0] = 0.0; square[2, 1] = 0.0; square[2, 2] = 1.0;

				return square;
			}
		}

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
		///A test for ConvertToSquare
		///</summary>
		[TestMethod()]
		public void ConvertToSquareTest()
		{
			var matrix = new RectangularMatrix(new [,] 
			{ 
				{ 0.0, 1.0, 2.0 },
				{ 3.0, 4.0, 5.0 },
				{ 0.0, 0.0, 0.0 }
			});

			int dimension = 3;
			SquareMatrix expected = new SquareMatrix(dimension);
			expected[0, 0] = 0.0; expected[0, 1] = 1.0; expected[0, 2] = 2.0;
			expected[1, 0] = 3.0; expected[1, 1] = 4.0; expected[1, 2] = 5.0;
			expected[2, 0] = 0.0; expected[2, 1] = 0.0; expected[2, 2] = 0.0;

			SquareMatrix actual = Utilities.ConvertToSquare(matrix, dimension);

			for (int r = 0; r < expected.Dimension; r++)
				for (int c = 0; c < expected.Dimension; c++)
					Assert.AreEqual(expected[r, c], actual[r, c]);
		}

		/// <summary>
		///A test for Diag
		///</summary>
		[TestMethod()]
		public void SquareToColumnDiagTest()
		{
			SquareMatrix matrix = this.ThreeByThreeSquare;
			Func<double, double> operation = x => x / 2.0;
			ColumnVector expected = this.ThreeByThreeDiagOverTwo; 
			
			ColumnVector actual = Utilities.Diag(matrix, operation);

			for (int r = 0; r < expected.Dimension; r++)
				Assert.AreEqual(expected[r], actual[r]);
		}

		/// <summary>
		///A test for Diag
		///</summary>
		[TestMethod()]
		public void ColumnToSquareDiagTest()
		{
			ColumnVector vector = this.ThreeRowColumn;
			Func<double, double> operation = x => x / 2.0;
			SquareMatrix expected = this.ThreeRowColumnSquareOverTwo; 
			
			SquareMatrix actual = Utilities.Diag(vector, operation);

			for (int r = 0; r < expected.Dimension; r++)
				for (int c = 0; c < expected.Dimension; c++)
					Assert.AreEqual(expected[r, c], actual[r, c]);
		}

		/// <summary>
		///A test for Eig
		///</summary>
		[TestMethod()]
		public void EigTest()
		{
			SquareMatrix matrix = this.LargeTenByTenSymmetrical; 
			SquareMatrix eigenVectors = null; 
			SquareMatrix eigenValues = null; 

			Utilities.EigAlgLib(matrix, out eigenVectors, out eigenValues, 1, isGetLower: false);

			SquareMatrix A = matrix * eigenVectors;
			SquareMatrix B = eigenVectors * eigenValues;
			
			for (int r = 0; r < matrix.Dimension; r++)
				for (int c = 0; c < matrix.Dimension; c++)
					Assert.IsTrue(Math.Abs(A[r, c] - B[r, c]) < VERY_SMALL_VALUE);
		}

		[TestMethod()]
		public void EigLargeTest()
		{
			SquareMatrix matrix = this.LargeTenByTenSymmetrical;
			SquareMatrix eigenVectors = null;
			SquareMatrix eigenValues = null;

			Utilities.EigAlgLib(matrix, out eigenVectors, out eigenValues, 1, isGetLower: false);

			SquareMatrix A = matrix * eigenVectors;
			SquareMatrix B = eigenVectors * eigenValues;

			for (int r = 0; r < matrix.Dimension; r++)
				for (int c = 0; c < matrix.Dimension; c++)
					Assert.IsTrue(Math.Abs(A[r, c] - B[r, c]) < VERY_SMALL_VALUE);
		}

		

		/// <summary>
		///A test for ElementByElementOperation
		///</summary>
		[TestMethod()]
		public void ElementByElementOperationTest()
		{
			ColumnVector vector = this.ThreeRowColumn;
			Func<double, int, double> operation = (x, i) => Math.Pow(x, 3);
			ColumnVector expected = this.ThreeRowColumnCubed;
			
			ColumnVector actual = Utilities.ElementByElementOperation(vector, operation);

			for (int r = 0; r < vector.Dimension; r++)
				Assert.AreEqual(expected[r], actual[r]);
		}

		/// <summary>
		///A test for Eye
		///</summary>
		[TestMethod()]
		public void EyeTest()
		{
			int dimension = 3; 
			
			SquareMatrix expected = this.ThreeByThreeEye;
			
			SquareMatrix actual = Utilities.Eye(dimension);

			for (int r = 0; r < 3; r++)
				for (int c = 0; c < 3; c++)
					Assert.AreEqual(expected[r, c], actual[r, c]);
		}

		/// <summary>
		///A test for GetColumns
		///</summary>
		[TestMethod()]
		public void GetColumnsTest()
		{
			AnyRectangularMatrix matrix = this.ThreeByThreeSquare;
			IEnumerable<int> columnIndices = new[] { 0, 2 };
			AnyRectangularMatrix expected = new RectangularMatrix(new[,] { { 0.0, 2.0 }, { 3.0, 5.0 }, { 0.0, 0.0 } });
			
			AnyRectangularMatrix actual = Utilities.GetColumns(matrix, columnIndices);

			for (int r = 0; r < expected.RowCount; r++)
				for (int c = 0; c < expected.ColumnCount; c++)
					Assert.AreEqual(expected, actual);
		}

		/// <summary>
		///A test for GetSymmetrical
		///</summary>
		[TestMethod()]
		public void GetSymmetricalTestUpperToLower()
		{
			SquareMatrix source = this.ThreeByThreeSquare; 
			bool lowerToUpper = false;
			SquareMatrix expected = new SquareMatrix(source.Dimension);
			expected[0, 0] = 0.0; expected[0, 1] = 1.0; expected[0, 2] = 2.0;
			expected[1, 0] = 1.0; expected[1, 1] = 4.0; expected[1, 2] = 5.0;
			expected[2, 0] = 2.0; expected[2, 1] = 5.0; expected[2, 2] = 0.0;

			SquareMatrix actual = Utilities.GetSymmetrical(source, lowerToUpper);

			for (int r = 0; r < expected.RowCount; r++)
				for (int c = 0; c < expected.ColumnCount; c++)
					Assert.AreEqual(expected[r, c], actual[r, c]);
		}

		[TestMethod()]
		public void GetSymmetricalTestLowerToUpper()
		{
			SquareMatrix source = this.ThreeByThreeSquare;
			bool lowerToUpper = true;
			SquareMatrix expected = new SquareMatrix(source.Dimension);
			expected[0, 0] = 0.0; expected[0, 1] = 3.0; expected[0, 2] = 0.0;
			expected[1, 0] = 3.0; expected[1, 1] = 4.0; expected[1, 2] = 0.0;
			expected[2, 0] = 0.0; expected[2, 1] = 0.0; expected[2, 2] = 0.0;

			SquareMatrix actual = Utilities.GetSymmetrical(source, lowerToUpper);

			for (int r = 0; r < expected.RowCount; r++)
				for (int c = 0; c < expected.ColumnCount; c++)
					Assert.AreEqual(expected[r, c], actual[r, c]);
		}

		[TestMethod()]
		public void GetSymmetricalTestLargeUpper()
		{
			SquareMatrix source = this.LargeTenByTenSquare;
			bool lowerToUpper = false;
			SquareMatrix expected = this.LargeTenByTenSymmetrical;

			SquareMatrix actual = Utilities.GetSymmetrical(source, lowerToUpper);

			for (int r = 0; r < expected.RowCount; r++)
				for (int c = 0; c < expected.ColumnCount; c++)
					Assert.AreEqual(expected[r, c], actual[r, c]);
		}

		/// <summary>
		///A test for GetTriangle
		///</summary>
		[TestMethod()]
		public void GetTriangleTestUpper()
		{
			SquareMatrix source = new SquareMatrix(3);
			source[0, 0] = 0.0; source[0, 1] = 1.0; source[0, 2] = 2.0;
			source[1, 0] = 3.0; source[1, 1] = 4.0; source[1, 2] = 5.0;
			source[2, 0] = 6.0; source[2, 1] = 7.0; source[2, 2] = 8.0;

			int diagonalIndex = 1; 
			bool isGetLower = false; 
			SquareMatrix expected = new SquareMatrix(source.Dimension);
			expected[0, 0] = 0.0; expected[0, 1] = 1.0; expected[0, 2] = 2.0;
			expected[1, 0] = 0.0; expected[1, 1] = 0.0; expected[1, 2] = 5.0;
			expected[2, 0] = 0.0; expected[2, 1] = 0.0; expected[2, 2] = 0.0;

			SquareMatrix actual = Utilities.GetTriangle(source, diagonalIndex, isGetLower);

			for (int r = 0; r < expected.RowCount; r++)
				for (int c = 0; c < expected.ColumnCount; c++)
					Assert.AreEqual(expected[r, c], actual[r, c]);
		}

		[TestMethod()]
		public void GetTriangleTestLower()
		{
			SquareMatrix source = this.ThreeByThreeSquare;
			int diagonalIndex = 1;
			bool isGetLower = true;
			SquareMatrix expected = new SquareMatrix(source.Dimension);
			expected[0, 0] = 0.0; expected[0, 1] = 1.0; expected[0, 2] = 0.0;
			expected[1, 0] = 3.0; expected[1, 1] = 4.0; expected[1, 2] = 5.0;
			expected[2, 0] = 0.0; expected[2, 1] = 0.0; expected[2, 2] = 0.0;

			SquareMatrix actual = Utilities.GetTriangle(source, diagonalIndex, isGetLower);

			for (int r = 0; r < expected.RowCount; r++)
				for (int c = 0; c < expected.ColumnCount; c++)
					Assert.AreEqual(expected[r, c], actual[r, c]);
		}

		[TestMethod()]
		public void GetTriangleTestLargeUpperInclusive()
		{
			SquareMatrix source = this.LargeTenByTenSquare;
			int diagonalIndex = 0;
			bool isGetLower = false;
			SquareMatrix expected = this.LargeTenByTenSquareUpperTriangleInclusive;

			SquareMatrix actual = Utilities.GetTriangle(source, diagonalIndex, isGetLower);

			for (int r = 0; r < expected.RowCount; r++)
				for (int c = 0; c < expected.ColumnCount; c++)
					Assert.AreEqual(expected[r, c], actual[r, c]);
		}

		[TestMethod()]
		public void GetTriangleTestLargeUpperExclusive()
		{
			SquareMatrix source = this.LargeTenByTenSquare;
			int diagonalIndex = 1;
			bool isGetLower = false;
			SquareMatrix expected = this.LargeTenByTenSquareUpperTriangleExclusive;

			SquareMatrix actual = Utilities.GetTriangle(source, diagonalIndex, isGetLower);

			for (int r = 0; r < expected.RowCount; r++)
				for (int c = 0; c < expected.ColumnCount; c++)
					Assert.AreEqual(expected[r, c], actual[r, c]);
		}


		/// <summary>
		///A test for Ones
		///</summary>
		[TestMethod()]
		public void OnesTest()
		{
			int dimension = 3;
			ColumnVector expected = new ColumnVector(new [] { 1.0, 1.0, 1.0 } );
			
			ColumnVector actual = Utilities.Ones(dimension);

			for (int r = 0; r < expected.RowCount; r++)
				Assert.AreEqual(expected[r], actual[r]);
		}

		/// <summary>
		///A test for Repmat
		///</summary>
		[TestMethod()]
		public void RepmatTest()
		{
			AnyRectangularMatrix source = this.ThreeByThreeSquare;
			int verticalTiles = 2; 
			int horizontalTiles = 3;

			AnyRectangularMatrix expected = new RectangularMatrix(new[,] 
			{ 
				{ 0.0, 1.0, 2.0, 0.0, 1.0, 2.0, 0.0, 1.0, 2.0 },
				{ 3.0, 4.0, 5.0, 3.0, 4.0, 5.0, 3.0, 4.0, 5.0 },
				{ 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 },
				{ 0.0, 1.0, 2.0, 0.0, 1.0, 2.0, 0.0, 1.0, 2.0 },
				{ 3.0, 4.0, 5.0, 3.0, 4.0, 5.0, 3.0, 4.0, 5.0 },
				{ 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 }
			});

			AnyRectangularMatrix actual = Utilities.Repmat(source, verticalTiles, horizontalTiles);

			for (int r = 0; r < expected.RowCount; r++)
				for (int c = 0; c < expected.ColumnCount; c++)
					Assert.AreEqual(expected[r, c], actual[r, c]);
		}

		/// <summary>
		///A test for Sort
		///</summary>
		[TestMethod()]
		public void SortTest()
		{
			ColumnVector vector = this.UnsortedColumn;
			int[] originalIndices = null;
			int[] originalIndicesExpected = new[] { 9, 7, 3, 2, 6, 0, 1, 4, 5, 8 };
			ColumnVector expected = new ColumnVector(new[] { 0.0, 1.0, 2.0, 3.0, 4.0, 5.0, 6.0, 7.0, 8.0, 9.0 });
			
			ColumnVector actual = Utilities.Sort(vector, out originalIndices);

			for (int r = 0; r < expected.RowCount; r++)
			{
				Assert.AreEqual(originalIndicesExpected[r], originalIndices[r]);
				Assert.AreEqual(expected[r], actual[r]);
			}
		}

		/// <summary>
		///A test for Zeros
		///</summary>
		[TestMethod()]
		public void ZerosTest()
		{
			int dimension = 3;
			ColumnVector expected = new ColumnVector(new[] { 0.0, 0.0, 0.0 });

			ColumnVector actual = Utilities.Zeros(dimension);

			for (int r = 0; r < expected.RowCount; r++)
				Assert.AreEqual(expected[r], actual[r]);
		}

		/// <summary>
		///A test for IsTrueForAll
		///</summary>
		[TestMethod()]
		public void IsTrueForAllSquareTestTrue()
		{
			SquareMatrix matrix = this.ThreeByThreeSquare;
			Func<double, bool> predicate = x => x >= 0;
			bool expected = true;
		
			bool actual = Utilities.IsTrueForAll(matrix, predicate);
			
			Assert.AreEqual(expected, actual);
		}

		[TestMethod()]
		public void IsTrueForAllSquareTestFalse()
		{
			SquareMatrix matrix = this.ThreeByThreeSquare;
			Func<double, bool> predicate = x => x <= 0;
			bool expected = false;

			bool actual = Utilities.IsTrueForAll(matrix, predicate);

			Assert.AreEqual(expected, actual);
		}

		/// <summary>
		///A test for IsTrueForAll
		///</summary>
		[TestMethod()]
		public void IsTrueForAllVectorTestTrue()
		{
			ColumnVector vector = this.ThreeByThreeDiagOverTwo;
			Func<double, bool> predicate = x => x >= 0;
			bool expected = true;

			bool actual = Utilities.IsTrueForAll(vector, predicate);

			Assert.AreEqual(expected, actual);
		}

		[TestMethod()]
		public void IsTrueForAllVectorTestFalse()
		{
			ColumnVector vector = this.ThreeByThreeDiagOverTwo;
			Func<double, bool> predicate = x => x == 2;
			bool expected = false;

			bool actual = Utilities.IsTrueForAll(vector, predicate);

			Assert.AreEqual(expected, actual);
		}
	}
}
