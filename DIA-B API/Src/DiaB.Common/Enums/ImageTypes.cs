using System.ComponentModel;

namespace DiaB.Common.Enums
{
    [Description("Loại hình ảnh được nhập vào")]
    public enum ImageTypes
    {
        [Description("Minh họa bài tập thể dục")]
        ExerciseCover = 1,
        [Description("Bản ghi lượng đường tiêu thụ hằng ngày")]
        GlucoseDailyInputRecord = 2,
        [Description("Bản ghi đường huyết nhập vào hàng ngày")]
        BloodPressureDailyInputRecord = 3,
        [Description("Bản ghi cân nặng nhập vào hàng ngày")]
        WeightInputRecord = 4,
        [Description("Bản ghi HbA1c nhập vào hàng ngày")]
        HbA1cInput = 5,
        [Description("Bản ghi cảm xúc nhập vào hàng ngày")]
        EmotionInput = 6,
        [Description("Bản ghi bài tập thể dục hàng ngày")]
        ExerciseInput = 7,
        [Description("Bản ghi chế độ dinh dưỡng hằng ngày")]
        DietInput = 8,
        [Description("Cảm xúc")]
        Emotion = 9,
        [Description("Avatar")]
        Account = 10,
        [Description("Thông báo")]
        Communication = 11,
        [Description("Bài viết/Khóa học")]
        LearningPost = 12,

        Activity = 13,

        Symptom = 14,

        [Description("Môn vận động")]
        ExerciseCategory = 15,

        FoodCategory = 16,

        Food = 17,
    }
}
