using System;

namespace Matrix_calculator
{
    public class MatrixOperationExeption : Exception
    {
        public override string Message 
        { 
            get 
            {
                return "Can not perform operation";
            }
        }
    }

    public class ReadMatrixException : Exception
    {
        public override string Message
        {
            get
            {
                return string.Format("Incorrect read format!");
            }
        }
    }
    //========================================

    //класс матрицы
    public class matrix
    {
        mtrx_str[] mass; //массив строк
        int num_swaps; //количество перестановок

        public string Name { get; private set; }

        //размер матрицы
        public int Length { get; private set; } 
        public int I_Length { get; private set; }
        public int J_Length { get; private set; }
        // ранг
        public int Rang
        {
            get
            {
                int rank = 1;
                matrix c = Gauss_view();
                for (int i = 0; i < I_Length; ++i)
                    if (!c.IsStrZero(i))
                        ++rank;
                return rank;
            }
        }


        public matrix(int i_, int j_, string new_name)
        {
            mass = new mtrx_str[i_];
            for (int i = 0; i < mass.Length; ++i)
                mass[i] = new mtrx_str(j_);
            Length = i_ * j_;
            I_Length = i_;
            J_Length = j_;
            num_swaps = 0;
            Name = new_name;
        }

        public matrix(matrix b, string new_name)
        {
            mass = new mtrx_str[b.I_Length];
            for (int i = 0; i < mass.Length; ++i)
                mass[i] = new mtrx_str(b.mass[i]);
            Length = b.Length;
            I_Length = b.I_Length;
            J_Length = b.J_Length;
            num_swaps = b.num_swaps;
            Name = new_name;
        }

        public matrix(int i_, int j_)
        {
            mass = new mtrx_str[i_];
            for (int i = 0; i < mass.Length; ++i)
                mass[i] = new mtrx_str(j_);
            Length = i_ * j_;
            I_Length = i_;
            J_Length = j_;
            num_swaps = 0;
            Name = "";
        }

        public matrix(matrix b)
        {
            mass = new mtrx_str[b.I_Length];
            for (int i = 0; i < mass.Length; ++i)
                mass[i] = new mtrx_str(b.mass[i]);
            Length = b.Length;
            I_Length = b.I_Length;
            J_Length = b.J_Length;
            num_swaps = b.num_swaps;
            Name = b.Name;
        }



        public double this[int i, int j]
        {
            get { return mass[i][j]; }
            set { mass[i][j] = value; }
        }

        //+++++++++++++++++++++++++++++++++++++++++
        //вывести матрицу 
        public override string ToString()
        {
            string str=string.Format("[matrix: Name={0}, Length={1}, Rang={2}]\n", Name, Length, Rang);
            for (int i = 0; i < I_Length; ++i)
            {
                for (int j = 0; j < J_Length; ++j)
                {
                    str += this[i, j].ToString();
                    if (j != J_Length - 1)
                        str += ' ';
                }
                if (i != I_Length - 1)
                    str += '\n';
            }
            return str;
        }


        //единичная матрица
        public static matrix E_matrix(int ixi)
        {
            matrix e = new matrix(ixi,ixi, "E");
            for (int i = 0; i < e.I_Length; ++i)
                e[i, i] = 1;
            return e;
        }

        //поменять строки местами
        public void Swap_Str(int a, int b)
        {
            mtrx_str c = mass[a];
            mass[a] = mass[b];
            mass[b] = c;
            ++num_swaps;
        }

        //поменять столбцы местами
        public void Swap_Rows(int a, int b)
        {
            for (int j = 0; j <= J_Length; ++j)
            {
                double Temp = this[a, j];
                this[a, j] = this[b, j];
                this[b, j] = Temp;
            } 
            ++num_swaps;
        }

        //проверка на нулевую строку
        bool IsStrZero(int a)
        {
            for (int i = 0; i < mass[a].Length; ++i)
                if (this[a, i].Equals(0))
                    return false;
            return true;
        }

        //Транспонированная матрица
        public matrix Transposed_Matrix()
        {
            matrix c = new matrix(J_Length, I_Length);
            for (int i = 0; i < J_Length; ++i)
                for (int j = 0; j < I_Length; ++j)
                    c[i, j] = this[j, i];
            return c;
        }

        //Представление матрицы в ступенчатом виде
        public matrix Gauss_view()
        {
            matrix c = new matrix(this);

            for (int i = 0, j = 0; i < c.I_Length && j < c.J_Length; ++i, ++j)
            {
                if (c[i, j].Equals(0))
                {
                    for (int k = i + 1; k < c.I_Length; ++k)
                        if (!c[k, j].Equals(0))
                        {
                            c.Swap_Str(k, i);
                            goto W;
                        }
                    --i;
                    continue;
                }
            W:
                for (int k = i + 1; k < c.I_Length; ++k)
                {
                    if (c[k, j].Equals(0))
                        continue;
                    c.mass[k] += (c.mass[i] * (-c[k, j] / c[i, i]));
                }
            }                    
            return c;
        }

        //Определитель
        public double Det()
        {
            if (I_Length != J_Length)
                throw new MatrixOperationExeption();
            double s = 1;
            matrix c = Gauss_view();
            for (int i = 0; i < I_Length; ++i)
                s *= c[i, i];
            if (num_swaps % 2 == 0)
                return s;
            return -s;
        }

        //Союзная матрица
        public matrix Adjugate()
        {
            if (I_Length != J_Length)
                throw new MatrixOperationExeption();
            matrix c = new matrix(I_Length, J_Length);
            for (int k = 0; k < I_Length; ++k)
                for (int m = 0; m < J_Length; ++m)
            {
                    matrix r = new matrix(I_Length - 1, J_Length - 1);
                    int i2 = 0, j2 = 0;
                    for (int i = 0; i < I_Length; ++i)
                        for (int j = 0; j < J_Length; ++j)
                            if (k != i && m != j)
                            {
                                r[i2, j2] = this[i, j];
                                if (j2 == r.J_Length -1)
                                {
                                    ++i2;
                                    j2 = 0;
                                }
                                else
                                    ++j2;
                            }
                    if ((m + k) % 2 == 0)
                        c[k, m] = r.Det();
                    else
                        c[k, m] = -r.Det();
            }
            return c.Transposed_Matrix();
        }

        //обратная матрицаw
        public matrix Invertible()
        {
            matrix c =Adjugate();
            double dt = Det();
            if (dt.Equals(0))
                throw new MatrixOperationExeption();
            
            for (int i = 0; i < c.I_Length; ++i)
                for (int j = 0; j < c.J_Length; ++j)
                    c[i, j] /= dt;
            return c;
        }


        //+++++++++Operations with matrices++++++++
        public static matrix operator+(matrix a, matrix b)
        {
            if (a.I_Length != b.I_Length || a.I_Length != b.J_Length)
                throw new MatrixOperationExeption();
            matrix c = new matrix(a.I_Length,a.J_Length);
            for (int i = 0; i < c.I_Length; ++i)
                c.mass[i] = a.mass[i] + b.mass[i];
            return c;
        }

        public static matrix operator -(matrix a, matrix b)
        {
            if (a.I_Length != b.I_Length || a.I_Length != b.J_Length)
                throw new MatrixOperationExeption();
            matrix c = new matrix(a.I_Length, a.J_Length);
            for (int i = 0; i < c.I_Length; ++i)
                c.mass[i] = a.mass[i] - b.mass[i];
            return c;
        }

        public static matrix operator *(matrix a, matrix b)
        {
            matrix c = new matrix(a.I_Length, b.J_Length);
            for (int i = 0; i < c.I_Length; ++i)
                for (int j = 0; j < c.J_Length; ++j)
                    for (int k = 0; k < a.J_Length; ++k)
                        c[i, j] += a[i, k] * b[k, j];
            return c;
        
        }

        public static matrix operator /(matrix a, matrix b)
        {
            matrix c = new matrix(a.I_Length, b.J_Length);
            for (int i = 0; i < c.I_Length; ++i)
                for (int j = 0; j < c.J_Length; ++j)
                    for (int k = 0; k < a.J_Length; ++k)
                        c[i, j] += a[i, k] / b[k, j];
            return c;
        }


        //+++++++Operations with numbers+++++++++++
        public static matrix operator +(double n, matrix a)
        {
            matrix c = new matrix(a.I_Length, a.J_Length);
            for (int i = 0; i < a.I_Length; ++i)
                c.mass[i] = a.mass[i] + n;
            return c;
        }
        public static matrix operator +(matrix a, double n)
        {
            matrix c = new matrix(a.I_Length, a.J_Length);
            for (int i = 0; i < a.I_Length; ++i)
                c.mass[i] = a.mass[i] + n;
            return c;
        }
        public static matrix operator -(double n, matrix a)
        {
            matrix c = new matrix(a.I_Length, a.J_Length);
            for (int i = 0; i < a.I_Length; ++i)
                c.mass[i] = n - a.mass[i];
            return c;
        }
        public static matrix operator -(matrix a, double n)
        {
            matrix c = new matrix(a.I_Length, a.J_Length);
            for (int i = 0; i < a.I_Length; ++i)
                c.mass[i] = a.mass[i] - n;
            return c;
        }
        public static matrix operator *(double n, matrix a)
        {
            matrix c = new matrix(a.I_Length, a.J_Length);
            for (int i = 0; i < a.I_Length; ++i)
                c.mass[i] = n * a.mass[i];
            return c;
        }
        public static matrix operator *(matrix a, double n)
        {
            matrix c = new matrix(a.I_Length, a.J_Length);
            for (int i = 0; i < a.I_Length; ++i)
                c.mass[i] = a.mass[i] * n;
            return c;
        }
        public static matrix operator /(double n, matrix a)
        {
            matrix c = new matrix(a.I_Length, a.J_Length);
            for (int i = 0; i < a.I_Length; ++i)
                c.mass[i] = n / a.mass[i];
            return c;
        }
        public static matrix operator /(matrix a, double n)
        {
            matrix c = new matrix(a.I_Length, a.J_Length);
            for (int i = 0; i < a.I_Length; ++i)
                c.mass[i] = a.mass[i] / n;
            return c;
        }


    }

    //===================================

    //класс строк матрицы
    class mtrx_str
    {
        double[] num_str; //массив чисел (строка)
        //длина строки
        public int Length { get { return num_str.Length; } } 

        public mtrx_str (int size)
        {
            num_str = new double[size];
            for (int i = 0; i < num_str.Length; ++i)
                num_str[i] = 0;
        }
        public mtrx_str(mtrx_str b)
        {
            num_str = new double[b.Length];
            for (int i = 0; i < num_str.Length; ++i)
                num_str[i] = b[i];
        }

        public double this[int n]
        {
            get { return num_str[n]; }
            set { num_str[n] = value; }
        }

        //++++++++++++++++++++Operations with string+++++++++++++++++++
        public static mtrx_str operator +(mtrx_str a, mtrx_str b)
        {
            if (a.Length != b.Length)
                throw new MatrixOperationExeption();
            
            mtrx_str c = new mtrx_str(a.Length);
            for (int i = 0; i < c.Length; ++i)
                c[i] = a[i] + b[i];
            return c;
        }

        public static mtrx_str operator -(mtrx_str a, mtrx_str b)
        {
            if (a.Length != b.Length)
                throw new MatrixOperationExeption();

            mtrx_str c = new mtrx_str(a.Length);
            for (int i = 0; i < c.Length; ++i)
                c[i] = a[i] - b[i];
            return c;
        }

        //++++++++++++++++++++Operations with numbers+++++++++++++++++
        public static mtrx_str operator +(mtrx_str a, double b)
        {
            mtrx_str c = new mtrx_str(a.Length);
            for (int i = 0; i < c.Length; ++i)
                c[i] = a[i] + b;
            return c;
        }

        public static mtrx_str operator +(double b, mtrx_str a)
        {
            mtrx_str c = new mtrx_str(a.Length);
            for (int i = 0; i < c.Length; ++i)
                c[i] = a[i] + b;
            return c;
        }

        public static mtrx_str operator -(mtrx_str a, double b)
        {
            mtrx_str c = new mtrx_str(a.Length);
            for (int i = 0; i < c.Length; ++i)
                c[i] = a[i] - b;
            return c;
        }

        public static mtrx_str operator -(double b, mtrx_str a)
        {
            mtrx_str c = new mtrx_str(a.Length);
            for (int i = 0; i < c.Length; ++i)
                c[i] = b - a[i];
            return c;
        }

        public static mtrx_str operator *(mtrx_str a, double b)
        {
            mtrx_str c = new mtrx_str(a.Length);
            for (int i = 0; i < c.Length; ++i)
                c[i] = a[i] * b;
            return c;
        }

        public static mtrx_str operator *(double b, mtrx_str a)
        {
            mtrx_str c = new mtrx_str(a.Length);
            for (int i = 0; i < c.Length; ++i)
                c[i] = a[i] * b;
            return c;
        }

        public static mtrx_str operator /(mtrx_str a, double b)
        {
            mtrx_str c = new mtrx_str(a.Length);
            if (b.Equals(0))
                throw new MatrixOperationExeption();
            for (int i = 0; i < c.Length; ++i)
                c[i] = a[i] / b;
            return c;
        }
        public static mtrx_str operator /(double b, mtrx_str a)
        {
            mtrx_str c = new mtrx_str(a.Length);
            for (int i = 0; i < c.Length; ++i)
            {
                if (a[i].Equals(0))
                    throw new MatrixOperationExeption();
                c[i] = b / a[i];
            }
            return c;
        }
    }
}
