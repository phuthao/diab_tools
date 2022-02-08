using CpTech.Core.Middle.Dtos;
using CoreFilterDto = DiaB.Middle.Dtos.BaseDtos.CoreFilterDto;
using PagingFilterDto = DiaB.Middle.Dtos.BaseDtos.PagingFilterDto;

namespace DiaB.Middle.Dtos.ConfigurationDtos
{
    public partial class ConfigurationDtos
    {
        public partial class AppFilter : PagingFilterDto
        {
        }

        public partial class Filter : CoreFilterDto
        {
        }

        public abstract partial class BaseFilter : CoreFilterDto
        {
        }

        public partial class ColorFilter : BaseFilter
        {
        }

        public partial class Item : CoreItemDto
        {
        }

        public partial class View : Item
        {
        }

        public partial class Insert : CoreInsertDto
        {
        }

        public partial class Update : CoreUpdateDto
        {
        }
    }
}
