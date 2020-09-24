namespace ApiWrapper
{
    public interface IFile
    {
        string ParameterName { get; set; }
        string FileName { get; set; }
        byte[] Bytes { get; set; }
    }
}