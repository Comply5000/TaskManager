namespace TaskManager.Application.Common.DTOs;

public sealed record BaseEnum
{
    public int Id { get; set; }
    public string? Name { get; set; }

    public static implicit operator BaseEnum(Enum? enumObject)
    {
        var baseEnum = new BaseEnum();
        
        baseEnum.Id = Convert.ToInt32(enumObject);
        baseEnum.Name = enumObject.ToString();

        return baseEnum;
    }
}