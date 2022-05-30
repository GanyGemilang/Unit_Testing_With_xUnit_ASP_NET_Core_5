using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace Unit_Testing_xUnit_ASP_NET_Core.Repositories
{
    public class PasswordRepository
    {
		// Define default min and max password lengths.
		private static int DEFAULT_MIN_PASSWORD_LENGTH = 8;

		private static int DEFAULT_MAX_PASSWORD_LENGTH = 10;
		// Define supported password characters divided into groups.
		// You can add (or remove) characters to (from) these groups.
		private static string PASSWORD_CHARS_LCASE = "abcdwfghijklmnopqrstuvqxyz";
		private static string PASSWORD_CHARS_UCASE = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
		private static string PASSWORD_CHARS_NUMERIC = "0123456789";

		private static string PASSWORD_CHARS_SPECIAL = "0123456789";
		public string Generate()
		{
			return Generate(DEFAULT_MIN_PASSWORD_LENGTH, DEFAULT_MAX_PASSWORD_LENGTH);
		}

		public string Generate(int length)
		{
			return Generate(length, length);
		}

		private string Generate(int minLength, int maxLength)
		{
			// Make sure that input parameters are valid.
			if (minLength <= 0 || maxLength <= 0 || minLength > maxLength)
			{
				return null;
			}
			char[][] charGroups = new char[][] {
			PASSWORD_CHARS_LCASE.ToCharArray(),
			PASSWORD_CHARS_UCASE.ToCharArray(),
			PASSWORD_CHARS_NUMERIC.ToCharArray(),
			PASSWORD_CHARS_SPECIAL.ToCharArray()
			};

			int[] charsLeftInGroup = new int[charGroups.Length];

			// Initially, all characters in each group are not used.
			for (int i = 0; i <= charsLeftInGroup.Length - 1; i++)
			{
				charsLeftInGroup[i] = charGroups[i].Length;
			}

			int[] leftGroupsOrder = new int[charGroups.Length];

			for (int i = 0; i <= leftGroupsOrder.Length - 1; i++)
			{
				leftGroupsOrder[i] = i;
			}
			byte[] randomBytes = new byte[4];

			RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
			rng.GetBytes(randomBytes);

			int seed = (randomBytes[0] & 0x7f) << 24 | randomBytes[1] << 16 | randomBytes[2] << 8 | randomBytes[3];

			Random random = new Random(seed);

			char[] password = null;

			if (minLength < maxLength)
			{
				password = new char[random.Next(minLength, maxLength + 1)];
			}
			else
			{
				password = new char[minLength];
			}

			int nextCharIdx = 0;

			int nextGroupIdx = 0;

			int nextLeftGroupsOrderIdx = 0;

			int lastCharIdx = 0;

			int lastLeftGroupsOrderIdx = leftGroupsOrder.Length - 1;

			for (int i = 0; i <= password.Length - 1; i++)
			{
				if (lastLeftGroupsOrderIdx == 0)
				{
					nextLeftGroupsOrderIdx = 0;
				}
				else
				{
					nextLeftGroupsOrderIdx = random.Next(0, lastLeftGroupsOrderIdx);
				}

				nextGroupIdx = leftGroupsOrder[nextLeftGroupsOrderIdx];

				lastCharIdx = charsLeftInGroup[nextGroupIdx] - 1;

				if (lastCharIdx == 0)
				{
					nextCharIdx = 0;
				}
				else
				{
					nextCharIdx = random.Next(0, lastCharIdx + 1);
				}

				// Add this character to the password.
				password[i] = charGroups[nextGroupIdx][nextCharIdx];

				if (lastCharIdx == 0)
				{
					charsLeftInGroup[nextGroupIdx] = charGroups[nextGroupIdx].Length;
				}
				else
				{
					if (lastCharIdx != nextCharIdx)
					{
						char temp = charGroups[nextGroupIdx][lastCharIdx];
						charGroups[nextGroupIdx][lastCharIdx] = charGroups[nextGroupIdx][nextCharIdx];
						charGroups[nextGroupIdx][nextCharIdx] = temp;
					}
					charsLeftInGroup[nextGroupIdx] -= 1;
				}

				if (lastLeftGroupsOrderIdx == 0)
				{
					lastLeftGroupsOrderIdx = leftGroupsOrder.Length - 1;
				}
				else
				{
					if (lastLeftGroupsOrderIdx != nextLeftGroupsOrderIdx)
					{
						int temp = leftGroupsOrder[lastLeftGroupsOrderIdx];
						leftGroupsOrder[lastLeftGroupsOrderIdx] = leftGroupsOrder[nextLeftGroupsOrderIdx];
						leftGroupsOrder[nextLeftGroupsOrderIdx] = temp;
					}
					lastLeftGroupsOrderIdx -= 1;
				}
			}

			return new string(password);
		}
	}
}
