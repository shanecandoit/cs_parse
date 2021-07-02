using System;
using System.Collections.Generic;

namespace parseTest
{
    class Program
    {
        enum TokenClass
        {
            ident,
            number,

        }

        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            test_sample1();

            test_sample2();

            test_sample3();
        }

        private static void test_sample3()
        {
            string sample = "a = 123 + 456";
            //               1 2 333 4 555
            List<string> toks = tokenize(sample);
            Console.WriteLine("tokens for: " + sample);
            Console.WriteLine("# " + toks.Count);
            foreach (string tok in toks)
            {
                Console.WriteLine("- " + tok);
            }
            assert(toks.Count == 5);
        }

        private static void test_sample2()
        {
            string sample2 = "b = 'hello ' + 'world'";
            //                1 2 33333333 4 5555555
            List<string> toks2 = tokenize(sample2);
            Console.WriteLine("tokens for: " + sample2);
            Console.WriteLine("# " + toks2.Count);
            foreach (string tok in toks2)
            {
                Console.WriteLine("- " + tok);
            }
            assert(toks2.Count == 5);
        }

        private static void test_sample1()
        {
            string sample1 = "a = 1 + 2";
            //                1 2 3 4 5
            List<string> toks1 = tokenize(sample1);
            Console.WriteLine("tokens for: " + sample1);
            Console.WriteLine("# " + toks1.Count);
            foreach (string tok in toks1)
            {
                Console.WriteLine("- " + tok);
            }
            assert(toks1.Count == 5);
        }

        public static List<string> tokenize(string sample)
        {
            Console.WriteLine("tokenize( " + sample + ")");
            List<string> result = new List<string>();

            bool inString = false;
            string tok = "";
            char prev = '\0';
            for (int i = 0; i < sample.Length; i++)
            {
                char ch = sample[i];
                // Console.WriteLine("- " + i + " " + ch + "\n");

                // are we inside a string
                if (inString)
                {
                    tok += ch;
                    // end string
                    if ((ch == '\'' && prev != '\\') || (ch == '"' && prev != '\\')
                        )
                    {
                        inString = false;
                    }
                }
                // start string
                else if (!inString && (ch == '\'' || ch == '"'))
                {
                    inString = true;
                    tok += ch;
                }

                // white space may end a token
                else if (ch == ' ' || ch == '\n' || ch == '\t' || ch == '\r')
                {
                    if (tok.Length > 0)
                    {
                        result.Add(tok);
                        tok = "";
                    }
                }
                else
                {
                    tok += ch;
                }

                prev = ch;
            }

            // and the last token
            if (tok.Length > 0)
                result.Add(tok);


            return result;
        }

        public static void assert(bool expression, string message = "ERR")
        {
            if (!expression)
            {
                Console.WriteLine(message);
                Console.WriteLine(
                new System.Diagnostics.StackTrace().ToString()
                );
                Environment.Exit(1);
            }
        }
    }
}
