using System;
using System.Collections;
using Matrix_calculator;


namespace MatrixParser
{
    public static class Dispenser
    {
        public static string Solver(matrix[] matrixes, string input)
        {
            var rev_input = Parser.ReversePolishNotation(input);

            Stack stack = new Stack();

            foreach (var thing in rev_input)
            {
                switch (thing.type)
                {
                    case StringPlusType.Type.Number : stack.Push(thing.Getdouble());
                        break;
                    case StringPlusType.Type.Field : stack.Push(FindMatrix(matrixes, thing.data));
                        break;
                    case StringPlusType.Type.Operator:
                        stack.Push(DoOperation(stack.Pop(), stack.Pop(), thing.data));
                        break;
                    case StringPlusType.Type.Function:
                        stack.Push(DoFunction(stack.Pop(), thing.data));
                        break;
                }
            }

            if (stack.Count == 0)
                throw new MatrixOperationExeption();

            return stack.Pop().ToString();
        }

        private static object DoFunction(object v, string data)
        {
            matrix a = v as matrix;
            switch (data)
            {
                case "Adjugate": return a.Adjugate();
                case "Det" : return a.Det();
                case "Gauss_view" : return a.Gauss_view();
                case "Transpose" : return a.Transposed_Matrix();
                case "Inverse" : return a.Invertible();
                case "Rang" : return (double)a.Rang;
                case "Length": return (double)a.Length;
            }
            throw new ReadMatrixException();
        }

        private static object DoOperation(object v1, object v2, string data)
        {
            if (v1 is double && v2 is double)
            {
                double a = (double)v1, b = (double)v2;
                switch (data)
                {
                    case "+": return a + b;
                    case "-": return a - b;
                    case "*": return a * b;
                    case "/": return a / b;    
                }
            }
            else if (v1 is matrix && v2 is matrix)
            {
                matrix a = (matrix)v1, b = (matrix)v2;
                switch (data)
                {
                    case "+": return a + b;
                    case "-": return a - b;
                    case "*": return a * b;
                    case "/": return a / b;
                }
            }
            else if (v1 is double && v2 is matrix)
            {
                double a = (double)v1;
                matrix b = (matrix)v2;
                switch (data)
                {
                    case "+": return a + b;
                    case "-": return a - b;
                    case "*": return a * b;
                    case "/": return a / b;
                }
            }
            else
            {
                matrix a = (matrix)v1;
                double b = (double)v2;
                switch (data)
                {
                    case "+": return a + b;
                    case "-": return a - b;
                    case "*": return a * b;
                    case "/": return a / b;
                }
            }
            throw new MatrixOperationExeption();
        }

        private static matrix FindMatrix(matrix[] matrixes, string data)
        {
            matrix mtrx = Array.Find(matrixes, (obj) => obj.Name == data);
            if (mtrx.Name == "")
                throw new ReadMatrixException();
            return mtrx;
        }


    }
}
