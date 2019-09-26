namespace Assets.Scripts.Utils
{
    public interface IConverter<TSource, TDestination>
    {
        TDestination Convert(TSource source);
    }
}
