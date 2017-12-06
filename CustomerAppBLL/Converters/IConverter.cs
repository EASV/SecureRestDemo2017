namespace CustomerAppBLL.Converters
{
    public interface IConverter<Entity, BO>
    {
        Entity Convert(BO bo);

        BO Convert(Entity ent);
    }
}
