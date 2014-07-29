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
using NUnit.Framework;
using System.Collections.Generic;

namespace ShovelTests
{
    public static class Utils
    {
        public static string TrimCarriageReturn(this string str) 
        {
            return str.Replace("\r", "");
        }
        
        public static void ExpectException<T> (Action action, Action<T> check)
            where T: class
        {
            try {
                action ();
                Assert.Fail ();
            } catch (Exception ex) {
                check (ex as T);
            }
        }

        public static string FactorialOfTenProgram ()
        {
            return @"
var fact = fn (n) {
    if n == 0
    1
    else n * fact(n - 1)
}
fact(10)";
        }

        public static string FibonacciOfTenProgram ()
        {
            return @"
var fib = fn (n) {
    if n == 0 || n == 1
    1
    else fib(n - 1) + fib(n - 2)
}
fib(10)
";
        }

        public static string QsortProgram ()
        {
            return "stdlib.sort(array(1, 3, 4, 5, 2), fn (a, b) a < b)";
        }

        internal static void TestValue (
            string source, Shovel.Value.Kinds expectedResultType, object expectedResult)
        {
            var sources = Shovel.Api.MakeSources ("test.sho", source);
            var result = Shovel.Api.TestRunVm (sources);
            Assert.IsTrue (result.Kind == expectedResultType);
            switch (expectedResultType) {
            case Shovel.Value.Kinds.Null:
                Assert.AreEqual (Shovel.Value.Kinds.Null, result.Kind);
                break;
            case Shovel.Value.Kinds.Integer:
                Assert.AreEqual ((long)expectedResult, result.Integer.Value);
                break;
            case Shovel.Value.Kinds.String:
                Assert.AreEqual ((string)expectedResult, result.String.Value);
                break;
            case Shovel.Value.Kinds.Double:
                Assert.AreEqual ((double)expectedResult, result.Double.Value);
                break;
            case Shovel.Value.Kinds.Bool:
                Assert.AreEqual ((bool)expectedResult, result.Bool.Value);
                break;
            case Shovel.Value.Kinds.Array:
                Assert.Fail();
                break;
            case Shovel.Value.Kinds.Hash:
                Assert.Fail();
                break;
            case Shovel.Value.Kinds.Callable:
                Assert.Fail();
                break;
            case Shovel.Value.Kinds.ReturnAddress:
                Assert.Fail();
                break;
            case Shovel.Value.Kinds.NamedBlock:
                Assert.Fail();
                break;
            }
        }

        internal static IEnumerable<Shovel.Callable> GetPrintAndStopUdps (List<string> log, bool retryStop)
        {
            Action<Shovel.VmApi, Shovel.Value[], Shovel.UdpResult> print = (api, args, result) => {
                if (args.Length > 0 && args [0].Kind == Shovel.Value.Kinds.String) {
                    log.Add (args [0].String.Value);
                } else {
                    throw new InvalidOperationException ();
                }
            };
            Action<Shovel.VmApi, Shovel.Value[], Shovel.UdpResult> stop;
            if (retryStop) {
                bool firstCallOfStop = true;
                stop = (api, args, result) => {
                    if (firstCallOfStop) {
                        result.After = Shovel.UdpResult.AfterCall.NapAndRetryOnWakeUp;
                        firstCallOfStop = false;
                    } else {
                        result.After = Shovel.UdpResult.AfterCall.Continue;
                        result.Result = Shovel.Value.Make ("world");
                    }
                };

            } else {
                stop = (api, args, result) => {
                    result.After = Shovel.UdpResult.AfterCall.Nap;
                };
            }
            return new Shovel.Callable[] {
                Shovel.Callable.MakeUdp ("print", print, 1),
                Shovel.Callable.MakeUdp ("stop", stop, 0),
            };
        }


    }
}

