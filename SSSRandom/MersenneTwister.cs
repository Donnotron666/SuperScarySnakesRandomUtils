using System;

namespace SSSRandom
{

    //This code was kindly donated by https://twitter.com/tedajax for use in Black Future '88
    public class MersenneTwister
	{

		// Variables as defined by MT19937-32
		private const int W = 32;
		private const int N = 624;
		private const int M = 397;
		private const int R = 31;
		private const uint A = 0x9908B0DF;
		private const int U = 11;
		private const uint D = 0xffffffff;
		private const int S = 7;
		private const uint B = 0x9D2C5680;
		private const int T = 15;
		private const uint C = 0xEFC60000;
		private const int L = 18;
		private const int F = 1812433253;
		private const int MAX = 2147483647;
		private const uint HIGH_MASK = 0x80000000;
		private const uint LOW_MASK = 0x7FFFFFFF;
		private static readonly uint[] MASK_MATRIX = new uint[2] { 0, A };

		private uint[] _data;
		private int _index;

		public int _Seed { get; private set; }

		public MersenneTwister(int seed)
        {
			_data = new uint[N];
			_Seed = seed;
			_index = N;
			_data[0] = (uint)seed;
			for (int i = 1; i < N; ++i) {
				_data[i] = F * (_data[i - 1] ^ (_data[i - 1] >> (W - 2))) + (uint)i;
			}
		}

		public MersenneTwister(MersenneTwister other)
		{
			_data = new uint[N];
			Array.Copy(other._data, _data, N);
			_index = other._index;
			_Seed = other._Seed;
		}

		private void Twist()
		{
			uint y = 0;
			int i = 0;

			for (i = 0; i < N - M; ++i) {
				y = (_data[i] & HIGH_MASK) | (_data[i + 1] & LOW_MASK);
				_data[i] = _data[i + M] ^ (y >> 1) ^ MASK_MATRIX[y & 0x1];
			}

			for (; i < N - 1; ++i) {
				y = (_data[i] & HIGH_MASK) | (_data[i + 1] & LOW_MASK);
				_data[i] = _data[i + (M - N)] ^ (y >> 1) ^ MASK_MATRIX[y & 0x1];
			}

			y = (_data[N - 1] & HIGH_MASK) | (_data[0] & LOW_MASK);
			_data[N - 1] = _data[M - 1] ^ (y >> 1) ^ MASK_MATRIX[y & 0x1];

			_index = 0;
		}

		private uint GetNextIteration()
		{
			if (_index >= N) {
				Twist();
			}

			uint y = _data[_index++];

			y ^= (y >> U) & D;
			y ^= (y << S) & B;
			y ^= (y << T) & C;
			y ^= (y >> L);

			return y;
		}

		/// <summary>
		/// Generate int in range [0, max) with uniform distribution
		/// </summary>
		public int GetIntUniform(int max)
		{
			int x;
			do {
				x = GetInt();
			} while (x >= (MAX - MAX % max));
			return x % max;
		}

		/// <summary>
		/// Generate int in range [min, max] with uniform distribution
		/// </summary>
		public int GetIntUniform(int min, int max)
		{
			if (min > max) {
				throw new ArgumentException("Min must be less than max.");
			}

			return GetIntUniform(max - min + 1) + min;
		}

		/// <summary>
		/// Generate int in range [0, int.MaxValue)
		/// </summary>
		public int GetInt()
		{
			return (int)(GetNextIteration() >> 1);
		}

		/// <summary>
		/// Generate int in range [0, max)
		/// </summary>
		public int GetInt(int max)
		{
			return GetInt() % max;
		}

		/// <summary>
		/// Generate int in range [min, max]
		/// </summary>
		public int GetInt(int min, int max)
		{
			if (min > max) {
				throw new ArgumentException("Min must be less than max.");
			}

			return GetInt() % (max - min + 1) + min;
		}

		/// <summary>
		/// Generate float in range [0, 1]
		/// </summary>
		public float GetFloat()
		{
			return GetInt() / 2147483648.0f;
		}

		/// <summary>
		/// Generate float in range [0, max]
		/// </summary>
		public float GetFloat(float max)
		{
			return GetFloat() * max;
		}

		/// <summary>
		/// Generate float in range [min, max]
		/// </summary>
		public float GetFloat(float min, float max)
		{
			return GetFloat(max - min) + min;
		}
	}
}
