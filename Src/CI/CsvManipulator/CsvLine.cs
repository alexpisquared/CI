namespace CsvManipulator
{
  internal class CsvLine
  {
    public string? B { get; set; }
    public string? C { get; set; }
    public string? D { get; set; }
    public string? E { get; set; }

    public override string ToString() => $"{B}\t{C}\t{D}\t{E}";
  }
}