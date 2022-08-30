namespace Audacia.Azure.Common.ReturnOptions
{
    public interface IBlobReturnOption<T>
    {
        string BlobName { get; }

        T Result { get; }

        T Parse(string blobName, byte[] bytes, Uri blobClientUrl);
    }
}
