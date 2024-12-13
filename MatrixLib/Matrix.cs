using System.Numerics;
using System.Text;

namespace MatrixLib;

public class Matrix<T> : ICloneable where T : INumber<T>
{
    private MSize _mSize;
    private T[,] _values;


    private T[,] Values
    {
        get => _values;
        set { _values = value; RefreshMatrix();}
    }
    
    public T[][] Rows { get; private set; }

    public T[][] Cols { get; private set; }


    public T this[int indexI, int indexJ]
    {
        get => _values[indexI, indexJ];
        set { _values[indexI, indexJ] = value; RefreshMatrix();}
    }

    public MSize Size
    {
        get => _mSize;
        private set => _mSize = value;
    }

    public Matrix(MSize size)
    {
        Size = size;
        _values = new T[size.RowCount, size.ColCount];
        RefreshMatrix();
    }

    public Matrix(int row, int col) : this(new MSize(row, col))
    {
    }

    public Matrix(T[,] values) : this(values.GetLength(0), values.GetLength(1))
    {
        Values = values;
        
    }


    private void RefreshMatrix()
    {
        Rows = Enumerable.Range(0, Size.RowCount)
            .Select(i => Enumerable.Range(0, Size.ColCount).Select(j => this[i, j]).ToArray()).ToArray();
        Cols = Enumerable.Range(0, Size.ColCount)
            .Select(i => Enumerable.Range(0, Size.RowCount).Select(j => this[j, i]).ToArray()).ToArray();
    }


    private static Matrix<T> HandleSameSizeMatrix(Matrix<T> left, Matrix<T> right, Func<T, T, T> func)
    {
        Matrix<T> result = new Matrix<T>(left.Size);
        if (left.Size == right.Size)
        {
            for (int i = 0; i < left.Size.RowCount; i++)
            {
                for (int j = 0; j < left.Size.ColCount; j++)
                {
                    result[i, j] = func(left[i, j], right[i, j]);
                }
            }

            return result;
        }

        throw new MatrixSizeException();
    }

    public static Matrix<T> operator *(Matrix<T> m, T value)
    {
        var temp = new Matrix<T>(m.Size);
        for (int i = 0; i < temp.Size.RowCount; i++)
        {
            for (int j = 0; j < temp.Size.ColCount; j++)
            {
                temp[i, j] = m[i, j] * value;
            }
        }

        return temp;
    }

    public static Matrix<T> operator +(Matrix<T> left, Matrix<T> right)
    {
        return HandleSameSizeMatrix(left, right, (x, y) => x + y);
        
    }


    public static Matrix<T> operator -(Matrix<T> left, Matrix<T> right) =>
        HandleSameSizeMatrix(left, right, (x, y) => x - y);

    public static Matrix<T> operator *(Matrix<T> left, Matrix<T> right)
    {
        Matrix<T> result = new Matrix<T>(left.Size.RowCount, right.Size.ColCount);
        if (left.Size.ColCount == right.Size.RowCount)
        {
            for (int i = 0; i < left.Size.RowCount; i++)
            {
                for (int j = 0; j < right.Size.ColCount; j++)
                {
                    for (int k = 0; k < left.Size.ColCount; k++)
                    {
                        result[i, j] += left[i, k] * right[k, j];
                    }
                }
            }

            return result;
        }

        throw new MatrixSizeException("* operation need first matrix col count be equal to second matrix row count");
    }

    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < _values.GetLength(0); i++)
        {
            for (int j = 0; j < _values.GetLength(1); j++)
                sb.Append($"{_values[i, j],10}");
            sb.Append('\n');
        }

        return sb.ToString();
    }

    public object Clone()
    {
        var temp = new Matrix<T>(Size);

        for (int i = 0; i < Size.RowCount; i++)
        {
            for (int j = 0; j < Size.ColCount; j++)
            {
                temp[i, j] = this[i, j];
            }
        }

        return temp;
    }
}