using System;
using Matrix_calculator;
using System.IO;
using MatrixParser;



class MainClass
{
    static StreamReader strm;
    static matrix[] mtx_mas;

    static void Fill_Matrix(int n)
    {
        for (int i = 0; i < mtx_mas[n].I_Length; ++i)
        {
            string[] str2 = strm.ReadLine().Split(' ');
            if (str2.Length != mtx_mas[n].J_Length)
                throw new ReadMatrixException();
            for (int j = 0; j < mtx_mas[n].J_Length; ++j)
            {
                double k;
                if (!double.TryParse(str2[j], out k))
                    throw new ReadMatrixException();
                mtx_mas[n][i, j] = k;
            }
        }
    }

    static int[] size_Parse()
    {
        string[] str2 = strm.ReadLine().Split(' ', 'X', 'x');
        if (str2.Length != 2)
            throw new ReadMatrixException();
        int[] size= new int[2];
        for (int i = 0; i < 2; ++i)
            if (!int.TryParse(str2[i], out size[i]))
                throw new ReadMatrixException();
        return size;
    }

    static void Read_matrices()
    {
        mtx_mas = new matrix[1];
        int r = 1;
        while (true)
        {
            char name = (char)strm.Read();
            if (!(name >= 'A' && name <= 'Z'))
            {
                if (name == ' ' || name == '\n') 
                    return;
                throw new ReadMatrixException();
            }

            int[] size = size_Parse();
            mtx_mas[r-1] = new matrix(size[0], size[1], name.ToString());
            Fill_Matrix(r-1);
            Array.Resize(ref mtx_mas, ++r);
        }
    }
    //==============================================//

    public static void Main(string[] args)
    {
        strm = new StreamReader(new FileStream("/Users/nk/Desktop/Matrix_calculator.txt", FileMode.Open, FileAccess.Read));
        try
        {
            Read_matrices();
            Array.Resize(ref mtx_mas, mtx_mas.Length-1);
        }
        catch (ReadMatrixException)
        {
            Console.WriteLine("Ошибка ввода матриц!");
            return;
        }

        string input = strm.ReadLine();

        try
        {
            Console.WriteLine(Dispenser.Solver(mtx_mas, input));

        }
        catch (MatrixOperationExeption)
        {
            Console.WriteLine("Невыполнимая операция!");
        }


    }
}

/*Пример входного файла
A3x3
10 -1 0
0 8 10
1 10 -8
C3x3
-5 -7 -4
3 5 -5
0 -8 1
D3x3
-9 9 -9
-3 -1 -7
2 4 8

Inverse(A)+Det((A+C)*D)+3-100   -  Gauss_view(A)
*/
//Доступные ф-ии:
//Det() - детерминант
//Adjugate() - матрица алгебраических дополнений
//Transpose() - транспонированная матрица
//Inverse() - обратая матрица
//Rang() - ранг матрицы
//Length() - размер матрицы