namespace CFT.MiddleWare.Base
{
    public interface ICFTInputFileInfo
    {
        byte[] FileContent { get; }
        string FileName { get; }
        string FullName { get; }
    }
}