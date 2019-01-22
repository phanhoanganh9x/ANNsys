﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;
using System.Text;

namespace IM_PJ.Bussiness
{


    /// <summary>
    /// Represent the set of code values to be output into barcode form
    /// </summary>

    public class Code128Content
    {
        /// <summary>
        /// Create content based on a string of ASCII data
        /// </summary>
        /// <param name="asciiData">the string that should be represented</param>
        public Code128Content(string asciiData)
        {
            this.Codes = this.StringToCode128(asciiData);
        }

        /// <summary>
        /// Provides the Code128 code values representing the object's string
        /// </summary>
        public int[] Codes { get; }

        /// <summary>
        /// Transform the string into integers representing the Code128 codes
        /// necessary to represent it
        /// </summary>
        /// <param name="asciiData">String to be encoded</param>
        /// <returns>Code128 representation</returns>
        private int[] StringToCode128(string asciiData)
        {
            // turn the string into ascii byte data
            var asciiBytes = Encoding.ASCII.GetBytes(asciiData);

            // decide which codeset to start with
            var csa1 = asciiBytes.Length > 0
                           ? Code128Code.CodesetAllowedForChar(asciiBytes[0])
                           : Code128Code.CodeSetAllowed.CodeAorB;
            var csa2 = asciiBytes.Length > 1
                           ? Code128Code.CodesetAllowedForChar(asciiBytes[1])
                           : Code128Code.CodeSetAllowed.CodeAorB;
            var currentCodeSet = this.GetBestStartSet(csa1, csa2);

            // set up the beginning of the barcode
            // assume no codeset changes, account for start, checksum, and stop
            var codes = new ArrayList(asciiBytes.Length + 3) { Code128Code.StartCodeForCodeSet(currentCodeSet) };

            // add the codes for each character in the string
            for (var i = 0; i < asciiBytes.Length; i++)
            {
                int thischar = asciiBytes[i];
                var nextchar = asciiBytes.Length > i + 1 ? asciiBytes[i + 1] : -1;

                codes.AddRange(Code128Code.CodesForChar(thischar, nextchar, ref currentCodeSet));
            }

            // calculate the check digit
            var checksum = (int)codes[0];
            for (var i = 1; i < codes.Count; i++)
            {
                checksum += i * (int)codes[i];
            }

            codes.Add(checksum % 103);

            codes.Add(Code128Code.StopCode());

            var result = codes.ToArray(typeof(int)) as int[];
            return result;
        }

        /// <summary>
        /// Determines the best starting code set based on the the first two 
        /// characters of the string to be encoded
        /// </summary>
        /// <param name="csa1">First character of input string</param>
        /// <param name="csa2">Second character of input string</param>
        /// <returns>The codeset determined to be best to start with</returns>
        private CodeSet GetBestStartSet(Code128Code.CodeSetAllowed csa1, Code128Code.CodeSetAllowed csa2)
        {
            var vote = 0;

            vote += csa1 == Code128Code.CodeSetAllowed.CodeA ? 1 : 0;
            vote += csa1 == Code128Code.CodeSetAllowed.CodeB ? -1 : 0;
            vote += csa2 == Code128Code.CodeSetAllowed.CodeA ? 1 : 0;
            vote += csa2 == Code128Code.CodeSetAllowed.CodeB ? -1 : 0;

            return vote > 0 ? CodeSet.CodeA : CodeSet.CodeB; // ties go to codeB due to my own prejudices
        }

        public static class Code128Code
        {
            private const int CShift = 98;

            private const int CCodeA = 101;

            private const int CCodeB = 100;

            private const int CStartA = 103;

            private const int CStartB = 104;

            private const int CStop = 106;

            /// <summary>
            /// Indicates which code sets can represent a character -- CodeA, CodeB, or either
            /// </summary>
            public enum CodeSetAllowed
            {
                CodeA,

                CodeB,

                CodeAorB
            }

            /// <summary>
            /// Get the Code128 code value(s) to represent an ASCII character, with 
            /// optional look-ahead for length optimization
            /// </summary>
            /// <param name="charAscii">The ASCII value of the character to translate</param>
            /// <param name="lookAheadAscii">The next character in sequence (or -1 if none)</param>
            /// <param name="currentCodeSet">The current codeset, that the returned codes need to follow;
            /// if the returned codes change that, then this value will be changed to reflect it</param>
            /// <returns>An array of integers representing the codes that need to be output to produce the 
            /// given character</returns>
            public static int[] CodesForChar(int charAscii, int lookAheadAscii, ref CodeSet currentCodeSet)
            {
                int[] result;
                var shifter = -1;

                if (!CharCompatibleWithCodeset(charAscii, currentCodeSet))
                {
                    // if we have a lookahead character AND if the next character is ALSO not compatible
                    if ((lookAheadAscii != -1) && !CharCompatibleWithCodeset(lookAheadAscii, currentCodeSet))
                    {
                        // we need to switch code sets
                        switch (currentCodeSet)
                        {
                            case CodeSet.CodeA:
                                shifter = CCodeB;
                                currentCodeSet = CodeSet.CodeB;
                                break;
                            case CodeSet.CodeB:
                                shifter = CCodeA;
                                currentCodeSet = CodeSet.CodeA;
                                break;
                        }
                    }
                    else
                    {
                        // no need to switch code sets, a temporary SHIFT will suffice
                        shifter = CShift;
                    }
                }

                if (shifter != -1)
                {
                    result = new int[2];
                    result[0] = shifter;
                    result[1] = CodeValueForChar(charAscii);
                }
                else
                {
                    result = new int[1];
                    result[0] = CodeValueForChar(charAscii);
                }

                return result;
            }

            /// <summary>
            /// Tells us which codesets a given character value is allowed in
            /// </summary>
            /// <param name="charAscii">ASCII value of character to look at</param>
            /// <returns>Which codeset(s) can be used to represent this character</returns>
            public static CodeSetAllowed CodesetAllowedForChar(int charAscii)
            {
                if (charAscii >= 32 && charAscii <= 95)
                {
                    return CodeSetAllowed.CodeAorB;
                }
                else
                {
                    return charAscii < 32 ? CodeSetAllowed.CodeA : CodeSetAllowed.CodeB;
                }
            }

            /// <summary>
            /// Determine if a character can be represented in a given codeset
            /// </summary>
            /// <param name="charAscii">character to check for</param>
            /// <param name="currentCodeSet">codeset context to test</param>
            /// <returns>true if the codeset contains a representation for the ASCII character</returns>
            public static bool CharCompatibleWithCodeset(int charAscii, CodeSet currentCodeSet)
            {
                var csa = CodesetAllowedForChar(charAscii);
                return csa == CodeSetAllowed.CodeAorB || (csa == CodeSetAllowed.CodeA && currentCodeSet == CodeSet.CodeA)
                       || (csa == CodeSetAllowed.CodeB && currentCodeSet == CodeSet.CodeB);
            }

            /// <summary>
            /// Gets the integer code128 code value for a character (assuming the appropriate code set)
            /// </summary>
            /// <param name="charAscii">character to convert</param>
            /// <returns>code128 symbol value for the character</returns>
            public static int CodeValueForChar(int charAscii)
            {
                return charAscii >= 32 ? charAscii - 32 : charAscii + 64;
            }

            /// <summary>
            /// Return the appropriate START code depending on the codeset we want to be in
            /// </summary>
            /// <param name="cs">The codeset you want to start in</param>
            /// <returns>The code128 code to start a barcode in that codeset</returns>
            public static int StartCodeForCodeSet(CodeSet cs)
            {
                return cs == CodeSet.CodeA ? CStartA : CStartB;
            }

            /// <summary>
            /// Return the Code128 stop code
            /// </summary>
            /// <returns>the stop code</returns>
            public static int StopCode()
            {
                return CStop;
            }

        }
        public enum CodeSet
        {
            CodeA,
            CodeB
            //// CodeC   // not supported
        }
    }
}