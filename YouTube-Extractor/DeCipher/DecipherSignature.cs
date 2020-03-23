using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace YouTubeExtractor.DeCipher
{
    internal static class DecipherSignature
    {
        internal static string Decrypt(string Signature, string URL)
        {
            string JsPlayerSource = Tool.JsPlayerSource;

            string ExtractedFunction = ExtractFunction(JsPlayerSource);

            string ExtractedBody = ExtractFunctionBody(JsPlayerSource, ExtractedFunction);

            string[] BodyStatements = ExtractedBody.Split(';');

            string ExtractedDefines = ExtractDefines(ExtractedBody);

            string ExtractedDefinitionBody = ExtractDefinitionBody(JsPlayerSource, ExtractedDefines);

            List<Operation> OperationList = ExtractOperation(BodyStatements, ExtractedDefinitionBody);

            string Sig = Decrypter(Signature, OperationList);

            return QueryHelper.ChangeQueryPara(URL, "sig", Sig);

        }


        private static string ExtractFunction(string JsPlayerSource)
        {
            string FunctionPattern = @"(\w+)=function\(\w+\){(\w+)=\2\.split\(\x22{2}\);.*?return\s+\2\.join\(\x22{2}\)}";

            string Function = Regex.Match(JsPlayerSource, FunctionPattern).Groups[1].Value;

            if (string.IsNullOrEmpty(Function))
            {
                throw new Exception("Function is null");
            }

            return Function;
        }

        private static string ExtractFunctionBody(string JsPlayerSource, string Function)
        {
            string FunctionBodyPattern = @"(?!h\.)" + Regex.Escape(Function) + @"=function\(\w+\)\{(.*?)\}";

            string FunctionBody = Regex.Match(JsPlayerSource, FunctionBodyPattern, RegexOptions.Singleline).Groups[1].Value;

            if (string.IsNullOrEmpty(FunctionBody))
            {
                throw new Exception("Function body is null");
            }

            return FunctionBody;
        }

        private static string ExtractDefines(string FunctionBody)
        {
            string DefinitionName = Regex.Match(FunctionBody, "(\\w+).\\w+\\(\\w+,\\d+\\);").Groups[1].Value;

            if (string.IsNullOrEmpty(DefinitionName))
            {
                throw new Exception("Defines is null");
            }

            return DefinitionName;
        }

        private static string ExtractDefinitionName(string BodyStatements)
        {
            string DefinitionName = Regex.Match(BodyStatements, "(\\w+).\\w+\\(\\w+,\\d+\\);").Groups[1].Value;

            return DefinitionName;
        }

        private static string ExtractDefinitionBody(string Source, string DefinitionName)
        {
            string DefinitionBody = Regex.Match(Source, @"var\s+" + Regex.Escape(DefinitionName) + @"=\{(\w+:function\(\w+(,\w+)?\)\{(.*?)\}),?\};", RegexOptions.Singleline).Groups[0].Value;

            return DefinitionBody;
        }
        private static List<Operation> ExtractOperation(string[] Statements, string DefinitionBody)
        {
            List<Operation> OperationList = new List<Operation>();

            foreach (string i in Statements)
            {

                string CalledFunction = Regex.Match(i, @"\w+(?:.|\[)(\""?\w+(?:\"")?)\]?\(").Groups[1].Value;

                if (string.IsNullOrWhiteSpace(CalledFunction))
                {
                    continue;
                }

                if (Regex.IsMatch(DefinitionBody, $@"{Regex.Escape(CalledFunction)}:\bfunction\b\([a],b\).(\breturn\b)?.?\w+\."))
                {
                    string Index = Regex.Match(i, @"\(\w+,(\d+)\)").Groups[1].Value;
                    OperationList.Add(new Operation() { Type = Operation.OperationType.Slice, Index = Index });
                }

                else if (Regex.IsMatch(DefinitionBody, $@"{Regex.Escape(CalledFunction)}:\bfunction\b\(\w+\,\w\).\bvar\b.\bc=a\b"))
                {
                    string Index = Regex.Match(i, @"\(\w+,(\d+)\)").Groups[1].Value;
                    OperationList.Add(new Operation() { Type = Operation.OperationType.CharSwap, Index = Index });
                }

                else if (Regex.IsMatch(DefinitionBody, $@"{Regex.Escape(CalledFunction)}:\bfunction\b\(\w+\)"))
                {
                    OperationList.Add(new Operation() { Type = Operation.OperationType.Reverse, Index = "" });
                }

            }

            return OperationList;

        }

        private static string CheckOperation(string Signature, Operation Operation)
        {
            switch (Operation.Type)
            {
                case Operation.OperationType.Reverse:
                    return new string(Signature.ToCharArray().Reverse().ToArray());

                case Operation.OperationType.CharSwap:
                    return SwapChar(Signature, int.Parse(Operation.Index));

                case Operation.OperationType.Slice:
                    return Signature.Substring(int.Parse(Operation.Index));

                default:
                    throw new NotImplementedException("Signature not found!");
            }
        }


        private static string SwapChar(string Signature, int Index)
        {
            StringBuilder Builder = new StringBuilder(Signature);

            Builder[0] = Signature[Index];
            Builder[Index] = Signature[0];
            
            return Builder.ToString();
        }

        private static string Decrypter(string Signature, List<Operation> OperationList)
        {
            foreach (var i in OperationList)
            {
                Signature = CheckOperation(Signature, i);
            }

            return Signature;
        }
    }
}
