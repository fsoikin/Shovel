// Copyright (c) 2012, Miron Brezuleanu
// All rights reserved.
//
// Redistribution and use in source and binary forms, with or without
// modification, are permitted provided that the following conditions are met:
//     * Redistributions of source code must retain the above copyright
//       notice, this list of conditions and the following disclaimer.
//     * Redistributions in binary form must reproduce the above copyright
//       notice, this list of conditions and the following disclaimer in the
//       documentation and/or other materials provided with the distribution.
//
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
// ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
// WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
// DISCLAIMED. IN NO EVENT SHALL <COPYRIGHT HOLDER> BE LIABLE FOR ANY
// DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
// (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
// LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
// ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
// (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
// SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
using System;
using System.Text;
using System.Collections.Generic;

namespace ConsoleTest
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			ParserErrorMessageHelper (@"
var a = fn [x] 1
",
			                         ex => {
				Console.WriteLine (ex.Message);
				Console.WriteLine (ex.FileName);
				Console.WriteLine (ex.Line);
				Console.WriteLine (ex.Column);
				Console.WriteLine (ex.AtEof);
			}
			);

		}

		static void ParserErrorMessageHelper (string source, Action<Shovel.ShovelException> exceptionTest)
		{
			var sources = MakeSources ("test.sho", source);
			var tokenizer = new Shovel.Compiler.Tokenizer (sources [0]);
			var parser = new Shovel.Compiler.Parser (tokenizer.Tokens, sources);
			try {
				foreach (var pt in parser.ParseTrees) {
					Console.WriteLine (pt);
				}
			} catch (Shovel.ShovelException ex) {
				exceptionTest (ex);
			}
		}

		public static List<Shovel.SourceFile> MakeSources (params string[] namesAndContents)
		{
			List<Shovel.SourceFile> result = new List<Shovel.SourceFile> ();
			for (var i = 0; i < namesAndContents.Length; i+=2) {
				result.Add (new Shovel.SourceFile () {
					FileName = namesAndContents[i],
					Content = namesAndContents[i+1]
				}
				);
			}
			return result;
		}

	}
}
