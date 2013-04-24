#define USE_OPTIMIZATIONS

using System;

namespace DeterministicSimulation
{
	//Fixed point math equivalent to Unity.Vector2
	public struct FVector2
	{
		public fint x;
		public fint y;

		//
		// Constructors
		//
		public FVector2(fint x, fint y)
		{
			this.x = x;
			this.y = y;
		}	


		//
		// Static Properties
		//
		public static FVector2 one = new FVector2(fint.one, fint.one);
		
		public static FVector2 zero = new FVector2(fint.zero, fint.zero);

		public static FVector2 right = new FVector2(fint.one, fint.zero);
		
		public static FVector2 up = new FVector2(fint.zero, fint.one);

		//
		// Properties
		//
		public fint magnitude
		{
			get
			{
				#if USE_OPTIMIZATIONS
				long temp = (((long) x.raw) * ((long) x.raw) + ((long) y.raw) * ((long) y.raw)) >> fint.SHIFT_AMOUNT;
				return FMath.Sqrt(fint.CreateRaw((int) temp));
				#else
				return FMath.Sqrt(this.x * this.x + this.y * this.y);
				#endif
			}
		}
		
		public FVector2 normalized
		{
			get
			{
				return FVector2.Normalize(this);
			}
		}
		
		public fint sqrMagnitude
		{
			get
			{
				#if USE_OPTIMIZATIONS
				long temp = (((long) x.raw) * ((long) x.raw) + ((long) y.raw) * ((long) y.raw)) >> fint.SHIFT_AMOUNT;
				return fint.CreateRaw((int) temp);
				#else
				return this.x * this.x + this.y * this.y;
				#endif
			}
		}
		
		//
		// Static Methods
		//
		public static fint Angle(FVector2 from, FVector2 to)
		{
			return FMath.Acos(FMath.Clamp(FVector2.Dot(from.normalized, to.normalized), -fint.one, fint.one)) * FMath.Rad2Deg;
		}
		
		public static FVector2 ClampMagnitude(FVector2 vector, fint maxLength)
		{
			if (vector.sqrMagnitude > maxLength * maxLength)
			{
				return vector.normalized * maxLength;
			}
			return vector;
		}
		
		public static fint Distance(FVector2 a, FVector2 b)
		{
			return (a - b).magnitude;
		}
		
		public static fint Dot(FVector2 lhs, FVector2 rhs)
		{
			return lhs.x * rhs.x + lhs.y * rhs.y;
		}
		
		public static FVector2 Lerp(FVector2 from, FVector2 to, fint t)
		{
			t = FMath.Clamp01(t);
			return new FVector2(from.x + (to.x - from.x) * t, from.y + (to.y - from.y) * t);
		}
		
		public static fint Magnitude(FVector2 a)
		{
			#if USE_OPTIMIZATIONS
			long temp = (((long) a.x.raw) * ((long) a.x.raw) + ((long) a.y.raw) * ((long) a.y.raw)) >> fint.SHIFT_AMOUNT;
			return FMath.Sqrt(fint.CreateRaw((int) temp));
			#else
			return FMath.Sqrt(a.x * a.x + a.y * a.y);
			#endif
		}

		public static FVector2 Max(FVector2 lhs, FVector2 rhs)
		{
			return new FVector2(FMath.Max(lhs.x, rhs.x), FMath.Max(lhs.y, rhs.y));
		}
		
		public static FVector2 Min(FVector2 lhs, FVector2 rhs)
		{
			return new FVector2(FMath.Min(lhs.x, rhs.x), FMath.Min(lhs.y, rhs.y));
		}
		
		public static FVector2 MoveTowards(FVector2 current, FVector2 target, fint maxDistanceDelta)
		{
			FVector2 a = target - current;
			fint magnitude = a.magnitude;
			if (magnitude <= maxDistanceDelta || magnitude == fint.zero)
			{
				return target;
			}
			return current + a / magnitude * maxDistanceDelta;
		}

		public static FVector2 Normalize(FVector2 value)
		{
			fint magnitude = FVector2.Magnitude(value);
			if (magnitude.raw > 1)
			{
				return value / magnitude;
			}
			return FVector3.zero;
		}

		public static FVector2 Scale(FVector2 a, FVector2 b)
		{
			return new FVector2(a.x * b.x, a.y * b.y);
		}
		
		public static fint SqrMagnitude(FVector2 a)
		{
			return a.x * a.x + a.y * a.y;
		}
		
		//
		// Methods
		//
		public override bool Equals(object other)
		{
			if (!(other is FVector2))
				return false;

			FVector2 vector = (FVector2)other;

			return this.x.raw == vector.x.raw && this.y.raw == vector.y.raw;
		}
		
		public override int GetHashCode()
		{
			return this.x.GetHashCode() ^ this.y.GetHashCode() << 2;
		}
		
		public void Normalize()
		{
			fint magnitude = this.magnitude;
			if (magnitude.raw > 1)
			{
				this /= magnitude;
			}
			else
			{
				this = FVector2.zero;
			}
		}
		
		public void Scale(FVector2 scale)
		{
			this.x *= scale.x;
			this.y *= scale.y;
		}
		
		public void Set(fint new_x, fint new_y)
		{
			this.x = new_x;
			this.y = new_y;
		}
		
		public fint SqrMagnitude()
		{
			return this.x * this.x + this.y * this.y;
		}
		
		public override string ToString()
		{
			return string.Format("({0}, {1})", this.x.ToFloat(), this.y.ToFloat());
		}
		
		//
		// Operators
		//
		public static FVector2 operator +(FVector2 a, FVector2 b)
		{
			return new FVector2(a.x + b.x, a.y + b.y);
		}
		
		public static FVector2 operator /(FVector2 a, fint d)
		{
			return new FVector2(a.x / d, a.y / d);
		}
		
		public static bool operator ==(FVector2 lhs, FVector2 rhs)
		{
			return lhs.x.raw == rhs.x.raw && lhs.y.raw == rhs.y.raw;
		}

		public static implicit operator FVector2(FVector3 v)
		{
			return new FVector2(v.x, v.y);
		}
		
		public static implicit operator FVector3(FVector2 v)
		{
			return new FVector3(v.x, v.y, fint.zero);
		}

		public static bool operator !=(FVector2 lhs, FVector2 rhs)
		{
			return lhs.x.raw != rhs.x.raw || lhs.y.raw == rhs.y.raw;
		}
		
		public static FVector2 operator *(FVector2 a, fint d)
		{
			return new FVector2(a.x * d, a.y * d);
		}
		
		public static FVector2 operator *(fint d, FVector2 a)
		{
			return new FVector2(a.x * d, a.y * d);
		}
		
		public static FVector2 operator -(FVector2 a, FVector2 b)
		{
			return new FVector2(a.x - b.x, a.y - b.y);
		}
		
		public static FVector2 operator -(FVector2 a)
		{
			return new FVector2(-a.x, -a.y);
		}
	}
}

