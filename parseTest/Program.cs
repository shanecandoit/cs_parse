using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace parseTest
{
    class Program
    {
        enum TokenKind
        {
            Empty,
            Ident,
            Number,
            String,
            Symbol
        }

        static Regex rx_ident = new Regex(@"^([a-zA-Z][a-zA-Z0-9_]*)$", RegexOptions.Compiled);
        static Regex rx_number = new Regex(@"^([-]?([0-9]+)|([0-9]+[.][0-9]+)|([.][0-9]+))$", RegexOptions.Compiled);

        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            // test tokenizer
            test_sample1();
            test_sample2();
            test_sample3();
            //test_sample4();

            test_regex_ident();
            test_regex_number1();

            test_token_kind();
        }

        private static void test_token_kind()
        {
            Console.WriteLine("TokenKind.Ident");
            assert(token_kind("abc") == TokenKind.Ident);
            assert(token_kind("abc123") == TokenKind.Ident);
            assert(token_kind("abc_") == TokenKind.Ident);

            Console.WriteLine("TokenKind.Empty");
            assert(token_kind(" ") == TokenKind.Empty);

            Console.WriteLine("TokenKind.Number");
            assert(token_kind("123") == TokenKind.Number);
            assert(token_kind("3.14") == TokenKind.Number);
            assert(token_kind(".5") == TokenKind.Number);

            Console.WriteLine("TokenKind.Symbol");
            Console.WriteLine("!=");
            assert(token_kind("!=") == TokenKind.Symbol);
            Console.WriteLine(".");
            assert(token_kind(".") == TokenKind.Symbol);
            Console.WriteLine("==");
            assert(token_kind("==") == TokenKind.Symbol);
            Console.WriteLine("+");
            assert(token_kind("+") == TokenKind.Symbol);
        }

        private static void test_regex_number1()
        {
            string num = "123";
            bool m = rx_number.IsMatch(num);
            Console.WriteLine(num + " is num? " + m);
            assert(m);

            num = "3.14";
            m = rx_number.IsMatch(num);
            Console.WriteLine(num + " is num? " + m);
            assert(m);

            num = ".99";
            m = rx_number.IsMatch(num);
            Console.WriteLine(num + " is num? " + m);
            assert(m);

            num = "0.5";
            m = rx_number.IsMatch(num);
            Console.WriteLine(num + " is num? " + m);
            assert(m);

            num = "0..5";
            m = rx_number.IsMatch(num);
            Console.WriteLine(num + " is num? " + m);
            assert(!m);
        }

        private static void test_regex_ident()
        {
            //Regex rx_ident = new Regex(@"\b([a-zA-Z][a-zA-Z0-9_]*)\b", RegexOptions.Compiled);
            string text = "The the23 q_ui_ck brown 12fox  _fox jumps over the lazy d dog.";
            MatchCollection matches = rx_ident.Matches(text);
            foreach (Match match in matches)
            {
                GroupCollection groups = match.Groups;
                string word = groups[0].Value;
                //Console.WriteLine("'{0}' repeated at positions {1} and {2}",
                //                  word,
                //                  groups[0].Index,
                //                  groups[1].Index);

                assert(!word.Contains("fox"));
            }
            // see that 12fox and _fox do not appear
            /* 
            'The' repeated at positions 0 and 0
            'the23' repeated at positions 4 and 4
            'q_ui_ck' repeated at positions 10 and 10
            'brown' repeated at positions 18 and 18
            'jumps' repeated at positions 36 and 36
            'over' repeated at positions 42 and 42
            'the' repeated at positions 47 and 47
            'lazy' repeated at positions 51 and 51
            'd' repeated at positions 56 and 56
            'dog' repeated at positions 58 and 58
            */
        }

        // dont depend on white space for token splitting
        private static void test_sample4()
        {
            string sample = "a=123+456";
            //               123334555
            List<string> toks = tokenize(sample);
            Console.WriteLine("tokens for: " + sample);
            Console.WriteLine("# " + toks.Count);
            foreach (string tok in toks)
            {
                Console.WriteLine("- " + tok);
            }
            assert(toks.Count == 5);
        }

        static TokenKind token_kind(string token)
        {
            token = token.Trim();
            if (token.Length == 0)
            {
                return TokenKind.Empty;
            }
            else if (rx_ident.IsMatch(token))
            {
                return TokenKind.Ident;
            }
            else if (rx_number.IsMatch(token))
            {
                return TokenKind.Number;
            }
            return TokenKind.Symbol;
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

        // split tokens on white space, preserve strings
        public static List<string> tokenize_on_kind(List<string> tokens)
        {
            Console.WriteLine("tokenize_on_kind( " + tokens + ")");
            List<string> results = new List<string>();

            foreach (string tok in tokens)
            {

            }

            return results;
        }

        // split tokens on white space, preserve strings
        public static List<string> tokenize(string source)
        {
            Console.WriteLine("tokenize( " + source + ")");
            List<string> result = new List<string>();

            bool inString = false;
            string tok = "";
            char prev = '\0';
            string last_tok = "";
            for (int i = 0; i < source.Length; i++)
            {
                char ch = source[i];
                // Console.WriteLine("- " + i + " " + ch + "\n");

                if (result.Count > 0)
                {
                    last_tok = result[result.Count - 1];
                    // Console.WriteLine("- " + i + " " + ch + "\n");
                }

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
