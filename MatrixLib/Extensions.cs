using System.Numerics;

namespace MatrixLib;

public static class Extensions
{
    public static T[][] ConvertToJaggedArray<T>(this Matrix<T> m) where T : INumber<T>
    {
        T[][]  result = new T[m.Size.RowCount][];
        for (int i = 0; i < m.Size.RowCount; i++)
        {
            result[i] = new T[m.Size.ColCount];
            for (int j = 0; j < m.Size.ColCount; j++)
            {
                
                result[i][j] = m[i, j];
            }
        }

        return result;
    }
    
    public static T Determinant<T>(this Matrix<T> matrix) where T: INumber<T>
    {
        int n = matrix.Size.RowCount;
        if (matrix.Size.RowCount != matrix.Size.ColCount)
            throw new ArgumentException("Матрица должна быть квадратной.");

        T det = T.One;
        Matrix<T> temp =(Matrix<T>) matrix.Clone(); // Копируем матрицу для изменений

        for (int i = 0; i < n; i++)
        {
            // Проверка на нулевой элемент на диагонали
            if (temp[i, i] == T.Zero)
            {
                bool swapped = false;
                for (int j = i + 1; j < n; j++)
                {
                    if (temp[j, i] != T.Zero)
                    {
                        SwapRows(temp, i, j);
                        det *= -T.One; // Меняем знак определителя при смене строк
                        swapped = true;
                        break;
                    }
                }
                if (!swapped) return T.Zero; // Если не удалось заменить строку, определитель = 0
            }

            // Прямой ход метода Гаусса
            for (int j = i + 1; j < n; j++)
            {
                T factor = temp[j, i] / temp[i, i];
                for (int k = i; k < n; k++)
                {
                    temp[j, k] -= factor * temp[i, k];
                }
            }

            // Умножение диагональных элементов
            det *= temp[i, i];
        }

        return det;
    }

    // Вспомогательная функция для обмена строк
    private static void SwapRows<T>(Matrix<T> matrix, int row1, int row2) where T : INumber<T>
    {
        int n = matrix.Size.ColCount;
        for (int i = 0; i < n; i++)
        {
            T temp = matrix[row1, i];
            matrix[row1, i] = matrix[row2, i];
            matrix[row2, i] = temp;
        }
    }
}
