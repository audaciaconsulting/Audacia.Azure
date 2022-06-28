namespace Audacia.Azure.Common.ReturnOptions
{
    public interface IQueueReturnOption<T>
    {
        T Result { get; set; }

        T Parse(string jsonString);
    }
}