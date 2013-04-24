using System;

namespace DeterministicSimulation
{
	public class FTestUtils
	{
		static public void Test()
		{
			UnityEngine.Debug.Log("== START Executing FTestUtils");

			try
			{
				TestFIntBasic();

				TestFIntFloating();

				TestSqrt();

				TestTrigonometric();

				TestFVector2();

				UnityEngine.Debug.Log("== END Executing FTestUtils");
			}
			catch(Exception ex)
			{
				UnityEngine.Debug.LogError("FTestUtils.Test() Error: " + ex.ToString());
			}
		}

		static public void TestFIntBasic()
		{
			//Test basic value
			fint one = fint.one;

			AssertEquals(one, 1);
			AssertEquals(one, 1.0f);

			//Test addition
			fint two = one + one;

			AssertEquals(two, 2);
			AssertEquals(two, 2.0f);

			//Test substraction
			fint zero = two - two;
			AssertEquals(zero, 0);
			AssertEquals(zero, 0.0f);

			//Test negation
			fint minustwo = -two;

			AssertEquals(minustwo, -2);
			AssertEquals(minustwo, -2.0f);

			//Test multiplication
			fint minusfour = two * minustwo;

			AssertEquals(minusfour, -4);
			AssertEquals(minusfour, -4.0f);

			//Test division
			AssertEquals(minusfour / two, -2);
			AssertEquals(minusfour / two, -2.0f);
		}

		static public void TestFIntFloating()
		{
			//Test basic value
			fint onePointOne = fint.CreateFromFloat(1.1f);
			AssertAlmostEquals(onePointOne, 1.1f);

			//Test addition
			fint twoPointTwo = onePointOne + onePointOne;
			AssertAlmostEquals(twoPointTwo, 2.2f);

			//Test substraction
			AssertAlmostEquals(twoPointTwo - onePointOne, 1.1f);
			AssertEquals(twoPointTwo - onePointOne, onePointOne);

			//Test negation
			AssertAlmostEquals(-twoPointTwo, -2.2f);

			//Test multiplication
			AssertAlmostEquals(twoPointTwo * twoPointTwo, 2.2f * 2.2f);

			//Test division
			AssertEquals(twoPointTwo / twoPointTwo, 1.0f);
			AssertAlmostEquals(twoPointTwo / (twoPointTwo * fint.CreateFromFloat(2.0f)), 0.5f);
		}

		static public void TestSqrt()
		{
			//Basic tests
			AssertEquals(FMath.Sqrt(fint.CreateFromInt(0)), 0.0f);
			AssertEquals(FMath.Sqrt(fint.CreateFromInt(1)), 1.0f);
			AssertEquals(FMath.Sqrt(fint.CreateFromInt(4)), 2.0f);
			AssertAlmostEquals(FMath.Sqrt(fint.CreateFromInt(2)), 1.41421356237f);

			//Iterative test
			for (float f = 0; f < 1; f += 0.001f)
				AssertAlmostEquals(FMath.Sqrt(fint.CreateFromFloat(f)), (float) Math.Sqrt(f));

			for (float f = 0; f < 100; f += 0.1f)
				AssertAlmostEquals(FMath.Sqrt(fint.CreateFromFloat(f)), (float) Math.Sqrt(f));

			for (float f = 0; f < 100000; f += 100.0f)
				AssertAlmostEquals(FMath.Sqrt(fint.CreateFromFloat(f)), (float) Math.Sqrt(f));


		}

		static public void TestTrigonometric()
		{
			//Test sin
			AssertAlmostEqualsTrig(FMath.Sin(fint.CreateFromFloat(0) * FMath.Deg2Rad), 0.0f);
			AssertAlmostEqualsTrig(FMath.Sin(fint.CreateFromFloat(90) * FMath.Deg2Rad), 1.0f);
			AssertAlmostEqualsTrig(FMath.Sin(fint.CreateFromFloat(180) * FMath.Deg2Rad), 0.0f);
			AssertAlmostEqualsTrig(FMath.Sin(fint.CreateFromFloat(270) * FMath.Deg2Rad), -1.0f);

			AssertAlmostEqualsTrig(FMath.Sin(fint.CreateFromFloat(30.0f) * FMath.Deg2Rad), 0.5f);
			AssertAlmostEqualsTrig(FMath.Sin(fint.CreateFromFloat(-30.0f) * FMath.Deg2Rad), -0.5f);
			AssertAlmostEqualsTrig(FMath.Sin(fint.CreateFromFloat(60.0f) * FMath.Deg2Rad), 0.86603f);
			AssertAlmostEqualsTrig(FMath.Sin(fint.CreateFromFloat(-60.0f) * FMath.Deg2Rad), -0.86603f);

			//Test cos
			AssertAlmostEqualsTrig(FMath.Cos(fint.CreateFromFloat(0) * FMath.Deg2Rad), 1.0f);
			AssertAlmostEqualsTrig(FMath.Cos(fint.CreateFromFloat(90) * FMath.Deg2Rad), 0.0f);
			AssertAlmostEqualsTrig(FMath.Cos(fint.CreateFromFloat(180) * FMath.Deg2Rad), -1.0f);
			AssertAlmostEqualsTrig(FMath.Cos(fint.CreateFromFloat(270) * FMath.Deg2Rad), 0.0f);
			
			AssertAlmostEqualsTrig(FMath.Cos(fint.CreateFromFloat(60.0f) * FMath.Deg2Rad), 0.5f);
			AssertAlmostEqualsTrig(FMath.Cos(fint.CreateFromFloat(-60.0f) * FMath.Deg2Rad), 0.5f);
			AssertAlmostEqualsTrig(FMath.Cos(fint.CreateFromFloat(30.0f) * FMath.Deg2Rad), 0.86603f);
			AssertAlmostEqualsTrig(FMath.Cos(fint.CreateFromFloat(-30.0f) * FMath.Deg2Rad), 0.86603f);

			//Test asin
			AssertAlmostEqualsAngleRadians(FMath.Asin(fint.CreateFromFloat(0.0f)), 0.0f);
			AssertAlmostEqualsAngleRadians(FMath.Asin(fint.CreateFromFloat(-1.0f)), (float) -Math.PI / 2.0f);
			AssertAlmostEqualsAngleRadians(FMath.Asin(fint.CreateFromFloat(1.0f)), (float) Math.PI / 2.0f);
			AssertAlmostEqualsAngleRadians(FMath.Asin(fint.CreateFromFloat(-0.8660254f)), (float) -Math.PI / 3.0f);
			AssertAlmostEqualsAngleRadians(FMath.Asin(fint.CreateFromFloat(0.8660254f)), (float) Math.PI / 3.0f);
			AssertAlmostEqualsAngleRadians(FMath.Asin(fint.CreateFromFloat(-0.7071068f)), (float) -Math.PI / 4.0f);
			AssertAlmostEqualsAngleRadians(FMath.Asin(fint.CreateFromFloat(0.7071068f)), (float) Math.PI / 4.0f);
			AssertAlmostEqualsAngleRadians(FMath.Asin(fint.CreateFromFloat(-0.5f)), (float) -Math.PI / 6.0f);
			AssertAlmostEqualsAngleRadians(FMath.Asin(fint.CreateFromFloat(0.5f)), (float) Math.PI / 6.0f);

			//Test acos
			AssertAlmostEqualsAngleRadians(FMath.Acos(fint.CreateFromFloat(0)), (float) Math.PI / 2.0f);
			AssertAlmostEqualsAngleRadians(FMath.Acos(fint.CreateFromFloat(-1.0f)), (float) Math.PI);
			AssertAlmostEqualsAngleRadians(FMath.Acos(fint.CreateFromFloat(1.0f)), (float) 0);
			AssertAlmostEqualsAngleRadians(FMath.Acos(fint.CreateFromFloat(-0.8660254f)), 5.0f * (float) Math.PI / 6.0f);
			AssertAlmostEqualsAngleRadians(FMath.Acos(fint.CreateFromFloat(0.8660254f)), (float) Math.PI / 6.0f);
			AssertAlmostEqualsAngleRadians(FMath.Acos(fint.CreateFromFloat(-0.7071068f)), 3.0f * (float) Math.PI / 4.0f);
			AssertAlmostEqualsAngleRadians(FMath.Acos(fint.CreateFromFloat(0.7071068f)), (float) Math.PI / 4.0f);
			AssertAlmostEqualsAngleRadians(FMath.Acos(fint.CreateFromFloat(-0.5f)), 2.0f * (float) Math.PI / 3.0f);
			AssertAlmostEqualsAngleRadians(FMath.Acos(fint.CreateFromFloat(0.5f)), (float) Math.PI / 3.0f);
		}

		static public void TestFVector2()
		{
			//Test basic value
			FVector2 one = FVector2.one;

			AssertEquals(one, 1.0f, 1.0f);

			//Test addition
			FVector2 two = one + one;
			
			AssertEquals(two, 2.0f, 2.0f);

			//Test substraction
			FVector2 zero = two - two;
			AssertEquals(zero, 0.0f, 0.0f);
			
			//Test negation
			FVector2 minustwo = -two;
			
			AssertEquals(minustwo, -2.0f, -2.0f);
			
			//Test multiplication
			FVector2 four = two * fint.CreateFromFloat(2.0f);
			AssertEquals(four, 4.0f, 4.0f);

			//Test division
			AssertEquals(four / fint.CreateFromFloat(-2.0f), -2.0f, -2.0f);

			//Test leng
			AssertAlmostEquals(new FVector2(fint.CreateFromFloat(2.0f), fint.CreateFromFloat(2.0f)).magnitude, (float) Math.Sqrt(2.0f * 2.0f + 2.0f * 2.0f));
			AssertAlmostEquals(new FVector2(fint.CreateFromFloat(2.0f), fint.CreateFromFloat(4.0f)).magnitude, (float) Math.Sqrt(2.0f * 2.0f + 4.0f * 4.0f));
			AssertAlmostEquals(new FVector2(fint.CreateFromFloat(4.0f), fint.CreateFromFloat(2.0f)).magnitude, (float) Math.Sqrt(2.0f * 2.0f + 4.0f * 4.0f));

			//Test angle between

			//Basic test
			AssertAlmostEqualsAngleDegress(FVector2.Angle(FVector2.up, FVector2.right), 90.0f);
			AssertAlmostEqualsAngleDegress(FVector2.Angle(FVector2.up, -FVector2.right), 90.0f);
			
			AssertAlmostEqualsAngleDegress(FVector2.Angle(-FVector2.up, FVector2.right), 90.0f);
			AssertAlmostEqualsAngleDegress(FVector2.Angle(-FVector2.up, -FVector2.right), 90.0f);
			
			AssertAlmostEqualsAngleDegress(FVector2.Angle(FVector2.up, FVector2.one), 45.0f);
			AssertAlmostEqualsAngleDegress(FVector2.Angle(FVector2.up, -FVector2.one), 135.0f);
			
			AssertAlmostEqualsAngleDegress(FVector2.Angle(FVector2.right, FVector2.right), 0.0f);
			AssertAlmostEqualsAngleDegress(FVector2.Angle(FVector2.up, -FVector2.up), 180.0f);

			//Deep iterative test
			for (float x = -1.0f; x <= 1.0f; x += 0.2f)
			{
				for (float y = -1.0f; y <= 1.0f; y += 0.2f)
				{
					UnityEngine.Vector2 v1 = new UnityEngine.Vector2(x, y);
					FVector2 fv1 = new FVector2(fint.CreateFromFloat(x), fint.CreateFromFloat(y));

					for (float x2 = -1.0f; x2 <= 1.0f; x2 += 0.2f)
					{
						for (float y2 = -1.0f; y2 <= 1.0f; y2 += 0.2f)
						{
							UnityEngine.Vector2 v2 = new UnityEngine.Vector2(x2, y2);
							FVector2 fv2 = new FVector2(fint.CreateFromFloat(x2), fint.CreateFromFloat(y2));

							AssertAlmostEqualsAngleDegress(
								FVector2.Angle(fv1, fv2),
								UnityEngine.Vector2.Angle(v1, v2));
						}
					}
				}
			}
		}

		static public void AssertEquals(fint fvalue1, fint fvalue2)
		{
			if (fvalue1 != fvalue2)
				throw new Exception(string.Format("Expected {0}, got {1}", fvalue1.ToFloat(), fvalue2.ToFloat()));
		}

		static public void AssertEquals(fint fvalue, int i)
		{
			if (fvalue.ToInt() != i)
				throw new Exception(string.Format("Expected {0}, got {1}", i, fvalue.ToInt().ToString()));
		}

		static public void AssertEquals(fint fvalue, float f)
		{
			if (fvalue.ToFloat() != f)
				throw new Exception(string.Format("Expected {0}, got {1}", f, fvalue.ToFloat().ToString()));
		}

		static public void AssertEquals(FVector2 v, float x, float y)
		{
			if (v.x.ToFloat() != x || v.y.ToFloat() != y)
				throw new Exception(string.Format("Expected {0}, got {1}", string.Format("({0}, {1})", x, y), v.ToString()));
		}


		static public void AssertAlmostEquals(fint fvalue, float f)
		{
			if (Math.Abs(fvalue.ToFloat() - f) > 0.001f)
				throw new Exception(string.Format("Expected {0}, got {1}", f, fvalue.ToFloat().ToString()));
		}

		static public void AssertAlmostEqualsTrig(fint fvalue, float f)
		{
			if (Math.Abs(fvalue.ToFloat() - f) > 0.09f)
				throw new Exception(string.Format("Expected {0}, got {1}", f, fvalue.ToFloat().ToString()));
		}

		static public void AssertAlmostEqualsAngleDegress(fint fvalue, float f)
		{
			if (Math.Abs(fvalue.ToFloat() - f) > 2.0f)
				throw new Exception(string.Format("Expected {0}, got {1}", f, fvalue.ToFloat().ToString()));
		}

		static public void AssertAlmostEqualsAngleRadians(fint fvalue, float f)
		{
			if (Math.Abs(fvalue.ToFloat() - f) > (float) (2.0f * Math.PI / 180.0f))
				throw new Exception(string.Format("Expected {0}, got {1}", f, fvalue.ToFloat().ToString()));
		}
	}
}
