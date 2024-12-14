namespace MatrixLib;

public class MatrixSizeException : Exception
{
    public MatrixSizeException()
    {
        
    }
    
    public MatrixSizeException(string message):base(message)
    {
        
    }
}

public struct MSize(int rowCount, int colCount)
{

    public int RowCount
    {
        get => rowCount;
        set { rowCount = value > 0 ? rowCount : throw new ArgumentException("Row count should be more then 0"); }
    }
    
    public int ColCount
    {
        get => colCount;
        
        set {colCount = value > 0 ? colCount : throw new ArgumentException("Row count should be more then 0"); }
    }
    
    
    public static bool operator != (MSize m1, MSize m2) => !(m1 == m2);
    
    public static bool operator == (MSize m1, MSize m2) => ((m1.RowCount == m2.RowCount) && (m1.ColCount == m2.ColCount));

    public override string ToString() => $"Row: {RowCount}, Col: {ColCount}";

}