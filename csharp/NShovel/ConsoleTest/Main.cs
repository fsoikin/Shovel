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
//			try {
//				//var sources = Shovel.Api.MakeSources ("test.sho", "hasKey(hash('a', 1, 'b', 2), 'a')");
//                var sources = Shovel.Api.MakeSources("fact.sho", @"var fact = fn (n) {
//    if n == 0
//    1
//    else n * fact(n - 1)
//}
//fact(10)");
//				Console.WriteLine (Shovel.Api.PrintCode(sources));
////				var result = (Dictionary<string, object>)Shovel.Api.NakedRunVm (sources);
////				foreach (var key in result.Keys) {
////					Console.WriteLine ("{0}: {1}", key, result[key]);
////				}
//				Console.WriteLine (Shovel.Api.NakedRunVm (sources));
//			} catch (Shovel.ShovelException ex) {
//				Console.WriteLine (ex.Message);
//			}

			var sources = Shovel.Api.MakeSources ("test.sho", "panic('test')");
			ExpectException<Shovel.ShovelException> (() => {
				var result = Shovel.Api.NakedRunVm (sources);
			},
			(ex) => {
				Console.WriteLine (ex.Message);
			}
			);

		}

		public static void ExpectException<T> (Action action, Action<T> check)
            where T: class
		{
			try {
				action ();
				Console.WriteLine("auch");
			} catch (Exception ex) {
				check (ex as T);
			}
		}


	}
}