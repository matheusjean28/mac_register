using MacSave.Models.Categories.OperationModelEnums;

namespace MacSave.Models.FromBodyParamsDarty
{
    public class CreateCategoryDarty
    {
        public string DeviceCategoryName { get; set; }

        public OperationModelEnum OperationMode { get; set; } = OperationModelEnum.None;

        public string MakerId { get; set; } = string.Empty;

    }
}
