using System;
using System.Collections;
using System.Collections.Generic;
using Matrix_calculator;

namespace MatrixParser
{
    public class StringPlusType
    {
        public enum Type
        {
            Number,
            Scobka1,
            Scobka2,
            Function,
            Operator,
            Field
        }

        public Type type;
        public string data;

        public StringPlusType(string str)
        {

            if (double.TryParse(str, out double n))
                type = Type.Number;
            else if (str == "(")
                type = Type.Scobka1;
            else if (str == ")")
                type = Type.Scobka2;
            else if (str == "Det" || str== "Adjugate" || str == "Gauss_view" || str== "Transpose" || str=="Inverse" || str =="Rang" || str=="Lenght") //...=====================
                type = Type.Function;
            else if (str == "+" || str == "-" || str == "*" || str == "/")
                type = Type.Operator;
            else
                type = Type.Field;
            data = str;
        }

        public double Getdouble() => (type == Type.Number) ? double.Parse(data) : throw new Exception();
    }

    public static class Parser
    {
        static List<StringPlusType> NormalizeInput(string input)
        {
            List<StringPlusType> lst= new List<StringPlusType>();

            string str = "";
            foreach (var c in input)
            {
                if (c == ' ')
                    continue;

                if (c == '+' || c == '-' || c == '*' || c == '/' || c == '(' || c == ')')
                {
                    if (str != "")
                    {
                        lst.Add(new StringPlusType(str));
                        str = "";
                    }
                    lst.Add(new StringPlusType(c + ""));
                }
                else
                    str += c;
            }

            if (str != "")
                lst.Add(new StringPlusType(str));
            
            return lst;
        }


        public static List<StringPlusType> ReversePolishNotation(string input)
        {
            var n_input = NormalizeInput(input);

            List<StringPlusType> result= new List<StringPlusType>();

            Stack stack = new Stack();

            foreach (var str in n_input)
            {
                switch (str.type)
                {
                    case StringPlusType.Type.Number: result.Add(str);
                        break;
                    case StringPlusType.Type.Function: stack.Push(str);
                        break;
                    case StringPlusType.Type.Scobka1: stack.Push(str);
                        break;
                    case StringPlusType.Type.Scobka2:
                        {
                            StringPlusType output= stack.Pop() as StringPlusType;
                            while (output.type!=StringPlusType.Type.Scobka1)
                            {
                                result.Add(output);
                                output = stack.Pop() as StringPlusType;
                                if (stack.Count == 0)
                                    throw new Exception();
                            }
                            break;
                        }
                    case StringPlusType.Type.Field : result.Add(str);
                        break;
                    case StringPlusType.Type.Operator :
                        
                        if (stack.Count==0)
                        {
                            stack.Push(str);
                            break;
                        }

                        var clone = stack.Peek() as StringPlusType;
                        if (clone.type != StringPlusType.Type.Operator)
                        {
                            stack.Push(str);
                            break;
                        }
                        if (str.data == "+" || str.data == "-")
                        {
                            result.Add(stack.Pop() as StringPlusType);
                            stack.Push(str);
                        }
                        else
                        {
                            stack.Push(str);
                        }
                        break;
                }
            }

            while (stack.Count!=0)
            {
                result.Add(stack.Pop() as StringPlusType);
            }

            return result;
        }
    }
}
